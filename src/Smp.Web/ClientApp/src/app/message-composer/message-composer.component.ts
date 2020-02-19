import { Component, OnInit } from '@angular/core';
import { User } from '../models/user';
import { GlobalHelper } from '../helpers/global';
import { CurrentUser } from '../models/current-user';
import { ConversationsService } from '../services/conversations.service';
import { CreateConversationRequest } from '../models/requests/create-conversation-request';

@Component({
  selector: 'app-message-composer',
  templateUrl: './message-composer.component.html',
  styleUrls: ['./message-composer.component.scss']
})
export class MessageComposerComponent implements OnInit {

  public createConversationRequest: CreateConversationRequest = new CreateConversationRequest();
  public friends: User[] = [ { id: "1", fullName: "john", email: "", profilePictureUrl: "" },
    { id: "2", fullName: "jessie", email : "", profilePictureUrl: "" } ];

  constructor(private globalHelper: GlobalHelper, private conversationsService: ConversationsService) { }

  ngOnInit(): void {
    this.createConversationRequest.senderId = this.globalHelper.localStorageItem<CurrentUser>('currentUser').id;
  }

  public sendMessage() {
    this.conversationsService.startConversation(this.createConversationRequest).subscribe({
      next: () => {
        
      }
    });
  }

  public displayFriend(userId: string) {
    return this.friends.find(friend => friend.id === userId).fullName;
  }
}
