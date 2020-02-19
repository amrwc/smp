import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateRequestRequest } from '../models/requests/create-request-request';
import { RequestType } from '../models/request-type.enum';
import { Request } from '../models/request';
import { GlobalHelper } from '../helpers/global';

@Injectable({
  providedIn: 'root'
})
export class RequestsService {

  constructor(private httpClient: HttpClient, private globalHelper: GlobalHelper, @Inject('BASE_URL') private baseUrl: string) {
  }

  public sendRequest(req: CreateRequestRequest): Observable<Object> {
    return this.httpClient.post(`${this.baseUrl}api/Requests/SendRequest/`, req, { headers: this.globalHelper.getAuthHeader() });
  }

  public getRequest(senderId: string, receiverId: string, reqType: RequestType): Observable<Request> {
    return this.httpClient.get<Request>(`${this.baseUrl}api/Requests/GetRequest/${senderId}/${<number>reqType}/${receiverId}`, { headers: this.globalHelper.getAuthHeader() })
  }

  public getIncomingRequests(userId: string): Observable<Request[]> {
    return this.httpClient.get<Request[]>(`${this.baseUrl}api/Requests/GetIncomingRequests/${userId}`, { headers: this.globalHelper.getAuthHeader() });
  }

  public acceptRequest(request: Request): Observable<Object> {
    return this.httpClient.get(`${this.baseUrl}api/Requests/AcceptRequest/${request.receiverId}/${request.requestType}/${request.senderId}`, { headers: this.globalHelper.getAuthHeader() });
  }

  public declineRequest(request: Request): Observable<Object> {
    return this.httpClient.get(`${this.baseUrl}api/Requests/DeclineRequest/${request.receiverId}/${request.requestType}/${request.senderId}`, { headers: this.globalHelper.getAuthHeader() });
  }
}
