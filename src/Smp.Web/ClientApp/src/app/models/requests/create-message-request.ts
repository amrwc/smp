import { User } from '../user';

export class CreateMessageRequest {
    public senderId: string;
    public receiverId: string;
    public receiver: User;
    public content: string;
}