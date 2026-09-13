import { Routes } from '@angular/router';

export const seekerRoutes: Routes = [

  {
    path: '',

    loadComponent: () =>
      import(
        '../../layout/seeker-layout/seeker-layout.component'
      ).then(
        (m) => m.SeekerLayoutComponent
      ),

    children: [

      // =========================
      // DEFAULT
      // =========================

      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'dashboard'
      },

      // =========================
      // DASHBOARD
      // =========================

      {
        path: 'dashboard',

        loadComponent: () =>
          import(
            './pages/dashboard/dashboard.component'
          ).then(
            (m) => m.DashboardComponent
          )
      },

      // =========================
      // PROFILE
      // =========================

      {
        path: 'profile',

        loadComponent: () =>
          import(
            './pages/profile/profile.component'
          ).then(
            (m) => m.ProfileComponent
          )
      },

      // =========================
      // CV
      // =========================

      {
        path: 'cv',

        loadComponent: () =>
          import(
            './pages/cv/cv.component'
          ).then(
            (m) => m.CvComponent
          )
      },

      // =========================
      // SEARCH JOBS
      // =========================

      {
        path: 'jobs',

        loadComponent: () =>
          import(
            '../jobs/pages/job-list/job-list.component'
          ).then(
            (m) => m.JobListComponent
          )
      },

      // =========================
      // MATCHING JOB DETAILS
      // =========================

      {
        path: 'matching-jobs/:jobVacancyId',

        loadComponent: () =>
          import(
            './pages/matching-jobs/matching-jobs.component'
          ).then(
            (m) => m.MatchingJobsComponent
          )
      },

      // =========================
      // APPLICATIONS
      // =========================

      {
        path: 'applications',

        loadComponent: () =>
          import(
            './pages/applications/applications.component'
          ).then(
            (m) => m.ApplicationsComponent
          )
      },

      // =========================
      // CONTACT REQUESTS
      // =========================

      {
        path: 'contact-requests',

        loadComponent: () =>
          import(
            './pages/contact-requests/contact-requests.component'
          ).then(
            (m) => m.ContactRequestsComponent
          )
      },

      // =========================
      // NOTIFICATIONS
      // =========================

      {
        path: 'notifications',

        loadChildren: () =>
          import(
            '../notifications/notification.routes'
          ).then(
            (m) => m.notificationRoutes
          )
      }

    ]
  }

];