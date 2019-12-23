import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CreateUserRequest } from '../models/create-user-request';
import { Error } from '../models/error';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent {
  private readonly baseUrl: string;
  private readonly httpClient: HttpClient;
  public createUserRequest = new CreateUserRequest();
  public loading: boolean = false;
  public validationErrors: Array<Error> = [];

  constructor(
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private router: Router
  ) {
    this.baseUrl = baseUrl;
    this.httpClient = http;
  }

  public signUp(): void {
    this.validationErrors = [];
    this.loading = true;
    this.createUserRequest.email = this.createUserRequest.email.toLowerCase();

    this.httpClient
      .post(this.baseUrl + 'api/User/CreateUser', this.createUserRequest)
      .subscribe(result => {
        this.loading = false;
        this.router.navigate(['/sign-in'], { queryParams: {signUpSuccessful: 'true' }});
      }, error => {
        this.validationErrors = error.error;
        this.loading = false;
      });
  }
}
