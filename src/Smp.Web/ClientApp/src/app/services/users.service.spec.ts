import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { CreateUserRequest } from '../models/requests/create-user-request';
import { UsersService } from './users.service';

describe('UsersService', () => {
  const baseUrl: string = 'https://www.smp.org/';
  let service: UsersService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [{ provide: 'BASE_URL', useValue: baseUrl }],
    });
    service = TestBed.get(UsersService);
    spyOn(TestBed.get(HttpClient), 'get');
    spyOn(TestBed.get(HttpClient), 'post');
  });

  describe('createUser()', () => {
    const createUserReq: CreateUserRequest = {
      fullName: 'John Doe',
      password: 'lk1jn43kl1nj34###XXCC',
      confirmPassword: 'lk1jn43kl1nj34###XXCC',
      email: 'my@email.com',
    } as CreateUserRequest;

    it('should have called HttpClient.post() correctly', () => {
      service.createUser(createUserReq);
      expect(TestBed.get(HttpClient).post).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).post).toHaveBeenCalledWith(`${baseUrl}api/Users/CreateUser/`, createUserReq);
    });
  });

  describe('getUser()', () => {
    const userId: string = 'aksdjn-123kwqjen-cxnzkj';

    beforeAll(() => {
      localStorage.setItem('currentUser', JSON.stringify({ token: 'n21k32n3j' }));
    });

    afterAll(() => {
      localStorage.removeItem('currentUser');
    });

    it('should have called HttpClient.get() correctly', () => {
      const headers: HttpHeaders = new HttpHeaders().set(
        'Authorization',
        `Bearer ${JSON.parse(localStorage.getItem('currentUser')).token}`
      );
      service.getUser(userId);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledWith(`${baseUrl}api/Users/GetUser/${userId}`, { headers });
    });
  });
});
