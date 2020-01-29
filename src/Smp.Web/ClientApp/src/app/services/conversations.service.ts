import { Injectable, Inject } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConversationsService {
  
  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {  }

  public getConversations(userId: string): Observable<Object> {
    let headers = new HttpHeaders();
    headers.set('Authorization', "Bearer " + JSON.parse(localStorage.getItem('currentUser')).token);
    return this.httpClient.get(`${this.baseUrl}api/Conversations/GetConversations/${userId}`, { headers: headers });
  }
}
