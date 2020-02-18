import { Component, OnInit } from '@angular/core';
import { CreateMessageRequest } from '../models/requests/create-message-request';
import { User } from '../models/user';
import { GlobalHelper } from '../helpers/global';
import { CurrentUser } from '../models/current-user';

@Component({
  selector: 'app-message-composer',
  templateUrl: './message-composer.component.html',
  styleUrls: ['./message-composer.component.scss']
})
export class MessageComposerComponent implements OnInit {

  public messageRequest: CreateMessageRequest = new CreateMessageRequest();
  public friends: User[] = [ { id: "1", fullName: "john", email: "", profilePictureUrl: "" },
    { id: "2", fullName: "jessie", email : "", profilePictureUrl: "" } ];

  constructor(private globalHelper: GlobalHelper) { }

  ngOnInit(): void {
    this.messageRequest.senderId = this.globalHelper.localStorageItem<CurrentUser>('currentUser').id;
  }

  public sendMessage() {
    this.messageRequest.receiverId = this.messageRequest.receiver.id;
    debugger;
    //create conversation?
    //send message
  }

  public displayFriend(user: User) {
    return user ? user.fullName : user;
  }
}
