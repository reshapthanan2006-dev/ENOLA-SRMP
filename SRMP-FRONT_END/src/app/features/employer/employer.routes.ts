import { Routes } from '@angular/router';

export const employerRoutes: Routes = [

  {
    path: '',

    loadComponent: () =>
      import(
        '../../layout/employer-layout/employer-layout.component'
      ).then(
        (m) => m.EmployerLayoutComponent
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
        path: 'company-profile',
        loadComponent: () =>
          import(
            './pages/company-profile/company-profile.component'
          ).then(
            (m) => m.CompanyProfileComponent
          )
      },

      // =========================
      // MEMBER 3 - VACANCIES
      // =========================

      {
        path: 'vacancies',
        loadComponent: () =>
          import(
            './pages/vacancies/vacancies.component'
          ).then(
            (m) => m.VacanciesComponent
          )
      },

      {
        path: 'vacancies/new',
        loadComponent: () =>
          import(
            './pages/vacancy-form/vacancy-form.component'
          ).then(
            (m) => m.VacancyFormComponent
          )
      },

      {
        path: 'vacancies/edit/:id',
        loadComponent: () =>
          import(
            './pages/vacancy-form/vacancy-form.component'
          ).then(
            (m) => m.VacancyFormComponent
          )
      },

      // =========================
      // EXISTING TEAM ROUTES
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

      {
        path: 'vacancies/:jobVacancyId/applications',
        loadComponent: () =>
          import(
            './pages/applications/applications.component'
          ).then(
            (m) => m.ApplicationsComponent
          )
      },

      {
        path: 'vacancies/:jobVacancyId/applicants',
        loadComponent: () =>
          import(
            './pages/applicants/applicants.component'
          ).then(
            (m) => m.ApplicantsComponent
          )
      },

      {
        path: 'vacancies/:jobVacancyId/applicants/:jobSeekerId',
        loadComponent: () =>
          import(
            './pages/applicant-profile/applicant-profile.component'
          ).then(
            (m) => m.ApplicantProfileComponent
          )
      },

      {
        path: 'applicants',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      }

    ]
  }

];