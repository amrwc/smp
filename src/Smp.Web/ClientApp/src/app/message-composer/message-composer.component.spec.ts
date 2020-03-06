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

    fixture = TestBed.createComponent(MessageComposerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    localStorage.removeItem('currentUser');
  })

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
