import { Component, OnInit } from '@angular/core';
import { Conversation, ExtendedConversation } from '../models/conversation';
import { GlobalHelper } from '../helpers/global';
import { ConversationsService } from '../services/conversations.service';
import { MessagesService } from '../services/messages.service';
import { CurrentUser } from '../models/current-user';
import { Message, FriendlyMessage } from '../models/message';
import { UsersService } from '../services/users.service';
import { User } from '../models/user';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {

  public conversations: ExtendedConversation[];

  constructor(private globalHelper: GlobalHelper,
    private conversationsService: ConversationsService,
    private messagesService: MessagesService,
    private usersService: UsersService) { }

  ngOnInit() {
    this.fetchConversationsData();
  }

  private fetchConversationsData(): void {
    this.conversationsService.getConversations(this.globalHelper.localStorageItem<CurrentUser>('currentUser').id).subscribe({
      next: (conversations: Conversation[]) => {
        this.conversations = conversations;
        this.fetchLastMessages();
      }
    });
  }

  private fetchLastMessages(): void {
    this.conversations.forEach((cnv: ExtendedConversation, index: number, conversationsArray: ExtendedConversation[]) => {
      this.messagesService.getMessagesFromConversation(cnv.id, 1, 0).subscribe({
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
