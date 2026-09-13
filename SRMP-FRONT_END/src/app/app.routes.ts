import { Routes } from '@angular/router';

import { LoginComponent } from './features/auth/pages/login/login.component';
import { RegisterComponent } from './features/auth/pages/register/register.component';

import { DashboardComponent } from './features/admin/pages/dashboard/dashboard.component';
import { UsersComponent } from './features/admin/pages/users/users.component';

import { ApplicantsComponent } from './features/employer/pages/applicants/applicants.component';

import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },

  {
    path: 'admin/dashboard',
    component: DashboardComponent,
    canActivate: [
      authGuard,
      roleGuard
    ],
    data: {
      roles: ['Administrator']
    }
  },
  {
    path: 'admin/users',
    component: UsersComponent,
    canActivate: [
      authGuard,
      roleGuard
    ],
    data: {
      roles: ['Administrator']
    }
  },

  {
    path: 'seeker',
    canActivate: [
      authGuard,
      roleGuard
    ],
    data: {
      roles: ['JobSeeker']
    },
    loadChildren: () =>
      import('./features/seeker/seeker.routes')
        .then(routes => routes.seekerRoutes)
  },

  {
    path: 'employer/applicants',
    component: ApplicantsComponent,
    canActivate: [
      authGuard,
      roleGuard
    ],
    data: {
      roles: ['Employer']
    }
  },

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