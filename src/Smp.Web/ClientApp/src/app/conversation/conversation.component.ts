import { Component, OnInit, Input } from '@angular/core';
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
  styleUrls: ['./conversation.component.scss']
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

  /**
   * This property runs every time a conversation is selected.
   * @param {string} id conversation ID
   */
  @Input() set conversationId(id: string) {
    if (id === this._conversationId) return;

    this._conversationId = id;
    this._currentPage = 0;
    this.loadedConversation = true;
    this.users = new Map<string, User>();
    this.initialiseMessages();

    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/hub')
      .build();
    this._hubConnection.start();
    this._hubConnection.on('newmessage', (conversationId: any) => {
      this.getNewestMessages();
    });

    // TODO: Add event listener for scroll to top (bottom, really) and change page + get new messages
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

  public sendMessage(): void {
    let createMessageRequest = new CreateMessageRequest();
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
      this.sendMessage();
      event.preventDefault();
    }
  }

  private getMessages(messagesCount: number, pageNumber: number): void {
    this.messagesService.getMessagesFromConversation(this._conversationId, messagesCount, pageNumber).subscribe({
      next: (messages: FriendlyMessage[]) => {
        this.messages.push(...messages);
      }
    });
  }

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

  private getNewestMessages(): void {
    this.messagesService.getMessagesFromConversation(this._conversationId, 10, 0).subscribe({
      next: (messages: FriendlyMessage[]) => {
        const stringifiedMessages: string[] = messages.concat(this.messages).map(el => JSON.stringify(el));
        const uniqueMessages: Set<string> = new Set<string>(stringifiedMessages);
        this.messages = Array.from(uniqueMessages).map(el => new FriendlyMessage(JSON.parse(el) as FriendlyMessage));
      }
    });
  }
}
