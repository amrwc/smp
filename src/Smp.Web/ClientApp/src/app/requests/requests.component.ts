import { Component, OnInit } from '@angular/core';
import { RequestsService } from '../services/requests.service';
import { Request, FriendlyRequest } from '../models/request';
import { GlobalHelper } from '../helpers/global';
import { CurrentUser } from '../models/current-user';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-requests',
  templateUrl: './requests.component.html',
  styleUrls: ['./requests.component.scss']
})
export class RequestsComponent implements OnInit {

  public requests: FriendlyRequest[] = new Array<FriendlyRequest>();

  constructor(private globalHelper: GlobalHelper, private requestsService: RequestsService, private usersService: UsersService) { }

  ngOnInit() {
    this.requestsService.getIncomingRequests(this.globalHelper.localStorageItem<CurrentUser>('currentUser').id).subscribe({
      next: (requests: any) => {
        requests.forEach((request: Request) => {
          this.requests.push(new Request(request).toFriendlyRequest());
        });
        this.fetchUserNames();
      }
    });
  }

  private fetchUserNames(): void {
    this.requests.forEach((request: FriendlyRequest, index: number, requestsArray: FriendlyRequest[]) => {
      this.usersService.getUser(request.receiverId).subscribe({
        next: (user: any) => {
          requestsArray[index].receiverName = user.fullName;
        }
      });
      this.usersService.getUser(request.senderId).subscribe({
        next: (user: any) => {
          requestsArray[index].senderName = user.fullName;
        }
      });
    });
  }
}
