import { ApiResponse } from './item.model';

export type ChatMessageType = 'User' | 'System';
export type SystemEventType = 'BookingRequested' | 'BookingConfirmed' | 'RentalOverdue';

export interface ChatMessage {
  id: number;
  senderId?: number | null;
  senderName?: string | null;
  recipientId: number;
  otherUserId: number;
  content: string;
  messageType: ChatMessageType;
  systemEventType?: SystemEventType | null;
  bookingId?: number | null;
  sentAt: string;
  isRead: boolean;
}

export interface Conversation {
  userId: number;
  fullName: string;
  lastMessage?: string | null;
  lastMessageAt?: string | null;
  unreadCount: number;
}

export type ChatMessageResponse = ApiResponse<ChatMessage>;
export type ChatMessagesResponse = ApiResponse<ChatMessage[]>;
export type ConversationsResponse = ApiResponse<Conversation[]>;
export type UnreadCountResponse = ApiResponse<number>;
