import { Component, OnInit, ViewChild } from '@angular/core';
import { Conversation, ExtendedConversation } from '../models/conversation';
import { GlobalHelper } from '../helpers/global';
import { ConversationsService } from '../services/conversations.service';
import { MessagesService } from '../services/messages.service';
import { CurrentUser } from '../models/current-user';
import { Message, FriendlyMessage } from '../models/message';
import { UsersService } from '../services/users.service';
import { User } from '../models/user';
import { ConversationComponent } from '../conversation/conversation.component';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {

  @ViewChild(ConversationComponent)
  conversation: ConversationComponent;

  public conversations: ExtendedConversation[];

  constructor(private globalHelper: GlobalHelper,
    private conversationsService: ConversationsService,
    private messagesService: MessagesService,
    private usersService: UsersService) { }

  ngOnInit() {
    this.fetchConversationsData();
  }

  public getConversationPicture(conversationId: string): string {
    const userId = this.globalHelper.localStorageItem<CurrentUser>('currentUser').id;
    const conversation = (this.conversations.filter((cnv) => {
      return cnv.id == conversationId;
    }))[0];

    return conversation.lastMessage.receiverId == userId
      ? conversation.lastMessage.sender?.profilePictureUrl
      : conversation.lastMessage.receiver?.profilePictureUrl;
  }

  public getConversationName(conversationId: string): string {
    const userId = this.globalHelper.localStorageItem<CurrentUser>('currentUser').id;
    const conversation = (this.conversations.filter((cnv) => {
      return cnv.id == conversationId;
    }))[0];

    return conversation.lastMessage.receiverId == userId
      ? conversation.lastMessage.sender?.fullName
      : conversation.lastMessage.receiver?.fullName;
  }

  public getLastMessageSender(conversationId: string): string {
    const userId = this.globalHelper.localStorageItem<CurrentUser>('currentUser').id;
    const conversation = (this.conversations.filter((cnv) => {
      return cnv.id == conversationId;
    }))[0];

    return conversation.lastMessage.receiverId == userId
      ? conversation.lastMessage.sender?.fullName
      : "You"
  }

  public loadConversation(conversationId: string): void {
    this.conversation.conversationId = conversationId;
  }

  private fetchConversationsData(): void {
    this.conversationsService.getConversations(this.globalHelper.localStorageItem<CurrentUser>('currentUser').id).subscribe({
      next: (conversations: Conversation[]) => {
        this.conversations = conversations;
        this.fetchMessages(1, 0);
      }
    });
  }

  private fetchMessages(count: number, page: number): void {
    this.conversations.forEach((cnv: ExtendedConversation, index: number, conversationsArray: ExtendedConversation[]) => {
      this.messagesService.getMessagesFromConversation(cnv.id, count, page).subscribe({
        next: (message: Message[]) => {
          if (message) {
            conversationsArray[index].lastMessage = new FriendlyMessage(message[0]);
            
            this.usersService.getUser(conversationsArray[index].lastMessage.receiverId).subscribe({
              next: (user: User) => {
                conversationsArray[index].lastMessage.receiver = user;
              }
            });
            this.usersService.getUser(conversationsArray[index].lastMessage.senderId).subscribe({
              next: (user: User) => {
                conversationsArray[index].lastMessage.sender = user;
              }
            });
          }
        }
      });
    });
  }
}
