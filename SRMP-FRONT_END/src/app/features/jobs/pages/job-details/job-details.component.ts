import { Component, OnInit } from '@angular/core';
import {
  ActivatedRoute,
  Router
} from '@angular/router';

import { Job } from '../../models/job.model';

@Component({
  selector: 'app-job-details',
  standalone: true,
  imports: [],
  templateUrl: './job-details.component.html',
  styleUrl: './job-details.component.css'
})
export class JobDetailsComponent implements OnInit {

  job: Job | null = null;

  isLoading = true;

  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadJob();
  }

  loadJob(): void {

    const idText =
      this.route.snapshot.paramMap.get('id');

    if (!idText) {
      this.errorMessage =
        'Job not found.';
      this.isLoading = false;
      return;
    }

    const jobId = Number(idText);

    if (Number.isNaN(jobId)) {
      this.errorMessage =
        'Job not found.';
      this.isLoading = false;
      return;
    }

    const savedJobs =
      localStorage.getItem('member3Vacancies');

    if (!savedJobs) {
      this.errorMessage =
        'Job not found.';
      this.isLoading = false;
      return;
    }

    const jobs: Job[] =
      JSON.parse(savedJobs);

    const foundJob =
      jobs.find(
        job =>
          job.jobVacancyId === jobId
      );

    if (!foundJob) {
      this.errorMessage =
        'Job not found.';
      this.isLoading = false;
      return;
    }

    this.job = foundJob;
    this.isLoading = false;
  }

  backToJobs(): void {
    this.router.navigate([
      '/jobs'
    ]);
  }
}