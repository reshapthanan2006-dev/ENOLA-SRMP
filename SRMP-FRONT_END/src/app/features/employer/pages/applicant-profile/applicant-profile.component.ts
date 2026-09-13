import {
  Component,
  DestroyRef,
  OnInit,
  inject
} from '@angular/core';

import {
  HttpErrorResponse
} from '@angular/common/http';

import {
  ActivatedRoute
} from '@angular/router';

import {
  takeUntilDestroyed
} from '@angular/core/rxjs-interop';

import {
  ApplicantProfile
} from '../../models/applicant-profile.model';

import {
  ApplicantCv
} from '../../models/applicant-cv.model';

import {
  ApplicantProfileService
} from '../../services/applicant-profile.service';

@Component({
  selector: 'app-applicant-profile',
  standalone: true,
  imports: [],
  templateUrl: './applicant-profile.component.html',
  styleUrl: './applicant-profile.component.css'
})
export class ApplicantProfileComponent
  implements OnInit {

  private readonly route =
    inject(ActivatedRoute);

  private readonly applicantProfileService =
    inject(ApplicantProfileService);

  private readonly destroyRef =
    inject(DestroyRef);

  profile:
    ApplicantProfile | null = null;

  cv:
    ApplicantCv | null = null;

  jobVacancyId = 0;

  jobSeekerId = 0;

  isLoading = false;

  isCvLoading = false;

  isDownloadingCv = false;

  errorMessage = '';

  cvMessage = '';

  get canViewCv(): boolean {

    return (
      this.cv?.contentType
        ?.toLowerCase() ===
      'application/pdf'
    );
  }

  ngOnInit(): void {

    this.jobVacancyId =
      Number(
        this.route.snapshot
          .paramMap
          .get('jobVacancyId')
      );

    this.jobSeekerId =
      Number(
        this.route.snapshot
          .paramMap
          .get('jobSeekerId')
      );

    if (
      !this.jobVacancyId ||
      !this.jobSeekerId
    ) {

      this.errorMessage =
        'Applicant information is missing.';

      return;
    }

    this.loadApplicantProfile();

    this.loadApplicantCv();
  }

  private loadApplicantProfile(): void {

    this.isLoading = true;

    this.errorMessage = '';

    this.applicantProfileService
      .getApplicantProfile(
        this.jobVacancyId,
        this.jobSeekerId
      )
      .pipe(
        takeUntilDestroyed(
          this.destroyRef
        )
      )
      .subscribe({

        next: profile => {

          this.profile = profile;

          this.isLoading = false;
        },

        error: (
          error: HttpErrorResponse
        ) => {

          this.isLoading = false;

          this.errorMessage =
            this.getProfileErrorMessage(
              error
            );
        }

      });
  }

  private loadApplicantCv(): void {

    this.isCvLoading = true;

    this.cvMessage = '';

    this.applicantProfileService
      .getApplicantCv(
        this.jobVacancyId,
        this.jobSeekerId
      )
      .pipe(
        takeUntilDestroyed(
          this.destroyRef
        )
      )
      .subscribe({

        next: cv => {

          this.cv = cv;

          this.isCvLoading = false;
        },

        error: (
          error: HttpErrorResponse
        ) => {

          this.cv = null;

          this.isCvLoading = false;

          if (error.status === 404) {

            this.cvMessage =
              'This applicant has not uploaded a CV yet.';

            return;
          }

          this.cvMessage =
            'Unable to load applicant CV.';
        }

      });
  }

  viewCv(): void {

    if (
      !this.cv ||
      !this.canViewCv ||
      this.isDownloadingCv
    ) {
      return;
    }

    const previewWindow =
      window.open(
        '',
        '_blank'
      );

    this.isDownloadingCv = true;

    this.cvMessage = '';

    this.applicantProfileService
      .downloadApplicantCv(
        this.jobVacancyId,
        this.jobSeekerId
      )
      .pipe(
        takeUntilDestroyed(
          this.destroyRef
        )
      )
      .subscribe({

        next: blob => {

          this.isDownloadingCv = false;

          const fileUrl =
            URL.createObjectURL(blob);

          if (previewWindow) {

            previewWindow.location.href =
              fileUrl;

          } else {

            window.open(
              fileUrl,
              '_blank'
            );
          }

          window.setTimeout(
            () => {
              URL.revokeObjectURL(
                fileUrl
              );
            },
            60000
          );
        },

        error: () => {

          this.isDownloadingCv = false;

          previewWindow?.close();

          this.cvMessage =
            'Unable to open applicant CV.';
        }

      });
  }

  downloadCv(): void {

    if (
      !this.cv ||
      this.isDownloadingCv
    ) {
      return;
    }

    this.isDownloadingCv = true;

    this.cvMessage = '';

    this.applicantProfileService
      .downloadApplicantCv(
        this.jobVacancyId,
        this.jobSeekerId
      )
      .pipe(
        takeUntilDestroyed(
          this.destroyRef
        )
      )
      .subscribe({

        next: blob => {

          this.isDownloadingCv = false;

          const fileUrl =
            URL.createObjectURL(blob);

          const anchor =
            document.createElement('a');

          anchor.href =
            fileUrl;

          anchor.download =
            this.cv?.originalFileName ??
            `Applicant-${this.jobSeekerId}-CV`;

          document.body
            .appendChild(anchor);

          anchor.click();

          anchor.remove();

          URL.revokeObjectURL(
            fileUrl
          );
        },

        error: () => {

          this.isDownloadingCv = false;

          this.cvMessage =
            'Unable to download applicant CV.';
        }

      });
  }

  formatFileSize(
    bytes: number
  ): string {

    if (bytes < 1024) {
      return `${bytes} B`;
    }

    const kilobytes =
      bytes / 1024;

    if (kilobytes < 1024) {
      return `${kilobytes.toFixed(1)} KB`;
    }

    const megabytes =
      kilobytes / 1024;

    return `${megabytes.toFixed(1)} MB`;
  }

  formatDate(
    value: string
  ): string {

    return new Date(value)
      .toLocaleString();
  }

  private getProfileErrorMessage(
    error: HttpErrorResponse
  ): string {

    if (error.status === 401) {
      return 'Please sign in as an employer.';
    }

    if (error.status === 403) {
      return 'You are not allowed to view this applicant.';
    }

    if (error.status === 404) {
      return (
        error.error?.message ??
        'Applicant profile not found.'
      );
    }

    return 'Unable to load applicant profile.';
  }
}