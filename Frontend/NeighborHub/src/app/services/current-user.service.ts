import { Injectable } from '@angular/core';

const STORAGE_KEY = 'neighborhub_user_id';

/**
 * Provides the current logged-in user id for API calls.
 * Set the user id when the user logs in (e.g. after auth).
 * When you integrate real auth, replace this with your auth service.
 */
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

  clearUserId(): void {
    localStorage.removeItem(STORAGE_KEY);
  }
}
