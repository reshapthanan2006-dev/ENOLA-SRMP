import { Component, OnInit } from '@angular/core';

import { Job } from '../../models/job.model';
import { JobCardComponent } from '../../components/job-card/job-card.component';

@Component({
  selector: 'app-job-list',
  standalone: true,
  imports: [
    JobCardComponent
  ],
  templateUrl: './job-list.component.html',
  styleUrl: './job-list.component.css'
})
export class JobListComponent implements OnInit {

  jobs: Job[] = [];

  ngOnInit(): void {
    this.loadJobs();
  }

  loadJobs(): void {

    const savedJobs =
      localStorage.getItem('member3Vacancies');

    if (!savedJobs) {
      this.jobs = [];
      return;
    }

    const allJobs: Job[] =
      JSON.parse(savedJobs);

    this.jobs =
      allJobs.filter(
        job => job.isOpen
      );
  }
}