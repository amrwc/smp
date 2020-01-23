import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RelationshipType } from '../models/relationship-type.enum';
import { Observable } from 'rxjs';
import { Relationship } from '../models/relationship';

@Injectable({
  providedIn: 'root'
})
export class RelationshipsService {

  private httpHeaders = new HttpHeaders();

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    let authToken = "Bearer " + JSON.parse(localStorage.getItem('currentUser')).token;
    this.httpHeaders = this.httpHeaders.set('Authorization', authToken);
  }

  public getRelationship(userOneId: string, userTwoId: string, relationshipType: RelationshipType): Observable<Relationship> {
    return this.httpClient.get<Relationship>(`${this.baseUrl}api/Relationships/GetRelationship/${userOneId}/${userTwoId}/${relationshipType}`, { headers: this.httpHeaders })
  }
}
