import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user';
import { Observable } from 'rxjs';
import { CreateUserRequest } from '../models/requests/create-user-request';
import { GlobalHelper } from '../helpers/global';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private httpClient: HttpClient, private globalHelper: GlobalHelper, @Inject('BASE_URL') private baseUrl: string) {
  }

  public createUser(createUserReq: CreateUserRequest): Observable<Object> {
    return this.httpClient.post(this.baseUrl + 'api/Users/CreateUser/', createUserReq);
  }

  public getUser(userId: string): Observable<User> {
    return this.httpClient.get<User>(this.baseUrl + 'api/Users/GetUser/' + userId, { headers: this.globalHelper.getAuthHeader() });
  }
}
