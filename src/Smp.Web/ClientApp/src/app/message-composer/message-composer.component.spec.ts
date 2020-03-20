import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { of, throwError } from 'rxjs';
import { AngularMaterialModule } from '../angular-material.module';
import { ConversationsService } from '../services/conversations.service';
import { CreateConversationRequest } from '../models/requests/create-conversation-request';
import { MessageComposerComponent } from './message-composer.component';
import { Relationship } from '../models/relationship';
import { RelationshipType } from '../models/relationship-type.enum';
import { RelationshipsService } from '../services/relationships.service';
import { User } from '../models/user';
import { UsersService } from '../services/users.service';

describe('MessageComposerComponent', () => {
  const friends: User[] = [
    { id: 'userid-1', fullName: 'Jack Ryan' },
    { id: 'userid-2', fullName: 'Tom Cruise' },
  ] as User[];
  const userId: string = 'userid-1';
  let component: MessageComposerComponent;
  let fixture: ComponentFixture<MessageComposerComponent>;

  beforeEach(() => {
    localStorage.setItem('currentUser', JSON.stringify({ id: userId }));
    TestBed.configureTestingModule({
      declarations: [MessageComposerComponent],
      imports: [HttpClientTestingModule, FormsModule, AngularMaterialModule, BrowserAnimationsModule],
      providers: [{ provide: 'BASE_URL', useValue: 'https://www.smp.org/' }],
    }).compileComponents();
    fixture = TestBed.createComponent(MessageComposerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    localStorage.removeItem('currentUser');
  });

  describe('filterFriends()', () => {
    beforeEach(() => {
      component.filteredFriends = null;
      component.friends = friends;
    });

    it('should have set filteredFriends correctly', () => {
      component.filterFriends(' Rya');
      expect(component.filteredFriends).toEqual([friends[0]]);
      component.filterFriends('om C');
      expect(component.filteredFriends).toEqual([friends[1]]);
    });
  });

  describe('displayFriend()', () => {
    beforeEach(() => {
      component.friends = friends;
    });

    it('should have returned the expected value', () => {
      const friend = component.displayFriend('userid-2');
      expect(friend).toEqual('Tom Cruise');
      const nonFriend = component.displayFriend('userid-3');
      expect(nonFriend).toEqual('Unknown');
    });
  });

  describe('sendMessage()', () => {
    beforeEach(() => {
      spyOn(TestBed.inject(ConversationsService), 'createConversation');
      spyOn(component.conversationReady, 'emit');
      component.friends = friends;
    });

    describe('if the receiver is not a friend', () => {
      beforeEach(() => {
        component.createConversationRequest.receiverId = 'userid-3';
      });

      it('should not have called ConversationsService.createConversation()', () => {
        component.sendMessage();
        expect(TestBed.get(ConversationsService).createConversation).toHaveBeenCalledTimes(0);
      });
    });

    describe('if ConversationsService.createConversation() returns a conflict error', () => {
      const convReq: CreateConversationRequest = new CreateConversationRequest();
      convReq.receiverId = friends[1].id;
      convReq.senderId = userId;

      beforeEach(() => {
        component.createConversationRequest.receiverId = friends[1].id;
        TestBed.get(ConversationsService).createConversation.and.returnValue(
          throwError({ status: 409, error: 'conversationid-error' })
        );
      });

      it('should have called ConversationsService.createConversation() correctly', () => {
        component.sendMessage();
        expect(TestBed.get(ConversationsService).createConversation).toHaveBeenCalledTimes(1);
        expect(TestBed.get(ConversationsService).createConversation).toHaveBeenCalledWith(convReq);
      });

      it('should have emitted a conversationReady event', () => {
        component.sendMessage();
        expect(component.conversationReady.emit).toHaveBeenCalledTimes(1);
        expect(component.conversationReady.emit).toHaveBeenCalledWith('conversationid-error');
      });

      it('should have set variables correctly', () => {
        component.sendMessage();
        expect(component.loading).toEqual(false);
      });
    });

    describe('if it completes successfully', () => {
      let convReq: CreateConversationRequest = new CreateConversationRequest();

      beforeEach(() => {
        component.createConversationRequest.receiverId = friends[1].id;
        TestBed.get(ConversationsService).createConversation.and.returnValue(of('conversationid-1'));
        convReq.receiverId = friends[1].id;
        convReq.senderId = userId;
      });

      it('should have called ConversationsService.createConversation() correctly', () => {
        component.sendMessage();
        expect(TestBed.get(ConversationsService).createConversation).toHaveBeenCalledTimes(1);
        expect(TestBed.get(ConversationsService).createConversation).toHaveBeenCalledWith(convReq);
      });

      it('should have emitted a conversationReady event', () => {
        component.sendMessage();
        expect(component.conversationReady.emit).toHaveBeenCalledTimes(1);
        expect(component.conversationReady.emit).toHaveBeenCalledWith('conversationid-1');
      });

      it('should have set variables correctly', () => {
        component.sendMessage();
        expect(component.loading).toEqual(false);
      });
    });
  });

  describe('ngOnInit()', () => {
    describe('when it executes successfully', () => {
      const relationships: Relationship[] = [
        {
          userOneId: userId,
          userTwoId: 'userid-2',
          relationshipType: RelationshipType.Friend,
          createdAt: new Date(),
        },
        {
          userOneId: userId,
          userTwoId: 'userid-3',
          relationshipType: RelationshipType.Friend,
          createdAt: new Date(),
        },
      ] as Relationship[];
      const user: User = { id: 'userid-2' } as User;

      beforeEach(() => {
        spyOn(TestBed.inject(RelationshipsService), 'getRelationships').and.returnValue(of(relationships));
        spyOn(TestBed.inject(UsersService), 'getUser').and.returnValue(of(user));
      });

      it('should set variable values correctly', () => {
        component.ngOnInit();
        expect(component.createConversationRequest.senderId).toEqual(userId);
        expect(component.friends).toEqual([user, user]);
        expect(component.filteredFriends).toEqual([user, user]);
      });

      it('should have called RelationshipsService.getRelationships() correctly', () => {
        component.ngOnInit();
        expect(TestBed.get(RelationshipsService).getRelationships).toHaveBeenCalledTimes(1);
        expect(TestBed.get(RelationshipsService).getRelationships).toHaveBeenCalledWith(
          userId,
          RelationshipType.Friend
        );
      });

      it('should have called UsersService.getUser() correctly', () => {
        component.ngOnInit();
        expect(TestBed.get(UsersService).getUser).toHaveBeenCalledTimes(2);
        expect(TestBed.get(UsersService).getUser).toHaveBeenCalledWith('userid-2');
        expect(TestBed.get(UsersService).getUser).toHaveBeenCalledWith('userid-3');
      });
    });
  });
});
