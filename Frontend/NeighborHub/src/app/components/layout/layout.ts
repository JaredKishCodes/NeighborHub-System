import { Component, inject, OnInit, ChangeDetectorRef, NgZone, ApplicationRef } from '@angular/core';
import { ItemService } from '../../services/item.service';
import { AuthService } from '../../services/auth.service';
import { NavigationEnd, Router, RouterModule, RouterOutlet } from '@angular/router';
import { ApiResponse, ItemResponse } from '../../models/item.model';
import { filter } from 'rxjs';

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
  private itemService = inject(ItemService);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);
  private ngZone = inject(NgZone);

  items: ItemResponse[] = [];
  sidebarClosed = true;
  private appRef = inject(ApplicationRef);
  openMenus: { [key: string]: boolean } = {};

  ngOnInit(): void {
    this.itemService.getItems().subscribe((res: ApiResponse<ItemResponse[]>) => {
      this.items = res.data ?? [];
    });

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      this.ngZone.run(() => {
        this.cdr.detectChanges();
      });
    });
  }

  navigate(path: string): void {
    this.ngZone.run(() => {
      this.router.navigate([path]).then((success) => {
        if (success) {
          // Force the entire application to synchronize with the browser
          this.appRef.tick(); 
        }
      });
    });
  }

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