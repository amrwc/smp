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

  @Output() conversationReady: EventEmitter<any> = new EventEmitter();

  public loading: boolean = false;

  public createConversationRequest: CreateConversationRequest = new CreateConversationRequest();
  public friends: User[] = new Array<User>();
  public filteredFriends: User[] = new Array<User>();

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
              this.filteredFriends.push(user);
            }
          });
        });
      }
    });
  }

  public sendMessage() {
    const availableReceiverIds = this.friends.map(friend => friend.id);
    if (!availableReceiverIds.includes(this.createConversationRequest.receiverId))
      return;

    this.loading = true;
    this.conversationsService.createConversation(this.createConversationRequest).subscribe({
      next: (conversationId: string) => {
        this.conversationReady.emit(conversationId);
        this.loading = false;
      },
      error: (error: any) => {
        if (error.status === 409) {
          this.conversationReady.emit(error.error);
        }
        this.loading = false;
      }
    });
  }

  public displayFriend(userId: string): string {
    const friend = this.friends.find(friend => friend.id === userId);
    return friend ? friend.fullName : "Unknown";
  }

  public filterFriends(name: string): void {
    this.filteredFriends = this.friends.filter(friend => friend.fullName.toLowerCase().includes(name.toLowerCase()));
  }
}
