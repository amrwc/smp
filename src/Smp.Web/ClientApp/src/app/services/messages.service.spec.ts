import { TestBed } from '@angular/core/testing';

import { MessagesService } from './messages.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('MessagesService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [HttpClientTestingModule],
    providers: [{ provide: 'BASE_URL', useValue: "https://www.smp.org/" }]
  }));

  it('should be created', () => {
    const service: MessagesService = TestBed.get(MessagesService);
    expect(service).toBeTruthy();
  });
});
