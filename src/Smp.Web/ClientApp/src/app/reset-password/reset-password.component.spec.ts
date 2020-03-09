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
      providers: [AccountsService, { provide: 'BASE_URL', useValue: 'https://www.smp.org/' }],
    }).compileComponents();
    fixture = TestBed.createComponent(ResetPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  describe('resetPassword', async () => {
    const accountsServiceCreateUserSpy: jasmine.Spy = spyOn(TestBed.get(AccountsService), 'resetPassword');
    const req: ResetPasswordRequest = {
      actionId: '1', // ActionType.ResetPassword
      newPassword: 'qwerty',
      confirmNewPassword: 'qwerty',
    } as ResetPasswordRequest;

    it('should have a validation error coming from the API', () => {
      const error = new Error('Descriptive error message.');
      accountsServiceCreateUserSpy.and.returnValue(throwError({ error }));
      component.resetPasswordRequest = req;
      component.resetPassword();
      expect(accountsServiceCreateUserSpy.calls.count()).toEqual(1);
      expect(accountsServiceCreateUserSpy.calls.argsFor(0)).toEqual([req]);
      expect(component.validationErrors.length).toEqual(1);
      expect(component.validationErrors[0]).toEqual(error);
      expect(component.resetPasswordSuccessful).toBeFalsy();
    });

    it('should successfully reset a password', () => {
      accountsServiceCreateUserSpy.and.returnValue(of({}));
      component.resetPasswordRequest = req;
      component.resetPassword();
      expect(accountsServiceCreateUserSpy.calls.count()).toEqual(1);
      expect(accountsServiceCreateUserSpy.calls.argsFor(0)).toEqual([req]);
      expect(component.validationErrors.length).toEqual(0);
      expect(component.resetPasswordSuccessful).toBeTruthy();
    });
  });
});
