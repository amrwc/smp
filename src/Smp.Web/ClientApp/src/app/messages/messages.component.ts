import { Component, OnInit } from '@angular/core';
import { Conversation } from '../models/conversation';
import { GlobalHelper } from '../helpers/global';
import { ConversationsService } from '../services/conversations.service';
import { MessagesService } from '../services/messages.service';
import { CurrentUser } from '../models/current-user';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {

  public conversations: Conversation[];

  constructor(private globalHelper: GlobalHelper, private conversationsService: ConversationsService, private messagesService: MessagesService) { }

  ngOnInit() {
    this.fetchConversations();
  }

  private fetchConversations(): void {
    this.conversationsService.getConversations(this.globalHelper.localStorageItem<CurrentUser>('currentUser').id).subscribe({
      next: (conversations: any) => {
        this.conversations = conversations;
        
        // use messagesService to get the most recent message in that conversation and display
        // can use message information to know how to order conversations.
        // endpoints on API may still need to be implemented.
      }
    });
  }

}
