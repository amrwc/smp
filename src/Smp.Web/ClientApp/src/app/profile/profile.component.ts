import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../models/user';
import { UsersService } from '../services/users.service';
import { GlobalHelper } from '../helpers/global';
import { CreatePostComponent } from '../create-post/create-post.component';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  private userId: string;
  private user: User;

  constructor(
    private userService: UsersService,
    private route: ActivatedRoute
  ) {  }

  ngOnInit() {
    debugger;
    this.userId = this.route.snapshot.paramMap.get('id');

    this.userService.getUser(this.userId)
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
    alert("UPDATE POSTS");
  }
}
