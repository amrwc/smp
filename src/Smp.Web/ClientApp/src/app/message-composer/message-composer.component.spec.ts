import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MessageComposerComponent } from './message-composer.component';
import { RelationshipsService } from '../services/relationships.service';
import { ConversationsService } from '../services/conversations.service';
import { UsersService } from '../services/users.service';
import { GlobalHelper } from '../helpers/global';
import { AngularMaterialModule } from '../angular-material.module';
import { of } from 'rxjs';
import { Relationship } from '../models/relationship';
import { RelationshipType } from '../models/relationship-type.enum';
import { User } from '../models/user';

describe('MessageComposerComponent', () => {
  let component: MessageComposerComponent;
  let fixture: ComponentFixture<MessageComposerComponent>;

  const userId = 'userid-1'

  beforeEach(() => {
    localStorage.setItem('currentUser', JSON.stringify({ id: userId }));
    TestBed.configureTestingModule({
      declarations: [ MessageComposerComponent ],
      imports: [HttpClientTestingModule, FormsModule, AngularMaterialModule, BrowserAnimationsModule],
      providers: [
        GlobalHelper,
        RelationshipsService,
        ConversationsService,
        UsersService,
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
