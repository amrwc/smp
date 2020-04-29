import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { AccountsService } from './accounts.service';
import { ResetPasswordRequest } from '../models/requests/reset-password-request';

describe('AccountsService', () => {
  const baseUrl: string = 'https://www.smp.org/';
  let service: AccountsService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [{ provide: 'BASE_URL', useValue: baseUrl }],
    });
    service = TestBed.get(AccountsService);
  });

  describe('forgottenPassword()', () => {
    const email: string = 'my@email.com';

    it('should have called HttpClient.get() correctly', () => {
      spyOn(TestBed.get(HttpClient), 'get');
      service.forgottenPassword(email);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledWith(`${baseUrl}api/Accounts/ForgottenPassword/${email}`);
    });
  });

  describe('resetPassword()', () => {
    const resetReq: ResetPasswordRequest = {
      actionId: 'actionId',
      newPassword: 'newPassword',
      confirmNewPassword: 'newPassword',
    } as ResetPasswordRequest;

    it('should have called HttpClient.post() correctly', () => {
      spyOn(TestBed.get(HttpClient), 'post');
      service.resetPassword(resetReq);
      expect(TestBed.get(HttpClient).post).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).post).toHaveBeenCalledWith(`${baseUrl}api/Accounts/ResetPassword`, resetReq);
    });
  });
});
