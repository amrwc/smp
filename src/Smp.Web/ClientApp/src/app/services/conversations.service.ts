import { Injectable, Inject } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Conversation } from '../models/conversation';

@Injectable({
  providedIn: 'root'
})
export class ConversationsService {
  
  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {  }

  public getConversations(userId: string): Observable<Conversation[]> {
    let headers = new HttpHeaders();
    headers.set('Authorization', "Bearer " + JSON.parse(localStorage.getItem('currentUser')).token);
    return this.httpClient.get<Conversation[]>(`${this.baseUrl}api/Conversations/GetConversations/${userId}`, { headers: headers });
  }
}
