import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { CreateUserRequest } from '../models/requests/create-user-request';
import { UsersService } from './users.service';

describe('UsersService', () => {
  const baseUrl = 'https://www.smp.org/';
  let httpClient: HttpClient;
  let httpClientGetSpy: jasmine.Spy;
  let httpClientPostSpy: jasmine.Spy;
  let service: UsersService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [{ provide: 'BASE_URL', useValue: baseUrl }],
    });
    httpClient = TestBed.get(HttpClient);
    service = TestBed.get(UsersService);
    httpClientGetSpy = spyOn(httpClient, 'get');
    httpClientPostSpy = spyOn(httpClient, 'post');
  });

  describe('createUser', () => {
    const createUserReq = {
      fullName: 'John Doe',
      password: 'lk1jn43kl1nj34###XXCC',
      confirmPassword: 'lk1jn43kl1nj34###XXCC',
      email: 'my@email.com',
    } as CreateUserRequest;

    it('should have called HttpClient.post correctly', () => {
      service.createUser(createUserReq);
      expect(httpClientPostSpy.calls.count()).toEqual(1);
      expect(httpClientPostSpy.calls.argsFor(0)).toEqual([
        `${baseUrl}api/Users/CreateUser/`,
        createUserReq,
      ]);
    });
  });

  describe('getUser', () => {
    const userId = 'aksdjn-123kwqjen-cxnzkj';
    beforeAll(() => {
      localStorage.setItem('currentUser', JSON.stringify({ token: 'n21k32n3j' }));
    });
    afterAll(() => {
      localStorage.removeItem('currentUser');
    });

    it('should have called HttpClient.get correctly', () => {
      const headers = new HttpHeaders().set(
        'Authorization',
        `Bearer ${JSON.parse(localStorage.getItem('currentUser')).token}`
      );
      service.getUser(userId);
      expect(httpClientGetSpy.calls.count()).toEqual(1);
      expect(httpClientGetSpy.calls.argsFor(0)).toEqual([
        `${baseUrl}api/Users/GetUser/${userId}`,
        { headers },
      ]);
    });
  });
});
