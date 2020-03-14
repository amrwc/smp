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

  describe('getPosts', () => {
    let postsServiceGetPostsSpy: jasmine.Spy;

    beforeEach(() => {
      postsServiceGetPostsSpy = spyOn(TestBed.inject(PostsService), 'getPosts');
    });

    describe('when there is no receiver id', () => {
      it('PostsService getPosts should not get called', () => {
        expect(postsServiceGetPostsSpy.calls.count()).toEqual(0);
      });
    });

    describe('when there is a receiver id', () => {
      const posts = [ { id: 'postid' } as Post ];

      beforeEach(() => {
        component.receiverId = 'receiverId';

        postsServiceGetPostsSpy.and.returnValue(of(posts));
      });

      it('PostsService getPosts should get called', () => {
        component.getPosts();

        expect(postsServiceGetPostsSpy.calls.count()).toEqual(1);
        expect(postsServiceGetPostsSpy.calls.argsFor(0)).toEqual(['receiverId']);
      });

      it('posts should be as expected', () => {
        component.getPosts();

        expect(component.posts).toEqual(posts);
      });
    })
  });
});
