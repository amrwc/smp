import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from '../models/user';
import { Observable } from 'rxjs';
import { CreateUserRequest } from '../models/requests/create-user-request';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  private httpHeaders = new HttpHeaders();

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    let currentUser = localStorage.getItem('currentUser');

    if (currentUser) {
      let authToken = "Bearer " + JSON.parse(localStorage.getItem('currentUser')).token;
      this.httpHeaders = this.httpHeaders.set('Authorization', authToken);
    }
  }

  public createUser(createUserReq: CreateUserRequest): Observable<Object> {
    return this.httpClient.post(this.baseUrl + "api/Users/CreateUser/", createUserReq);
  }

  public getUser(userId: string): Observable<User> {
    return this.httpClient.get<User>(this.baseUrl + "api/Users/GetUser/" + userId, { headers: this.httpHeaders });
  }
}
