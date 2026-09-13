import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../../../../shared/components/navbar/navbar.component';
import { CvUploadComponent } from '../../components/cv-upload/cv-upload.component';
import { SeekerCv } from '../../models/cv.model';
import { CvService } from '../../services/cv.service';

@Component({
  selector: 'app-seeker-cv',
  standalone: true,
  imports: [CvUploadComponent,NavbarComponent],
  templateUrl: './cv.component.html',
  styleUrl: './cv.component.css'
})
export class CvComponent implements OnInit {
  cv: SeekerCv | null = null;
  selectedFile: File | null = null;

  isLoading = true;
  isUploading = false;
  isDownloading = false;
  isDeleting = false;

  errorMessage = '';
  successMessage = '';

  constructor(private cvService: CvService) { }

  ngOnInit(): void {
    this.loadCv();
  }

  onFileSelected(file: File | null): void {
    this.selectedFile = file;
    this.errorMessage = '';
    this.successMessage = '';
  }

  uploadCv(): void {
    if (!this.selectedFile) {
      this.errorMessage = 'Please choose a CV file first.';
      return;
    }

    this.isUploading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.cvService.uploadCv(this.selectedFile).subscribe({
      next: (cv) => {
        this.cv = cv;
        this.selectedFile = null;
        this.isUploading = false;
        this.successMessage = 'CV uploaded successfully.';
      },
      error: (error: HttpErrorResponse) => {
        this.isUploading = false;
        this.errorMessage = this.getErrorMessage(
          error,
          'Unable to upload CV.'
        );
      }
    });
  }

  viewCv(): void {
    if (!this.cv) {
      return;
    }

    this.isDownloading = true;
    this.errorMessage = '';

    this.cvService.downloadCv().subscribe({
      next: (blob) => {
        const fileUrl = URL.createObjectURL(blob);
        window.open(fileUrl, '_blank', 'noopener');
        window.setTimeout(() => URL.revokeObjectURL(fileUrl), 60_000);
        this.isDownloading = false;
      },
      error: (error: HttpErrorResponse) => {
        this.isDownloading = false;
        this.errorMessage = this.getErrorMessage(
          error,
          'Unable to open CV.'
        );
      }
    });
  }

  downloadCv(): void {
    if (!this.cv) {
      return;
    }

    this.isDownloading = true;
    this.errorMessage = '';

    this.cvService.downloadCv().subscribe({
      next: (blob) => {
        const fileUrl = URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = fileUrl;
        link.download = this.cv?.originalFileName || 'cv';
        link.click();
        URL.revokeObjectURL(fileUrl);
        this.isDownloading = false;
      },
      error: (error: HttpErrorResponse) => {
        this.isDownloading = false;
        this.errorMessage = this.getErrorMessage(
          error,
          'Unable to download CV.'
        );
      }
    });
  }

  deleteCv(): void {
    if (!this.cv) {
      return;
    }

    const confirmed = window.confirm(
      'Are you sure you want to delete your CV?'
    );

    if (!confirmed) {
      return;
    }

    this.isDeleting = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.cvService.deleteCv().subscribe({
      next: () => {
        this.cv = null;
        this.isDeleting = false;
        this.successMessage = 'CV deleted successfully.';
      },
      error: (error: HttpErrorResponse) => {
        this.isDeleting = false;
        this.errorMessage = this.getErrorMessage(
          error,
          'Unable to delete CV.'
        );
      }
    });
  }

  formatFileSize(bytes: number): string {
    if (bytes < 1024) {
      return `${bytes} B`;
    }

    if (bytes < 1024 * 1024) {
      return `${(bytes / 1024).toFixed(1)} KB`;
    }

    return `${(bytes / (1024 * 1024)).toFixed(2)} MB`;
  }

  formatDate(value: string): string {
    return new Date(value).toLocaleString();
  }

  private loadCv(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.cvService.getCv().subscribe({
      next: (cv) => {
        this.cv = cv;
        this.isLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        this.isLoading = false;

        if (error.status === 404) {
          this.cv = null;
          return;
        }

        this.errorMessage = this.getErrorMessage(
          error,
          'Unable to load CV information.'
        );
      }
    });
  }

  private getErrorMessage(
    error: HttpErrorResponse,
    fallbackMessage: string
  ): string {
    if (typeof error.error?.message === 'string') {
      return error.error.message;
    }

    return fallbackMessage;
  }
}
