import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SignInRequest } from '../models/sign-in-request';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {
  private readonly baseUrl: string;
  private readonly httpClient: HttpClient;
  public signInRequest: SignInRequest = new SignInRequest();
  public loading: boolean = false;
  public returnUrl: string;
  public signUpSuccessful: boolean = false;
  public signInUnsuccessful: boolean = false;
  public errorMessage: string;

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

  public signIn() {
    this.loading = true;
    this.signUpSuccessful = false;
    this.signInRequest.email = this.signInRequest.email.toLowerCase();

    this.httpClient
      .post(this.baseUrl + 'api/Auth/SignIn', this.signInRequest)
      .subscribe(result => {
        localStorage.setItem('currentUser', JSON.stringify(result));
        this.loading = false;
        this.router.navigate([this.returnUrl]);
      }, error => {
        this.signInUnsuccessful = true;
        this.errorMessage = error.status === 401
          ? "Invalid sign in details. Please try again."
          : "We are experiencing technical difficulties right now. Please try again later.";
          this.loading = false;
      });
  }
}
