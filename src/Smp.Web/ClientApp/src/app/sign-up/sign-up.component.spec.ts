import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { TestBed, ComponentFixture } from '@angular/core/testing';

import { throwError, of } from 'rxjs';
import { CreateUserRequest } from '../models/requests/create-user-request';
import { Error } from '../models/error';
import { SignUpComponent } from './sign-up.component';
import { UsersService } from '../services/users.service';

describe('SignUpComponent', () => {
  let component: SignUpComponent;
  let fixture: ComponentFixture<SignUpComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SignUpComponent],
      imports: [RouterTestingModule, HttpClientTestingModule, FormsModule],
      providers: [{ provide: 'BASE_URL', useValue: 'https://www.smp.org/' }, UsersService],
    }).compileComponents();
    fixture = TestBed.createComponent(SignUpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  describe('signUp', () => {
    it('should have a validation error due to passwords not matching', () => {
      component.createUserRequest.password = '09876';
      component.createUserRequest.confirmPassword = '12345';
      component.signUp();
      expect(component.validationErrors.length).toEqual(1);
      expect(component.validationErrors[0].key).toEqual('invalid_password');
    });

    it('should have a validation error coming from the API', () => {
      const createUserRequest: CreateUserRequest = {
        fullName: 'c',
        password: '09876',
        confirmPassword: '09876',
        email: 'my@EMAIL.com',
      } as CreateUserRequest;
      const usersServiceCreateUserSpy: jasmine.Spy = spyOn(TestBed.get(UsersService), 'createUser').and.returnValue(
        throwError({
          error: new Error('invalid_full_name', 'Full name must have at least 3 characters.'),
        })
      );
      component.createUserRequest = createUserRequest;
      component.signUp();
      expect(component.createUserRequest.email).toEqual('my@email.com');
      expect(component.validationErrors.length).toEqual(1);
      expect(component.validationErrors[0].key).toEqual('invalid_full_name');
      expect(usersServiceCreateUserSpy.calls.count()).toEqual(1);
      expect(usersServiceCreateUserSpy.calls.argsFor(0).toString()).toEqual(createUserRequest.toString());
    });

    it('should navigate to /sign-in', () => {
      const createUserRequest: CreateUserRequest = {
        fullName: 'cknjqwer',
        password: '09876',
        confirmPassword: '09876',
        email: 'my@EMAIL.com',
      } as CreateUserRequest;
      const usersServiceCreateUserSpy: jasmine.Spy = spyOn(TestBed.get(UsersService), 'createUser').and.returnValue(
        of({})
      );
      const routerNavigateSpy: jasmine.Spy = spyOn(TestBed.get(Router), 'navigate');
      component.createUserRequest = createUserRequest;
      component.signUp();
      expect(component.createUserRequest.email).toEqual('my@email.com');
      expect(component.validationErrors.length).toEqual(0);
      expect(usersServiceCreateUserSpy.calls.count()).toEqual(1);
      expect(usersServiceCreateUserSpy.calls.argsFor(0).toString()).toEqual(createUserRequest.toString());
      expect(routerNavigateSpy.calls.count()).toEqual(1);
      expect(routerNavigateSpy.calls.argsFor(0)).toEqual([['/sign-in'], { queryParams: { signUpSuccessful: 'true' } }]);
    });
  });
});
