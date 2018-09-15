import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent {
  private readonly baseUrl: string;
  private readonly httpClient: HttpClient;
  public loading: boolean;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.httpClient = http;
  }

  // TODO: Sign in functionality.
  public signIn() {
    this.loading = true;
//    this.httpClient
//      .post(this.baseUrl + 'api/SignIn', this.createUserRequest)
//      .subscribe(result => {}, error => console.error(error));
  }
}
