import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import {
  Application as JobApplication
} from '../../../seeker/models/application.model';

import {
  ApplicationService
} from '../../../seeker/services/application.service';

@Component({
  selector: 'app-employer-applications',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './applications.component.html',
  styleUrl: './applications.component.css'
})
export class ApplicationsComponent implements OnInit {

  applications: JobApplication[] = [];

  jobVacancyId = 0;

  isLoading = false;
  errorMessage = '';

  constructor(
    private applicationService: ApplicationService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {

    this.jobVacancyId = Number(
      this.route.snapshot.paramMap.get(
        'jobVacancyId'
      )
    );

    if (!this.jobVacancyId) {

      this.errorMessage =
        'Vacancy information is missing.';

      return;
    }

    this.loadApplications();
  }

  loadApplications(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.applicationService
      .getApplicationsByVacancy(
        this.jobVacancyId
      )
      .subscribe({

        next: (data) => {

          this.applications = data;

          this.isLoading = false;
        },

        error: (
          error: HttpErrorResponse
        ) => {

          this.isLoading = false;
          this.applications = [];

          if (error.status === 401) {

            this.errorMessage =
              'Please sign in as an employer.';

            return;
          }

          if (error.status === 403) {

            this.errorMessage =
              'You are not allowed to view applications for this vacancy.';

            return;
          }

          if (error.status === 404) {

            this.errorMessage =
              'Vacancy not found.';

            return;
          }

          this.errorMessage =
            'Unable to load applications.';
        }

      });
  }

  viewRankedApplicants(): void {

    if (!this.jobVacancyId) {
      return;
    }

    this.router.navigate([
      '/employer/vacancies',
      this.jobVacancyId,
      'applicants'
    ]);
  }

  getPendingCount(): number {

    return this.applications.filter(
      (application) =>
        application.status === 'Pending'
    ).length;
  }

  getShortlistedCount(): number {

    return this.applications.filter(
      (application) =>
        application.status === 'Shortlisted'
    ).length;
  }

  getAcceptedCount(): number {

    return this.applications.filter(
      (application) =>
        application.status === 'Accepted'
    ).length;
  }

  getRejectedCount(): number {

    return this.applications.filter(
      (application) =>
        application.status === 'Rejected'
    ).length;
  }
}