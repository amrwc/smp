import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { of, throwError} from 'rxjs';

import { CreateUserRequest } from '../models/requests/create-user-request';
import { Error } from '../models/error';
import { SignUpComponent } from './sign-up.component';
import { UsersService } from '../services/users.service';

describe('SignUpComponent', () => {
  const createUserRequest: CreateUserRequest = {
    fullName: 'c',
    password: '09876',
    confirmPassword: '09876',
    email: 'my@EMAIL.com',
  } as CreateUserRequest;
  let component: SignUpComponent;
  let fixture: ComponentFixture<SignUpComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SignUpComponent],
      imports: [RouterTestingModule, HttpClientTestingModule, FormsModule],
      providers: [{ provide: 'BASE_URL', useValue: 'https://www.smp.org/' }],
    }).compileComponents();
    fixture = TestBed.createComponent(SignUpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterAll(() => {
    if (fixture.nativeElement && 'remove' in fixture.nativeElement) {
      (fixture.nativeElement as HTMLElement).remove();
    }
  });

  describe('signUp()', () => {
    it('should have set a validation error due to passwords not matching', () => {
      component.createUserRequest.password = createUserRequest.password;
      component.createUserRequest.confirmPassword = '12345';
      component.signUp();
      expect(component.validationErrors.length).toEqual(1);
      expect(component.validationErrors[0].key).toEqual('invalid_password');
    });

    it('should have set a validation error coming from the API', () => {
      spyOn(TestBed.get(UsersService), 'createUser').and.callFake(() =>
        throwError({ error: new Error('invalid_full_name', '') })
      );
      component.createUserRequest = createUserRequest;
      component.signUp();
      expect(component.createUserRequest.email).toEqual('my@email.com');
      expect(component.validationErrors.length).toEqual(1);
      expect(component.validationErrors[0].key).toEqual('invalid_full_name');
      expect(TestBed.get(UsersService).createUser).toHaveBeenCalledTimes(1);
      expect(TestBed.get(UsersService).createUser).toHaveBeenCalledWith(createUserRequest);
    });

    it('should have navigated to /sign-in', () => {
      spyOn(TestBed.get(UsersService), 'createUser').and.callFake(() => of({}));
      spyOn(TestBed.get(Router), 'navigate');
      component.createUserRequest = createUserRequest;
      component.signUp();
      expect(component.createUserRequest.email).toEqual('my@email.com');
      expect(component.validationErrors.length).toEqual(0);
      expect(TestBed.get(UsersService).createUser).toHaveBeenCalledTimes(1);
      expect(TestBed.get(UsersService).createUser).toHaveBeenCalledWith(createUserRequest);
      expect(TestBed.get(Router).navigate).toHaveBeenCalledTimes(1);
      expect(TestBed.get(Router).navigate).toHaveBeenCalledWith(
        ['/sign-in'],
        { queryParams: { signUpSuccessful: 'true' } }
      );
    });
  });
});
