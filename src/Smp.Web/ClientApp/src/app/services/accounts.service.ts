import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ResetPasswordRequest } from '../models/requests/reset-password-request';

@Injectable({
  providedIn: 'root'
})
export class AccountsService {

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  public forgottenPassword(email: string): Observable<Object> {
    return this.httpClient.get(this.baseUrl + "api/Accounts/ForgottenPassword/" + email);
  }

  public resetPassword(resetPasswordRequest: ResetPasswordRequest) {
    return this.httpClient.post(this.baseUrl + "api/Accounts/ResetPassword", resetPasswordRequest);
  }
}