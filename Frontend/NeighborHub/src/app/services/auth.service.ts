import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { env } from '../../environments/environment.production';
import { CurrentUserService } from './current-user.service';
import { UserProfileService } from './user-profile.service';
import { Router } from '@angular/router';
//import { env } from '../../environments/environment.production';

interface RegisterRequest {
  firstName: string;
  lastName: string;
  streetAddress: string;
  city: string;
  baranggay: string;
  email: string;
  password: string;
  confirmPassword: string;
}

interface AuthResponse {
  success: boolean;
  message: string;
  firstName: string;
  lastName: string;
  email: string;
  token: string;
}

interface LoginRequest {
  email: string;
  password: string;
}

interface LoginResponse {
  success: boolean;
  message: string;
  role?: string;
  firstName: string;
  lastName: string;
  ownerId?: number;
  email: string;
  token: string;
}

const TOKEN_KEY = 'neighborhub_auth_token';
const OWNER_ID_KEY = 'neighborhub_owner_id';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private currentUserService = inject(CurrentUserService);
  private userProfileService = inject(UserProfileService);
  router = inject(Router);

  private apiUrl = `${env.apiBaseUrl}/api/Account`;

  private loggedIn$ = new BehaviorSubject<boolean>(this.hasToken());

  get isLoggedIn$(): Observable<boolean> {
    return this.loggedIn$.asObservable();
  }

  isLoggedIn(): boolean {
    return this.loggedIn$.value;
  }

  register(payload: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, payload);
  }

  login(payload: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, payload).pipe(
      tap((res) => {
        if (res.success && res.token) {
          localStorage.setItem(TOKEN_KEY, res.token);
          localStorage.setItem(OWNER_ID_KEY, (res.ownerId ?? '').toString());
          const displayName = `${res.firstName ?? ''} ${res.lastName ?? ''}`.trim();
          this.currentUserService.setDisplayName(displayName || 'User');
          this.currentUserService.setEmail(res.email ?? '');
          if (res.ownerId != null) {
            this.currentUserService.setUserId(res.ownerId);
            this.loadUserProfile(res.ownerId);
          }
          this.loggedIn$.next(true);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(OWNER_ID_KEY);
    this.currentUserService.clearUserId();
    this.loggedIn$.next(false);
    this.router.navigateByUrl('/auth');
  }

  loadUserProfile(userId: number): void {
    this.userProfileService.getProfile(userId).subscribe({
      next: (res) => {
        if (res.data) {
          const profile = res.data;
          const name = profile.fullName?.trim()
            || `${profile.firstName} ${profile.lastName}`.trim();
          if (name) {
            this.currentUserService.setDisplayName(name);
          }
          if (profile.email) {
            this.currentUserService.setEmail(profile.email);
          }
          this.currentUserService.setProfilePictureUrl(profile.profilePictureUrl ?? null);
        }
      },
    });
  }

  resolveProfileImageUrl(path: string | null | undefined): string | null {
    if (!path) return null;
    if (path.startsWith('http')) return path;
    return `${env.profileImageBaseUrl}${path}`;
  }

  private hasToken(): boolean {
    const token = localStorage.getItem(TOKEN_KEY);
    const has = !!token;
    if (has) {
      const ownerIdRaw = localStorage.getItem(OWNER_ID_KEY);
      const ownerId = ownerIdRaw ? parseInt(ownerIdRaw, 10) : null;
      if (ownerId != null && !Number.isNaN(ownerId)) {
        this.currentUserService.setUserId(ownerId);
        this.loadUserProfile(ownerId);
      }
    }
    return has;
  }
}

