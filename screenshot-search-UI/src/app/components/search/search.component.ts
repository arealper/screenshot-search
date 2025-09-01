import { DecimalPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { ImagePreviewDialogComponent } from '../../image-preview-dialog/image-preview-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-search',
  imports: [DecimalPipe, FormsModule, MatCardModule, MatInputModule, MatFormFieldModule, MatButtonModule],
  templateUrl: './search.component.html',
  styleUrl: './search.component.css'
})
export class SearchComponent {
  query: string = '';
  results: any[] = [];
  private apiService = inject(ApiService);
  private dialog = inject(MatDialog);

  constructor() { }

  onSearch() {
    if (!this.query.trim()) return;

    this.results = [];
    this.apiService.search(`${encodeURIComponent(this.query)}`)
      .subscribe({
        next: (res) => this.results = res,
        error: (err) => console.error(err)
      });
  }

  openImage(filePath: string) {
    this.dialog.open(ImagePreviewDialogComponent, {
      data: { filePath },
      panelClass: 'full-screen-dialog'
    });
  }
}
