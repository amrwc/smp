import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormBuilder } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of } from 'rxjs';

import { ConversationComponent } from './conversation.component';
import { CreateMessageRequest } from '../models/requests/create-message-request';
import { MessagesService } from '../services/messages.service';

describe('ConversationComponent', () => {
  const msgReq: CreateMessageRequest = new CreateMessageRequest();
  msgReq.content = 'Message Content';
  msgReq.conversationId = 'conversationid-1';
  msgReq.senderId = 'userid-1';
  const userId: string = 'userid-1';
  let component: ConversationComponent;
  let fixture: ComponentFixture<ConversationComponent>;

  beforeEach(() => {
    localStorage.setItem('currentUser', JSON.stringify({ id: userId }));
    TestBed.configureTestingModule({
      declarations: [ConversationComponent],
      imports: [HttpClientTestingModule],
      providers: [FormBuilder, { provide: 'BASE_URL', useValue: 'https://www.smp.org/' }],
    }).compileComponents();

    fixture = TestBed.createComponent(ConversationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    let cnvId: string = '';
    spyOnProperty(component, 'conversationId', 'set').and.callFake((convId: string) => {
      cnvId = convId;
    });

    spyOnProperty(component, 'conversationId', 'get').and.callFake(() => cnvId);
  });

  afterEach(() => {
    localStorage.removeItem('currentUser');
  });

  describe('sendMessage()', () => {
    beforeEach(() => {
      spyOn(TestBed.inject(MessagesService), 'createMessage').and.returnValue(of(' '));
      component.form.value.content = 'Message Content';
      component.conversationId = 'conversationid-1';
    });

    it('should have called MessagesServices.createMessages() correctly', () => {
      component.sendMessage();
      expect(TestBed.inject(MessagesService).createMessage).toHaveBeenCalled();
      expect(TestBed.inject(MessagesService).createMessage).toHaveBeenCalledWith(msgReq);
    });

    it('should have reset the form', () => {
      component.sendMessage();
      expect(component.form.value.content).toEqual(null);
    });
  });

  describe('keyDown()', () => {
    let defaultPrevented: boolean;
    const nonEnterEvent: KeyboardEvent = {
      keyCode: 12,
      preventDefault: () => {
        defaultPrevented = true;
        return;
      },
    } as KeyboardEvent;

    const enterEvent: KeyboardEvent = {
      keyCode: 13,
      preventDefault: () => {
        defaultPrevented = true;
        return;
      },
    } as KeyboardEvent;

    beforeEach(() => {
      spyOn(TestBed.inject(MessagesService), 'createMessage').and.returnValue(of(' '));
      component.form.value.content = 'Message Content';
      component.conversationId = 'conversationid-1';
      defaultPrevented = false;
    });

    describe('when enter has not been pressed', () => {
      it('should not have called event.preventDefault()', () => {
        component.keyDown(nonEnterEvent);
        expect(defaultPrevented).toEqual(false);
      });
    });

    describe('when enter has been pressed', () => {
      it('should have called event.preventDefault()', () => {
        component.keyDown(enterEvent);
        expect(defaultPrevented).toEqual(true);
      });

      it('should have called MessagesServices.createMessages() correctly', () => {
        component.keyDown(enterEvent);
        expect(TestBed.inject(MessagesService).createMessage).toHaveBeenCalled();
        expect(TestBed.inject(MessagesService).createMessage).toHaveBeenCalledWith(msgReq);
      });

      it('should have reset the form', () => {
        component.keyDown(enterEvent);
        expect(component.form.value.content).toEqual(null);
      });
    });
  });
});
