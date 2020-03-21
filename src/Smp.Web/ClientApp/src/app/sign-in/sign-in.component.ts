import { ActivatedRoute, Router } from '@angular/router';
import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { SignInRequest } from '../models/requests/sign-in-request';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss'],
})
export class SignInComponent implements OnInit {
  public errorMessage: string;
  public loading: boolean = false;
  public returnUrl: string;
  public signInRequest: SignInRequest = new SignInRequest();
  public signInUnsuccessful: boolean = false;
  public signUpSuccessful: boolean = false;
  private readonly baseUrl: string;
  private readonly httpClient: HttpClient;

  constructor(
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.baseUrl = baseUrl;
    this.httpClient = http;
  }

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    this.route.queryParams.subscribe(params => {
      if (params.signUpSuccessful !== undefined) {
        this.signUpSuccessful = params.signUpSuccessful === 'true';
      }
    });
  }

  public signIn(): void {
    this.loading = true;
    this.signInRequest.email = this.signInRequest.email.toLowerCase();
    this.signUpSuccessful = false;

    this.httpClient.post(this.baseUrl + 'api/Auth/SignIn', this.signInRequest).subscribe({
      next: (result: any) => {
        localStorage.setItem('currentUser', JSON.stringify(result));
        this.loading = false;
        this.router.navigate([this.returnUrl]);
      },
      error: (error: any) => {
        this.signInUnsuccessful = true;
        this.errorMessage =error.status === 401
          ? 'Invalid sign in details. Please try again.'
          : 'We are experiencing technical difficulties right now. Please try again later.';
        this.loading = false;
      },
    });
  }
}
