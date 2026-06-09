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

const TOKEN_KEY = 'neighborhub_auth_token';

@Injectable({
  providedIn: 'root',
})
export class ChatService implements OnDestroy {
  private http = inject(HttpClient);
  private apiUrl = `${env.apiBaseUrl}/api/Chat`;
  private hubConnection: signalR.HubConnection | null = null;

  private unreadCountSubject = new BehaviorSubject<number>(0);
  private messageReceivedSubject = new BehaviorSubject<ChatMessage | null>(null);

  unreadCount$ = this.unreadCountSubject.asObservable();
  messageReceived$ = this.messageReceivedSubject.asObservable();

  async connect(): Promise<void> {
    if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
      return;
    }

    const token = localStorage.getItem(TOKEN_KEY);
    if (!token) return;

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${env.apiBaseUrl}/hubs/chat`, {
        accessTokenFactory: () => token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('ReceiveMessage', (message: ChatMessage) => {
      this.messageReceivedSubject.next(message);
    });

    this.hubConnection.on('UnreadCountUpdated', (count: number) => {
      this.unreadCountSubject.next(count);
    });

    this.hubConnection.on('ConversationLoaded', () => {
      // handled by component via joinConversation promise
    });

    await this.hubConnection.start();
    await this.refreshUnreadCount();
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
      map.set(c.userId, { ...c, unreadCount: 0 });
    }
    for (const c of existing) {
      map.set(c.userId, { ...map.get(c.userId), ...c });
    }
    return Array.from(map.values()).sort((a, b) => {
      const aTime = a.lastMessageAt ? new Date(a.lastMessageAt).getTime() : 0;
      const bTime = b.lastMessageAt ? new Date(b.lastMessageAt).getTime() : 0;
      return bTime - aTime || a.fullName.localeCompare(b.fullName);
    });
  }
}
