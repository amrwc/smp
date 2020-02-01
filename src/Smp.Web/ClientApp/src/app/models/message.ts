export interface Message {
  id: number;
  senderId: string;
  receiverId: string;
  createdAt: Date;
  content: string;
  conversationId: string;
}