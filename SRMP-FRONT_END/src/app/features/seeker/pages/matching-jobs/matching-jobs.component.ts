import { Component } from '@angular/core';

import { MatchingJob } from '../../../matching/models/matching-job.model';
import { MatchingService } from '../../../matching/services/matching.service';
import { ScoreBadgeComponent } from '../../../../shared/components/score-badge/score-badge.component';
import { SkillListPipe } from '../../../../shared/pipes/skill-list.pipe';

@Component({
  selector: 'app-matching-jobs',
  standalone: true,
  imports: [
    ScoreBadgeComponent,
    SkillListPipe
  ],
  templateUrl: './matching-jobs.component.html',
  styleUrl: './matching-jobs.component.css'
})
export class MatchingJobsComponent {

  job: MatchingJob | null = null;

  isLoading = false;
  errorMessage = '';

  constructor(private matchingService: MatchingService) { }

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

}