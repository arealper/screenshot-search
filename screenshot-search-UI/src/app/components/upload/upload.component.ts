import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { ApiService } from '../../services/api.service';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@Component({
  selector: 'app-upload',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatProgressBarModule],
  templateUrl: './upload.component.html',
  styleUrl: './upload.component.css'
})
export class UploadComponent {
  selectedFile?: File;
  uploadProgress: number = 0;
  isUploading: boolean = false;
  uploadStatus: 'idle' | 'uploading' | 'success' | 'error' = 'idle';
  message: string = '';
  extractedText: string = '';

  private apiService = inject(ApiService);

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
    this.uploadStatus = 'idle';
    this.uploadProgress = 0;
    this.message = '';
    this.extractedText = '';
  }

  onUpload() {
    if (!this.selectedFile) return;

    this.isUploading = true;
    this.uploadProgress = 0;
    this.uploadStatus = 'uploading';
    this.message = '';

    this.apiService.upload(this.selectedFile).subscribe({
      next: (event: any) => {
        if (event.type === HttpEventType.UploadProgress) {
          this.uploadProgress = Math.round(100 * event.loaded / event.total);
        } else if (event instanceof HttpResponse) {
          this.uploadStatus = 'success';
          this.isUploading = false;
          this.message = 'Screenshot uploaded and processed successfully!';
          this.extractedText = event.body?.extractedText || '';
        }
      },
      error: (err) => {
        this.uploadStatus = 'error';
        this.isUploading = false;
        this.message = 'Upload failed. Please try again.';
        console.error(err);
      }
    });
  }

  reset() {
    this.selectedFile = undefined;
    this.uploadProgress = 0;
    this.isUploading = false;
    this.uploadStatus = 'idle';
    this.message = '';
    this.extractedText = '';
  }
}
