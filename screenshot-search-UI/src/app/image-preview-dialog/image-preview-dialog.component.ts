import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-image-preview-dialog',
  imports: [],
  templateUrl: './image-preview-dialog.component.html',
  styleUrl: './image-preview-dialog.component.css'
})
export class ImagePreviewDialogComponent {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { filePath: string },
    public dialogRef: MatDialogRef<ImagePreviewDialogComponent>
  ) { }
}
