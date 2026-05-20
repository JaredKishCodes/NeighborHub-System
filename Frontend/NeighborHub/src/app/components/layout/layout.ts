import { Component, inject, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CurrentUserService } from '../../services/current-user.service';
import { UserProfileService } from '../../services/user-profile.service';
import { RouterModule, RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterModule, RouterOutlet, FormsModule, CommonModule],
  templateUrl: './layout.html',
  styleUrl: './layout.css',
})
export class Layout implements OnInit {
  private authService = inject(AuthService);
  private currentUserService = inject(CurrentUserService);
  private userProfileService = inject(UserProfileService);

  sidebarClosed = true;
  openMenus: { [key: string]: boolean } = {};

  displayName = 'User';
  userEmail = '';
  profileImageUrl: string | null = null;
  userInitials = 'U';

  profileModalOpen = false;
  profileLoading = false;
  profileError: string | null = null;
  profileSuccess: string | null = null;
  selectedProfileFile: File | null = null;
  pictureSubmitting = false;
  passwordSubmitting = false;

  passwordModel = {
    currentPassword: '',
    newPassword: '',
    confirmNewPassword: '',
  };

  ngOnInit(): void {
    this.refreshUserDisplay();
  }

  refreshUserDisplay(): void {
    this.displayName = this.currentUserService.getDisplayName();
    this.userEmail = this.currentUserService.getEmail();
    const picturePath = this.currentUserService.getProfilePictureUrl();
    this.profileImageUrl = this.authService.resolveProfileImageUrl(picturePath);
    this.userInitials = this.getInitials(this.displayName);
  }

  getInitials(name: string): string {
    const parts = name.trim().split(/\s+/).filter(Boolean);
    if (parts.length === 0) return 'U';
    if (parts.length === 1) return parts[0].charAt(0).toUpperCase();
    return `${parts[0].charAt(0)}${parts[parts.length - 1].charAt(0)}`.toUpperCase();
  }

  toggleSidebar(): void {
    this.sidebarClosed = !this.sidebarClosed;
  }

  toggleSubMenu(menuId: string): void {
    this.openMenus[menuId] = !this.openMenus[menuId];
  }

  openProfileModal(): void {
    this.profileModalOpen = true;
    this.profileError = null;
    this.profileSuccess = null;
    this.selectedProfileFile = null;
    this.passwordModel = {
      currentPassword: '',
      newPassword: '',
      confirmNewPassword: '',
    };
    const dialog = document.getElementById('profile-modal') as HTMLDialogElement;
    if (dialog) dialog.showModal();
  }

  closeProfileModal(): void {
    this.profileModalOpen = false;
    const dialog = document.getElementById('profile-modal') as HTMLDialogElement;
    if (dialog) dialog.close();
  }

  onProfilePictureSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.selectedProfileFile = input.files?.[0] ?? null;
  }

  saveProfilePicture(): void {
    const userId = this.currentUserService.getUserId();
    if (userId == null) {
      this.profileError = 'User session not found. Please log in again.';
      return;
    }
    if (!this.selectedProfileFile) {
      this.profileError = 'Please choose an image first.';
      return;
    }

    this.pictureSubmitting = true;
    this.profileError = null;
    this.profileSuccess = null;

    this.userProfileService.updateProfilePicture(userId, this.selectedProfileFile).subscribe({
      next: (res) => {
        this.pictureSubmitting = false;
        if (res.data?.profilePictureUrl) {
          this.currentUserService.setProfilePictureUrl(res.data.profilePictureUrl);
          this.refreshUserDisplay();
        }
        this.profileSuccess = 'Profile picture updated.';
        this.selectedProfileFile = null;
      },
      error: (err) => {
        this.pictureSubmitting = false;
        this.profileError = err?.error?.message ?? err?.message ?? 'Failed to update profile picture.';
      },
    });
  }

  changePassword(): void {
    const userId = this.currentUserService.getUserId();
    if (userId == null) {
      this.profileError = 'User session not found. Please log in again.';
      return;
    }
    if (!this.passwordModel.currentPassword || !this.passwordModel.newPassword) {
      this.profileError = 'Please fill in all password fields.';
      return;
    }
    if (this.passwordModel.newPassword !== this.passwordModel.confirmNewPassword) {
      this.profileError = 'New passwords do not match.';
      return;
    }

    this.passwordSubmitting = true;
    this.profileError = null;
    this.profileSuccess = null;

    this.userProfileService.changePassword(userId, this.passwordModel).subscribe({
      next: () => {
        this.passwordSubmitting = false;
        this.profileSuccess = 'Password changed successfully.';
        this.passwordModel = {
          currentPassword: '',
          newPassword: '',
          confirmNewPassword: '',
        };
      },
      error: (err) => {
        this.passwordSubmitting = false;
        this.profileError = err?.error?.message ?? err?.message ?? 'Failed to change password.';
      },
    });
  }

  onLogout(event: Event): void {
    event.preventDefault();
    if (window.confirm('Are you sure you want to logout?')) {
      this.authService.logout();
    }
  }
}
