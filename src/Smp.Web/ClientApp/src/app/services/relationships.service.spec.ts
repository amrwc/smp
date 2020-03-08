import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { of } from 'rxjs';
import { Relationship } from '../models/relationship';
import { RelationshipType } from '../models/relationship-type.enum';
import { RelationshipsService } from './relationships.service';

describe('RelationshipsService', () => {
  const baseUrl = 'https://www.smp.org/';
  const userOneId = 'aksdfnknkj-123sdf-0asdfasd';
  const userTwoId = 'adsifasfuh1231231-asxzcz9v8bc9-213123';
  const relationshipType = RelationshipType.Friend;
  let headers: Object;
  let httpClientGetSpy: jasmine.Spy;
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

  describe('getRelationship', () => {
    const expected: Relationship = {
      userOneId,
      userTwoId,
      relationshipType,
      createdAt: new Date(),
    } as Relationship;

    beforeEach(() => {
      httpClientGetSpy = spyOn(TestBed.get(HttpClient), 'get').and.returnValue(of(expected));
    });

    it('should have called HttpClient.get correctly', () => {
      service.getRelationship(userOneId, userTwoId, relationshipType);
      expect(httpClientGetSpy.calls.count()).toEqual(1);
      expect(httpClientGetSpy.calls.argsFor(0)).toEqual([
        `${baseUrl}api/Relationships/GetRelationship/${userOneId}/${userTwoId}/${relationshipType}`,
        headers,
      ]);
    });

    it('should have returned the expected value', () => {
      const result = service.getRelationship(userOneId, userTwoId, relationshipType);
      result.subscribe({
        next: (relationship: Relationship) => {
          expect(relationship).toEqual(expected);
        },
      });
    });
  });

  describe('getRelationships', () => {
    const expected: Relationship[] = [
      {
        userOneId,
        userTwoId,
        relationshipType,
        createdAt: new Date(),
      },
      {
        userOneId,
        userTwoId,
        relationshipType,
        createdAt: new Date(),
      },
    ] as Relationship[];

    beforeEach(() => {
      httpClientGetSpy = spyOn(TestBed.get(HttpClient), 'get').and.returnValue(of());
    });

    it('should have called HttpClient.get correctly', () => {
      service.getRelationships(userOneId, relationshipType);
      expect(httpClientGetSpy.calls.count()).toEqual(1);
      expect(httpClientGetSpy.calls.argsFor(0)).toEqual([
        `${baseUrl}api/Relationships/GetRelationships/${userOneId}/${relationshipType}`,
        headers,
      ]);
    });

    it('should have returned the expected values', () => {
      const result = service.getRelationships(userOneId, relationshipType);
      result.subscribe({
        next: (relationships: Relationship[]) => {
          expect(relationships).toEqual(expected);
        },
      });
    });
  });
});
