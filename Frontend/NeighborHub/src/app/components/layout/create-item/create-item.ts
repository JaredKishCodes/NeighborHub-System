import { Component } from '@angular/core';
import { ItemService } from '../../../services/item.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-create-item',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-item.html',
  styleUrl: './create-item.css',
})
export class CreateItem {
  selectedFile: File | null = null;
  uploadedImageUrl: string | null = null;
  
  // Matches your C# DTO
  itemModel = {
    name: '',
    description: '',
    category: '',
    itemStatus: 0, // 0 = Available, 1 = OnLoan
    createdAt: new Date(),
    ownerId: 1 // Temporary hardcoded owner ID
  };

  constructor(private itemService: ItemService) {}

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  onSubmit() {
    // We send this to the service that handles the FormData
    this.itemService.createItem(this.itemModel, this.selectedFile!).subscribe(res => {
      console.log('NeighborHub Item Added!');
    });
  }
}
