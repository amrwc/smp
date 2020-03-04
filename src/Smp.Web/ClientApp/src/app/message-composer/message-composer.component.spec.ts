import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageComposerComponent } from './message-composer.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RelationshipsService } from '../services/relationships.service';
import { ConversationsService } from '../services/conversations.service';
import { UsersService } from '../services/users.service';
import { GlobalHelper } from '../helpers/global';
import { FormsModule } from '@angular/forms';
import { AngularMaterialModule } from '../angular-material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('MessageComposerComponent', () => {
  let component: MessageComposerComponent;
  let fixture: ComponentFixture<MessageComposerComponent>;

  beforeEach(() => {
    localStorage.setItem('currentUser', '{ "id": "id" }');
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
  });

  afterEach(() => {
    localStorage.removeItem('currentUser');
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(MessageComposerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
