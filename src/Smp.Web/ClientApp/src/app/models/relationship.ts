import { RelationshipType } from './relationship-type.enum';

export interface Relationship {
  userOneId: string;
  userTwoId: string;
  relationshipType: RelationshipType;
  createdAt: Date;
}