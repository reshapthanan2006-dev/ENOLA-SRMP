import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  ActivatedRoute,
  Router
} from '@angular/router';

import { Job } from '../../models/job.model';
import { JobCardComponent } from '../../components/job-card/job-card.component';
import { JobsService } from '../../services/jobs.service';

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

  allJobs: Job[] = [];
  jobs: Job[] = [];

  keyword = '';
  location = '';
  minExperience: number | null = null;

  constructor(
    private jobsService: JobsService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {

    this.loadJobs();

    const queryParams =
      this.route.snapshot.queryParamMap;

    this.keyword =
      queryParams.get('keyword') ?? '';

    this.location =
      queryParams.get('location') ?? '';

    const experienceText =
      queryParams.get('minExperience');

    if (experienceText !== null) {

      const experience =
        Number(experienceText);

      this.minExperience =
        Number.isNaN(experience)
          ? null
          : experience;
    }

    if (
      this.keyword ||
      this.location ||
      this.minExperience !== null
    ) {
      this.filterJobs();
    }
  }

  loadJobs(): void {

    this.allJobs =
      this.jobsService.getOpenJobs();

    this.jobs = [
      ...this.allJobs
    ];
  }

  searchJobs(): void {

    this.router.navigate(
      ['/jobs'],
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

    this.filterJobs();
  }

  clearFilters(): void {

    this.keyword = '';
    this.location = '';
    this.minExperience = null;

    this.jobs = [
      ...this.allJobs
    ];

    this.router.navigate(
      ['/jobs']
    );
  }

  private filterJobs(): void {

    this.jobs =
      this.jobsService.searchJobs(
        this.keyword,
        this.location,
        this.minExperience
      );
  }

}