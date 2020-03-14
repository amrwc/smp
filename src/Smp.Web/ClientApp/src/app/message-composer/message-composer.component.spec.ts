import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MessageComposerComponent } from './message-composer.component';
import { RelationshipsService } from '../services/relationships.service';
import { ConversationsService } from '../services/conversations.service';
import { UsersService } from '../services/users.service';
import { AngularMaterialModule } from '../angular-material.module';
import { of, throwError } from 'rxjs';
import { Relationship } from '../models/relationship';
import { RelationshipType } from '../models/relationship-type.enum';
import { User } from '../models/user';
import { CreateConversationRequest } from '../models/requests/create-conversation-request';

describe('MessageComposerComponent', () => {
  let component: MessageComposerComponent;
  let fixture: ComponentFixture<MessageComposerComponent>;

  const userId = 'userid-1'

  const friends = [
    { id: 'userid-1', fullName: 'Jack Ryan' } as User,
    { id: 'userid-2', fullName: 'Tom Cruise' } as User
  ];

  beforeEach(() => {
    localStorage.setItem('currentUser', JSON.stringify({ id: userId }));
    TestBed.configureTestingModule({
      declarations: [ MessageComposerComponent ],
      imports: [HttpClientTestingModule, FormsModule, AngularMaterialModule, BrowserAnimationsModule],
      providers: [
        { provide: 'BASE_URL', useValue: "https://www.smp.org/" }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MessageComposerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    localStorage.removeItem('currentUser');
  });

  describe('filterFriends', () => {
    beforeEach(() => {
      component.filteredFriends = null;
      component.friends = friends;
    });

    it('should set filteredFriends correctly', () => {
      component.filterFriends(' Rya');
      expect(component.filteredFriends).toEqual([friends[0]]);

      component.filterFriends('om C');
      expect(component.filteredFriends).toEqual([friends[1]]);
    });
  });

  describe('displayFriend', () => {
    beforeEach(() => {
      component.friends = friends;
    });

    it('should return the expected value', () => {
      const friend = component.displayFriend('userid-2');
      expect(friend).toEqual('Tom Cruise');

      const nonFriend = component.displayFriend('userid-3');
      expect(nonFriend).toEqual('Unknown');
    });
  });

  describe('sendMessage', () => {
    let cnvSvcCreateConversationSpy: jasmine.Spy;
    let conversationReadyEmitSpy: jasmine.Spy;

    beforeEach(() => {
      cnvSvcCreateConversationSpy = spyOn(TestBed.inject(ConversationsService), 'createConversation')
      conversationReadyEmitSpy = spyOn(component.conversationReady, 'emit');
      component.friends = friends;
    });

    describe('if the receiver is not a friend', () => {
      beforeEach(() => {
        component.createConversationRequest.receiverId = 'userid-3';
      });

      it('should not call ConversationsService createConversation', () => {
        component.sendMessage();

        expect(cnvSvcCreateConversationSpy.calls.count()).toEqual(0);
      });
    });

    describe('if ConversationsService createConversation returns a conflict error', () => {
      let convReq = new CreateConversationRequest();

      beforeEach(() => {
        component.createConversationRequest.receiverId = friends[1].id;
        cnvSvcCreateConversationSpy
          .and.returnValue(throwError({ status: 409, error: 'conversationid-error' }));
        convReq.receiverId = friends[1].id;
        convReq.senderId = userId;
      });

      it('should call ConversationsService createConversation correctly', () => {
        component.sendMessage();

        expect(cnvSvcCreateConversationSpy.calls.count()).toEqual(1);
        expect(cnvSvcCreateConversationSpy.calls.argsFor(0)).toEqual([convReq]);
      });

      it('should emit a conversationReady event', () => {
        component.sendMessage();

        expect(conversationReadyEmitSpy.calls.count()).toEqual(1);
        expect(conversationReadyEmitSpy.calls.argsFor(0)).toEqual(['conversationid-error']);
      });

      it('should have set variables correctly', () => {
        component.sendMessage();

        expect(component.loading).toEqual(false);
      });
    });

    describe('if it completes successfully', () => {
      let convReq = new CreateConversationRequest();

      beforeEach(() => {
        component.createConversationRequest.receiverId = friends[1].id;
        cnvSvcCreateConversationSpy
          .and.returnValue(of('conversationid-1'));
        convReq.receiverId = friends[1].id;
        convReq.senderId = userId;
      });

      it('should call ConversationsService createConversation correctly', () => {
        component.sendMessage();

        expect(cnvSvcCreateConversationSpy.calls.count()).toEqual(1);
        expect(cnvSvcCreateConversationSpy.calls.argsFor(0)).toEqual([convReq]);
      });

      it('should emit a conversationReady event', () => {
        component.sendMessage();

        expect(conversationReadyEmitSpy.calls.count()).toEqual(1);
        expect(conversationReadyEmitSpy.calls.argsFor(0)).toEqual(['conversationid-1']);
      });

      it('should have set variables correctly', () => {
        component.sendMessage();

        expect(component.loading).toEqual(false);
      });
    });
  });

  describe('ngOnInit', () => {
    let relSvcGetRelationshipsSpy: jasmine.Spy;
    let usrSvcGetUserSpy: jasmine.Spy;

    describe('when it executes successfully', () => {
      const relationships: Relationship[] = [
        {
          userOneId: userId,
          userTwoId: 'userid-2',
          relationshipType: RelationshipType.Friend,
          createdAt: new Date()
        } as Relationship,
        {
          userOneId: userId,
          userTwoId: 'userid-3',
          relationshipType: RelationshipType.Friend,
          createdAt: new Date()
        } as Relationship
      ];

      const user: User = { id: 'userid-2' } as User;

      beforeEach(() => {
        relSvcGetRelationshipsSpy = spyOn(TestBed.inject(RelationshipsService), 'getRelationships')
          .and.returnValue(of(relationships));
        usrSvcGetUserSpy = spyOn(TestBed.inject(UsersService), 'getUser')
          .and.returnValue(of(user));
      });

      it('should set variable values correctly', () => {
        component.ngOnInit();

        expect(component.createConversationRequest.senderId).toEqual(userId);
        expect(component.friends).toEqual([user, user]);
        expect(component.filteredFriends).toEqual([user, user]);
      });

      it('should call RelationshipsService getRelationships correctly', () => {
        component.ngOnInit();

        expect(relSvcGetRelationshipsSpy.calls.count()).toEqual(1);
        expect(relSvcGetRelationshipsSpy.calls.argsFor(0)).toEqual([ userId, RelationshipType.Friend ]);
      });

      it('should call UsersService getUser correctly', () => {
        component.ngOnInit();

        expect(usrSvcGetUserSpy.calls.count()).toEqual(2);
        expect(usrSvcGetUserSpy.calls.allArgs()).toEqual([['userid-2'], ['userid-3']]);
      });
    });
  });
});
