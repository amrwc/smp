import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ConversationsService } from '../services/conversations.service';
import { UsersService } from '../services/users.service';
import { GlobalHelper } from '../helpers/global';
import { MessagesComponent } from './messages.component';
import { MessagesService } from '../services/messages.service';
import { ConversationComponent } from '../conversation/conversation.component';
import { FormBuilder } from '@angular/forms';
import { ExtendedConversation } from '../models/conversation';
import { User } from '../models/user';
import { FriendlyMessage } from '../models/message';
import { of } from 'rxjs';

describe('MessagesComponent', () => {
  let component: MessagesComponent;
  let fixture: ComponentFixture<MessagesComponent>;

  const users = [
    { id: 'userid-1', fullName: 'Jack Ryan', profilePictureUrl: 'website.com' } as User,
    { id: 'userid-2', fullName: 'Tom Cruise', profilePictureUrl: 'website.co.uk' } as User,
    { id: 'userid-3', fullName: 'Leonardo DiCaprio', profilePictureUrl: 'website.org' } as User
  ];

  const participantsOne = new Map<string, User>();
  participantsOne.set(users[0].id, users[0]);
  participantsOne.set(users[1].id, users[1]);

  const participantsTwo = new Map<string, User>();
  participantsOne.set(users[0].id, users[0]);
  participantsTwo.set(users[2].id, users[2]);

  const lastMessages = [
    {
      senderId: users[0].id,
      sender: users[0]
    } as FriendlyMessage,
    {
      senderId: users[2].id,
      sender: users[2]
    } as FriendlyMessage
  ];

  const conversations = [
    { 
      id: 'conversationid-1',
      participants: participantsOne,
      lastMessage: lastMessages[0]
    } as ExtendedConversation,
    { 
      id: 'conversationid-2',
      participants: participantsTwo,
      lastMessage: lastMessages[1]
    } as ExtendedConversation
  ];

  const unloadedConversation = { 
      id: 'conversationid-3',
      participants: participantsOne,
      lastMessage: lastMessages[0]
    } as ExtendedConversation;

  beforeEach(() => {
    localStorage.setItem('currentUser', JSON.stringify({ id: users[0].id }));
    TestBed.configureTestingModule({
      declarations: [ MessagesComponent, ConversationComponent ],
      imports: [HttpClientTestingModule],
      providers: [
        GlobalHelper,
        ConversationsService,
        MessagesService,
        UsersService,
        FormBuilder,
        { provide: 'BASE_URL', useValue: "https://www.smp.org/" }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MessagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    localStorage.removeItem('currentUser');
  });

  describe('ngOnInit', () => {

  });

  describe('getConversationName', () => {
    beforeEach(() => {
      component.conversations = conversations;
    });

    it('should return the expected value', () => {
      const conversationName = component.getConversationName('conversationid-1');
      
      expect(conversationName).toEqual('Tom Cruise');
    });
  });

  describe('getConversationPicture', () => {
    beforeEach(() => {
      component.conversations = conversations;
    });

    it('should return the expected value', () => {
      const conversationName = component.getConversationPicture('conversationid-1');
      
      expect(conversationName).toEqual('website.co.uk');
    });
  });

  describe('getLastMessageSender', () => {
    beforeEach(() => {
      component.conversations = conversations;
    });

    it('should return the expected value', () => {
      let lastMessageSender = component.getLastMessageSender('conversationid-1');
      expect(lastMessageSender).toEqual('You');

      lastMessageSender = component.getLastMessageSender('conversationid-2');
      expect(lastMessageSender).toEqual('Leonardo DiCaprio');
    });
  });

  describe('loadConversation', () => {
    let cnvSvcGetConversationsSpy: jasmine.Spy;

    beforeEach(() => {
      cnvSvcGetConversationsSpy = spyOn(TestBed.inject(ConversationsService), 'getConversations');
      component.conversations = conversations;
    });

    describe('if the conversation has not already been loaded', () => {
      let cnvSvcGetConversationParticipantsSpy: jasmine.Spy;
      let usrSvcGetUserSpy: jasmine.Spy;
      let msgSvcGetMessagesFromConversationSpy: jasmine.Spy;

      const convId = unloadedConversation.id;

      beforeEach(() => {
        component.conversation.conversationId = unloadedConversation.id;
        cnvSvcGetConversationsSpy.and.returnValue(of([unloadedConversation]));
        cnvSvcGetConversationParticipantsSpy = spyOn(TestBed.inject(ConversationsService), 'getConversationParticipants')
          .and.returnValue(of([users[0].id, users[1].id]));
        usrSvcGetUserSpy = spyOn(TestBed.inject(UsersService), 'getUser')
          .and.returnValue(of(users[0]));
        msgSvcGetMessagesFromConversationSpy = spyOn(TestBed.inject(MessagesService), 'getMessagesFromConversation')
          .and.returnValue(of(lastMessages[0]));
      });

      it('should set variables correctly', () => {
        component.loadConversation(convId);

        expect(component.startNewConversation).toEqual(false);
        expect(component.conversation.conversationId).toEqual(convId);
        expect(component.conversations).toEqual([unloadedConversation]);
      });

      it('should call ConversationsService getConversations',() => {
        component.loadConversation(convId);

        expect(cnvSvcGetConversationsSpy.calls.count()).toEqual(1);
        expect(cnvSvcGetConversationsSpy.calls.argsFor(0)).toEqual([users[0].id]);
      });

      it('should call ConversationsService getConversationParticipants', () => {
        component.loadConversation(convId);

        expect(cnvSvcGetConversationParticipantsSpy.calls.count()).toEqual(1);
        expect(cnvSvcGetConversationParticipantsSpy.calls.argsFor(0)).toEqual([unloadedConversation.id]);
      });

      it('should call UsersService getUser', () => {
        component.loadConversation(convId);

        expect(usrSvcGetUserSpy.calls.count()).toEqual(2);
        expect(usrSvcGetUserSpy.calls.argsFor(0)).toEqual([users[0].id]);
        expect(usrSvcGetUserSpy.calls.argsFor(1)).toEqual([users[1].id]);
      });

      it('should call MessagesService getMessagesFromConversation', () => {
        component.loadConversation(convId);

        expect(msgSvcGetMessagesFromConversationSpy.calls.count()).toEqual(1);
        expect(msgSvcGetMessagesFromConversationSpy.calls.argsFor(0)).toEqual([
          convId, 1, 0
        ]);
      });
    });

    describe('if the conversation has already been loaded', () => {
      it('should set variables correctly', () => {
        component.loadConversation(conversations[0].id);

        expect(component.startNewConversation).toEqual(false);
        //expect(component.conversation.conversationId).toEqual(conversations[0].id);
      });

      it('should not call ConversationsService getConversations', () => {
        component.loadConversation(conversations[0].id);

        expect(cnvSvcGetConversationsSpy.calls.count()).toEqual(0);
      });
    });
  });

  describe('showStartConversation', () => {
    beforeEach(() => {
      component.startNewConversation = false;
    });

    it('should set variables correctly', () => {
      component.showStartConversation();

      expect(component.startNewConversation).toEqual(true);
    });
  });
});
