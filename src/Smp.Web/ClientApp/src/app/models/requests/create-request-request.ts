import { RequestType } from '../request-type.enum';

export class CreateRequestRequest {
    constructor(senderId: string, receiverId: string, requestType: RequestType) {
        this.senderId = senderId;
        this.receiverId = receiverId;
        this.requestTypeId = <number>requestType;
      }

    public senderId: string;
    public receiverId: string;
    public requestTypeId: number;
}