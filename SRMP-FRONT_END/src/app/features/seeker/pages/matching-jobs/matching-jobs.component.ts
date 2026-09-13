import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { MatchingJob } from '../../../matching/models/matching-job.model';
import { MatchingService } from '../../../matching/services/matching.service';
import { ScoreBadgeComponent } from '../../../../shared/components/score-badge/score-badge.component';
import { ExperienceYearsPipe } from '../../../../shared/pipes/experience-years.pipe';
import { ShortTextPipe } from '../../../../shared/pipes/short-text.pipe';
import { HighlightDirective } from '../../../../shared/directives/highlight.directive';
import { SkillGapPanelComponent } from '../../components/skill-gap-panel/skill-gap-panel.component';

@Component({
  selector: 'app-matching-jobs',
  standalone: true,
  imports: [
    ScoreBadgeComponent,
    ExperienceYearsPipe,
    ShortTextPipe,
    HighlightDirective,
    SkillGapPanelComponent
  ],
  templateUrl: './matching-jobs.component.html',
  styleUrl: './matching-jobs.component.css'
})
export class MatchingJobsComponent implements OnInit {

  job: MatchingJob | null = null;

  isLoading = false;

  errorMessage = '';

  constructor(
    private matchingService: MatchingService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {

    const jobVacancyId = Number(
      this.route.snapshot.paramMap.get('jobVacancyId')
    );

    if (!jobVacancyId || jobVacancyId <= 0) {
      this.errorMessage = 'Invalid job vacancy.';
      return;
    }

    this.loadJob(jobVacancyId);
  }

  loadJob(jobVacancyId: number): void {

    this.isLoading = true;
    this.errorMessage = '';
    this.job = null;

    this.matchingService
      .getJobSeekerJobDetail(jobVacancyId)
      .subscribe({

        next: (data) => {
          this.job = data;
          this.isLoading = false;
        },

        error: () => {
          this.errorMessage = 'Unable to load matching job.';
          this.isLoading = false;
        }

      });
  }

  openSkillInProfile(skill: string): void {

    this.router.navigate(
      ['/seeker/profile'],
      {
        queryParams: {
          skill: skill
        }
      }
    );
  }

}