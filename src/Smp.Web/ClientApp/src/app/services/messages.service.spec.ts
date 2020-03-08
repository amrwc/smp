import { TestBed } from '@angular/core/testing';

import { MessagesService } from './messages.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CreateMessageRequest } from '../models/requests/create-message-request';
import { of } from 'rxjs';
import { Message, FriendlyMessage } from '../models/message';

describe('MessagesService', () => {
  const baseUrl = 'https://www.smp.org/';

  let httpClient: HttpClient;
  let service: MessagesService;

  let httpClientGetSpy: jasmine.Spy;
  let httpClientPostSpy: jasmine.Spy;

  const authHeaders = new HttpHeaders().set('Authorization', 'Bearer token');

  beforeAll(() => {
    localStorage.setItem('currentUser', '{ "id": "id", "token": "token" }');
  });

  afterAll(() => {
    localStorage.removeItem('currentUser');
  });

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [{ provide: 'BASE_URL', useValue: "https://www.smp.org/" }]
    });

    service = TestBed.get(MessagesService);

    httpClientPostSpy = spyOn(TestBed.get(HttpClient), 'post');
});

  describe('getMessagesFromConversation', () => {
    beforeEach(() => {
      httpClientGetSpy = spyOn(TestBed.get(HttpClient), 'get')
        .and.returnValue(of(expectedMessages));
    });

    const conversationId = 'conversationId';
    const expectedMessages: Message[] = [
      new Message({
        id: 1,
        senderId: 'senderId-1',
        createdAt: new Date(),
        content: 'content-1',
        conversationId: conversationId
      } as Message),
      new Message({
        id: 2,
        senderId: 'senderId-2',
        createdAt: new Date(),
        content: 'content-2',
        conversationId: conversationId
      } as Message)
    ];

    it('shouild have returned the expected value', () => {
      const messagesObserv = service.getMessagesFromConversation(conversationId);

      messagesObserv.subscribe({
        next: (messages: FriendlyMessage[]) => {
          expect(messages).toEqual(expectedMessages.map(msg => new FriendlyMessage(msg)));
        }
      });
    });

    it('should have called HttpClient get correctly', () => {
      service.getMessagesFromConversation(conversationId);

      expect(httpClientGetSpy.calls.count()).toEqual(1);
      expect(httpClientGetSpy.calls.argsFor(0)).toEqual([
        `${baseUrl}api/Messages/GetMessagesFromConversation/${conversationId}?count=10&page=0`,
        { headers: authHeaders }
      ]);
    });
  });

  describe('createMessage', () => {
    const msgReq = {
      senderId: 'senderId'
    } as CreateMessageRequest;

    it('should have called HttpClient post correctly', () => {
      service.createMessage(msgReq);

      expect(httpClientPostSpy.calls.count()).toEqual(1);
      expect(httpClientPostSpy.calls.argsFor(0)).toEqual([
        `${baseUrl}api/Messages/CreateMessage`,
        msgReq,
        { headers: authHeaders }
      ]);
    });
  });
});
