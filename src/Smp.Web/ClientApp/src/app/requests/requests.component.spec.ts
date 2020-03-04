import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestsComponent } from './requests.component';
import { GlobalHelper } from '../helpers/global';
import { RequestsService } from '../services/requests.service';
import { UsersService } from '../services/users.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('RequestsComponent', () => {
  let component: RequestsComponent;
  let fixture: ComponentFixture<RequestsComponent>;

  beforeEach(() => {
    localStorage.setItem('currentUser', '{ "id": "id" }');

    TestBed.configureTestingModule({
      declarations: [ RequestsComponent ],
      imports: [HttpClientTestingModule],
      providers: [
        GlobalHelper,
        RequestsService,
        UsersService,
        { provide: 'BASE_URL', useValue: "https://www.smp.org/" }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RequestsComponent);
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
