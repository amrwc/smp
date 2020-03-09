import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';

import { of, throwError } from 'rxjs';
import { SignInComponent } from './sign-in.component';
import { SignInRequest } from '../models/requests/sign-in-request';

describe('SignInComponent', () => {
  const baseUrl: string = 'https://www.smp.org/';
  const signInRequest: SignInRequest = {
    email: 'MY@EMAIL.com',
    password: '280913',
  } as SignInRequest;
  let component: SignInComponent;
  let fixture: ComponentFixture<SignInComponent>;
  let httpClientPostSpy: jasmine.Spy;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SignInComponent],
      imports: [RouterTestingModule, HttpClientTestingModule, FormsModule],
      providers: [{ provide: 'BASE_URL', useValue: baseUrl }],
    }).compileComponents();
    fixture = TestBed.createComponent(SignInComponent);
    component = fixture.componentInstance;
    component.signInRequest = signInRequest;
    fixture.detectChanges();
    httpClientPostSpy = spyOn(TestBed.get(HttpClient), 'post');
  });

  describe('signIn', () => {
    it('should get a 401 error from the API', () => {
      httpClientPostSpy.and.returnValue(throwError({ status: 401 }));
      component.signIn();
      expect(component.signInRequest.email).toEqual('my@email.com');
      expect(component.signInUnsuccessful).toEqual(true);
      expect(component.errorMessage).toEqual('Invalid sign in details. Please try again.');
      expect(httpClientPostSpy.calls.count()).toEqual(1);
      expect(httpClientPostSpy.calls.argsFor(0)).toEqual([`${baseUrl}api/Auth/SignIn`, signInRequest]);
    });

    it('should get an error other than 401 from the API', () => {
      httpClientPostSpy.and.returnValue(throwError({ status: 500 }));
      component.signIn();
      expect(component.signInRequest.email).toEqual('my@email.com');
      expect(component.signInUnsuccessful).toEqual(true);
      expect(component.errorMessage).toEqual(
        'We are experiencing technical difficulties right now. Please try again later.'
      );
      expect(httpClientPostSpy.calls.count()).toEqual(1);
      expect(httpClientPostSpy.calls.argsFor(0)).toEqual([`${baseUrl}api/Auth/SignIn`, signInRequest]);
    });

    it('should navigate to /', () => {
      const routerNavigateSpy: jasmine.Spy = spyOn(TestBed.get(Router), 'navigate');
      httpClientPostSpy.and.returnValue(of({ currentUser: 'bob' }));
      component.signIn();
      expect(component.signInRequest.email).toEqual('my@email.com');
      expect(localStorage.getItem('currentUser')).toEqual(JSON.stringify({ currentUser: 'bob' }));
      localStorage.removeItem('currentUser');
      expect(component.signInUnsuccessful).toEqual(false);
      expect(routerNavigateSpy.calls.count()).toEqual(1);
      expect(routerNavigateSpy.calls.argsFor(0)).toEqual([['/']]);
      expect(httpClientPostSpy.calls.count()).toEqual(1);
      expect(httpClientPostSpy.calls.argsFor(0)).toEqual([`${baseUrl}api/Auth/SignIn`, signInRequest]);
    });
  });
});
