import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConversationComponent } from './conversation.component';
import { ConversationsService } from '../services/conversations.service';
import { MessagesService } from '../services/messages.service';
import { UsersService } from '../services/users.service';
import { GlobalHelper } from '../helpers/global';
import { FormBuilder } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of } from 'rxjs';
import { CreateMessageRequest } from '../models/requests/create-message-request';

describe('ConversationComponent', () => {
  let component: ConversationComponent;
  let fixture: ComponentFixture<ConversationComponent>;
  const userId = 'userid-1'

  const msgReq = new CreateMessageRequest();
  msgReq.content = 'Message Content';
  msgReq.conversationId = 'conversationid-1';
  msgReq.senderId = 'userid-1';

  beforeEach(() => {
    localStorage.setItem('currentUser', JSON.stringify({ id: userId }));
    TestBed.configureTestingModule({
      declarations: [ConversationComponent],
      imports: [HttpClientTestingModule],
      providers: [ConversationsService, MessagesService, UsersService, GlobalHelper, FormBuilder, { provide: 'BASE_URL', useValue: "https://www.smp.org/" }]
    }).compileComponents();

    fixture = TestBed.createComponent(ConversationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    let cnvId = '';

    spyOnProperty(component, 'conversationId', 'set')
      .and.callFake((convId: string) => {
        cnvId = convId;
    });

    spyOnProperty(component, 'conversationId', 'get')
      .and.callFake(() => {
        return cnvId;
    });
  });

  afterEach(() => {
    localStorage.removeItem('currentUser');
  });

  describe('sendMessage', () => {
    beforeEach(() => {
      spyOn(TestBed.inject(MessagesService), "createMessage")
        .and.returnValue(of(' '));

      component.form.value.content = 'Message Content';
      component.conversationId = 'conversationid-1';
    });

    it('should call MessagesServices createMessages correctly', () => {
      component.sendMessage();

      expect(TestBed.inject(MessagesService).createMessage).toHaveBeenCalled();
      expect(TestBed.inject(MessagesService).createMessage).toHaveBeenCalledWith(msgReq);
    });

    it('should reset the form', () => {
      component.sendMessage();

      expect(component.form.value.content).toEqual(null);
    });
  });

  describe('keyDown', () => {
    let defaultPrevented: boolean;

    const nonEnterEvent = {
      keyCode: 12,
      preventDefault: () => {
        defaultPrevented = true;
        return;
      }
    } as KeyboardEvent;

    const enterEvent = {
      keyCode: 13,
      preventDefault: () => {
        defaultPrevented = true;
        return;
      }
    } as KeyboardEvent;

    beforeEach(() => {
      spyOn(TestBed.inject(MessagesService), "createMessage")
        .and.returnValue(of(' '));

      component.form.value.content = 'Message Content';
      component.conversationId = 'conversationid-1';

      defaultPrevented = false;
    });

    describe('when enter has not been pressed', () => {
      it('should not call event.preventDefault', () => {
        component.keyDown(nonEnterEvent);

        expect(defaultPrevented).toEqual(false);
      });
    });

    describe('when enter has been pressed', () => {
      it('should call event.preventDefault', () => {
        component.keyDown(enterEvent);

        expect(defaultPrevented).toEqual(true);
      });

      it('should call MessagesServices createMessages correctly', () => {
        component.keyDown(enterEvent);
  
        expect(TestBed.inject(MessagesService).createMessage).toHaveBeenCalled();
        expect(TestBed.inject(MessagesService).createMessage).toHaveBeenCalledWith(msgReq);
      });
  
      it('should reset the form', () => {
        component.keyDown(enterEvent);
  
        expect(component.form.value.content).toEqual(null);
      });
    });
  });
});
