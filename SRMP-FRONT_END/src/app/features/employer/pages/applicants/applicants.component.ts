import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NavbarComponent } from '../../../../shared/components/navbar/navbar.component';
import { MatchResult } from '../../../matching/models/match-result.model';
import { MatchingService } from '../../../matching/services/matching.service';
import { ScoreBadgeComponent } from '../../../../shared/components/score-badge/score-badge.component';
import { SkillListPipe } from '../../../../shared/pipes/skill-list.pipe';

@Component({
  selector: 'app-applicants',
  standalone: true,
  imports: [
    ScoreBadgeComponent,
    SkillListPipe,
    NavbarComponent
  ],
  templateUrl: './applicants.component.html',
  styleUrl: './applicants.component.css'
})
export class ApplicantsComponent implements OnInit {

  applicants: MatchResult[] = [];

  jobVacancyId = 0;

  isLoading = false;
  errorMessage = '';

  constructor(
    private matchingService: MatchingService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {

    this.jobVacancyId =
      Number(this.route.snapshot.paramMap.get('jobVacancyId'));

    if (!this.jobVacancyId) {
      this.errorMessage = 'Vacancy information is missing.';
      return;
    }

    this.loadApplicants(this.jobVacancyId);
  }

  loadApplicants(jobVacancyId: number): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.matchingService
      .getRankedApplicants(jobVacancyId)
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

  viewProfile(jobSeekerId: number): void {

    this.router.navigate([
      '/employer/vacancies',
      this.jobVacancyId,
      'applicants',
      jobSeekerId
    ]);
  }
}