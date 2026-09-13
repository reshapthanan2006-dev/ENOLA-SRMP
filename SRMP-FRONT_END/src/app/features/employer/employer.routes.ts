import { Routes } from '@angular/router';

export const employerRoutes: Routes = [
  {
    path: 'vacancies/:jobVacancyId/applicants',
    loadComponent: () =>
      import('./pages/applicants/applicants.component')
        .then(component => component.ApplicantsComponent)
  },
  {
    path: 'vacancies/:jobVacancyId/applicants/:jobSeekerId',
    loadComponent: () =>
      import('./pages/applicant-profile/applicant-profile.component')
        .then(component => component.ApplicantProfileComponent)
  }
];