import { Injectable, Inject } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateRequestRequest } from '../models/requests/create-request-request';
import { RequestType } from '../models/request-type.enum';

@Injectable({
  providedIn: 'root'
})
export class RequestsService {

  private httpHeaders = new HttpHeaders();

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    let authToken = "Bearer " + JSON.parse(localStorage.getItem('currentUser')).token;
    this.httpHeaders = this.httpHeaders.set('Authorization', authToken);
  }

  public sendRequest(req: CreateRequestRequest): Observable<Object> {
    return this.httpClient.post(this.baseUrl + "api/Requests/SendRequest/", req, { headers: this.httpHeaders });
  }

  public getRequest(senderId: string, receiverId: string, reqType: RequestType): Observable<Request> {
    return this.httpClient.get<Request>(`${this.baseUrl}api/Requests/GetRequest/${senderId}/${<number>reqType}/${receiverId}`, { headers: this.httpHeaders })
  }

  public getIncomingRequests(userId: string): Observable<Request[]> {
    return this.httpClient.get<Request[]>(`${this.baseUrl}api/Requests/GetIncomingRequests/${userId}`, { headers: this.httpHeaders });
  }
}
