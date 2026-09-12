import { Routes } from '@angular/router';

export const routes: Routes = [

  // Temporary home route.
  // Final team integration-la /jobs or role home-ku change pannuvom.
  {
    path: '',
    redirectTo: 'seeker/applications',
    pathMatch: 'full'
  },

  // =========================
  // JOB SEEKER
  // =========================

  {
    path: 'seeker/applications',
    loadComponent: () =>
      import(
        './features/seeker/pages/applications/applications.component'
      ).then(
        (m) => m.ApplicationsComponent
      )
  },

  {
    path: 'seeker/matching-jobs/:jobVacancyId',
    loadComponent: () =>
      import(
        './features/seeker/pages/matching-jobs/matching-jobs.component'
      ).then(
        (m) => m.MatchingJobsComponent
      )
  },

  {
    path: 'seeker/contact-requests',
    loadComponent: () =>
      import(
        './features/seeker/pages/contact-requests/contact-requests.component'
      ).then(
        (m) => m.ContactRequestsComponent
      )
  },

  {
    path: 'seeker/notifications',
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
    loadComponent: () =>
      import(
        './features/employer/pages/applications/applications.component'
      ).then(
        (m) => m.ApplicationsComponent
      )
  },

  {
    path: 'employer/vacancies/:jobVacancyId/applicants',
    loadComponent: () =>
      import(
        './features/employer/pages/applicants/applicants.component'
      ).then(
        (m) => m.ApplicantsComponent
      )
  },

  {
    path: 'employer/vacancies/:jobVacancyId/applicants/:jobSeekerId',
    loadComponent: () =>
      import(
        './features/employer/pages/applicant-profile/applicant-profile.component'
      ).then(
        (m) => m.ApplicantProfileComponent
      )
  },

  {
    path: 'employer/contact-requests',
    loadComponent: () =>
      import(
        './features/employer/pages/contact-requests/contact-requests.component'
      ).then(
        (m) => m.ContactRequestsComponent
      )
  }

];