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

  beforeEach(() => {
    localStorage.setItem('currentUser', '{ "id": "userid-1" }');
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
