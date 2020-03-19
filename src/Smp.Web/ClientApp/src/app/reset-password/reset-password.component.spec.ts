import { ActivatedRoute } from '@angular/router';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { of, throwError } from 'rxjs';
import { AccountsService } from '../services/accounts.service';
import { ResetPasswordComponent } from './reset-password.component';
import { ResetPasswordRequest } from '../models/requests/reset-password-request';

describe('ResetPasswordComponent', () => {
  let component: ResetPasswordComponent;
  let fixture: ComponentFixture<ResetPasswordComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ResetPasswordComponent],
      imports: [HttpClientTestingModule, RouterTestingModule, FormsModule],
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
    fixture = TestBed.createComponent(ResetPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  describe('ngOnInit()', () => {
    it('should have gotten the correct actionId', () => {
      expect(component.resetPasswordRequest.actionId).toEqual('bob');
    });
  });

  describe('resetPassword()', () => {
    const req: ResetPasswordRequest = {
      actionId: '1', // ActionType.ResetPassword
      newPassword: 'qwerty',
      confirmNewPassword: 'qwerty',
    } as ResetPasswordRequest;
    let accountsServiceResetPasswordSpy: jasmine.Spy;

    beforeEach(() => {
      component.resetPasswordRequest = req;
      accountsServiceResetPasswordSpy = spyOn(TestBed.get(AccountsService), 'resetPassword');
    });

    it('should have stored a validation error coming from the API', () => {
      const error: Error = new Error('Descriptive error message.');
      accountsServiceResetPasswordSpy.and.returnValue(throwError({ error }));
      component.resetPassword();
      expect(component.validationErrors.length).toEqual(1);
      expect(component.validationErrors[0]).toEqual(error);
      expect(component.resetPasswordSuccessful).toBeFalsy();
      expect(TestBed.get(AccountsService).resetPassword).toHaveBeenCalledTimes(1);
      expect(TestBed.get(AccountsService).resetPassword).toHaveBeenCalledWith(req);
    });

    it('should have successfully reset a password', () => {
      accountsServiceResetPasswordSpy.and.returnValue(of({}));
      component.resetPassword();
      expect(component.validationErrors.length).toEqual(0);
      expect(component.resetPasswordSuccessful).toBeTruthy();
      expect(TestBed.get(AccountsService).resetPassword).toHaveBeenCalledTimes(1);
      expect(TestBed.get(AccountsService).resetPassword).toHaveBeenCalledWith(req);
    });
  });
});
