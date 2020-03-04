import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePostComponent } from './create-post.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { GlobalHelper } from '../helpers/global';
import { PostsService } from '../services/posts.service';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('CreatePostComponent', () => {
  let component: CreatePostComponent;
  let fixture: ComponentFixture<CreatePostComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CreatePostComponent],
      imports: [HttpClientTestingModule, FormsModule, BrowserAnimationsModule],
      providers: [GlobalHelper, PostsService, { provide: 'BASE_URL', useValue: "https://www.smp.org/" }]
    }).compileComponents();

    fixture = TestBed.createComponent(CreatePostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
