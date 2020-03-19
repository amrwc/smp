import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { of, Observable } from 'rxjs';
import { CreateRequestRequest } from '../models/requests/create-request-request';
import { Request } from '../models/request';
import { RequestType } from '../models/request-type.enum';
import { RequestsService } from './requests.service';

describe('RequestsService', () => {
  const baseUrl: string = 'https://www.smp.org/';
  let headers: Object;
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
    spyOn(TestBed.get(HttpClient), 'post');
  });

  afterAll(() => {
    localStorage.removeItem('currentUser');
  });

  describe('sendRequest()', () => {
    const req: CreateRequestRequest = new CreateRequestRequest(
      'aksdfnknkj-123sdf-0asdfasd',
      'adsifasfuh1231231-asxzcz9v8bc9-213123',
      RequestType.Friend
    );

    it('should have called HttpClient.post() correctly', () => {
      service.sendRequest(req);
      expect(TestBed.get(HttpClient).post).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).post).toHaveBeenCalledWith(`${baseUrl}api/Requests/SendRequest/`, req, headers);
    });
  });

  describe('getRequest()', () => {
    const req = {
      senderId: 'aksdfnknkj-123sdf-0asdfasd',
      receiverId: 'adsifasfuh1231231-asxzcz9v8bc9-213123',
      requestType: RequestType.Friend,
    };
    const expected: Request = {
      ...req,
      createdAt: new Date(),
    } as Request;

    beforeEach(() => {
      spyOn(TestBed.get(HttpClient), 'get').and.returnValue(of(expected));
    });

    it('should have called HttpClient.get() correctly', () => {
      service.getRequest(req.senderId, req.receiverId, req.requestType);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledWith(
        `${baseUrl}api/Requests/GetRequest/${req.senderId}/${req.requestType}/${req.receiverId}`,
        headers
      );
    });

    it('should have returned the expected value', () => {
      const result: Observable<Request> = service.getRequest(req.senderId, req.receiverId, req.requestType);
      result.subscribe({
        next: (request: Request) => {
          expect(request).toEqual(expected);
        },
      });
    });
  });

  describe('getIncomingRequests()', () => {
    const userId: string = 'aksdfnknkj-123sdf-0asdfasd';
    const expected: Request[] = [
      {
        senderId: 'aksdfnknkj-123sdf-0asdfasd',
        receiverId: 'adsifasfuh1231231-asxzcz9v8bc9-213123',
        requestType: RequestType.Friend,
        createdAt: new Date(),
      },
      {
        senderId: 'aksdfnknkj-123sdf-0asdfasd',
        receiverId: 'adsifasfuh1231231-asxzcz9v8bc9-213123',
        requestType: RequestType.Friend,
        createdAt: new Date(),
      },
    ] as Request[];

    beforeEach(() => {
      spyOn(TestBed.get(HttpClient), 'get').and.returnValue(of(expected));
    });

    it('should have called HttpClient.get() correctly', () => {
      service.getIncomingRequests(userId);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledWith(
        `${baseUrl}api/Requests/GetIncomingRequests/${userId}`,
        headers
      );
    });

    it('should have returned the expected values', () => {
      const result: Observable<Request[]> = service.getIncomingRequests(userId);
      result.subscribe({
        next: (request: Request[]) => {
          expect(request).toEqual(expected);
        },
      });
    });
  });

  describe('acceptRequest', () => {
    beforeEach(() => {
      spyOn(TestBed.get(HttpClient), 'get');
    });

    it('should have called HttpClient.get() correctly', () => {
      const req: Request = {
        receiverId: 'adsifasfuh1231231-asxzcz9v8bc9-213123',
        senderId: 'aksdfnknkj-123sdf-0asdfasd',
        createdAt: new Date(),
        requestType: RequestType.Friend,
      } as Request;
      service.acceptRequest(req);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledWith(
        `${baseUrl}api/Requests/AcceptRequest/${req.receiverId}/${req.requestType}/${req.senderId}`,
        headers
      );
    });
  });

  describe('declineRequest()', () => {
    beforeEach(() => {
      spyOn(TestBed.get(HttpClient), 'get');
    });

    it('should have called HttpClient.get() correctly', () => {
      const req: Request = {
        receiverId: 'adsifasfuh1231231-asxzcz9v8bc9-213123',
        senderId: 'aksdfnknkj-123sdf-0asdfasd',
        createdAt: new Date(),
        requestType: RequestType.Friend,
      } as Request;
      service.declineRequest(req);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledWith(
        `${baseUrl}api/Requests/DeclineRequest/${req.receiverId}/${req.requestType}/${req.senderId}`,
        headers
      );
    });
  });
});
