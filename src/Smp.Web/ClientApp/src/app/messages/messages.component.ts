import { Component, OnInit, ViewChild } from '@angular/core';
import { Conversation, ExtendedConversation } from '../models/conversation';
import { GlobalHelper } from '../helpers/global';
import { ConversationsService } from '../services/conversations.service';
import { MessagesService } from '../services/messages.service';
import { CurrentUser } from '../models/current-user';
import { Message, FriendlyMessage } from '../models/message';
import { UsersService } from '../services/users.service';
import { User } from '../models/user';
import { ConversationComponent } from '../conversation/conversation.component';
import { MessageComposerComponent } from '../message-composer/message-composer.component';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {

  public startNewConversation: boolean;

  @ViewChild(ConversationComponent)
  conversation: ConversationComponent;

  @ViewChild(MessageComposerComponent)
  composer: MessageComposerComponent;

  public conversations: ExtendedConversation[];

  constructor(private globalHelper: GlobalHelper,
    private conversationsService: ConversationsService,
    private messagesService: MessagesService,
    private usersService: UsersService) { }

  ngOnInit() {
    this.fetchConversationsData();
  }

  public getConversationPicture(conversationId: string): string {
    const userId = this.globalHelper.localStorageItem<CurrentUser>('currentUser').id;
    const conversation = (this.conversations.filter((cnv) => {
      return cnv.id == conversationId;
    }))[0];

    let participant: User;

    conversation.participants.forEach((usr: User, key: string) => {
      if (key != userId) {
        participant = usr;
      }
    });

    return participant.profilePictureUrl;
  }

  public getConversationName(conversationId: string): string {
    const userId = this.globalHelper.localStorageItem<CurrentUser>('currentUser').id;
    const conversation = (this.conversations.filter((cnv) => {
      return cnv.id == conversationId;
    }))[0];

    let participant: User;

    conversation.participants.forEach((usr: User, key: string) => {
      if (key != userId) {
        participant = usr;
      }
    });

    return participant.fullName;
  }

  public getLastMessageSender(conversationId: string): string {
    const userId = this.globalHelper.localStorageItem<CurrentUser>('currentUser').id;
    const conversation = (this.conversations.filter((cnv) => {
      return cnv.id == conversationId;
    }))[0];

    return conversation.lastMessage.receiverId == userId
      ? conversation.lastMessage.sender?.fullName
      : "You"
  }

  public loadConversation(conversationId: string): void {
    this.startNewConversation = false;
    this.conversation.conversationId = conversationId;
  }

  public showStartConversation(): void {
    this.startNewConversation = true;
  }

  private fetchConversationsData(): void {
    this.conversationsService.getConversations(this.globalHelper.localStorageItem<CurrentUser>('currentUser').id).subscribe({
      next: (conversations: Conversation[]) => {
        this.conversations = conversations;
        this.fetchConversationParticipants();
        this.fetchMessages(1, 0);
      }
    });
  }

  private fetchConversationParticipants(): void {
    this.conversations.forEach((cnv: ExtendedConversation, index: number, conversationsArray: ExtendedConversation[]) => {
      this.conversationsService.getConversationParticipants(cnv.id).subscribe({
        next: (ids: string[]) => {
          conversationsArray[index].participants = new Map<string, User>(); 
          ids.forEach(id => {
            this.usersService.getUser(id).subscribe({
              next: (user: User) => {
                conversationsArray[index].participants.set(user.id, user);
              }
            });
          });
        }
      });
    });
  }

  private fetchMessages(count: number, page: number): void {
    this.conversations.forEach((cnv: ExtendedConversation, index: number, conversationsArray: ExtendedConversation[]) => {
      this.messagesService.getMessagesFromConversation(cnv.id, count, page).subscribe({
        next: (message: FriendlyMessage[]) => {
          if (message.length > 0) {
            conversationsArray[index].lastMessage = message[0];
            
            this.usersService.getUser(conversationsArray[index].lastMessage.receiverId).subscribe({
              next: (user: User) => {
                conversationsArray[index].lastMessage.receiver = user;
              }
            });
            this.usersService.getUser(conversationsArray[index].lastMessage.senderId).subscribe({
              next: (user: User) => {
                conversationsArray[index].lastMessage.sender = user;
              }
            });
          }
        }
      });
    });
  }
}
