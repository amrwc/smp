import { Component, OnInit, Input } from '@angular/core';
import { Post } from '../models/post';
import { PostsService } from '../services/posts.service';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.scss']
})
export class FeedComponent implements OnInit {

  @Input() receiverId: string;

  public posts: Post[];

  constructor(private postsService: PostsService) { }

  ngOnInit() {
    this.getPosts();
  }

  public getPosts(): void {
    if (this.receiverId) {
      this.postsService.getPosts(this.receiverId).subscribe({
        next: (posts: any) => {
          this.posts = posts;
        }
      });
    }
  }
}
