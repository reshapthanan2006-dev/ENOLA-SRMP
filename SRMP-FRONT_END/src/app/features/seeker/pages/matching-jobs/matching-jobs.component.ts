import { Component } from '@angular/core';
import { NavbarComponent } from '../../../../shared/components/navbar/navbar.component';
import { MatchingJob } from '../../../matching/models/matching-job.model';
import { MatchingService } from '../../../matching/services/matching.service';
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
    HighlightDirective,
    NavbarComponent
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