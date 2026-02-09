import { AfterViewInit, Component, ElementRef, inject } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { Item } from "./item/item";
import { ItemService } from '../../services/item.service';
import { ApiResponse, ItemResponse } from '../../models/item.model';

@Component({
  selector: 'app-layout',
  imports: [RouterModule, RouterOutlet],
  templateUrl: './layout.html',
  styleUrl: './layout.css',
})
export class Layout  implements AfterViewInit{
  constructor(private el: ElementRef) {}

  itemService = inject(ItemService)
  items : ItemResponse[] = []
  ngOnInit(): void {
    this.itemService.getItems().subscribe((res: ApiResponse<ItemResponse[]>) => { 
          this.items = res.data;
        });

  }

  ngAfterViewInit(): void {
    // Handle arrow clicks for sub-menu toggling
    const arrows = this.el.nativeElement.querySelectorAll('.arrow');
    arrows.forEach((arrow: HTMLElement) => {
      arrow.addEventListener('click', (e: Event) => {
        const arrowParent = (e.target as HTMLElement).parentElement?.parentElement;
        if (arrowParent) {
          arrowParent.classList.toggle('showMenu');
        }
      });
    });

    // Handle sidebar toggle
    const sidebar = this.el.nativeElement.querySelector('.sidebar');
    const sidebarBtn = this.el.nativeElement.querySelector('.bx-menu');
    if (sidebarBtn) {
      sidebarBtn.addEventListener('click', () => {
        sidebar.classList.toggle('close');
      });
    }
  }
}
