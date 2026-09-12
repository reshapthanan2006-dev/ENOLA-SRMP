import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-cv-upload',
  standalone: true,
  templateUrl: './cv-upload.component.html',
  styleUrl: './cv-upload.component.css'
})
export class CvUploadComponent {
  @Input() disabled = false;
  @Output() fileSelected = new EventEmitter<File | null>();

  selectedFileName = '';
  errorMessage = '';

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0] ?? null;

    this.errorMessage = '';
    this.selectedFileName = '';

    if (!file) {
      this.fileSelected.emit(null);
      return;
    }

    const extension = file.name.split('.').pop()?.toLowerCase() ?? '';
    const allowedExtensions = ['pdf', 'doc', 'docx'];
    const maxFileSize = 5 * 1024 * 1024;

    if (!allowedExtensions.includes(extension)) {
      this.errorMessage = 'Only PDF, DOC, and DOCX files are allowed.';
      input.value = '';
      this.fileSelected.emit(null);
      return;
    }

    if (file.size > maxFileSize) {
      this.errorMessage = 'CV file size must not exceed 5 MB.';
      input.value = '';
      this.fileSelected.emit(null);
      return;
    }

    this.selectedFileName = file.name;
    this.fileSelected.emit(file);
  }
}
