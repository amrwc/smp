import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { CreateRequestRequest } from '../models/requests/create-request-request';
import { Request } from '../models/request';
import { RequestType } from '../models/request-type.enum';
import { RequestsService } from './requests.service';

describe('RequestsService', () => {
  const baseUrl = 'https://www.smp.org/';
  let headers: Object;
  let httpClient: HttpClient;
  let httpClientGetSpy: jasmine.Spy;
  let httpClientPostSpy: jasmine.Spy;
  let service: RequestsService;

  beforeAll(() => {
    localStorage.setItem('currentUser', JSON.stringify({ token: 'n21k32n3j' }));
    headers = {
      headers: new HttpHeaders().set(
        'Authorization',
        `Bearer ${JSON.parse(localStorage.getItem('currentUser')).token}`
      ),
    };
  });

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [{ provide: 'BASE_URL', useValue: baseUrl }],
    });
    service = TestBed.get(RequestsService);
    httpClient = TestBed.get(HttpClient);
    httpClientGetSpy = spyOn(httpClient, 'get');
    httpClientPostSpy = spyOn(httpClient, 'post');
  });

  afterAll(() => {
    localStorage.removeItem('currentUser');
  });

  describe('sendRequest', () => {
    const req: CreateRequestRequest = new CreateRequestRequest(
      'aksdfnknkj-123sdf-0asdfasd',
      'adsifasfuh1231231-asxzcz9v8bc9-213123',
      RequestType.Friend
    );

    it('should have called HttpClient.post correctly', () => {
      service.sendRequest(req);
      expect(httpClientPostSpy.calls.count()).toEqual(1);
      expect(httpClientPostSpy.calls.argsFor(0)).toEqual([
        `${baseUrl}api/Requests/SendRequest/`,
        req,
        headers,
      ]);
    });
  });

  describe('getRequest', () => {
    it('should have called HttpClient.get correctly', () => {
      const req = {
        senderId: 'aksdfnknkj-123sdf-0asdfasd',
        receiverId: 'adsifasfuh1231231-asxzcz9v8bc9-213123',
        reqType: RequestType.Friend,
      };
      service.getRequest(req.senderId, req.receiverId, req.reqType);
      expect(httpClientGetSpy.calls.count()).toEqual(1);
      expect(httpClientGetSpy.calls.argsFor(0)).toEqual([
        `${baseUrl}api/Requests/GetRequest/${req.senderId}/${req.reqType}/${req.receiverId}`,
        headers,
      ]);
    });
  });

  describe('getIncomingRequests', () => {
    it('should have called HttpClient.get correctly', () => {
      const userId: string = 'aksdfnknkj-123sdf-0asdfasd';
      service.getIncomingRequests(userId);
      expect(httpClientGetSpy.calls.count()).toEqual(1);
      expect(httpClientGetSpy.calls.argsFor(0)).toEqual([
        `${baseUrl}api/Requests/GetIncomingRequests/${userId}`,
        headers,
      ]);
    });
  });

  describe('acceptRequest', () => {
    it('should have called HttpClient.get correctly', () => {
      const req: Request = {
        receiverId: 'adsifasfuh1231231-asxzcz9v8bc9-213123',
        senderId: 'aksdfnknkj-123sdf-0asdfasd',
        createdAt: new Date(),
        requestType: RequestType.Friend,
      } as Request;
      service.acceptRequest(req);
      expect(httpClientGetSpy.calls.count()).toEqual(1);
      expect(httpClientGetSpy.calls.argsFor(0)).toEqual([
        `${baseUrl}api/Requests/AcceptRequest/${req.receiverId}/${req.requestType}/${req.senderId}`,
        headers,
      ]);
    });
  });

  describe('declineRequest', () => {
    it('should have called HttpClient.get correctly', () => {
      const req: Request = {
        receiverId: 'adsifasfuh1231231-asxzcz9v8bc9-213123',
        senderId: 'aksdfnknkj-123sdf-0asdfasd',
        createdAt: new Date(),
        requestType: RequestType.Friend,
      } as Request;
      service.declineRequest(req);
      expect(httpClientGetSpy.calls.count()).toEqual(1);
      expect(httpClientGetSpy.calls.argsFor(0)).toEqual([
        `${baseUrl}api/Requests/DeclineRequest/${req.receiverId}/${req.requestType}/${req.senderId}`,
        headers,
      ]);
    });
  });
});
