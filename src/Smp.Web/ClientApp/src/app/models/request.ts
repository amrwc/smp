import { RequestType } from './request-type.enum';
import { UsersService } from '../services/users.service';

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
  public requestType: number;

  public toFriendlyRequest(): FriendlyRequest {
    return new FriendlyRequest(this.receiverId, this.senderId, this.requestType);
  }
}

export class FriendlyRequest {
  constructor(receiverId: string, senderId: string, requestType: number) {
    this.receiverId = receiverId;
    this.senderId = senderId;
    this.requestTypeName = RequestType[requestType];
    this.requestType = requestType;
  }

  public receiverName: string;
  public receiverId: string;
  public senderName: string;
  public senderId: string;
  public requestTypeName: string;
  public requestType: RequestType;
}