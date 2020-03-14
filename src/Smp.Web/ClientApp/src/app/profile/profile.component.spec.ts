import { ActivatedRoute } from '@angular/router';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { of, throwError } from 'rxjs';
import { CreateRequestRequest } from '../models/requests/create-request-request';
import { FeedComponent } from '../feed/feed.component';
import { PostsService } from '../services/posts.service';
import { ProfileComponent } from './profile.component';
import { RelationshipsService } from '../services/relationships.service';
import { RequestsService } from '../services/requests.service';
import { User } from '../models/user';
import { UsersService } from '../services/users.service';
import { RequestType } from '../models/request-type.enum';
import { BrowserModule } from '@angular/platform-browser';

fdescribe('ProfileComponent', () => {
  const userMock: User = {
    id: 'aknfdkanjdf-123213-asdfdfas',
    fullName: 'bob',
    email: 'my@email.com',
    profilePictureUrl: 'example.com/pics/pic.png',
  } as User;
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;

  beforeEach(() => {
    localStorage.setItem('currentUser', '{ "id": "id" }');
    TestBed.configureTestingModule({
      declarations: [ProfileComponent],
      imports: [HttpClientTestingModule, RouterTestingModule],
      providers: [
        { provide: 'BASE_URL', useValue: 'https://www.smp.org/' },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: {
                get: anyArg => 'bob',
              },
            },
          },
        },
      ],
    }).compileComponents();
    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    localStorage.removeItem('currentUser');
  });

  describe('ngOnInit', () => {
    let getRequestsServiceGetRequestArgs: Array<any>;
    let getRelationshipsServiceGetRelationshipArgs: Array<any>;
    let requestsServiceGetRequestSpy: jasmine.Spy;
    let relationshipsServiceGetRelationshipSpy: jasmine.Spy;

    beforeEach(() => {
      const currentUserId = JSON.parse(localStorage.getItem('currentUser')).id;
      getRequestsServiceGetRequestArgs = [currentUserId, 'bob', RequestType.Friend];
      getRelationshipsServiceGetRelationshipArgs = ['bob', currentUserId, RequestType.Friend];
      requestsServiceGetRequestSpy = spyOn(TestBed.get(RequestsService), 'getRequest');
      relationshipsServiceGetRelationshipSpy = spyOn(TestBed.get(RelationshipsService), 'getRelationship');
    });

    describe("should have shown 'Add Friend' button", () => {
      afterEach(() => {
        expect(component.showAddFriendButton).toBeTruthy();
        expect(component.friends).toBeFalsy();
        expect(component.requestPending).toBeFalsy();
        expect(TestBed.get(RequestsService).getRequest).toHaveBeenCalledTimes(1);
        expect(TestBed.get(RequestsService).getRequest).toHaveBeenCalledWith(...getRequestsServiceGetRequestArgs);
      });

      it('when RequestsService.getRequest() returned error other than 404', () => {
        requestsServiceGetRequestSpy.and.returnValue(throwError({ status: 321 }));
        component.ngOnInit();
      });

      it('when RequestsService.getRequest() returned 404 error', () => {
        requestsServiceGetRequestSpy.and.returnValue(throwError({ status: 404 }));
        relationshipsServiceGetRelationshipSpy.and.returnValue(throwError(new Error()));
        component.ngOnInit();
        expect(TestBed.get(RelationshipsService).getRelationship).toHaveBeenCalledTimes(1);
        expect(TestBed.get(RelationshipsService).getRelationship).toHaveBeenCalledWith(
          ...getRelationshipsServiceGetRelationshipArgs
        );
      });
    });

    describe("should not have shown 'Add Friend' button", () => {
      afterEach(() => {
        expect(component.showAddFriendButton).toBeFalsy();
      });

      it('when the user visited their own profile', () => {
        localStorage.removeItem('currentUser');
        localStorage.setItem('currentUser', '{ "id": "bob" }');
        component.ngOnInit();
      });

      describe('when RequestsService.getRequest()', () => {
        afterEach(() => {
          expect(TestBed.get(RequestsService).getRequest).toHaveBeenCalledTimes(1);
          expect(TestBed.get(RequestsService).getRequest).toHaveBeenCalledWith(...getRequestsServiceGetRequestArgs);
        });

        it('found a pending friend request', () => {
          requestsServiceGetRequestSpy.and.returnValue(of({}));
          component.ngOnInit();
          expect(component.friends).toBeFalsy();
          expect(component.requestPending).toBeTruthy();
        });

        it('returned 404 error and RelationshipsService.getRelationship() found a relationship', () => {
          requestsServiceGetRequestSpy.and.returnValue(throwError({ status: 404 }));
          relationshipsServiceGetRelationshipSpy.and.returnValue(of({}));
          component.ngOnInit();
          expect(component.friends).toBeTruthy();
          expect(component.requestPending).toBeFalsy();
          expect(TestBed.get(RelationshipsService).getRelationship).toHaveBeenCalledTimes(1);
          expect(TestBed.get(RelationshipsService).getRelationship).toHaveBeenCalledWith(
            ...getRelationshipsServiceGetRelationshipArgs
          );
        });
      });

      describe('UsersService.getUser()', () => {
        let usersServiceGetUserSpy: jasmine.Spy;

        beforeEach(() => {
          usersServiceGetUserSpy = spyOn(TestBed.get(UsersService), 'getUser');
          // Skip the logic around the 'Add Friend' button
          localStorage.removeItem('currentUser');
          localStorage.setItem('currentUser', '{ "id": "bob" }');
        });

        afterEach(() => {
          expect(TestBed.get(UsersService).getUser).toHaveBeenCalledTimes(1);
          expect(TestBed.get(UsersService).getUser).toHaveBeenCalledWith('bob');
        });

        it("should have set the 'user' field correctly", () => {
          usersServiceGetUserSpy.and.callFake(userId => of(userMock));
          component.ngOnInit();
          expect(component.user).toEqual(userMock);
        });

        it("should not have set the 'user' field on error", () => {
          usersServiceGetUserSpy.and.callFake(userId => throwError(new Error()));
          component.ngOnInit();
          expect(component.user).toBeUndefined();
        });
      });
    });
  });

  describe('updatePosts()', () => {
    it('should have called FeedComponent.getPosts()', () => {
      component.user = userMock;
      component.feedComponent = new FeedComponent(TestBed.get(PostsService));
      spyOn(component.feedComponent, 'getPosts');
      component.updatePosts();
      expect(component.feedComponent.getPosts).toHaveBeenCalledTimes(1);
      expect(component.feedComponent.getPosts).toHaveBeenCalledWith();
    });
  });

  describe('addFriend()', () => {
    it('should have sent a friend request', () => {
      spyOn(TestBed.get(RequestsService), 'sendRequest').and.returnValue(of({}));
      component.addFriend();
      expect(component.showAddFriendButton).toBeFalsy();
      expect(component.requestPending).toBeTruthy();
      expect(TestBed.get(RequestsService).sendRequest).toHaveBeenCalledTimes(1);
      expect(TestBed.get(RequestsService).sendRequest).toHaveBeenCalledWith(
        new CreateRequestRequest('id', 'bob', RequestType.Friend)
      );
    });
  });
});
