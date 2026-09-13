import { HttpErrorResponse } from '@angular/common/http';
import {
  Component,
  DestroyRef,
  OnInit
} from '@angular/core';
import {
  takeUntilDestroyed
} from '@angular/core/rxjs-interop';
import { ActivatedRoute } from '@angular/router';

import { MatchingJob } from '../../../matching/models/matching-job.model';
import { MatchingService } from '../../../matching/services/matching.service';
import { ApplicationService } from '../../services/application.service';

import { ScoreBadgeComponent } from '../../../../shared/components/score-badge/score-badge.component';
import { SkillListPipe } from '../../../../shared/pipes/skill-list.pipe';
import { ExperienceYearsPipe } from '../../../../shared/pipes/experience-years.pipe';
import { ShortTextPipe } from '../../../../shared/pipes/short-text.pipe';
import { HighlightDirective } from '../../../../shared/directives/highlight.directive';

@Component({
  selector: 'app-matching-jobs',
  standalone: true,
  imports: [
    ScoreBadgeComponent,
    SkillListPipe,
    ExperienceYearsPipe,
    ShortTextPipe,
    HighlightDirective
  ],
  templateUrl: './matching-jobs.component.html',
  styleUrl: './matching-jobs.component.css'
})
export class MatchingJobsComponent implements OnInit {

  job: MatchingJob | null = null;

  isLoading = false;
  errorMessage = '';

  isApplying = false;
  isApplied = false;

  applyMessage = '';
  applyErrorMessage = '';

  constructor(
    private matchingService: MatchingService,
    private applicationService: ApplicationService,
    private route: ActivatedRoute,
    private destroyRef: DestroyRef
  ) { }

  ngOnInit(): void {

    const jobVacancyId = Number(
      this.route.snapshot.paramMap.get('jobVacancyId')
    );

    if (jobVacancyId > 0) {
      this.loadJob(jobVacancyId);
    }
  }

  loadJob(
    jobVacancyId: number
  ): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.job = null;

    this.isApplying = false;
    this.isApplied = false;

    this.applyMessage = '';
    this.applyErrorMessage = '';

    this.matchingService
      .getJobSeekerJobDetail(jobVacancyId)
      .pipe(
        takeUntilDestroyed(
          this.destroyRef
        )
      )
      .subscribe({

        next: (data) => {

          this.job = data;
          this.isLoading = false;

          this.checkAlreadyApplied(
            data.jobVacancyId
          );
        },

        error: () => {

          this.errorMessage =
            'Unable to load matching job.';

          this.isLoading = false;
        }

      });
  }

  checkAlreadyApplied(
    jobVacancyId: number
  ): void {

    this.applicationService
      .getMyApplications()
      .pipe(
        takeUntilDestroyed(
          this.destroyRef
        )
      )
      .subscribe({

        next: (applications) => {

          this.isApplied =
            applications.some(
              (application) =>
                application.jobVacancyId === jobVacancyId
            );
        },

        error: () => {

          this.isApplied = false;
        }

      });
  }

  applyForJob(): void {

    if (
      !this.job ||
      this.isApplying ||
      this.isApplied
    ) {
      return;
    }

    const jobVacancyId =
      this.job.jobVacancyId;

    this.isApplying = true;

    this.applyMessage = '';
    this.applyErrorMessage = '';

    this.applicationService
      .applyForJob(jobVacancyId)
      .pipe(
        takeUntilDestroyed(
          this.destroyRef
        )
      )
      .subscribe({

        next: () => {

          this.isApplying = false;

          this.applyMessage =
            'Application submitted successfully.';

          this.checkAlreadyApplied(
            jobVacancyId
          );
        },

        error: (
          error: HttpErrorResponse
        ) => {

          this.isApplying = false;

          if (error.status === 409) {

            this.applyMessage =
              'You have already applied for this job.';

            this.checkAlreadyApplied(
              jobVacancyId
            );

            return;
          }

          if (error.status === 401) {

            this.applyErrorMessage =
              'Please sign in before applying for this job.';

            return;
          }

          this.applyErrorMessage =
            'Unable to submit your application. Please try again.';
        }

      });
  }
}