import { AfterViewInit, Component, ElementRef, inject } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { ItemService } from '../../services/item.service';
import { ApiResponse, ItemResponse } from '../../models/item.model';

@Component({
  selector: 'app-layout',
  imports: [RouterModule, RouterOutlet],
  templateUrl: './layout.html',
  styleUrl: './layout.css',
})
export class Layout implements AfterViewInit {
  private router = inject(Router);
  private el = inject(ElementRef);

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

  onNavClick(event: Event, url: string): void {
    event.preventDefault();
    event.stopPropagation();
    this.router.navigateByUrl(url);
  }
}
