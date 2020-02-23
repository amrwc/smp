import { Component, OnInit, Input } from '@angular/core';
import { ConversationsService } from '../services/conversations.service';
import { MessagesService } from '../services/messages.service';
import { UsersService } from '../services/users.service';
import { FriendlyMessage } from '../models/message';
import { User } from '../models/user';
import { CreateMessageRequest } from '../models/requests/create-message-request';

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.scss']
})
export class ConversationComponent implements OnInit {

  private _conversationId: string;
  public loadedConversation: boolean = false;

  public createMessageRequest: CreateMessageRequest = new CreateMessageRequest();
  public users: Map<string, User> = new Map<string, User>();

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

  public messages: FriendlyMessage[];

  constructor(private conversationsService: ConversationsService,
    private messagesService: MessagesService,
    private usersService: UsersService) { }

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
        this.messages = messages;
      }
    });
  }

  public sendMessage(): void {
    alert(`HI MESSAGE CONTENT IS: ${this.createMessageRequest.content}`);
  }
}
