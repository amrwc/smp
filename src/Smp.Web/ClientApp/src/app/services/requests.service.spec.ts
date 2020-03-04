import { TestBed } from '@angular/core/testing';

import { RequestsService } from './requests.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('RequestsService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [HttpClientTestingModule],
    providers: [{ provide: 'BASE_URL', useValue: "https://www.smp.org/" }]
  }));

  it('should be created', () => {
    const service: RequestsService = TestBed.get(RequestsService);
    expect(service).toBeTruthy();
  });
});
