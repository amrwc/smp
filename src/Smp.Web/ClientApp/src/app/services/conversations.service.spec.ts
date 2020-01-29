import { TestBed } from '@angular/core/testing';

import { ConversationsService } from './conversations.service';

describe('ConversationsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ConversationsService = TestBed.get(ConversationsService);
    expect(service).toBeTruthy();
  });
});
