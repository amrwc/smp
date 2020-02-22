import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RelationshipType } from '../models/relationship-type.enum';
import { Observable } from 'rxjs';
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
    return this.httpClient.get<Relationship[]>(`${this.baseUrl}api/Relationships/GetRelationships/${userId}/${relationshipType}`, { headers: this.globalHelper.getAuthHeader() });
  }
}
