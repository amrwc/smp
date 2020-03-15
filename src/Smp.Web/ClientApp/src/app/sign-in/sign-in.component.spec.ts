import { ActivatedRoute, Router } from '@angular/router';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { of, throwError } from 'rxjs';
import { SignInComponent } from './sign-in.component';
import { SignInRequest } from '../models/requests/sign-in-request';

describe('SignInComponent', () => {
  const baseUrl: string = 'https://www.smp.org/';
  let component: SignInComponent;
  let fixture: ComponentFixture<SignInComponent>;

  describe('ngOnInit()', () => {
    function setUp(returnUrl: string, signUpSuccessful: string) {
      TestBed.configureTestingModule({
        declarations: [SignInComponent],
        imports: [FormsModule, HttpClientTestingModule, RouterTestingModule],
        providers: [
          { provide: 'BASE_URL', useValue: baseUrl },
          {
            provide: ActivatedRoute,
            useValue: {
              queryParams: of({ signUpSuccessful }),
              snapshot: { queryParams: { returnUrl } },
            },
          },
        ],
      }).compileComponents();
      fixture = TestBed.createComponent(SignInComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
    }

    it('should have set returnUrl to root and signUpSuccessful to true', () => {
      setUp('/', 'true');
      component.ngOnInit();
      expect(component.returnUrl).toEqual('/');
      expect(component.signUpSuccessful).toBeTruthy();
    });

    it('should have set returnUrl to supplied param and signUpSuccessful to false', () => {
      setUp('/not-root', 'false');
      component.ngOnInit();
      expect(component.returnUrl).toEqual('/not-root');
      expect(component.signUpSuccessful).toBeFalsy();
    });
  });

  describe('signIn()', () => {
    const signInRequest: SignInRequest = {
      email: 'MY@EMAIL.com',
      password: '280913',
    } as SignInRequest;
    let httpClientPostSpy: jasmine.Spy;

    beforeEach(() => {
      TestBed.configureTestingModule({
        declarations: [SignInComponent],
        imports: [FormsModule, HttpClientTestingModule, RouterTestingModule],
        providers: [{ provide: 'BASE_URL', useValue: baseUrl }],
      }).compileComponents();
      fixture = TestBed.createComponent(SignInComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
      component.signInRequest = signInRequest;
      httpClientPostSpy = spyOn(TestBed.get(HttpClient), 'post');
    });

    afterEach(() => {
      expect(component.signInRequest.email).toEqual('my@email.com');
      expect(component.loading).toBeFalsy();
      expect(TestBed.get(HttpClient).post).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).post).toHaveBeenCalledWith(`${baseUrl}api/Auth/SignIn`, signInRequest);
    });

    it('should have received a 401 error from the API', () => {
      httpClientPostSpy.and.callFake(() => throwError({ status: 401 }));
      component.signIn();
      expect(component.signInUnsuccessful).toBeTruthy();
      expect(component.errorMessage).toEqual('Invalid sign in details. Please try again.');
    });

    it('should have received an error other than 401 from the API', () => {
      httpClientPostSpy.and.callFake(() => throwError({ status: 500 }));
      component.signIn();
      expect(component.signInUnsuccessful).toBeTruthy();
      expect(component.errorMessage).toEqual(
        'We are experiencing technical difficulties right now. Please try again later.'
      );
    });

    it("should have navigated to '/'", () => {
      httpClientPostSpy.and.callFake(() => of({ currentUser: 'bob' }));
      spyOn(TestBed.get(Router), 'navigate');
      component.signIn();
      expect(localStorage.getItem('currentUser')).toEqual(JSON.stringify({ currentUser: 'bob' }));
      expect(component.signInUnsuccessful).toBeFalsy();
      expect(TestBed.get(Router).navigate).toHaveBeenCalledTimes(1);
      expect(TestBed.get(Router).navigate).toHaveBeenCalledWith(['/']);
      // Clean up
      localStorage.removeItem('currentUser');
    });
  });
});
