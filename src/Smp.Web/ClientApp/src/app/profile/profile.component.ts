import { Component, OnInit, NgZone, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from '../models/user';
import { UsersService } from '../services/users.service';
import { FeedComponent } from '../feed/feed.component';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  @ViewChild(FeedComponent, { static: false }) feedComponent: FeedComponent;

  private userId: string;
  private user: User;

  constructor(
    private usersService: UsersService,
    private route: ActivatedRoute
  ) {  }

  ngOnInit() {
    this.userId = this.route.snapshot.paramMap.get('id');

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
}
