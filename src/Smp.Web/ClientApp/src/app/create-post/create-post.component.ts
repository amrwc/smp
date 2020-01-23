import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { PostsService } from '../services/posts.service';
import { CreatePostRequest } from '../models/requests/create-post-request';
import { GlobalHelper } from '../helpers/global';
import { CurrentUser } from '../models/current-user';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.scss']
})
export class CreatePostComponent implements OnInit {

  @Input() postToId: string;

  public createPostRequest: CreatePostRequest = new CreatePostRequest();
  public loading: boolean = false;
  public postSuccessful: boolean = true;
  public errorMessage: string;

  @Output() postCreated: EventEmitter<any> = new EventEmitter();

  constructor(private global: GlobalHelper, private postsService: PostsService) { }

  ngOnInit() {
  }
 
  public createPost(): void {
    this.loading = true;
    this.createPostRequest.senderId = this.global.localStorageItem<CurrentUser>('currentUser').id;
    this.createPostRequest.receiverId = this.postToId;
    this.postsService.createPost(this.createPostRequest).subscribe({
      next: () => {
        this.loading = false;
        this.postCreated.emit(null);
      },
      error: (error: any) => {
        this.loading = false;
        this.errorMessage = error.error;
        //IF ERROR 401. REDIRECT TO HOME BECAUSE FAILED AUTHORIZATION
      }
    });
  }
}
