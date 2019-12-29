import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountsService {

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  public forgottenPassword(email: string): Observable<Object> {
    return this.httpClient.get(this.baseUrl + "api/Accounts/ForgottenPassword/" + email);
  }
}