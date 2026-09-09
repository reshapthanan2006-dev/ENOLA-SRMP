import { Component } from '@angular/core';

import { MatchResult } from '../../../matching/models/match-result.model';
import { MatchingService } from '../../../matching/services/matching.service';
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
export class ApplicantsComponent {

  applicants: MatchResult[] = [];
  isLoading = false;
  errorMessage = '';

  constructor(private matchingService: MatchingService) { }

  loadApplicants(jobVacancyId: number, employerId: number): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.matchingService
      .getRankedApplicants(jobVacancyId, employerId)
      .subscribe({
        next: (data) => {
          this.applicants = data;
          this.isLoading = false;
        },

        error: () => {
          this.errorMessage = 'Unable to load applicants.';
          this.isLoading = false;
        }
      });
  }

}