import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'jobs'
  },
  {
    path: 'employer',
    loadChildren: () =>
      import('./features/employer/employer.routes')
        .then(routes => routes.employerRoutes)
  },
  {
    path: 'jobs',
    loadChildren: () =>
      import('./features/jobs/jobs.routes')
        .then(routes => routes.jobsRoutes)
  }
];