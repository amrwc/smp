import { Component, OnInit } from '@angular/core';
import { Conversation, ExtendedConversation } from '../models/conversation';
import { GlobalHelper } from '../helpers/global';
import { ConversationsService } from '../services/conversations.service';
import { MessagesService } from '../services/messages.service';
import { CurrentUser } from '../models/current-user';
import { Message } from '../models/message';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {

  public conversations: ExtendedConversation[];

  constructor(private globalHelper: GlobalHelper, private conversationsService: ConversationsService, private messagesService: MessagesService) { }

  ngOnInit() {
    this.fetchConversationsData();
  }

  private fetchConversationsData(): void {
    this.conversationsService.getConversations(this.globalHelper.localStorageItem<CurrentUser>('currentUser').id).subscribe({
      next: (conversations: Conversation[]) => {
        this.conversations = conversations;
        
        this.conversations.forEach((cnv: ExtendedConversation, index: number, conversationsArray: ExtendedConversation[]) => {
          this.messagesService.getMessagesFromConversation(cnv.id, 1, 0).subscribe({
            next: (message: Message[]) => {
              if (message) {
                conversationsArray[index].lastMessage = message[0];
              }
            }
          });
        });
        // use messagesService to get the most recent message in that conversation and display
        // can use message information to know how to order conversations.
        // endpoints on API may still need to be implemented.
      }
    });
  }

}
