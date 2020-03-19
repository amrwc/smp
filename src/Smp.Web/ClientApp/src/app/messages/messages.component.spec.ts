import { FormBuilder } from '@angular/forms';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { of } from 'rxjs';
import { ConversationComponent } from '../conversation/conversation.component';
import { ConversationsService } from '../services/conversations.service';
import { ExtendedConversation } from '../models/conversation';
import { FriendlyMessage } from '../models/message';
import { MessagesComponent } from './messages.component';
import { MessagesService } from '../services/messages.service';
import { User } from '../models/user';
import { UsersService } from '../services/users.service';

describe('MessagesComponent', () => {
  const participantsOne = new Map<string, User>();
  const participantsTwo = new Map<string, User>();
  const users: User[] = [
    { id: 'userid-1', fullName: 'Jack Ryan', profilePictureUrl: 'website.com' },
    { id: 'userid-2', fullName: 'Tom Cruise', profilePictureUrl: 'website.co.uk' },
    { id: 'userid-3', fullName: 'Leonardo DiCaprio', profilePictureUrl: 'website.org' },
  ] as User[];
  const lastMessages: FriendlyMessage[] = [
    { senderId: users[0].id, sender: users[0] },
    { senderId: users[2].id, sender: users[2] },
  ] as FriendlyMessage[];
  const conversations: ExtendedConversation[] = [
    {
      id: 'conversationid-1',
      participants: participantsOne,
      lastMessage: lastMessages[0],
    },
    {
      id: 'conversationid-2',
      participants: participantsTwo,
      lastMessage: lastMessages[1],
    },
  ] as ExtendedConversation[];
  const unloadedConversation: ExtendedConversation = {
    id: 'conversationid-3',
    participants: participantsOne,
    lastMessage: lastMessages[0],
  } as ExtendedConversation;
  let component: MessagesComponent;
  let fixture: ComponentFixture<MessagesComponent>;

  beforeAll(() => {
    participantsOne.set(users[0].id, users[0]);
    participantsOne.set(users[1].id, users[1]);
    participantsOne.set(users[0].id, users[0]);
    participantsTwo.set(users[2].id, users[2]);
  });

  beforeEach(() => {
    localStorage.setItem('currentUser', JSON.stringify({ id: users[0].id }));
    TestBed.configureTestingModule({
      declarations: [MessagesComponent, ConversationComponent],
      imports: [HttpClientTestingModule],
      providers: [FormBuilder, { provide: 'BASE_URL', useValue: 'https://www.smp.org/' }],
    }).compileComponents();
    fixture = TestBed.createComponent(MessagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    localStorage.removeItem('currentUser');
  });

  describe('ngOnInit()', () => {});

  describe('getConversationName()', () => {
    it('should have returned the expected value', () => {
      component.conversations = conversations;
      const conversationName = component.getConversationName('conversationid-1');
      expect(conversationName).toEqual('Tom Cruise');
    });
  });

  describe('getConversationPicture()', () => {
    it('should have returned the expected value', () => {
      component.conversations = conversations;
      const conversationName = component.getConversationPicture('conversationid-1');
      expect(conversationName).toEqual('website.co.uk');
    });
  });

  describe('getLastMessageSender()', () => {
    it('should have returned the expected value', () => {
      component.conversations = conversations;
      let lastMessageSender = component.getLastMessageSender('conversationid-1');
      expect(lastMessageSender).toEqual('You');
      lastMessageSender = component.getLastMessageSender('conversationid-2');
      expect(lastMessageSender).toEqual('Leonardo DiCaprio');
    });
  });

  describe('loadConversation()', () => {
    beforeEach(() => {
      component.conversations = conversations;
      let cnvId = '';
      spyOnProperty(component.conversation, 'conversationId', 'set').and.callFake((convId: string) => {
        cnvId = convId;
      });
      spyOnProperty(component.conversation, 'conversationId', 'get').and.callFake(() => {
        return cnvId;
      });
    });

    describe('if the conversation has not already been loaded', () => {
      const convId = unloadedConversation.id;

      beforeEach(() => {
        spyOn(TestBed.inject(ConversationsService), 'getConversations').and.returnValue(of([unloadedConversation]));
        spyOn(TestBed.inject(ConversationsService), 'getConversationParticipants').and.returnValue(
          of([users[0].id, users[1].id])
        );
        spyOn(TestBed.inject(UsersService), 'getUser').and.returnValue(of(users[0]));
        spyOn(TestBed.inject(MessagesService), 'getMessagesFromConversation').and.returnValue(of(lastMessages[0]));
        component.conversation.conversationId = unloadedConversation.id;
        component.loadConversation(convId);
      });

      it('should set variables correctly', () => {
        expect(component.startNewConversation).toEqual(false);
        expect(component.conversation.conversationId).toEqual(convId);
        expect(component.conversations).toEqual([unloadedConversation]);
      });

      it('should call ConversationsService.getConversations()', () => {
        expect(TestBed.get(ConversationsService).getConversations).toHaveBeenCalledTimes(1);
        expect(TestBed.get(ConversationsService).getConversations).toHaveBeenCalledWith(users[0].id);
      });

      it('should call ConversationsService.getConversationParticipants()', () => {
        expect(TestBed.get(ConversationsService).getConversationParticipants).toHaveBeenCalledTimes(1);
        expect(TestBed.get(ConversationsService).getConversationParticipants).toHaveBeenCalledWith(
          unloadedConversation.id
        );
      });

      it('should call UsersService.getUser()', () => {
        expect(TestBed.get(UsersService).getUser).toHaveBeenCalledTimes(2);
        expect(TestBed.get(UsersService).getUser).toHaveBeenCalledWith(users[0].id);
        expect(TestBed.get(UsersService).getUser).toHaveBeenCalledWith(users[1].id);
      });

      it('should call MessagesService.getMessagesFromConversation()', () => {
        expect(TestBed.get(MessagesService).getMessagesFromConversation).toHaveBeenCalledTimes(1);
        expect(TestBed.get(MessagesService).getMessagesFromConversation).toHaveBeenCalledWith(convId, 1, 0);
      });
    });

    describe('if the conversation has already been loaded', () => {
      it('should set variables correctly', () => {
        component.loadConversation(conversations[0].id);
        expect(component.startNewConversation).toEqual(false);
        expect(component.conversation.conversationId).toEqual(conversations[0].id);
      });

      it('should not call ConversationsService.getConversations()', () => {
        spyOn(TestBed.inject(ConversationsService), 'getConversations');
        component.loadConversation(conversations[0].id);
        expect(TestBed.get(ConversationsService).getConversations).toHaveBeenCalledTimes(0);
      });
    });
  });

  describe('showStartConversation()', () => {
    it('should set variables correctly', () => {
      component.startNewConversation = false;
      component.showStartConversation();
      expect(component.startNewConversation).toEqual(true);
    });
  });
});
