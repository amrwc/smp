import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Message, FriendlyMessage } from '../models/message';
import { map } from 'rxjs/operators';
import { GlobalHelper } from '../helpers/global';
import { CreateMessageRequest } from '../models/requests/create-message-request';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {

  constructor(private httpClient: HttpClient, private globalHelper: GlobalHelper, @Inject('BASE_URL') private baseUrl: string) {  }

  public getMessagesFromConversation(conversationId: string, count: number = 10, page: number = 0): Observable<FriendlyMessage[]> {
    const messages = this.httpClient.get<Message[]>(
      `${this.baseUrl}api/Messages/GetMessagesFromConversation/${conversationId}?count=${count}&page=${page}`, { headers: this.globalHelper.getAuthHeader() });
    return messages.pipe(map(msgs => msgs.map(m => new FriendlyMessage(m))));
  }

  public createMessage(createMessageRequest: CreateMessageRequest): Observable<Object> {
    return this.httpClient.post(`${this.baseUrl}api/Messages/CreateMessage`, createMessageRequest, { headers: this.globalHelper.getAuthHeader() });
  }
}
