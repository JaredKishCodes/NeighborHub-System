import { Injectable } from '@angular/core';

const STORAGE_KEY = 'neighborhub_user_id';
const NAME_KEY = 'neighborhub_user_name';
const EMAIL_KEY = 'neighborhub_user_email';
const PICTURE_KEY = 'neighborhub_profile_picture';

@Injectable({
  providedIn: 'root',
})
export class CurrentUserService {
  getUserId(): number | null {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (raw === null) return null;
    const id = parseInt(raw, 10);
    return Number.isNaN(id) ? null : id;
  }

  setUserId(userId: number): void {
    localStorage.setItem(STORAGE_KEY, String(userId));
  }

  getDisplayName(): string {
    return localStorage.getItem(NAME_KEY) ?? 'User';
  }

  setDisplayName(name: string): void {
    localStorage.setItem(NAME_KEY, name);
  }

  getEmail(): string {
    return localStorage.getItem(EMAIL_KEY) ?? '';
  }

  setEmail(email: string): void {
    localStorage.setItem(EMAIL_KEY, email);
  }

  getProfilePictureUrl(): string | null {
    return localStorage.getItem(PICTURE_KEY);
  }

  setProfilePictureUrl(url: string | null): void {
    if (url) {
      localStorage.setItem(PICTURE_KEY, url);
    } else {
      localStorage.removeItem(PICTURE_KEY);
    }
  }

  clearUserId(): void {
    localStorage.removeItem(STORAGE_KEY);
    localStorage.removeItem(NAME_KEY);
    localStorage.removeItem(EMAIL_KEY);
    localStorage.removeItem(PICTURE_KEY);
  }
}
