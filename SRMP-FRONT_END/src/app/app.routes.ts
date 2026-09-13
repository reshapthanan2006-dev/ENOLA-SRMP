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
    path: 'seeker',

    canActivate: [
      authGuard,
      roleGuard
    ],

    data: {
      roles: ['JobSeeker']
    },

    loadChildren: () =>
      import(
        './features/seeker/seeker.routes'
      ).then(
        (m) => m.seekerRoutes
      )
  },

  // =========================
  // EMPLOYER
  // =========================

  {
    path: 'employer',

    canActivate: [
      authGuard,
      roleGuard
    ],

    data: {
      roles: ['Employer']
    },

    loadChildren: () =>
      import(
        './features/employer/employer.routes'
      ).then(
        (m) => m.employerRoutes
      )
  },

  // =========================
  // ADMIN DASHBOARD
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
  // ADMIN USERS
  // =========================

  {
    path: 'admin/users',

    canActivate: [
      authGuard,
      roleGuard
    ],

    data: {
      roles: ['Administrator']
    },

    loadComponent: () =>
      import(
        './features/admin/pages/users/users.component'
      ).then(
        (m) => m.UsersComponent
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

  // =========================
  // FALLBACK
  // =========================

  {
    path: '**',
    redirectTo: 'login'
  }

];