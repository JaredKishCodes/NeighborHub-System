import { Component, inject, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { RouterModule, RouterOutlet } from '@angular/router';

// The decorator MUST stay exactly here, with no text/comments between it and the class
@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterModule, RouterOutlet],
  templateUrl: './layout.html',
  styleUrl: './layout.css',
})
export class Layout implements OnInit { // This must follow the closing ')' of @Component
  private authService = inject(AuthService);

  sidebarClosed = true;
  openMenus: { [key: string]: boolean } = {};

  ngOnInit(): void {}

  toggleSidebar(): void {
    this.sidebarClosed = !this.sidebarClosed;
  }

  toggleSubMenu(menuId: string): void {
    this.openMenus[menuId] = !this.openMenus[menuId];
  }

  onLogout(event: Event): void {
    event.preventDefault();
    if (window.confirm('Are you sure you want to logout?')) {
      this.authService.logout();
    }
  }
}