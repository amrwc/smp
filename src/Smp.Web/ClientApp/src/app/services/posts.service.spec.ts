import { TestBed } from '@angular/core/testing';

import { PostsService } from './posts.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { CreatePostRequest } from '../models/requests/create-post-request';
import { of } from 'rxjs';
import { Post } from '../models/post';

describe('PostsService', () => {
  const baseUrl = 'https://www.smp.org/';

  let service: PostsService;

  let httpClientGetSpy: jasmine.Spy;
  let httpClientPostSpy: jasmine.Spy;

  const authHeaders = new HttpHeaders().set('Authorization', 'Bearer token');

  beforeAll(() => {
    localStorage.setItem('currentUser', '{ "id": "id", "token": "token" }');
  });

  afterAll(() => {
    localStorage.removeItem('currentUser');
  });

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [{ provide: 'BASE_URL', useValue: "https://www.smp.org/" }]
    });

    httpClientPostSpy = spyOn(TestBed.get(HttpClient), 'post');

    service = TestBed.get(PostsService);
  });

  describe('createPost', () => {
    const postReq = {
      senderId: 'senderId'
    } as CreatePostRequest;

    it('should have called HttpClient post correctly', () => {
      service.createPost(postReq);

      expect(httpClientPostSpy.calls.count()).toEqual(1);
      expect(httpClientPostSpy.calls.argsFor(0)).toEqual([
        `${baseUrl}api/Posts/CreatePost`,
        postReq,
        { headers: authHeaders }
      ]);
    });
  });

  describe('getPosts', () => {
    beforeEach(() => {
      httpClientGetSpy = spyOn(TestBed.get(HttpClient), 'get')
        .and.returnValue(of(expectedPosts));
    });

    const receiverId = 'receiverId';
    const expectedPosts: Post[] = [
      { content: 'content-1' } as Post,
      { content: 'content-2' } as Post
    ]

    it('should have returned the expected value', () => {
      const postsObserv = service.getPosts(receiverId);

      postsObserv.subscribe({
        next: ((posts: Post[]) => {
          expect(posts).toEqual(expectedPosts);
        })
      });
    });

    it('should have called HttpClient get correctly', () => {
      service.getPosts(receiverId);

      expect(httpClientGetSpy.calls.count()).toEqual(1);
      expect(httpClientGetSpy.calls.argsFor(0)).toEqual([
        `${baseUrl}api/Posts/GetPosts/${receiverId}`,
        { headers: authHeaders }
      ]);
    });
  });
});
