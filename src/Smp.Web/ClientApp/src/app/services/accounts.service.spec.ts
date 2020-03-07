import { TestBed } from '@angular/core/testing';

import { AccountsService } from './accounts.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HttpClient } from '@angular/common/http';
import { ResetPasswordRequest } from '../models/requests/reset-password-request';

describe('AccountsService', () => {
  const baseUrl = 'https://www.smp.org/';

  let httpClient: HttpClient;
  let service: AccountsService;

  let httpClientGetSpy: jasmine.Spy;
  let httpClientPostSpy: jasmine.Spy;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [{ provide: 'BASE_URL', useValue: baseUrl }]
    });

    httpClient = TestBed.get(HttpClient);
    service = TestBed.get(AccountsService);

    httpClientGetSpy = spyOn(httpClient, 'get');
    httpClientPostSpy = spyOn(httpClient, 'post');
  });

  describe('forgottenPassword', () => {
    const email = 'my@email.com';

    it('should have called HttpClient.get correctly', () => {
      service.forgottenPassword(email);
      expect(httpClientGetSpy.calls.count()).toEqual(1);
      expect(httpClientGetSpy.calls.argsFor(0))
        .toEqual([`${baseUrl}api/Accounts/ForgottenPassword/${email}`]);
    });
  });

  describe('resetPassword', () => {
    const resetReq = { 
      actionId: 'actionId',
      newPassword: 'newPassword',
      confirmNewPassword: 'newPassword'
    } as ResetPasswordRequest;

    it('should have called HttpClient.post correctly', () => {
      service.resetPassword(resetReq);
      expect(httpClientPostSpy.calls.count()).toEqual(1);
      expect(httpClientPostSpy.calls.argsFor(0))
        .toEqual([`${baseUrl}api/Accounts/ResetPassword`, resetReq]);
    });
  });
});
