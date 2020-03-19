import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { of, Observable } from 'rxjs';
import { Relationship } from '../models/relationship';
import { RelationshipType } from '../models/relationship-type.enum';
import { RelationshipsService } from './relationships.service';

describe('RelationshipsService', () => {
  const baseUrl: string = 'https://www.smp.org/';
  const relationship: Relationship = {
    userOneId: 'aksdfnknkj-123sdf-0asdfasd',
    userTwoId: 'adsifasfuh1231231-asxzcz9v8bc9-213123',
    relationshipType: RelationshipType.Friend,
    createdAt: new Date(),
  } as Relationship;
  let headers: Object;
  let service: RelationshipsService;

  beforeAll(() => {
    localStorage.setItem('currentUser', JSON.stringify({ token: 'n21k32n3j' }));
    headers = {
      headers: new HttpHeaders().set(
        'Authorization',
        `Bearer ${JSON.parse(localStorage.getItem('currentUser')).token}`
      ),
    };
  });

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [{ provide: 'BASE_URL', useValue: baseUrl }],
    });
    service = TestBed.get(RelationshipsService);
  });

  afterAll(() => {
    localStorage.removeItem('currentUser');
  });

  describe('getRelationship()', () => {
    beforeEach(() => {
      spyOn(TestBed.get(HttpClient), 'get').and.returnValue(of(relationship));
    });

    it('should have called HttpClient.get() correctly', () => {
      service.getRelationship(relationship.userOneId, relationship.userTwoId, relationship.relationshipType);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledWith(
        `${baseUrl}api/Relationships/GetRelationship/${relationship.userOneId}/` +
          `${relationship.userTwoId}/${relationship.relationshipType}`,
        headers
      );
    });

    it('should have returned the expected value', () => {
      const result: Observable<Relationship> = service.getRelationship(
        relationship.userOneId,
        relationship.userTwoId,
        relationship.relationshipType
      );
      result.subscribe({
        next: (ship: Relationship) => {
          expect(ship).toEqual(relationship);
        },
      });
    });
  });

  describe('getRelationships()', () => {
    const expected: Relationship[] = [relationship, relationship] as Relationship[];

    beforeEach(() => {
      spyOn(TestBed.get(HttpClient), 'get').and.returnValue(of());
    });

    it('should have called HttpClient.get() correctly', () => {
      service.getRelationships(relationship.userOneId, relationship.relationshipType);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledTimes(1);
      expect(TestBed.get(HttpClient).get).toHaveBeenCalledWith(
        `${baseUrl}api/Relationships/GetRelationships/${relationship.userOneId}/${relationship.relationshipType}`,
        headers
      );
    });

    it('should have returned the expected values', () => {
      const result: Observable<Relationship[]> = service.getRelationships(
        relationship.userOneId,
        relationship.relationshipType
      );
      result.subscribe({
        next: (relationships: Relationship[]) => {
          expect(relationships).toEqual(expected);
        },
      });
    });
  });
});
