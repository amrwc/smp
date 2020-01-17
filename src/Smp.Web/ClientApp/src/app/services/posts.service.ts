import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreatePostRequest } from '../models/requests/create-post-request';
import { Post } from '../models/post';

@Injectable({
  providedIn: 'root'
})
export class PostsService {

  private httpHeaders = new HttpHeaders();

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    let authToken = "Bearer " + JSON.parse(localStorage.getItem('currentUser')).token;
    this.httpHeaders = this.httpHeaders.set('Authorization', authToken);
  }

  public createPost(createPostReq: CreatePostRequest): Observable<Object> {
    return this.httpClient.post(this.baseUrl + "api/Posts/CreatePost", createPostReq, { headers: this.httpHeaders });
  }

  public getPosts(receiverId: string): Observable<Post[]> {
    return this.httpClient.get<Post[]>(this.baseUrl + "api/Posts/GetPosts/" + receiverId, { headers: this.httpHeaders });
  }
}