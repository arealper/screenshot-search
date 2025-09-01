import { Component, inject } from '@angular/core';
import { JsonPipe } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-upload',
  imports: [JsonPipe, MatCardModule, MatButtonModule],
  templateUrl: './upload.component.html',
  styleUrl: './upload.component.css'
})
export class UploadComponent {
  selectedFile?: File;
  result?: any;
  private apiService = inject(ApiService);

  constructor() { }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  onUpload() {
    if (this.selectedFile) {
      this.result = '';
      this.apiService.upload(this.selectedFile).subscribe(res => this.result = res);
    }
  }
}
