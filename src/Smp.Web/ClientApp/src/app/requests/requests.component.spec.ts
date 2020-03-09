import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { of, throwError } from 'rxjs';
import { FriendlyRequest, Request } from '../models/request';
import { GlobalHelper } from '../helpers/global';
import { RequestType } from '../models/request-type.enum';
import { RequestsComponent } from './requests.component';
import { RequestsService } from '../services/requests.service';
import { User } from '../models/user';
import { UsersService } from '../services/users.service';

describe('RequestsComponent', () => {
  const req: Request = {
    receiverId: '12308n-98afsd',
    senderId: '09xm1-sf9m0d',
    createdAt: new Date(),
    requestType: RequestType.Friend,
  } as Request;
  const friendlyRequest: FriendlyRequest = {
    ...req,
  } as FriendlyRequest;
  let component: RequestsComponent;
  let fixture: ComponentFixture<RequestsComponent>;
  let requestsServiceGetIncomingRequestsSpy: jasmine.Spy;

  beforeEach(() => {
    localStorage.setItem('currentUser', '{ "id": "id" }');
    TestBed.configureTestingModule({
      declarations: [RequestsComponent],
      imports: [HttpClientTestingModule],
      providers: [
        GlobalHelper,
        RequestsService,
        UsersService,
        { provide: 'BASE_URL', useValue: 'https://www.smp.org/' },
      ],
    }).compileComponents();
    fixture = TestBed.createComponent(RequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    requestsServiceGetIncomingRequestsSpy = spyOn(TestBed.get(RequestsService), 'getIncomingRequests');
  });

  afterEach(() => {
    localStorage.removeItem('currentUser');
  });

  describe('ngOnInit', () => {
    it('should call getRequests', () => {
      requestsServiceGetIncomingRequestsSpy.and.returnValue(of([]));
      component.ngOnInit();
      expect(requestsServiceGetIncomingRequestsSpy.calls.count()).toEqual(1);
    });
  });

  describe('answerRequest', () => {
    let requestsServiceAcceptRequestSpy: jasmine.Spy;
    let requestsServiceDeclineRequestSpy: jasmine.Spy;

    beforeEach(() => {
      requestsServiceAcceptRequestSpy = spyOn(TestBed.get(RequestsService), 'acceptRequest');
      requestsServiceDeclineRequestSpy = spyOn(TestBed.get(RequestsService), 'declineRequest');
    });

    it('should not have called getRequest when answer is true and on API error', () => {
      requestsServiceAcceptRequestSpy.and.returnValue(throwError(new Error()));
      component.answerRequest(friendlyRequest, true);
      expect(requestsServiceAcceptRequestSpy.calls.count()).toEqual(1);
      expect(requestsServiceAcceptRequestSpy.calls.argsFor(0)).toEqual([friendlyRequest]);
      expect(requestsServiceGetIncomingRequestsSpy.calls.count()).toEqual(0);
    });

    it('should not have called getRequest when answer is false and on API error', () => {
      requestsServiceDeclineRequestSpy.and.returnValue(throwError(new Error()));
      component.answerRequest(friendlyRequest, false);
      expect(requestsServiceDeclineRequestSpy.calls.count()).toEqual(1);
      expect(requestsServiceDeclineRequestSpy.calls.argsFor(0)).toEqual([friendlyRequest]);
      expect(requestsServiceGetIncomingRequestsSpy.calls.count()).toEqual(0);
    });

    it('should have called getRequest when answer is true', () => {
      requestsServiceAcceptRequestSpy.and.returnValue(of({}));
      requestsServiceGetIncomingRequestsSpy.and.returnValue(of([]));
      component.answerRequest(friendlyRequest, true);
      expect(requestsServiceAcceptRequestSpy.calls.count()).toEqual(1);
      expect(requestsServiceAcceptRequestSpy.calls.argsFor(0)).toEqual([friendlyRequest]);
      expect(requestsServiceGetIncomingRequestsSpy.calls.count()).toEqual(1);
    });

    it('should have called getRequest when answer is false', () => {
      requestsServiceDeclineRequestSpy.and.returnValue(of({}));
      requestsServiceGetIncomingRequestsSpy.and.returnValue(of([]));
      component.answerRequest(friendlyRequest, false);
      expect(requestsServiceDeclineRequestSpy.calls.count()).toEqual(1);
      expect(requestsServiceDeclineRequestSpy.calls.argsFor(0)).toEqual([friendlyRequest]);
      expect(requestsServiceGetIncomingRequestsSpy.calls.count()).toEqual(1);
    });
  });

  describe('getRequests', () => {
    const friendlyReq = new Request(req).toFriendlyRequest();

    it('should have called RequestsService.getIncomingRequests and set this.requests', () => {
      spyOn(TestBed.get(RequestsService), 'acceptRequest').and.returnValue(of({}));
      requestsServiceGetIncomingRequestsSpy.and.returnValue(of([req, req]));
      component.answerRequest(friendlyReq, true);
      expect(requestsServiceGetIncomingRequestsSpy.calls.count()).toEqual(1);
      expect(requestsServiceGetIncomingRequestsSpy.calls.argsFor(0)).toEqual(['id']);
      expect(component.requests).toEqual([friendlyReq, friendlyReq]);
    });
  });

  describe('fetchUsers', () => {
    const friendlyReq = new Request(req).toFriendlyRequest();
    const user: User = {
      id: 'adjfs-0123',
      fullName: 'bob',
      email: 'my@email.com',
      profilePictureUrl: 'http://example.com',
    } as User;

    it('should have called RequestsService.getUser and set receiver and sender', () => {
      spyOn(TestBed.get(RequestsService), 'acceptRequest').and.returnValue(of({}));
      requestsServiceGetIncomingRequestsSpy.and.returnValue(of([friendlyReq]));
      const usersServiceGetUserSpy = spyOn(TestBed.get(UsersService), 'getUser').and.returnValue(of(user));
      component.answerRequest(friendlyReq, true);
      expect(usersServiceGetUserSpy.calls.count()).toEqual(2);
      expect(component.requests.length).toEqual(1);
      expect(usersServiceGetUserSpy.calls.argsFor(0)).toEqual([friendlyReq.receiverId]);
      expect(component.requests[0].receiver).toEqual(user);
      expect(usersServiceGetUserSpy.calls.argsFor(1)).toEqual([friendlyReq.senderId]);
      expect(component.requests[0].sender).toEqual(user);
    });
  });
});
