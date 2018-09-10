import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CreateUserRequest } from '../models/create-user-request';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html'
})
export class SignUpComponent {
  private baseUrl: string;
  private httpClient: HttpClient;
  private createUserRequest: any = {};

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.httpClient = http;
  }

  public signUp() {
    debugger;
    this.httpClient
      .post(this.baseUrl + 'api/User/CreateUser', this.createUserRequest)
      .subscribe(result => {}, error => console.error(error));
  }
}
