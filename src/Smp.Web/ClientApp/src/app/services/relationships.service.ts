import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RelationshipType } from '../models/relationship-type.enum';
import { Observable, of } from 'rxjs';
import { Relationship } from '../models/relationship';
import { GlobalHelper } from '../helpers/global';

@Injectable({
  providedIn: 'root'
})
export class RelationshipsService {

  constructor(private httpClient: HttpClient, private globalHelper: GlobalHelper, @Inject('BASE_URL') private baseUrl: string) {
  }

  public getRelationship(userOneId: string, userTwoId: string, relationshipType: RelationshipType): Observable<Relationship> {
    return this.httpClient.get<Relationship>(`${this.baseUrl}api/Relationships/GetRelationship/${userOneId}/${userTwoId}/${relationshipType}`, { headers: this.globalHelper.getAuthHeader() });
  }

  public getRelationships(userId: string, relationshipType: RelationshipType): Observable<Relationship[]> {
    //const resp = this.httpClient.get<Relationship[]>(`${this.baseUrl}api/Relationships/GetRelationships/${userId}/${relationshipType}`, { headers: this.globalHelper.getAuthHeader() });

    const relationships: Relationship[] = 
    [
      { userOneId: "03e160c2-fa5b-4845-a230-5906ee555f5e", userTwoId: "238f5b8c-e31e-40a0-850c-aa41643ee8f0", relationshipType: RelationshipType.Friend, createdAt: new Date() },
      { userOneId: "7d5c41f5-003a-4594-9526-3ea4a32f319e", userTwoId: "03e160c2-fa5b-4845-a230-5906ee555f5e", relationshipType: RelationshipType.Friend, createdAt: new Date() }
    ];

    return of(relationships);
  }
}
