import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { SettingsComponent } from './settings.component';

describe('SettingsComponent', () => {
  let component: SettingsComponent;
  let fixture: ComponentFixture<SettingsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SettingsComponent],
      imports: [RouterTestingModule],
    }).compileComponents();
    fixture = TestBed.createComponent(SettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  describe('signOut', () => {
    it('should navigate to /sign-in', () => {
      const routerNavigateSpy: jasmine.Spy = spyOn(TestBed.get(Router), 'navigate');
      localStorage.setItem('currentUser', JSON.stringify({ currentUser: 'bob' }));
      component.signOut();
      expect(localStorage.getItem('currentUser')).toBeNull();
      expect(routerNavigateSpy.calls.count()).toEqual(1);
      expect(routerNavigateSpy.calls.argsFor(0)).toEqual([['/sign-in']]);
    });
  });
});
