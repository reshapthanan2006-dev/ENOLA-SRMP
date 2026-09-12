import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { MatchResult } from '../../../matching/models/match-result.model';
import { MatchingService } from '../../../matching/services/matching.service';

import { ApplicationService } from '../../../seeker/services/application.service';
import { ContactRequestService } from '../../../seeker/services/contact-request.service';

import { ScoreBadgeComponent } from '../../../../shared/components/score-badge/score-badge.component';
import { SkillListPipe } from '../../../../shared/pipes/skill-list.pipe';

@Component({
  selector: 'app-applicants',
  standalone: true,
  imports: [
    ScoreBadgeComponent,
    SkillListPipe
  ],
  templateUrl: './applicants.component.html',
  styleUrl: './applicants.component.css'
})
export class ApplicantsComponent implements OnInit {

  applicants: MatchResult[] = [];

  jobVacancyId = 0;

  isLoading = false;
  errorMessage = '';

  updatingApplicationId: number | null = null;
  sendingContactApplicationId: number | null = null;

  actionMessage = '';
  actionErrorMessage = '';

  sentContactApplicationIds = new Set<number>();

  readonly applicationStatuses = [
    'Pending',
    'Shortlisted',
    'Accepted',
    'Rejected'
  ];

  constructor(
    private matchingService: MatchingService,
    private applicationService: ApplicationService,
    private contactRequestService: ContactRequestService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {

    this.jobVacancyId = Number(
      this.route.snapshot.paramMap.get('jobVacancyId')
    );

    if (!this.jobVacancyId) {

      this.errorMessage =
        'Vacancy information is missing.';

      return;
    }

    this.loadApplicants(
      this.jobVacancyId
    );
  }

  loadApplicants(
    jobVacancyId: number
  ): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.matchingService
      .getRankedApplicants(jobVacancyId)
      .subscribe({

        next: (data) => {

          this.applicants = data;

          this.isLoading = false;

          this.loadSentContactRequests();
        },

        error: (
          error: HttpErrorResponse
        ) => {

          this.isLoading = false;

          if (error.status === 401) {

            this.errorMessage =
              'Please sign in as an employer.';

            return;
          }

          if (error.status === 403) {

            this.errorMessage =
              'You are not allowed to view applicants for this vacancy.';

            return;
          }

          if (error.status === 404) {

            this.errorMessage =
              'Vacancy not found.';

            return;
          }

          this.errorMessage =
            'Unable to load applicants.';
        }

      });
  }

  loadSentContactRequests(): void {

    this.contactRequestService
      .getSentRequests()
      .subscribe({

        next: (requests) => {

          this.sentContactApplicationIds =
            new Set(
              requests.map(
                (request) =>
                  request.applicationId
              )
            );
        },

        error: () => {

          this.sentContactApplicationIds =
            new Set<number>();
        }

      });
  }

  updateStatus(
    applicationId: number,
    status: string
  ): void {

    if (
      this.updatingApplicationId !== null
    ) {
      return;
    }

    this.updatingApplicationId =
      applicationId;

    this.actionMessage = '';
    this.actionErrorMessage = '';

    this.applicationService
      .updateApplicationStatus(
        applicationId,
        status
      )
      .subscribe({

        next: () => {

          this.updatingApplicationId = null;

          this.actionMessage =
            `Application status updated to ${status}.`;

          // Re-fetch ranked applicants from backend
          // so the screen always reflects backend data.
          this.loadApplicants(
            this.jobVacancyId
          );
        },

        error: (
          error: HttpErrorResponse
        ) => {

          this.updatingApplicationId = null;

          if (error.status === 400) {

            this.actionErrorMessage =
              'Invalid application status.';

            return;
          }

          if (error.status === 401) {

            this.actionErrorMessage =
              'Please sign in as an employer.';

            return;
          }

          if (error.status === 403) {

            this.actionErrorMessage =
              'You are not allowed to update this application.';

            return;
          }

          if (error.status === 404) {

            this.actionErrorMessage =
              'Application not found.';

            return;
          }

          this.actionErrorMessage =
            'Unable to update application status.';
        }

      });
  }

  sendContactRequest(
    applicant: MatchResult
  ): void {

    if (
      this.sendingContactApplicationId !== null ||
      this.hasContactRequest(
        applicant.applicationId
      )
    ) {
      return;
    }

    const confirmed = window.confirm(
      'Send a contact request to this candidate?'
    );

    if (!confirmed) {
      return;
    }

    this.sendingContactApplicationId =
      applicant.applicationId;

    this.actionMessage = '';
    this.actionErrorMessage = '';

    this.contactRequestService
      .createContactRequest(
        applicant.jobSeekerId,
        applicant.applicationId
      )
      .subscribe({

        next: (request) => {

          this.sentContactApplicationIds.add(
            request.applicationId
          );

          this.sendingContactApplicationId =
            null;

          this.actionMessage =
            'Contact request sent successfully.';
        },

        error: (
          error: HttpErrorResponse
        ) => {

          this.sendingContactApplicationId =
            null;

          if (error.status === 409) {

            this.sentContactApplicationIds.add(
              applicant.applicationId
            );

            this.actionMessage =
              'A contact request already exists for this application.';

            return;
          }

          if (error.status === 400) {

            this.actionErrorMessage =
              'Unable to send the contact request for this applicant.';

            return;
          }

          if (error.status === 401) {

            this.actionErrorMessage =
              'Please sign in as an employer.';

            return;
          }

          if (error.status === 403) {

            this.actionErrorMessage =
              'You are not allowed to contact this applicant.';

            return;
          }

          if (error.status === 404) {

            this.actionErrorMessage =
              'Application or vacancy not found.';

            return;
          }

          this.actionErrorMessage =
            'Unable to send contact request.';
        }

      });
  }

  hasContactRequest(
    applicationId: number
  ): boolean {

    return this.sentContactApplicationIds
      .has(applicationId);
  }

  isUpdating(
    applicationId: number
  ): boolean {

    return this.updatingApplicationId ===
      applicationId;
  }

  isSendingContact(
    applicationId: number
  ): boolean {

    return this.sendingContactApplicationId ===
      applicationId;
  }

  viewProfile(
    jobSeekerId: number
  ): void {

    this.router.navigate([
      '/employer/vacancies',
      this.jobVacancyId,
      'applicants',
      jobSeekerId
    ]);
  }
}