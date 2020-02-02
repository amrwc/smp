import { User } from './user';

export class Message {
  constructor(message: Message) {
    this.id = message.id;
    this.senderId = message.senderId;
    this.receiverId = message.receiverId;
    this.createdAt = message.createdAt;
    this.content = message.content;
    this.conversationId = message.conversationId;
  }

  public id: number;
  public senderId: string;
  public receiverId: string;
  public createdAt: Date;
  public content: string;
  public conversationId: string;
}

export class FriendlyMessage extends Message {
  constructor(message: Message) {
    super(message);
    this.createdAtFriendly = new Date(this.createdAt).toLocaleString();
  }

  public receiver: User;
  public sender: User;
  public createdAtFriendly: string;
}