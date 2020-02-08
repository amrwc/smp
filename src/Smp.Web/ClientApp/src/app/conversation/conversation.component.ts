import { Component, OnInit, Input } from '@angular/core';
import { ConversationsService } from '../services/conversations.service';
import { MessagesService } from '../services/messages.service';
import { UsersService } from '../services/users.service';
import { Message } from '../models/message';

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.scss']
})
export class ConversationComponent implements OnInit {

  private _conversationId: string;

  @Input() set conversationId(id: string) {
    this._conversationId = id;
    this.getMessages();
  }

  get conversationId() {
    return this._conversationId;
  }

  public messages: Message[];

  constructor(private conversationsService: ConversationsService,
    private messagesService: MessagesService,
    private usersService: UsersService) { }

  ngOnInit() {
  }

  private getMessages(): void {
    alert("getting messages for " + this._conversationId);
  }
}
