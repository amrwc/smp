import { Component, OnInit, NgZone, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from '../models/user';
import { UsersService } from '../services/users.service';
import { FeedComponent } from '../feed/feed.component';
import { GlobalHelper } from '../helpers/global';
import { CurrentUser } from '../models/current-user';
import { RequestsService } from '../services/requests.service';
import { CreateRequestRequest } from '../models/requests/create-request-request';
import { RequestType } from '../models/request-type.enum';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  @ViewChild(FeedComponent, { static: false }) feedComponent: FeedComponent;

  private userId: string;
  private user: User;

  private showAddFriendButton: boolean = true;
  private requestPending: boolean = false;

  constructor(
    private usersService: UsersService,
    private requestsService: RequestsService,
    private globalHelper: GlobalHelper,
    private route: ActivatedRoute
  ) {  }

  ngOnInit() {
    this.userId = this.route.snapshot.paramMap.get('id');
    let visitingUserId = this.globalHelper.localStorageItem<CurrentUser>('currentUser').id;
    this.showAddFriendButton = this.userId != visitingUserId;

    if (this.showAddFriendButton) {
      this.requestsService.getRequest(visitingUserId, this.userId, RequestType.Friend).subscribe(
        () => {
          this.showAddFriendButton = false;
          this.requestPending = true;
        }
      );
    }

    this.usersService.getUser(this.userId)
      .subscribe(
        result => {
          this.user = result;
        },
        error => {
          console.error(error);
        }
      );
  }

  public updatePosts(): void {
    this.feedComponent.getPosts();
  }

  public addFriend(): void {
    let req = new CreateRequestRequest(this.globalHelper.localStorageItem<CurrentUser>('currentUser').id, this.userId, RequestType.Friend);
    this.requestsService.sendRequest(req).subscribe(
      () => {
        this.showAddFriendButton = false;
        this.requestPending = true;
      }
    );
  }
}
