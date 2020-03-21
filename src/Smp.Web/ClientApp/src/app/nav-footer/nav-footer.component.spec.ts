import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { NavFooterComponent } from './nav-footer.component';

describe('NavFooterComponent', () => {
  let component: NavFooterComponent;
  let fixture: ComponentFixture<NavFooterComponent>;

  beforeEach(() => {
    localStorage.setItem('currentUser', '{ "id": "id" }');
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [NavFooterComponent],
    }).compileComponents();
    fixture = TestBed.createComponent(NavFooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    localStorage.removeItem('currentUser');
  });

  it('should have been created', () => {
    expect(component).toBeTruthy();
  });
});
