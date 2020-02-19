import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { User } from '../models/user';
import { GlobalHelper } from '../helpers/global';
import { CurrentUser } from '../models/current-user';
import { ConversationsService } from '../services/conversations.service';
import { CreateConversationRequest } from '../models/requests/create-conversation-request';
import { RelationshipsService } from '../services/relationships.service';
import { RelationshipType } from '../models/relationship-type.enum';
import { Relationship } from '../models/relationship';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-message-composer',
  templateUrl: './message-composer.component.html',
  styleUrls: ['./message-composer.component.scss']
})
export class MessageComposerComponent implements OnInit {

  @Output() conversationCreated: EventEmitter<any> = new EventEmitter();

  public loading: boolean = false;

  public createConversationRequest: CreateConversationRequest = new CreateConversationRequest();
  public friends: User[] = new Array<User>();

  constructor(
    private globalHelper: GlobalHelper,
    private relationshipsService: RelationshipsService,
    private conversationsService: ConversationsService,
    private usersService: UsersService
    ) { }

  ngOnInit(): void {
    const userId = this.globalHelper.localStorageItem<CurrentUser>('currentUser').id;
    this.createConversationRequest.senderId = userId;
    this.relationshipsService.getRelationships(userId, RelationshipType.Friend).subscribe({
      next: (relationships: Relationship[]) => {
        let usersToGet = new Array<string>();

        relationships.forEach((rel) => {
          usersToGet.push(rel.userOneId === userId ? rel.userTwoId : rel.userOneId);
        });

        usersToGet.forEach((id) => {
          this.usersService.getUser(id).subscribe({
            next: (user: User) => {
              this.friends.push(user);
            }
          });
        });
      }
    });
  }

  public sendMessage() {
    this.loading = true;
    this.conversationsService.createConversation(this.createConversationRequest).subscribe({
      next: (conversationId: string) => {
        this.conversationCreated.emit(conversationId);
        this.loading = false;
      }
    });
  }

  public displayFriend(userId: string) {
    return this.friends.find(friend => friend.id === userId).fullName;
  }
}
