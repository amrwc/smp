import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { of, Observable } from 'rxjs';
import { Conversation } from '../models/conversation';
import { ConversationsService } from './conversations.service';
import { CreateConversationRequest } from '../models/requests/create-conversation-request';

describe('ConversationsService', () => {
  const authHeaders: HttpHeaders = new HttpHeaders().set('Authorization', 'Bearer token');
  const baseUrl: string = 'https://www.smp.org/';
  let service: ConversationsService;

  beforeAll(() => {
    localStorage.setItem('currentUser', '{ "id": "id", "token": "token" }');
  });

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [{ provide: 'BASE_URL', useValue: baseUrl }],
    });
    service = TestBed.get(ConversationsService);
  });

  afterAll(() => {
    localStorage.removeItem('currentUser');
  });

  describe('getConversations()', () => {
    const userId: string = 'userId';
    const expectedConversations: Conversation[] = [
      {
        id: 'conversationId-1',
        createdAt: new Date(),
      },
      {
        id: 'conversationId-2',
        createdAt: new Date(),
      },
    ];

    beforeEach(() => {
      spyOn(TestBed.get(HttpClient), 'get').and.returnValue(of(expectedConversations));
    });

    it('should have returned the expected values', () => {
      const conversationsObserv: Observable<Conversation[]> = service.getConversations(userId);
      conversationsObserv.subscribe({
        next: (conversations: Conversation[]) => {
          expect(conversations).toEqual(expectedConversations);
        },
      });
    });

    it('should have called HttpClient.get() correctly', () => {
      service.getConversations(userId);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledWith(
        `${baseUrl}api/Conversations/GetConversations/${userId}`,
        { headers: authHeaders }
      );
    });
  });

  describe('getConversationParticipants()', () => {
    const conversationId: string = 'conversationId';
    const expectedUserIds: string[] = ['userOne', 'userTwo', 'userThree'];

    beforeEach(() => {
      spyOn(TestBed.get(HttpClient), 'get').and.returnValue(of(expectedUserIds));
    });

    it('should have returned the expected values', () => {
      const userIdsObserv: Observable<string[]> = service.getConversationParticipants(conversationId);
      userIdsObserv.subscribe({
        next: (ids: string[]) => {
          expect(ids).toEqual(expectedUserIds);
        },
      });
    });

    it('should have called HttpClient.get() correctly', () => {
      service.getConversationParticipants(conversationId);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledWith(
        `${baseUrl}api/Conversations/GetConversationParticipants/${conversationId}`,
        { headers: authHeaders }
      );
    });
  });

  describe('createConversation()', () => {
    const convReq: CreateConversationRequest = { senderId: 'senderId' } as CreateConversationRequest;
    const expectedConversationId: string = 'conversationId';

    beforeEach(async () => {
      spyOn(TestBed.get(HttpClient), 'post').and.returnValue(of(expectedConversationId));
    });

    it('should have returned the expected value', () => {
      const convIdObserv: Observable<Object> = service.createConversation(convReq);
      convIdObserv.subscribe({
        next: (id: string) => {
          expect(id).toEqual(expectedConversationId);
        },
      });
    });

    it('should have called HttpClient.post() correctly', () => {
      service.createConversation(convReq);
      expect(TestBed.get(HttpClient).post).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).post).toHaveBeenCalledWith(
        `${baseUrl}api/Conversations/CreateConversation`,
        convReq,
        { headers: authHeaders }
      );
    });
  });
});
