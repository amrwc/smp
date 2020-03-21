import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedComponent } from './feed.component';
import { PostsService } from '../services/posts.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of } from 'rxjs';
import { Post } from '../models/post';

describe('FeedComponent', () => {
  let component: FeedComponent;
  let fixture: ComponentFixture<FeedComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ FeedComponent ],
      imports: [ HttpClientTestingModule ],
      providers: [ { provide: 'BASE_URL', useValue: "https://www.smp.org/" }]
    }).compileComponents();

    fixture = TestBed.createComponent(FeedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  describe('getPosts()', () => {
    const posts = [ { id: 'postid' } as Post ];

    beforeEach(() => {
      spyOn(TestBed.inject(PostsService), 'getPosts').and.returnValue(of(posts));
    });

    describe('when there is no receiver id', () => {
      it('should not have called PostsService.getPosts()', () => {
        expect(TestBed.inject(PostsService).getPosts).toHaveBeenCalledTimes(0);
      });
    });

    describe('when there is a receiver id', () => {
      beforeEach(() => {
        component.receiverId = 'receiverId';
      });

      it('should not have called PostsService.getPosts()', () => {
        component.getPosts();

        expect(TestBed.inject(PostsService).getPosts).toHaveBeenCalledTimes(1);
        expect(TestBed.inject(PostsService).getPosts).toHaveBeenCalledWith('receiverId');
      });

      it('posts should be as expected', () => {
        component.getPosts();

        expect(component.posts).toEqual(posts);
      });
    });
  });
});
