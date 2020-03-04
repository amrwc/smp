import { TestBed } from '@angular/core/testing';

import { RelationshipsService } from './relationships.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('RelationshipsService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [HttpClientTestingModule],
    providers: [{ provide: 'BASE_URL', useValue: "https://www.smp.org/" }]
  }));

  it('should be created', () => {
    const service: RelationshipsService = TestBed.get(RelationshipsService);
    expect(service).toBeTruthy();
  });
});
