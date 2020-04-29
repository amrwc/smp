import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of, throwError } from 'rxjs';

import { FriendlyRequest, Request } from '../models/request';
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

  beforeEach(() => {
    localStorage.setItem('currentUser', '{ "id": "id" }');
    TestBed.configureTestingModule({
      declarations: [RequestsComponent],
      imports: [HttpClientTestingModule],
      providers: [{ provide: 'BASE_URL', useValue: 'https://www.smp.org/' }],
    }).compileComponents();
    fixture = TestBed.createComponent(RequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    localStorage.removeItem('currentUser');
  });

  describe('ngOnInit()', () => {
    it('should have called getRequests()', () => {
      spyOn(TestBed.get(RequestsService), 'getIncomingRequests').and.returnValue(of([]));
      component.ngOnInit();
      expect(TestBed.get(RequestsService).getIncomingRequests).toHaveBeenCalledTimes(1);
      expect(TestBed.get(RequestsService).getIncomingRequests).toHaveBeenCalledWith(
        JSON.parse(localStorage.getItem('currentUser')).id
      );
    });
  });

  describe('answerRequest()', () => {
    beforeEach(() => {
      spyOn(TestBed.get(RequestsService), 'getIncomingRequests').and.returnValue(of([]));
    });

    it('should not have called getRequest() when answer is true and on API error', () => {
      spyOn(TestBed.get(RequestsService), 'acceptRequest').and.returnValue(throwError(new Error()));
      component.answerRequest(friendlyRequest, true);
      expect(TestBed.get(RequestsService).acceptRequest).toHaveBeenCalledTimes(1);
      expect(TestBed.get(RequestsService).acceptRequest).toHaveBeenCalledWith(friendlyRequest);
      expect(TestBed.get(RequestsService).getIncomingRequests).toHaveBeenCalledTimes(0);
    });

    it('should not have called getRequest() when answer is false and on API error', () => {
      spyOn(TestBed.get(RequestsService), 'declineRequest').and.returnValue(throwError(new Error()));
      component.answerRequest(friendlyRequest, false);
      expect(TestBed.get(RequestsService).declineRequest).toHaveBeenCalledTimes(1);
      expect(TestBed.get(RequestsService).declineRequest).toHaveBeenCalledWith(friendlyRequest);
      expect(TestBed.get(RequestsService).getIncomingRequests).toHaveBeenCalledTimes(0);
    });

    it('should have called getRequest() when answer is true', () => {
      spyOn(TestBed.get(RequestsService), 'acceptRequest').and.returnValue(of({}));
      component.answerRequest(friendlyRequest, true);
      expect(TestBed.get(RequestsService).acceptRequest).toHaveBeenCalledTimes(1);
      expect(TestBed.get(RequestsService).acceptRequest).toHaveBeenCalledWith(friendlyRequest);
      expect(TestBed.get(RequestsService).getIncomingRequests).toHaveBeenCalledTimes(1);
      expect(TestBed.get(RequestsService).getIncomingRequests).toHaveBeenCalledWith(
        JSON.parse(localStorage.getItem('currentUser')).id
      );
    });

    it('should have called getRequest() when answer is false', () => {
      spyOn(TestBed.get(RequestsService), 'declineRequest').and.returnValue(of({}));
      component.answerRequest(friendlyRequest, false);
      expect(TestBed.get(RequestsService).declineRequest).toHaveBeenCalledTimes(1);
      expect(TestBed.get(RequestsService).declineRequest).toHaveBeenCalledWith(friendlyRequest);
      expect(TestBed.get(RequestsService).getIncomingRequests).toHaveBeenCalledTimes(1);
      expect(TestBed.get(RequestsService).getIncomingRequests).toHaveBeenCalledWith(
        JSON.parse(localStorage.getItem('currentUser')).id
      );
    });
  });

  describe('getRequests()', () => {
    const friendlyReq: FriendlyRequest = new Request(req).toFriendlyRequest();

    it('should have called RequestsService.getIncomingRequests() and set this.requests', () => {
      spyOn(TestBed.get(RequestsService), 'acceptRequest').and.returnValue(of({}));
      spyOn(TestBed.get(RequestsService), 'getIncomingRequests').and.returnValue(of([req, req]));
      component.answerRequest(friendlyReq, true);
      expect(component.requests).toEqual([friendlyReq, friendlyReq]);
      expect(TestBed.get(RequestsService).getIncomingRequests).toHaveBeenCalledTimes(1);
      expect(TestBed.get(RequestsService).getIncomingRequests).toHaveBeenCalledWith(
        JSON.parse(localStorage.getItem('currentUser')).id
      );
    });
  });

  describe('fetchUsers()', () => {
    const friendlyReq = new Request(req).toFriendlyRequest();
    const user: User = {
      id: 'adjfs-0123',
      fullName: 'bob',
      email: 'my@email.com',
      profilePictureUrl: 'http://example.com',
    } as User;

    it('should have called RequestsService.getUser() and set receiver and sender', () => {
      spyOn(TestBed.get(RequestsService), 'acceptRequest').and.returnValue(of({}));
      spyOn(TestBed.get(RequestsService), 'getIncomingRequests').and.returnValue(of([friendlyReq]));
      spyOn(TestBed.get(UsersService), 'getUser').and.returnValue(of(user));
      component.answerRequest(friendlyReq, true);
      expect(component.requests.length).toEqual(1);
      expect(component.requests[0].receiver).toEqual(user);
      expect(component.requests[0].sender).toEqual(user);
      expect(TestBed.get(UsersService).getUser).toHaveBeenCalledTimes(2);
      expect(TestBed.get(UsersService).getUser).toHaveBeenCalledWith(friendlyReq.receiverId);
      expect(TestBed.get(UsersService).getUser).toHaveBeenCalledWith(friendlyReq.senderId);
    });
  });
});
