import { inject, Injectable, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { env } from '../../environments/environment';
import {
  ChatMessage,
  ConversationsResponse,
  ChatMessagesResponse,
  ChatMessageResponse,
  UnreadCountResponse,
  Conversation,
} from '../models/chat.model';
import { normalizeDisplayName } from '../utils/display-name.util';

const TOKEN_KEY = 'neighborhub_auth_token';

@Injectable({
  providedIn: 'root',
})
export class ChatService implements OnDestroy {
  private http = inject(HttpClient);
  private apiUrl = `${env.apiBaseUrl}/api/Chat`;
  private hubConnection: signalR.HubConnection | null = null;
  private connectPromise: Promise<void> | null = null;

  private unreadCountSubject = new BehaviorSubject<number>(0);
  private messageReceivedSubject = new BehaviorSubject<ChatMessage | null>(null);

  unreadCount$ = this.unreadCountSubject.asObservable();
  messageReceived$ = this.messageReceivedSubject.asObservable();

  async connect(): Promise<void> {
    if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
      return;
    }

    if (this.connectPromise) {
      return this.connectPromise;
    }

    this.connectPromise = this.startConnection();
    try {
      await this.connectPromise;
    } finally {
      this.connectPromise = null;
    }
  }

  private async startConnection(): Promise<void> {
    const token = localStorage.getItem(TOKEN_KEY);
    if (!token) return;

    if (!this.hubConnection) {
      this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(`${env.apiBaseUrl}/hubs/chat`, {
          accessTokenFactory: () => token,
        })
        .withAutomaticReconnect()
        .build();

      this.hubConnection.on('ReceiveMessage', (message: ChatMessage) => {
        this.messageReceivedSubject.next(this.normalizeChatMessage(message));
      });

      this.hubConnection.on('UnreadCountUpdated', (count: number) => {
        this.unreadCountSubject.next(count);
      });
    }

    if (this.hubConnection.state === signalR.HubConnectionState.Disconnected) {
      await this.hubConnection.start();
    }

    this.refreshUnreadCount();
  }

  private normalizeChatMessage(raw: ChatMessage): ChatMessage {
    const record = raw as unknown as Record<string, unknown>;
    return {
      id: Number(record['id'] ?? record['Id'] ?? 0),
      senderId: (record['senderId'] ?? record['SenderId']) as number | null | undefined,
      senderName: (record['senderName'] ?? record['SenderName']) as string | null | undefined,
      recipientId: Number(record['recipientId'] ?? record['RecipientId'] ?? 0),
      otherUserId: Number(record['otherUserId'] ?? record['OtherUserId'] ?? 0),
      content: String(record['content'] ?? record['Content'] ?? ''),
      messageType: (record['messageType'] ?? record['MessageType']) as ChatMessage['messageType'],
      systemEventType: (record['systemEventType'] ?? record['SystemEventType']) as ChatMessage['systemEventType'],
      bookingId: (record['bookingId'] ?? record['BookingId']) as number | null | undefined,
      sentAt: String(record['sentAt'] ?? record['SentAt'] ?? ''),
      isRead: Boolean(record['isRead'] ?? record['IsRead'] ?? false),
    };
  }

  async disconnect(): Promise<void> {
    if (this.hubConnection) {
      await this.hubConnection.stop();
      this.hubConnection = null;
    }
  }

  ngOnDestroy(): void {
    void this.disconnect();
  }

  getConversations(): Observable<ConversationsResponse> {
    return this.http.get<ConversationsResponse>(`${this.apiUrl}/conversations`);
  }

  getContacts(): Observable<ConversationsResponse> {
    return this.http.get<ConversationsResponse>(`${this.apiUrl}/contacts`);
  }

  getMessages(otherUserId: number): Observable<ChatMessagesResponse> {
    return this.http.get<ChatMessagesResponse>(`${this.apiUrl}/messages/${otherUserId}`);
  }

  sendMessage(recipientId: number, content: string): Observable<ChatMessageResponse> {
    return this.http.post<ChatMessageResponse>(`${this.apiUrl}/send`, {
      recipientId,
      content,
    });
  }

  async joinConversation(otherUserId: number): Promise<void> {
    if (!this.hubConnection || this.hubConnection.state !== signalR.HubConnectionState.Connected) {
      await this.connect();
    }
    await this.hubConnection?.invoke('JoinConversation', otherUserId);
  }

  async sendRealtimeMessage(recipientId: number, content: string): Promise<void> {
    if (!this.hubConnection || this.hubConnection.state !== signalR.HubConnectionState.Connected) {
      await this.connect();
    }
    await this.hubConnection?.invoke('SendMessage', { recipientId, content });
  }

  refreshUnreadCount(): Observable<UnreadCountResponse> {
    const request = this.http.get<UnreadCountResponse>(`${this.apiUrl}/unread-count`);
    request.subscribe({
      next: (res) => this.unreadCountSubject.next(res.data ?? 0),
    });
    return request;
  }

  mergeConversations(existing: Conversation[], contacts: Conversation[]): Conversation[] {
    const map = new Map<number, Conversation>();
    for (const c of contacts) {
      map.set(c.userId, {
        ...c,
        fullName: normalizeDisplayName(c.fullName) || c.fullName,
        unreadCount: 0,
      });
    }
    for (const c of existing) {
      const merged = { ...map.get(c.userId), ...c };
      merged.fullName = normalizeDisplayName(merged.fullName) || merged.fullName;
      map.set(c.userId, merged);
    }
    return Array.from(map.values()).sort((a, b) => {
      const aTime = a.lastMessageAt ? new Date(a.lastMessageAt).getTime() : 0;
      const bTime = b.lastMessageAt ? new Date(b.lastMessageAt).getTime() : 0;
      return bTime - aTime || a.fullName.localeCompare(b.fullName);
    });
  }
}
