import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConversationComponent } from './conversation.component';
import { ConversationsService } from '../services/conversations.service';
import { MessagesService } from '../services/messages.service';
import { UsersService } from '../services/users.service';
import { GlobalHelper } from '../helpers/global';
import { FormBuilder } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('ConversationComponent', () => {
  let component: ConversationComponent;
  let fixture: ComponentFixture<ConversationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ConversationComponent],
      imports: [HttpClientTestingModule],
      providers: [ConversationsService, MessagesService, UsersService, GlobalHelper, FormBuilder, { provide: 'BASE_URL', useValue: "https://www.smp.org/" }]
    }).compileComponents();

    fixture = TestBed.createComponent(ConversationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
