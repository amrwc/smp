import { Component, OnInit, Input, ViewChildren, QueryList } from '@angular/core';
import { ConversationsService } from '../services/conversations.service';
import { MessagesService } from '../services/messages.service';
import { UsersService } from '../services/users.service';
import { FriendlyMessage } from '../models/message';
import { User } from '../models/user';
import { CreateMessageRequest } from '../models/requests/create-message-request';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { GlobalHelper } from '../helpers/global';
import { CurrentUser } from '../models/current-user';
import * as signalR from '@aspnet/signalr';

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.scss'],
})
export class ConversationComponent implements OnInit {

  public loadedConversation: boolean = false;
  public users: Map<string, User> = new Map<string, User>();
  public messages: FriendlyMessage[] = new Array<FriendlyMessage>();
  public form: FormGroup;
  public content: FormControl = new FormControl("", Validators.required);
  private _hubConnection: signalR.HubConnection;
  private _conversationId: string;
  private _currentPage: number;

  private _lastMessageElement: Element;
  @ViewChildren('message') _messageElements: QueryList<any>;

  /**
   * This property runs every time a conversation is selected.
   * @param id conversation ID
   */
  @Input() set conversationId(id: string) {
    if (id === this._conversationId) return;

    this._conversationId = id;
    this.messages = new Array<FriendlyMessage>();
    this._currentPage = 0;
    this.loadedConversation = true;
    this.users = new Map<string, User>();
    this.initialiseMessages();

    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/hubs/messages', { accessTokenFactory: () => this.globalHelper.localStorageItem<CurrentUser>('currentUser').token })
      .build();
    this._hubConnection.start();
    this._hubConnection.on(`newmessage/${this._conversationId}`, () => {
      this.getNewestMessages();
    });
  }

  get conversationId() {
    return this._conversationId;
  }

  constructor(private conversationsService: ConversationsService,
    private messagesService: MessagesService,
    private usersService: UsersService,
    private globalHelper: GlobalHelper,
    formBuilder: FormBuilder) {
      this.form = formBuilder.group({
        "content": this.content
      });
    }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this._messageElements.changes.subscribe(msg => {
      if (this._messageElements.length) {
        this._lastMessageElement = this.getOldestMessageElement();
        const observer = new IntersectionObserver(
          (entries, observer) => this.lastMessageObserverCallback(entries, observer),
          { threshold: 0.5 }
        );
        observer.observe(this._lastMessageElement);
      }
    });
  }

  public sendMessage(): void {
    const createMessageRequest = new CreateMessageRequest();
    createMessageRequest.content = this.form.value.content;
    createMessageRequest.conversationId = this._conversationId;
    createMessageRequest.senderId = this.globalHelper.localStorageItem<CurrentUser>('currentUser').id;

    this.messagesService.createMessage(createMessageRequest).subscribe({
      next: () => { }
    });

    this.form.reset();
  }

  public keyDown(event: KeyboardEvent): void {
    if (event.keyCode === 13) {
      event.preventDefault();
      this.sendMessage();
    }
  }

  /**
   * Fetches messages, appends them to `this.messages` and returns count of fetched messages.
   * @private
   * @param messagesCount number of messages to fetch
   * @param pageNumber which page to fetch
   * @returns fetched messages count
   */
  private async getMessages(messagesCount: number, pageNumber: number): Promise<number> {
    return new Promise((resolve, reject) => {
      this.messagesService.getMessagesFromConversation(this._conversationId, messagesCount, pageNumber).subscribe({
        next: (messages: FriendlyMessage[]) => {
          this.messages.push(...messages);
          resolve(messages.length);
        }
      });
    });
  }

  /**
   * Initialises the conversation component by fetching the last 25 messages.
   * @private
   */
  private initialiseMessages(): void {
    this.conversationsService.getConversationParticipants(this._conversationId).subscribe({
      next: (userIds: string[]) => {
        userIds.forEach((userId: string) => {
          this.usersService.getUser(userId).subscribe({
            next: (user: User) => {
              this.users.set(user.id, user);
            }
          });
        });
      }
    });
    this.getMessages(25, this._currentPage);
  }

  /**
   * Fetches 10 newest messages and only appends unique entries to `this.messages` in case of any networking issue that
   * could cause multiple copies of the same message being displayed.
   * @private
   */
  private getNewestMessages(): void {
    this.messagesService.getMessagesFromConversation(this._conversationId, 10, 0).subscribe({
      next: (messages: FriendlyMessage[]) => {
        const stringifiedMessages: string[] = messages.concat(this.messages).map(el => JSON.stringify(el));
        const uniqueMessages: Set<string> = new Set<string>(stringifiedMessages);
        this.messages = Array.from(uniqueMessages).map(el => new FriendlyMessage(JSON.parse(el) as FriendlyMessage));
      }
    });
  }

  /**
   * Returns the oldest message element currently loaded.
   * @private
   * @returns the oldest message on the current page
   */
  private getOldestMessageElement(): Element {
    const messages = document.getElementsByClassName('message');
    return messages[messages.length - 1];
  }

  /**
   * Detects whether the last message element is in the viewport and loads the next 25 messages from the next page.
   * Then, it changes the observation target to the current oldest message. If the most recent target is the last
   * available message, it bails.
   * @private
   * @param entries Intersection Observer entries
   * @param observer the Intersection Observer instance
   * @returns void promise
   */
  private async lastMessageObserverCallback(
    entries: IntersectionObserverEntry[],
    observer: IntersectionObserver
  ): Promise<void> {
    if (entries[entries.length - 1].isIntersecting) {
      observer.unobserve(this._lastMessageElement);
      if (!await this.getMessages(25, ++this._currentPage)) {
        --this._currentPage;
        return;
      }
      setTimeout(() => {
        this._lastMessageElement = this.getOldestMessageElement();
        observer.observe(this._lastMessageElement);
      }, 100);
    }
  }
}
