import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';

import { CreateRequestRequest } from '../models/requests/create-request-request';
import { CurrentUser } from '../models/current-user';
import { FeedComponent } from '../feed/feed.component';
import { GlobalHelper } from '../helpers/global';
import { RelationshipType } from '../models/relationship-type.enum';
import { RelationshipsService } from '../services/relationships.service';
import { RequestType } from '../models/request-type.enum';
import { RequestsService } from '../services/requests.service';
import { User } from '../models/user';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  @ViewChild(FeedComponent)
  feedComponent: FeedComponent;

  private userId: string;
  public user: User;

  public showAddFriendButton: boolean = true;
  public requestPending: boolean = false;
  public friends: boolean = false;

  constructor(
    private usersService: UsersService,
    private requestsService: RequestsService,
    private relationshipsService: RelationshipsService,
    private globalHelper: GlobalHelper,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    const visitingUserId = this.globalHelper.localStorageItem<CurrentUser>('currentUser').id;
    this.userId = this.route.snapshot.paramMap.get('id') ?? visitingUserId;
    this.showAddFriendButton = this.userId != visitingUserId;

    if (this.showAddFriendButton) {
      this.requestsService.getRequest(visitingUserId, this.userId, RequestType.Friend).subscribe({
        next: () => {
          this.showAddFriendButton = false;
          this.requestPending = true;
        },
        error: (error: any) => {
          if (error.status === 404) {
            this.relationshipsService.getRelationship(this.userId, visitingUserId, RelationshipType.Friend).subscribe({
              next: () => {
                this.showAddFriendButton = false;
                this.friends = true;
              },
              error: (error: any) => {},
            });
          }
        },
      });
    }

    this.usersService.getUser(this.userId).subscribe({
      next: (result: any) => {
        this.user = result;
      },
      error: (error: any) => {
        console.error(error);
      },
    });
  }

  public updatePosts(): void {
    this.feedComponent.getPosts();
  }

  public addFriend(): void {
    const req = new CreateRequestRequest(
      this.globalHelper.localStorageItem<CurrentUser>('currentUser').id,
      this.userId,
      RequestType.Friend
    );
    this.requestsService.sendRequest(req).subscribe({
      next: () => {
        this.showAddFriendButton = false;
        this.requestPending = true;
      },
    });
  }
}
