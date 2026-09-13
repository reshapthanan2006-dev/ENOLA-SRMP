import { Routes } from '@angular/router';

import { JobListComponent } from './pages/job-list/job-list.component';
import { JobDetailsComponent } from './pages/job-details/job-details.component';

export const jobsRoutes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: JobListComponent
  },
  {
    path: ':id',
    component: JobDetailsComponent
  }
];