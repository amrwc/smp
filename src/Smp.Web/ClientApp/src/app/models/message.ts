import { User } from './user';

export class Message {
  constructor(message: Message) {
    this.id = message.id;
    this.senderId = message.senderId;
    this.createdAt = message.createdAt;
    this.content = message.content;
    this.conversationId = message.conversationId;
  }

  public id: number;
  public senderId: string;
  public createdAt: Date;
  public content: string;
  public conversationId: string;

  public getFriendlyDate(): string {
    return new Date(this.createdAt).toLocaleString();
  }
}

export class FriendlyMessage extends Message {
  constructor(message: Message) {
    super(message);
    this.createdAtFriendly = new Date(this.createdAt).toLocaleString();
  }

  public sender?: User;
  public createdAtFriendly?: string;
}