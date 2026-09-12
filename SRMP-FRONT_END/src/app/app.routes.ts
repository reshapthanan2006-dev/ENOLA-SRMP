import { Routes } from '@angular/router';

import { CompanyProfileComponent } from './features/employer/pages/company-profile/company-profile.component';
import { VacanciesComponent } from './features/employer/pages/vacancies/vacancies.component';
import { VacancyFormComponent } from './features/employer/pages/vacancy-form/vacancy-form.component';
import { DashboardComponent } from './features/employer/pages/dashboard/dashboard.component';

export const routes: Routes = [
  {
    path: 'employer/company-profile',
    component: CompanyProfileComponent
  },
  {
    path: 'employer/vacancies',
    component: VacanciesComponent
  },
  {
    path: 'employer/vacancies/new',
    component: VacancyFormComponent
  },
  {
    path: 'employer/vacancies/edit/:id',
    component: VacancyFormComponent
  },
  {
    path: 'employer/dashboard',
    component: DashboardComponent
  },
  {
    path: 'jobs',
    loadChildren: () =>
      import('./features/jobs/jobs.routes')
        .then(routes => routes.jobsRoutes)
  }
];