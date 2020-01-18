import { TestBed } from '@angular/core/testing';

import { RelationshipsService } from './relationships.service';

describe('RelationshipsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RelationshipsService = TestBed.get(RelationshipsService);
    expect(service).toBeTruthy();
  });
});
