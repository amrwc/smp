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

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.scss']
})
export class ConversationComponent implements OnInit {

  private _conversationId: string;
  public loadedConversation: boolean = false;

  public users: Map<string, User> = new Map<string, User>();
  public messages: FriendlyMessage[];

  public form: FormGroup;
  public content: FormControl = new FormControl("", Validators.required);

  @Input() set conversationId(id: string) {
    if (id == this._conversationId) return;

    this._conversationId = id;
    this.loadedConversation = true;
    this.users = new Map<string, User>();
    this.getMessages();
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

  private getMessages(): void {
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

    this.messagesService.getMessagesFromConversation(this._conversationId, 25, 0).subscribe({
      next: (messages: FriendlyMessage[]) => {
        this.messages = messages.reverse();
      }
    });
  }

  public sendMessage(): void {
    let createMessageRequest = new CreateMessageRequest();
    createMessageRequest.content = this.form.value.content;
    createMessageRequest.conversationId = this._conversationId;
    createMessageRequest.senderId = this.globalHelper.localStorageItem<CurrentUser>('currentUser').id;
    console.log(createMessageRequest);

    this.messagesService.createMessage(createMessageRequest).subscribe({
      next: () => {

      }
    });

    this.form.reset();
  }

  public keyDown(event: KeyboardEvent): void {
    if(event.keyCode === 13) {
      this.sendMessage();
      event.preventDefault();
    }
  }
}
