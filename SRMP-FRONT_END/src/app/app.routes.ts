import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

import { LoginComponent } from './features/auth/pages/login/login.component';
import { RegisterComponent } from './features/auth/pages/register/register.component';

export const routes: Routes = [

  // =========================
  // AUTH
  // =========================

  {
    path: 'login',
    component: LoginComponent
  },

  {
    path: 'register',
    component: RegisterComponent
  },

  // =========================
  // JOB SEEKER
  // =========================

  {
    path: 'seeker/applications',
    canActivate: [
      authGuard,
      roleGuard
    ],
    data: {
      roles: ['JobSeeker']
    },
    loadComponent: () =>
      import(
        './features/seeker/pages/applications/applications.component'
      ).then(
        (m) => m.ApplicationsComponent
      )
  },

  {
    path: 'seeker/matching-jobs/:jobVacancyId',
    canActivate: [
      authGuard,
      roleGuard
    ],
    data: {
      roles: ['JobSeeker']
    },
    loadComponent: () =>
      import(
        './features/seeker/pages/matching-jobs/matching-jobs.component'
      ).then(
        (m) => m.MatchingJobsComponent
      )
  },

  {
    path: 'seeker/contact-requests',
    canActivate: [
      authGuard,
      roleGuard
    ],
    data: {
      roles: ['JobSeeker']
    },
    loadComponent: () =>
      import(
        './features/seeker/pages/contact-requests/contact-requests.component'
      ).then(
        (m) => m.ContactRequestsComponent
      )
  },

  {
    path: 'seeker/notifications',
    canActivate: [
      authGuard,
      roleGuard
    ],
    data: {
      roles: ['JobSeeker']
    },
    loadChildren: () =>
      import(
        './features/notifications/notification.routes'
      ).then(
        (m) => m.notificationRoutes
      )
  },

  // =========================
  // EMPLOYER
  // =========================

  {
    path: 'employer/vacancies/:jobVacancyId/applications',
    canActivate: [
      authGuard,
      roleGuard
    ],
    data: {
      roles: ['Employer']
    },
    loadComponent: () =>
      import(
        './features/employer/pages/applications/applications.component'
      ).then(
        (m) => m.ApplicationsComponent
      )
  },

  {
    path: 'employer/vacancies/:jobVacancyId/applicants',
    canActivate: [
      authGuard,
      roleGuard
    ],
    data: {
      roles: ['Employer']
    },
    loadComponent: () =>
      import(
        './features/employer/pages/applicants/applicants.component'
      ).then(
        (m) => m.ApplicantsComponent
      )
  },

  {
    path: 'employer/vacancies/:jobVacancyId/applicants/:jobSeekerId',
    canActivate: [
      authGuard,
      roleGuard
    ],
    data: {
      roles: ['Employer']
    },
    loadComponent: () =>
      import(
        './features/employer/pages/applicant-profile/applicant-profile.component'
      ).then(
        (m) => m.ApplicantProfileComponent
      )
  },

  {
    path: 'employer/contact-requests',
    canActivate: [
      authGuard,
      roleGuard
    ],
    data: {
      roles: ['Employer']
    },
    loadComponent: () =>
      import(
        './features/employer/pages/contact-requests/contact-requests.component'
      ).then(
        (m) => m.ContactRequestsComponent
      )
  },

  // =========================
  // ADMIN
  // =========================

  {
    path: 'admin/dashboard',
    canActivate: [
      authGuard,
      roleGuard
    ],
    data: {
      roles: ['Administrator']
    },
    loadComponent: () =>
      import(
        './features/admin/pages/dashboard/dashboard.component'
      ).then(
        (m) => m.DashboardComponent
      )
  },

  // =========================
  // DEFAULT
  // =========================

  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  {
    path: '**',
    redirectTo: 'login'
  }

];