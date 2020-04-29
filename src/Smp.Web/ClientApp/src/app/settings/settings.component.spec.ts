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

  describe('signOut()', () => {
    it('should have navigated to /sign-in', () => {
      localStorage.setItem('currentUser', JSON.stringify({ currentUser: 'bob' }));
      spyOn(TestBed.get(Router), 'navigate');
      component.signOut();
      expect(localStorage.getItem('currentUser')).toBeNull();
      expect(TestBed.get(Router).navigate).toHaveBeenCalledTimes(1);
      expect(TestBed.get(Router).navigate).toHaveBeenCalledWith(['/sign-in']);
    });
  });
});
