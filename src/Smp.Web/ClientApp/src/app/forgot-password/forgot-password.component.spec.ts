import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForgotPasswordComponent } from './forgot-password.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { AccountsService } from '../services/accounts.service';
import { Error } from '../models/error';
import { FormsModule } from '@angular/forms';
import { throwError, of } from 'rxjs';

describe('ForgotPasswordComponent', () => {
  let component: ForgotPasswordComponent;
  let fixture: ComponentFixture<ForgotPasswordComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ ForgotPasswordComponent ],
      imports: [ HttpClientTestingModule, FormsModule ],
      providers: [ AccountsService, { provide: 'BASE_URL', useValue: "https://www.smp.org/" }]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ForgotPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    component.accountEmail = "MiXeDcAsE@email.com";
  });

  describe('startPasswordReset', () => {
    let accountsServiceForgottenPasswordSpy: jasmine.Spy;

    beforeEach(() => {
      accountsServiceForgottenPasswordSpy = spyOn(TestBed.inject(AccountsService), 'forgottenPassword');
    });

    describe('when AccountsService forgottenPassword is successful', () => {
      const message = 'An email has been sent to you. Click it to reset your password!';

      beforeEach(() => {
        accountsServiceForgottenPasswordSpy
          .and.returnValue(of(' '));
      });

      it('should call AccountsService forgottenPassword correctly', () => {
        component.startPasswordReset();

        expect(accountsServiceForgottenPasswordSpy.calls.count()).toEqual(1);
        expect(accountsServiceForgottenPasswordSpy.calls.argsFor(0)).toEqual(['mixedcase@email.com']);
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

    describe('when AccountsService forgottenPassword returns an error', () => {
      const error = {
        key: 'error',
        value: 'error-message' 
      } as Error;

      beforeEach(() => {
        accountsServiceForgottenPasswordSpy
          .and.returnValue(throwError({error: error}));
      });

      it('should call AccountsService forgottenPassword correctly', () => {
        component.startPasswordReset();

        expect(accountsServiceForgottenPasswordSpy.calls.count()).toEqual(1);
        expect(accountsServiceForgottenPasswordSpy.calls.argsFor(0)).toEqual(['mixedcase@email.com']);
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
