import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Conversation } from '../models/conversation';
import { CreateConversationRequest } from '../models/requests/create-conversation-request';
import { GlobalHelper } from '../helpers/global';

@Injectable({
  providedIn: 'root'
})
export class ConversationsService {
  
  constructor(private httpClient: HttpClient, private globalHelper: GlobalHelper, @Inject('BASE_URL') private baseUrl: string) {  }

  public getConversations(userId: string): Observable<Conversation[]> {
    return this.httpClient.get<Conversation[]>(`${this.baseUrl}api/Conversations/GetConversations/${userId}`, { headers: this.globalHelper.getAuthHeader() });
  }

  public getConversationParticipants(conversationId: string): Observable<string[]> {
    return this.httpClient.get<string[]>(`${this.baseUrl}api/Conversations/GetConversationParticipants/${conversationId}`, { headers: this.globalHelper.getAuthHeader() });
  }

  public createConversation(createConversationRequest: CreateConversationRequest): Observable<Object> {
    //let resp = this.httpClient.post<string>(`${this.baseUrl}api/Conversations/CreateConversation`, createConversationRequest, { headers: this.globalHelper.getAuthHeader() });

    return of("2e365c68-d1ea-4ef7-8fab-2efcf1b60911");
  }
}
