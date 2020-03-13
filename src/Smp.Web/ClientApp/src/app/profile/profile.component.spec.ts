import { ActivatedRoute } from '@angular/router';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { of, throwError } from 'rxjs';
import { GlobalHelper } from '../helpers/global';
import { ProfileComponent } from './profile.component';
import { RelationshipsService } from '../services/relationships.service';
import { RequestsService } from '../services/requests.service';
import { User } from '../models/user';
import { UsersService } from '../services/users.service';
import { RequestType } from '../models/request-type.enum';

describe('ProfileComponent', () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;

  beforeEach(() => {
    localStorage.setItem('currentUser', '{ "id": "id" }');
    TestBed.configureTestingModule({
      declarations: [ProfileComponent],
      imports: [HttpClientTestingModule, RouterTestingModule],
      providers: [
        UsersService,
        RequestsService,
        RelationshipsService,
        GlobalHelper,
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
    let requestsServiceGetRequestSpy: jasmine.Spy;
    let relationshipsServiceGetRelationshipSpy: jasmine.Spy;

    beforeEach(() => {
      requestsServiceGetRequestSpy = spyOn(TestBed.get(RequestsService), 'getRequest');
      relationshipsServiceGetRelationshipSpy = spyOn(TestBed.get(RelationshipsService), 'getRelationship');
    });

    describe("should show 'Add Friend' button", () => {
      afterEach(() => {
        expect(component.showAddFriendButton).toBeTruthy();
        expect(component.friends).toBeFalsy();
        expect(component.requestPending).toBeFalsy();
        expect(requestsServiceGetRequestSpy.calls.count()).toEqual(1);
        expect(requestsServiceGetRequestSpy.calls.argsFor(0)).toEqual(getRequestsServiceGetRequestArgs());
      });

      it('when RequestsService.getRequest() returns error other than 404', () => {
        requestsServiceGetRequestSpy.and.returnValue(throwError({ status: 400 }));
        component.ngOnInit();
      });

      it('when RequestsService.getRequest() returns 404 error', () => {
        requestsServiceGetRequestSpy.and.returnValue(throwError({ status: 404 }));
        relationshipsServiceGetRelationshipSpy.and.returnValue(throwError(new Error()));
        component.ngOnInit();
        expect(relationshipsServiceGetRelationshipSpy.calls.count()).toEqual(1);
        expect(relationshipsServiceGetRelationshipSpy.calls.argsFor(0)).toEqual(
          getRelationshipsServiceGetRelationshipArgs()
        );
      });
    });

    describe("should not show 'Add Friend' button", () => {
      afterEach(() => {
        expect(component.showAddFriendButton).toBeFalsy();
      });

      it('when the user visits their own profile', () => {
        localStorage.removeItem('currentUser');
        localStorage.setItem('currentUser', '{ "id": "bob" }');
        component.ngOnInit();
      });

      describe('when RequestsService.getRequest()', () => {
        it('finds a pending friend request', () => {
          requestsServiceGetRequestSpy.and.returnValue(of({}));
          component.ngOnInit();
          expect(component.friends).toBeFalsy();
          expect(component.requestPending).toBeTruthy();
          expect(requestsServiceGetRequestSpy.calls.count()).toEqual(1);
          expect(requestsServiceGetRequestSpy.calls.argsFor(0)).toEqual(getRequestsServiceGetRequestArgs());
        });

        it('returns 404 error and RelationshipsService.getRelationship() finds a relationship', () => {
          requestsServiceGetRequestSpy.and.returnValue(throwError({ status: 404 }));
          relationshipsServiceGetRelationshipSpy.and.returnValue(of({}));
          component.ngOnInit();
          expect(component.friends).toBeTruthy();
          expect(component.requestPending).toBeFalsy();
          expect(requestsServiceGetRequestSpy.calls.count()).toEqual(1);
          expect(requestsServiceGetRequestSpy.calls.argsFor(0)).toEqual(getRequestsServiceGetRequestArgs());
          expect(relationshipsServiceGetRelationshipSpy.calls.count()).toEqual(1);
          expect(relationshipsServiceGetRelationshipSpy.calls.argsFor(0)).toEqual(
            getRelationshipsServiceGetRelationshipArgs()
          );
        });
      });

      describe('UsersService.getUser()', () => {
        const userMock: User = {
          id: 'aknfdkanjdf-123213-asdfdfas',
          fullName: 'bob',
          email: 'my@email.com',
          profilePictureUrl: 'example.com/pics/pic.png',
        } as User;
        let usersServiceGetUserSpy: jasmine.Spy;

        beforeEach(() => {
          usersServiceGetUserSpy = spyOn(TestBed.get(UsersService), 'getUser');
          // Skip the logic around the 'Add Friend' button
          localStorage.removeItem('currentUser');
          localStorage.setItem('currentUser', '{ "id": "bob" }');
        });

        afterEach(() => {
          expect(usersServiceGetUserSpy.calls.count()).toEqual(1);
          expect(usersServiceGetUserSpy.calls.argsFor(0)).toEqual(['bob']);
        });

        it("should set the 'user' field correctly", () => {
          usersServiceGetUserSpy.and.returnValue(of(userMock));
          component.ngOnInit();
          expect(component.user).toEqual(userMock);
        });

        it("should not set the 'user' field on error", () => {
          usersServiceGetUserSpy.and.returnValue(throwError(new Error()));
          component.ngOnInit();
          expect(component.user).toBeUndefined();
        });
      });
    });
  });
});

function getRequestsServiceGetRequestArgs(): Array<any> {
  return [JSON.parse(localStorage.getItem('currentUser')).id, 'bob', RequestType.Friend];
}

function getRelationshipsServiceGetRelationshipArgs(): Array<any> {
  return ['bob', JSON.parse(localStorage.getItem('currentUser')).id, RequestType.Friend];
}
