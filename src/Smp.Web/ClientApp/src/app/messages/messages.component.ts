import { Component, OnInit } from '@angular/core';
import { Conversation } from '../models/conversation';
import { GlobalHelper } from '../helpers/global';
import { ConversationsService } from '../services/conversations.service';
import { MessagesService } from '../services/messages.service';

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
    // TODO: call conversationsService method to get conversations for current user.
    // then use messagesService to get the most recent message in that conversation and display
    // endpoints on API may still need to be implemented.
  }

}
