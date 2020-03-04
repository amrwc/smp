import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ConversationsService } from '../services/conversations.service';
import { UsersService } from '../services/users.service';
import { GlobalHelper } from '../helpers/global';
import { MessagesComponent } from './messages.component';
import { MessagesService } from '../services/messages.service';

describe('MessagesComponent', () => {
  let component: MessagesComponent;
  let fixture: ComponentFixture<MessagesComponent>;

  beforeEach(() => {
    localStorage.setItem('currentUser', '{ "id": "id" }');
    TestBed.configureTestingModule({
      declarations: [ MessagesComponent ],
      imports: [HttpClientTestingModule],
      providers: [
        GlobalHelper,
        ConversationsService,
        MessagesService,
        UsersService,
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
