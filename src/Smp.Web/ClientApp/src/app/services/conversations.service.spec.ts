import { TestBed } from '@angular/core/testing';

import { ConversationsService } from './conversations.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('ConversationsService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [HttpClientTestingModule],
    providers: [{ provide: 'BASE_URL', useValue: "https://www.smp.org/" }]
  }));

  it('should be created', () => {
    const service: ConversationsService = TestBed.get(ConversationsService);
    expect(service).toBeTruthy();
  });
});
