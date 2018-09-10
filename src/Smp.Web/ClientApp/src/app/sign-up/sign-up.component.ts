import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CreateUserRequest } from '../models/create-user-request';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html'
})
export class SignUpComponent {
  private readonly baseUrl: string;
  private readonly httpClient: HttpClient;
  private createUserRequest = new CreateUserRequest();

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.httpClient = http;
  }

  public signUp() {
    this.httpClient
      .post(this.baseUrl + 'api/User/CreateUser', this.createUserRequest)
      .subscribe(result => {}, error => console.error(error));
  }
}
