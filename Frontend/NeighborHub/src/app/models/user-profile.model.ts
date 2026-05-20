import { ApiResponse } from './item.model';

export interface UserProfile {
  userId: number;
  fullName: string;
  firstName: string;
  lastName: string;
  email: string;
  profilePictureUrl?: string | null;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}

export type UserProfileResponse = ApiResponse<UserProfile>;
