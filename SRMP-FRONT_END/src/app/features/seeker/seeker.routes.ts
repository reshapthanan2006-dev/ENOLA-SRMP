import { Routes } from '@angular/router';

export const seekerRoutes: Routes = [
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./pages/dashboard/dashboard.component')
        .then(component => component.DashboardComponent)
  },
  {
    path: 'profile',
    loadComponent: () =>
      import('./pages/profile/profile.component')
        .then(component => component.ProfileComponent)
  },
  {
    path: 'cv',
    loadComponent: () =>
      import('./pages/cv/cv.component')
        .then(component => component.CvComponent)
  },
  {
    path: 'matching-jobs',
    loadComponent: () =>
      import('./pages/matching-jobs/matching-jobs.component')
        .then(component => component.MatchingJobsComponent)
  },
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  }
];