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
  FormsModule
} from '@angular/forms';

import {
  ActivatedRoute,
  Router
} from '@angular/router';

import {
  takeUntilDestroyed
} from '@angular/core/rxjs-interop';

import {
  Job
} from '../../models/job.model';

import {
  JobCardComponent
} from '../../components/job-card/job-card.component';

import {
  JobsService
} from '../../services/jobs.service';

@Component({
  selector: 'app-job-list',
  standalone: true,
  imports: [
    FormsModule,
    JobCardComponent
  ],
  templateUrl: './job-list.component.html',
  styleUrl: './job-list.component.css'
})
export class JobListComponent implements OnInit {

  private readonly jobsService =
    inject(JobsService);

  private readonly route =
    inject(ActivatedRoute);

  private readonly router =
    inject(Router);

  private readonly destroyRef =
    inject(DestroyRef);

  jobs: Job[] = [];

  keyword = '';

  location = '';

  minExperience:
    number | null = null;

  isLoading = false;

  errorMessage = '';

  get hasActiveFilters(): boolean {

    return (
      this.keyword.trim().length > 0 ||
      this.location.trim().length > 0 ||
      this.minExperience !== null
    );
  }

  ngOnInit(): void {

    const queryParams =
      this.route.snapshot
        .queryParamMap;

    this.keyword =
      queryParams.get('keyword') ?? '';

    this.location =
      queryParams.get('location') ?? '';

    const experienceText =
      queryParams.get(
        'minExperience'
      );

    if (experienceText !== null) {

      const experience =
        Number(experienceText);

      this.minExperience =
        Number.isNaN(experience)
          ? null
          : experience;
    }

    this.loadJobs();
  }

  loadJobs(): void {

    this.isLoading = true;

    this.errorMessage = '';

    this.jobsService
      .searchJobs(
        this.keyword,
        this.location,
        this.minExperience
      )
      .pipe(
        takeUntilDestroyed(
          this.destroyRef
        )
      )
      .subscribe({

        next: jobs => {

          this.jobs = jobs;

          this.isLoading = false;
        },

        error: (
          error: HttpErrorResponse
        ) => {

          this.jobs = [];

          this.isLoading = false;

          if (error.status === 401) {

            this.errorMessage =
              'Please sign in as a Job Seeker to search jobs.';

            return;
          }

          if (error.status === 403) {

            this.errorMessage =
              'Your account is not allowed to search jobs.';

            return;
          }

          this.errorMessage =
            'Unable to load jobs. Please try again.';
        }

      });
  }

  searchJobs(): void {

    this.router.navigate(
      ['/seeker/jobs'],
      {
        queryParams: {

          keyword:
            this.keyword.trim() || null,

          location:
            this.location.trim() || null,

          minExperience:
            this.minExperience

        }
      }
    );

    this.loadJobs();
  }

  clearFilters(): void {

    this.keyword = '';

    this.location = '';

    this.minExperience = null;

    this.router.navigate(
      ['/seeker/jobs']
    );

    this.loadJobs();
  }
}