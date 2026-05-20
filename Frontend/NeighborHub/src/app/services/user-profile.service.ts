import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { env } from '../../environments/environment.production';
import { ApiResponse } from '../models/item.model';
import { ChangePasswordRequest, UserProfile } from '../models/user-profile.model';

@Injectable({
  providedIn: 'root',
})
export class UserProfileService {
  private apiUrl = env.apiBaseUrl;
  private http = inject(HttpClient);

  getProfile(userId: number): Observable<ApiResponse<UserProfile>> {
    return this.http.get<ApiResponse<UserProfile>>(`${this.apiUrl}/api/UserProfile/${userId}`);
  }

  updateProfilePicture(userId: number, file: File): Observable<ApiResponse<UserProfile>> {
    const formData = new FormData();
    formData.append('profilePicture', file);
    return this.http.put<ApiResponse<UserProfile>>(
      `${this.apiUrl}/api/UserProfile/${userId}/picture`,
      formData
    );
  }

  changePassword(userId: number, payload: ChangePasswordRequest): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(
      `${this.apiUrl}/api/UserProfile/${userId}/password`,
      payload
    );
  }
}
