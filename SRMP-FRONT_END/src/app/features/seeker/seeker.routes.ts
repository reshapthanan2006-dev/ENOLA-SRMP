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

      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'dashboard'
      },

      {
        path: 'dashboard',
        loadComponent: () =>
          import(
            './pages/dashboard/dashboard.component'
          ).then(
            (m) => m.DashboardComponent
          )
      },

      {
        path: 'profile',
        loadComponent: () =>
          import(
            './pages/profile/profile.component'
          ).then(
            (m) => m.ProfileComponent
          )
      },

      {
        path: 'cv',
        loadComponent: () =>
          import(
            './pages/cv/cv.component'
          ).then(
            (m) => m.CvComponent
          )
      },

      {
        path: 'applications',
        loadComponent: () =>
          import(
            './pages/applications/applications.component'
          ).then(
            (m) => m.ApplicationsComponent
          )
      },

      {
        path: 'matching-jobs/:jobVacancyId',
        loadComponent: () =>
          import(
            './pages/matching-jobs/matching-jobs.component'
          ).then(
            (m) => m.MatchingJobsComponent
          )
      },

      {
        path: 'contact-requests',
        loadComponent: () =>
          import(
            './pages/contact-requests/contact-requests.component'
          ).then(
            (m) => m.ContactRequestsComponent
          )
      },

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