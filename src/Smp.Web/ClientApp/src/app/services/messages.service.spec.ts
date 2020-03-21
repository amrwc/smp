import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { of, Observable } from 'rxjs';

import { CreateMessageRequest } from '../models/requests/create-message-request';
import { FriendlyMessage, Message } from '../models/message';
import { MessagesService } from './messages.service';

describe('MessagesService', () => {
  const authHeaders: HttpHeaders = new HttpHeaders().set('Authorization', 'Bearer token');
  const baseUrl: string = 'https://www.smp.org/';
  let service: MessagesService;

  beforeAll(() => {
    localStorage.setItem('currentUser', '{ "id": "id", "token": "token" }');
  });

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [{ provide: 'BASE_URL', useValue: baseUrl }],
    });
    service = TestBed.get(MessagesService);
    spyOn(TestBed.get(HttpClient), 'post');
  });

  afterAll(() => {
    localStorage.removeItem('currentUser');
  });

  describe('getMessagesFromConversation()', () => {
    const conversationId: string = 'conversationId';
    const expectedMessages: Message[] = [
      {
        id: 1,
        senderId: 'senderId-1',
        createdAt: new Date(),
        content: 'content-1',
        conversationId,
      },
      {
        id: 2,
        senderId: 'senderId-2',
        createdAt: new Date(),
        content: 'content-2',
        conversationId,
      },
    ] as Message[];

    beforeEach(() => {
      spyOn(TestBed.get(HttpClient), 'get').and.returnValue(of(expectedMessages));
    });

    it('shouild have returned the expected value', () => {
      const messagesObserv: Observable<FriendlyMessage[]> = service.getMessagesFromConversation(conversationId);

      messagesObserv.subscribe({
        next: (messages: FriendlyMessage[]) => {
          expect(messages).toEqual(expectedMessages.map(msg => new FriendlyMessage(msg)));
        },
      });
    });

    it('should have called HttpClient.get() correctly', () => {
      service.getMessagesFromConversation(conversationId);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledWith(
        `${baseUrl}api/Messages/GetMessagesFromConversation/${conversationId}?count=10&page=0`,
        { headers: authHeaders }
      );
    });
  });

  describe('createMessage()', () => {
    const msgReq: CreateMessageRequest = { senderId: 'senderId' } as CreateMessageRequest;

    it('should have called HttpClient.post() correctly', () => {
      service.createMessage(msgReq);
      expect(TestBed.get(HttpClient).post).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).post).toHaveBeenCalledWith(
        `${baseUrl}api/Messages/CreateMessage`,
        msgReq,
        { headers: authHeaders }
      );
    });
  });
});
