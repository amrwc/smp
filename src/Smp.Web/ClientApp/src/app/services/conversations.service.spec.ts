import { TestBed } from '@angular/core/testing';

import { ConversationsService } from './conversations.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Conversation } from '../models/conversation';
import { of } from 'rxjs';
import { CreateConversationRequest } from '../models/requests/create-conversation-request';

describe('ConversationsService', () => {
  const baseUrl = 'https://www.smp.org/';

  let service: ConversationsService;

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

    service = TestBed.get(ConversationsService);
  });

  describe('getConversations', () => {
    beforeEach(async() => {
      httpClientGetSpy = spyOn(TestBed.get(HttpClient), 'get')
        .and.returnValue(of(expectedConversations));
    });

    const userId = 'userId';
    const expectedConversations: Conversation[] = [
      {
        id: 'conversationId-1',
        createdAt: new Date()
      },
      {
        id: 'conversationId-2',
        createdAt: new Date()
      }
    ];

    it('should have returned the expected values', () => {
      const conversationsObserv = service.getConversations(userId);
      
      conversationsObserv.subscribe({
        next: ((conversations: Conversation[]) => {
          expect(conversations).toEqual(expectedConversations);
        })
      });
    });

    it('should have called HttpClient get correctly', () => {
      service.getConversations(userId);

      expect(httpClientGetSpy.calls.count()).toEqual(1);
      expect(httpClientGetSpy.calls.argsFor(0))
        .toEqual([
          `${baseUrl}api/Conversations/GetConversations/${userId}`,
          { headers: authHeaders }
      ]);
    });
  });

  describe('getConversationParticipants', () => {
    beforeEach(async() => {
      httpClientGetSpy = spyOn(TestBed.get(HttpClient), 'get')
        .and.returnValue(of(expectedUserIds));
    });

    const conversationId = 'conversationId';
    const expectedUserIds: string[] = [
      'userOne',
      'userTwo',
      'userThree'
    ];

    it('should have returned the expected values', () => {
      const userIdsObserv = service.getConversationParticipants(conversationId);
      
      userIdsObserv.subscribe({
        next: ((ids: string[]) => {
          expect(ids).toEqual(expectedUserIds);
        })
      });
    });

    it('should have called HttpClient get correctly', () => {
      service.getConversationParticipants(conversationId);

      expect(httpClientGetSpy.calls.count()).toEqual(1);
      expect(httpClientGetSpy.calls.argsFor(0))
        .toEqual([
          `${baseUrl}api/Conversations/GetConversationParticipants/${conversationId}`,
          { headers: authHeaders }
        ]);
    });
  });

  describe('createConversation', () => {
    beforeEach(async() => {
      httpClientPostSpy = spyOn(TestBed.get(HttpClient), 'post')
        .and.returnValue(of(expectedConversationId));
    });

    const expectedConversationId = 'conversationId';
    const convReq = {
      senderId: 'senderId'
    } as CreateConversationRequest;

    it('should have returned the expected value', () => {
      const convIdObserv = service.createConversation(convReq);

      convIdObserv.subscribe({
        next: ((id: string) => {
          expect(id).toEqual(expectedConversationId);
        })
      });
    });

    it('should have called HttpClient post correctly', () => {
      service.createConversation(convReq);

      expect(httpClientPostSpy.calls.count()).toEqual(1);
      expect(httpClientPostSpy.calls.argsFor(0))
        .toEqual([
          `${baseUrl}api/Conversations/CreateConversation`,
          convReq,
          { headers: authHeaders }
        ]);
    });
  })
});
