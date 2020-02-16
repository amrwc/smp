import { FriendlyMessage } from './message';
import { User } from './user';

export class Conversation {
  public id: string;
  public createdAt: Date;
}

export class ExtendedConversation extends Conversation {
  public lastMessage? : FriendlyMessage;
  public participants? : Map<string, User>;
}