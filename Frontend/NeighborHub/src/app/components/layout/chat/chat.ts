import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgClass } from '@angular/common';
import { Subscription } from 'rxjs';
import { ChatService } from '../../../services/chat.service';
import { CurrentUserService } from '../../../services/current-user.service';
import { ChatMessage, Conversation } from '../../../models/chat.model';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [FormsModule, NgClass],
  templateUrl: './chat.html',
  styleUrl: './chat.css',
})
export class Chat implements OnInit, OnDestroy {
  private chatService = inject(ChatService);
  private currentUserService = inject(CurrentUserService);

  conversations: Conversation[] = [];
  messages: ChatMessage[] = [];
  selectedUser: Conversation | null = null;
  newMessage = '';
  loading = true;
  sending = false;
  error: string | null = null;
  connectionError: string | null = null;

  private subs = new Subscription();
  private currentUserId: number | null = null;
  private cdr = inject(ChangeDetectorRef);
  
  ngOnInit(): void {
    this.currentUserId = this.currentUserService.getUserId();
    if (this.currentUserId == null) {
      this.error = 'Please log in to use chat.';
      this.loading = false;
      return;
    }

    void this.initChat();

    this.subs.add(
      this.chatService.messageReceived$.subscribe((message) => {
        if (!message) return;
        this.handleIncomingMessage(message);
         this.cdr.detectChanges();
      })
    );
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  private async initChat(): Promise<void> {
    try {
      await this.chatService.connect();
      this.loadSidebar();
    } catch {
      this.connectionError = 'Could not connect to real-time chat. Messages may not update live.';
      this.loadSidebar();
       this.cdr.detectChanges();
    }
  }

  loadSidebar(): void {
    this.loading = true;
    this.error = null;

    this.chatService.getConversations().subscribe({
      next: (convRes) => {
        this.chatService.getContacts().subscribe({
          next: (contactRes) => {
            this.conversations = this.chatService.mergeConversations(
              convRes.data ?? [],
              contactRes.data ?? []
            );
            this.loading = false;
             this.cdr.detectChanges();
          },
          error: () => {
            this.conversations = convRes.data ?? [];
            this.loading = false;
             this.cdr.detectChanges();
          },
        });
      },
      error: (err) => {
        this.error = err?.error?.message ?? 'Failed to load conversations.';
        this.loading = false;
         this.cdr.detectChanges();
      },
    });
  }

  selectConversation(conversation: Conversation): void {
    this.selectedUser = conversation;
    this.messages = [];
    this.error = null;

    this.chatService.getMessages(conversation.userId).subscribe({
      next: (res) => {
        this.messages = res.data ?? [];
        void this.chatService.joinConversation(conversation.userId);
        this.markConversationReadLocally(conversation.userId);
         this.cdr.detectChanges();
      },
      error: (err) => {
        this.error = err?.error?.message ?? 'Failed to load messages.';
         this.cdr.detectChanges();
      },
    });
  }

  sendMessage(): void {
    if (!this.selectedUser || !this.newMessage.trim() || this.sending) return;

    const content = this.newMessage.trim();
    const recipientId = this.selectedUser.userId;
    this.sending = true;

    this.chatService.sendMessage(recipientId, content).subscribe({
      next: (res) => {
        if (res.data) {
          this.messages = [...this.messages, res.data];
          this.updateConversationPreview(res.data);
        }
        this.newMessage = '';
        this.sending = false;
        void this.chatService.sendRealtimeMessage(recipientId, content);
         this.cdr.detectChanges();
      },
      error: (err) => {
        this.error = err?.error?.message ?? 'Failed to send message.';
        this.sending = false;
         this.cdr.detectChanges();
      },
    });
  }

  private handleIncomingMessage(message: ChatMessage): void {
    this.updateConversationPreview(message);

    if (
      this.selectedUser &&
      (message.senderId === this.selectedUser.userId ||
        message.recipientId === this.selectedUser.userId ||
        message.otherUserId === this.selectedUser.userId)
    ) {
      const exists = this.messages.some((m) => m.id === message.id);
      if (!exists) {
        this.messages = [...this.messages, message];
      }
      if (message.recipientId === this.currentUserId) {
        this.markConversationReadLocally(message.senderId ?? this.selectedUser.userId);
      }
    }
  }

  private updateConversationPreview(message: ChatMessage): void {
    const partnerId =
      message.senderId === this.currentUserId
        ? message.recipientId
        : message.senderId ?? message.otherUserId;

    const idx = this.conversations.findIndex((c) => c.userId === partnerId);
    if (idx >= 0) {
      const updated = {
        ...this.conversations[idx],
        lastMessage: message.content,
        lastMessageAt: message.sentAt,
        unreadCount:
          this.selectedUser?.userId === partnerId
            ? 0
            : message.recipientId === this.currentUserId
              ? this.conversations[idx].unreadCount + 1
              : this.conversations[idx].unreadCount,
      };
      this.conversations = [
        updated,
        ...this.conversations.slice(0, idx),
        ...this.conversations.slice(idx + 1),
      ];
    }
  }

  private markConversationReadLocally(userId: number): void {
    this.conversations = this.conversations.map((c) =>
      c.userId === userId ? { ...c, unreadCount: 0 } : c
    );
  }

  isOwnMessage(message: ChatMessage): boolean {
    return message.senderId === this.currentUserId;
  }

  isSystemMessage(message: ChatMessage): boolean {
    return message.messageType === 'System';
  }

  formatTime(value: string): string {
    const d = new Date(value);
    return d.toLocaleString(undefined, {
      month: 'short',
      day: 'numeric',
      hour: 'numeric',
      minute: '2-digit',
    });
  }

  getInitials(name: string): string {
    const parts = name.trim().split(/\s+/).filter(Boolean);
    if (parts.length === 0) return '?';
    if (parts.length === 1) return parts[0].charAt(0).toUpperCase();
    return `${parts[0].charAt(0)}${parts[parts.length - 1].charAt(0)}`.toUpperCase();
  }
}
