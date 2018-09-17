import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SignInRequest } from '../models/sign-in-request';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent {
  private readonly baseUrl: string;
  private readonly httpClient: HttpClient;
  public signInRequest = new SignInRequest();
  public loading: boolean;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.httpClient = http;
  }

  public signIn() {
    this.loading = true;

    this.httpClient
      .post(this.baseUrl + 'api/Auth/SignIn', this.signInRequest)
      .subscribe(result => {
        localStorage.setItem('currentUser', JSON.stringify(result));
        this.loading = false;
      }, error => {
        console.error(error);
        this.loading = false;
      });
  }

  // TODO: Sign out functionality
  // public signOut() {
  //   localStorage.removeItem('currentUser');
  // }
}
