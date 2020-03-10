import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePostComponent } from './create-post.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { GlobalHelper } from '../helpers/global';
import { PostsService } from '../services/posts.service';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { throwError, of } from 'rxjs';
import { CreatePostRequest } from '../models/requests/create-post-request';

describe('CreatePostComponent', () => {
  let component: CreatePostComponent;
  let fixture: ComponentFixture<CreatePostComponent>;

  beforeAll(() => {
    localStorage.setItem('currentUser', '{ "id": "id", "token": "token" }');
  });

  afterAll(() => {
    localStorage.removeItem('currentUser');
  });

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CreatePostComponent],
      imports: [HttpClientTestingModule, FormsModule, BrowserAnimationsModule],
      providers: [GlobalHelper, PostsService, { provide: 'BASE_URL', useValue: "https://www.smp.org/" }]
    }).compileComponents();

    fixture = TestBed.createComponent(CreatePostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  describe('createPost', () => {
    beforeEach(() => {
      component.postToId = postToId;
      component.createPostRequest.content = 'content';
    });

    let eventEmitterEmitSpy: jasmine.Spy;
    let postsServiceCreatePostSpy: jasmine.Spy;
    const postToId = 'postToId';

    const postReq = new CreatePostRequest();
    postReq.content = 'content';
    postReq.senderId = 'id';
    postReq.receiverId = postToId;

    describe('when it completes successfully', () => {
      beforeEach(() => {
        postsServiceCreatePostSpy = spyOn(TestBed.get(PostsService), 'createPost')
          .and.returnValue(of('x'));
        eventEmitterEmitSpy = spyOn(component.postCreated, 'emit');
      });

      it('should have emitted an event', () => {
        component.createPost();

        expect(component.postCreated.emit).toHaveBeenCalledTimes(1);
      });

      it('should have called PostsService createPost correctly', () => {
        component.createPost();

        expect(postsServiceCreatePostSpy.calls.count()).toEqual(1);
        expect(postsServiceCreatePostSpy.calls.argsFor(0)).toEqual([
          postReq
        ]);
      });
    });

    describe('when an error gets returned', () => {
      beforeEach(() => {
        postsServiceCreatePostSpy = spyOn(TestBed.get(PostsService), 'createPost').and.returnValue(
          throwError({
            error: 'error-message'
          })
        );
      });

      it('should have called PostsService createPost correctly', () => {
        component.createPost();

        expect(postsServiceCreatePostSpy.calls.count()).toEqual(1);
        expect(postsServiceCreatePostSpy.calls.argsFor(0)).toEqual([
          postReq
        ]);
      });
      
      it('should set the error message correctly', () => {
        component.createPost();

        expect(component.errorMessage).toEqual('error-message');
      });
    });
  });
});
