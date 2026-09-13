import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';

import { Job } from '../../models/job.model';

@Component({
  selector: 'app-job-card',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './job-card.component.html',
  styleUrl: './job-card.component.css'
})
export class JobCardComponent {

  @Input({ required: true })
  job!: Job;

}