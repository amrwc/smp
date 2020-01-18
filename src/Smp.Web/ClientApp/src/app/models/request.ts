import { RequestType } from './request-type.enum';
import { User } from './user';

export class Request {
  constructor(req: Request) {
    this.receiverId = req.receiverId;
    this.senderId = req.senderId;
    this.createdAt = req.createdAt;
    this.requestType = req.requestType;
  }

  public receiverId: string;
  public senderId: string;
  public createdAt: Date;
  public requestType: RequestType;

  public toFriendlyRequest(): FriendlyRequest {
    return new FriendlyRequest(this);
  }
}

export class FriendlyRequest extends Request {
  constructor(request: Request) {
    super(request);
    this.requestTypeName = RequestType[this.requestType];
    this.dateTimeSent = new Date(this.createdAt).toLocaleString();
  }

  public receiver: User;
  public sender: User;
  public requestTypeName: string;
  public dateTimeSent: string;
}