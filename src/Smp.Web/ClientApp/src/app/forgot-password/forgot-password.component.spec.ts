import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of, throwError } from 'rxjs';

import { AccountsService } from '../services/accounts.service';
import { CurrentUser } from '../models/current-user';
import { Error } from '../models/error';
import { ForgotPasswordComponent } from './forgot-password.component';

describe('ForgotPasswordComponent', () => {
  let component: ForgotPasswordComponent;
  let fixture: ComponentFixture<ForgotPasswordComponent>;

  const user: CurrentUser = {
    id: 'cuId',
    fullName: 'cuFullName',
    email: 'cu@email.com',
    token: 'cuToken',
  } as CurrentUser;

  beforeAll(() => {
    localStorage.setItem('currentUser', JSON.stringify(user));
  });

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ForgotPasswordComponent],
      imports: [HttpClientTestingModule, FormsModule],
      providers: [{ provide: 'BASE_URL', useValue: 'https://www.smp.org/' }],
    }).compileComponents();
    fixture = TestBed.createComponent(ForgotPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    component.accountEmail = 'MiXeDcAsE@email.com';
  });

  afterAll(() => {
    localStorage.removeItem('currentUser');
  });

  describe('startPasswordReset()', () => {
    beforeEach(() => {
      spyOn(TestBed.inject(AccountsService), 'forgottenPassword');
    });

    describe('when AccountsService.forgottenPassword() is successful', () => {
      const message: string = 'An email has been sent to you. Click it to reset your password!';

      beforeEach(() => {
        TestBed.get(AccountsService).forgottenPassword.and.returnValue(of(' '));
      });

      it('should have called AccountsService.forgottenPassword() correctly', () => {
        component.startPasswordReset();
        expect(TestBed.inject(AccountsService).forgottenPassword).toHaveBeenCalledTimes(1);
        expect(TestBed.inject(AccountsService).forgottenPassword).toHaveBeenCalledWith('mixedcase@email.com');
      });

      it('should have set the correct variable values', () => {
        component.startPasswordReset();
        expect(component.validationError).toBeFalsy();
        expect(component.startPasswordResetSuccessful).toEqual(true);
        expect(component.startPasswordResetUnsuccessful).toEqual(false);
        expect(component.loading).toEqual(false);
        expect(component.response).toEqual(message);
      });
    });

    describe('when AccountsService.forgottenPassword() returns an error', () => {
      const error: Error = {
        key: 'error',
        value: 'error-message',
      } as Error;

      beforeEach(() => {
        TestBed.get(AccountsService).forgottenPassword.and.returnValue(throwError({ error }));
      });

      it('should have called AccountsService.forgottenPassword() correctly', () => {
        component.startPasswordReset();
        expect(TestBed.inject(AccountsService).forgottenPassword).toHaveBeenCalledTimes(1);
        expect(TestBed.inject(AccountsService).forgottenPassword).toHaveBeenCalledWith('mixedcase@email.com');
      });

      it('should have set the correct variable values', () => {
        component.startPasswordReset();
        expect(component.validationError).toEqual(error);
        expect(component.startPasswordResetSuccessful).toEqual(false);
        expect(component.startPasswordResetUnsuccessful).toEqual(true);
        expect(component.loading).toEqual(false);
      });
    });
  });
});
