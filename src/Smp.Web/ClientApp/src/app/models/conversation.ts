import { FriendlyMessage } from './message';

export class Conversation {
  public id: string;
  public createdAt: Date;
}

export class ExtendedConversation extends Conversation {
  public lastMessage? : FriendlyMessage;
}