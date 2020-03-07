import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ConversationsService } from '../services/conversations.service';
import { UsersService } from '../services/users.service';
import { GlobalHelper } from '../helpers/global';
import { MessagesComponent } from './messages.component';
import { MessagesService } from '../services/messages.service';
import { ConversationComponent } from '../conversation/conversation.component';
import { FormBuilder } from '@angular/forms';

describe('MessagesComponent', () => {
  let component: MessagesComponent;
  let fixture: ComponentFixture<MessagesComponent>;

  beforeEach(() => {
    localStorage.setItem('currentUser', '{ "id": "id" }');
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

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
