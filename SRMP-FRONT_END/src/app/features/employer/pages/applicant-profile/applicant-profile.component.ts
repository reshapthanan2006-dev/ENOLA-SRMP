import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ApplicantProfile } from '../../models/applicant-profile.model';
import { ApplicantProfileService } from '../../services/applicant-profile.service';

@Component({
  selector: 'app-applicant-profile',
  standalone: true,
  imports: [],
  templateUrl: './applicant-profile.component.html',
  styleUrl: './applicant-profile.component.css'
})
export class ApplicantProfileComponent implements OnInit {

  profile: ApplicantProfile | null = null;

  isLoading = false;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private applicantProfileService: ApplicantProfileService
  ) { }

  ngOnInit(): void {

    const jobVacancyId =
      Number(this.route.snapshot.paramMap.get('jobVacancyId'));

    const jobSeekerId =
      Number(this.route.snapshot.paramMap.get('jobSeekerId'));

    if (!jobVacancyId || !jobSeekerId) {
      this.errorMessage = 'Applicant information is missing.';
      return;
    }

    this.loadApplicantProfile(
      jobVacancyId,
      jobSeekerId
    );
  }

  loadApplicantProfile(
    jobVacancyId: number,
    jobSeekerId: number
  ): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.applicantProfileService
      .getApplicantProfile(
        jobVacancyId,
        jobSeekerId
      )
      .subscribe({
        next: (data) => {
          this.profile = data;
          this.isLoading = false;
        },

        error: () => {
          this.errorMessage =
            'Unable to load applicant profile.';

          this.isLoading = false;
        }
      });
  }
}