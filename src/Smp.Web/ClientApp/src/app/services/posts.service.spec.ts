import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { of, Observable } from 'rxjs';
import { CreatePostRequest } from '../models/requests/create-post-request';
import { Post } from '../models/post';
import { PostsService } from './posts.service';

describe('PostsService', () => {
  const authHeaders: HttpHeaders = new HttpHeaders().set('Authorization', 'Bearer token');
  const baseUrl: string = 'https://www.smp.org/';
  let service: PostsService;

  beforeAll(() => {
    localStorage.setItem('currentUser', '{ "id": "id", "token": "token" }');
  });

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [{ provide: 'BASE_URL', useValue: baseUrl }]
    });
    service = TestBed.get(PostsService);
    spyOn(TestBed.get(HttpClient), 'post');
  });

  afterAll(() => {
    localStorage.removeItem('currentUser');
  });

  describe('createPost()', () => {
    const postReq: CreatePostRequest = { senderId: 'senderId' } as CreatePostRequest;

    it('should have called HttpClient.post() correctly', () => {
      service.createPost(postReq);
      expect(TestBed.get(HttpClient).post).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).post).toHaveBeenCalledWith(
        `${baseUrl}api/Posts/CreatePost`,
        postReq,
        { headers: authHeaders }
      );
    });
  });

  describe('getPosts()', () => {
    const receiverId: string = 'receiverId';
    const expectedPosts: Post[] = [{ content: 'content-1' }, { content: 'content-2' }] as Post[];

    beforeEach(() => {
      spyOn(TestBed.get(HttpClient), 'get').and.returnValue(of(expectedPosts));
    });

    it('should have returned the expected value', () => {
      const postsObserv: Observable<Post[]> = service.getPosts(receiverId);
      postsObserv.subscribe({
        next: ((posts: Post[]) => {
          expect(posts).toEqual(expectedPosts);
        })
      });
    });

    it('should have called HttpClient.get() correctly', () => {
      service.getPosts(receiverId);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledWith(
        `${baseUrl}api/Posts/GetPosts/${receiverId}`,
        { headers: authHeaders }
      );
    });
  });
});
