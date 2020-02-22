import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreatePostRequest } from '../models/requests/create-post-request';
import { Post } from '../models/post';
import { GlobalHelper } from '../helpers/global';

@Injectable({
  providedIn: 'root'
})
export class PostsService {

  constructor(private httpClient: HttpClient, private globalHelper: GlobalHelper, @Inject('BASE_URL') private baseUrl: string) {
  }

  public createPost(createPostReq: CreatePostRequest): Observable<Object> {
    return this.httpClient.post(`${this.baseUrl}api/Posts/CreatePost`, createPostReq, { headers: this.globalHelper.getAuthHeader() });
  }

  public getPosts(receiverId: string): Observable<Post[]> {
    return this.httpClient.get<Post[]>(`${this.baseUrl}api/Posts/GetPosts/${receiverId}`, { headers: this.globalHelper.getAuthHeader() });
  }
}