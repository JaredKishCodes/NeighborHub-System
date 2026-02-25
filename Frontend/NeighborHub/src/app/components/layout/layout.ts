import { AfterViewInit, Component, ElementRef, inject } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { ItemService } from '../../services/item.service';
import { ApiResponse, ItemResponse } from '../../models/item.model';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-layout',
  imports: [RouterModule, RouterOutlet],
  templateUrl: './layout.html',
  styleUrl: './layout.css',
})
export class Layout implements AfterViewInit {
  private el = inject(ElementRef);
  private authService = inject(AuthService);

  itemService = inject(ItemService);
  items: ItemResponse[] = [];

  /** Sidebar starts collapsed (narrow). */
  sidebarClosed = true;

  ngOnInit(): void {
    this.itemService.getItems().subscribe((res: ApiResponse<ItemResponse[]>) => {
      this.items = res.data ?? [];
    });
  }

  ngAfterViewInit(): void {
    const arrows = this.el.nativeElement.querySelectorAll('.arrow');
    arrows.forEach((arrow: HTMLElement) => {
      arrow.addEventListener('click', (e: Event) => {
        const li = (e.target as HTMLElement).closest('li');
        if (li) li.classList.toggle('showMenu');
      });
    });
  }

  toggleSidebar(): void {
    this.sidebarClosed = !this.sidebarClosed;
  }

  onLogout(event: Event): void {
    event.preventDefault();
    const confirmed = window.confirm('Are you sure you want to logout?');
    if (!confirmed) return;
    this.authService.logout();
  }
}
