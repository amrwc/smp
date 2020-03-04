import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedComponent } from './feed.component';
import { PostsService } from '../services/posts.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('FeedComponent', () => {
  let component: FeedComponent;
  let fixture: ComponentFixture<FeedComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ FeedComponent ],
      imports: [ HttpClientTestingModule ],
      providers: [ PostsService, { provide: 'BASE_URL', useValue: "https://www.smp.org/" }]
    }).compileComponents();

    fixture = TestBed.createComponent(FeedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
