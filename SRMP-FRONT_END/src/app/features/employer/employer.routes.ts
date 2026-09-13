import { Routes } from '@angular/router';

import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { CompanyProfileComponent } from './pages/company-profile/company-profile.component';
import { VacanciesComponent } from './pages/vacancies/vacancies.component';
import { VacancyFormComponent } from './pages/vacancy-form/vacancy-form.component';

export const employerRoutes: Routes = [
  {
    path: 'dashboard',
    component: DashboardComponent
  },
  {
    path: 'company-profile',
    component: CompanyProfileComponent
  },
  {
    path: 'vacancies',
    component: VacanciesComponent
  },
  {
    path: 'vacancies/new',
    component: VacancyFormComponent
  },
  {
    path: 'vacancies/edit/:id',
    component: VacancyFormComponent
  }
];