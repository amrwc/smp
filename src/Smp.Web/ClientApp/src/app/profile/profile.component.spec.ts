import { ActivatedRoute } from '@angular/router';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { throwError } from 'rxjs';
import { GlobalHelper } from '../helpers/global';
import { ProfileComponent } from './profile.component';
import { RelationshipsService } from '../services/relationships.service';
import { RequestsService } from '../services/requests.service';
import { UsersService } from '../services/users.service';
import { RequestType } from '../models/request-type.enum';

describe('ProfileComponent', () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;

  beforeEach(() => {
    localStorage.setItem('currentUser', '{ "id": "id" }');
    TestBed.configureTestingModule({
      declarations: [ProfileComponent],
      imports: [HttpClientTestingModule, RouterTestingModule],
      providers: [
        UsersService,
        RequestsService,
        RelationshipsService,
        GlobalHelper,
        { provide: 'BASE_URL', useValue: 'https://www.smp.org/' },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: {
                get: anyArg => 'bob',
              },
            },
          },
        },
      ],
    }).compileComponents();
    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    localStorage.removeItem('currentUser');
  });

  describe('ngOnInit', () => {
    it("should show 'Add Friend' button", () => {
      const requestsServiceGetRequestSpy = spyOn(TestBed.get(RequestsService), 'getRequest').and.returnValue(
        throwError({ status: 404 })
      );
      const relationshipsServiceGetRelationshipSpy = spyOn(
        TestBed.get(RelationshipsService),
        'getRelationship'
      ).and.returnValue(throwError(new Error()));
      component.ngOnInit();
      expect(component.showAddFriendButton).toBeTruthy();
      expect(component.friends).toBeFalsy();
      expect(requestsServiceGetRequestSpy.calls.count()).toEqual(1);
      expect(requestsServiceGetRequestSpy.calls.argsFor(0)).toEqual([
        JSON.parse(localStorage.getItem('currentUser')).id,
        'bob',
        RequestType.Friend,
      ]);
      expect(relationshipsServiceGetRelationshipSpy.calls.count()).toEqual(1);
      expect(relationshipsServiceGetRelationshipSpy.calls.argsFor(0)).toEqual([
        'bob',
        JSON.parse(localStorage.getItem('currentUser')).id,
        RequestType.Friend,
      ]);
    });
  });
});
