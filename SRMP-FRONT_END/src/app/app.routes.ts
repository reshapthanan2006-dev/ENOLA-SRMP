import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

import { LoginComponent } from './features/auth/pages/login/login.component';
import { RegisterComponent } from './features/auth/pages/register/register.component';

export const routes: Routes = [

  // =====================================================
  // AUTH
  // =====================================================

  {
    path: 'login',
    component: LoginComponent
  },

  {
    path: 'register',
    component: RegisterComponent
  },


  // =====================================================
  // JOB SEEKER
  // =====================================================

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


  // =====================================================
  // EMPLOYER
  // =====================================================

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


  // =====================================================
  // PUBLIC JOBS
  // =====================================================

  {
    path: 'jobs',

    loadChildren: () =>
      import(
        './features/jobs/jobs.routes'
      ).then(
        (m) => m.jobsRoutes
      )
  },


  // =====================================================
  // ADMIN
  // =====================================================

  {
    path: 'admin',

    canActivate: [
      authGuard,
      roleGuard
    ],

    data: {
      roles: ['Administrator']
    },

    loadChildren: () =>
      import(
        './features/admin/admin.routes'
      ).then(
        (m) => m.adminRoutes
      )
  },


  // =====================================================
  // DEFAULT
  // =====================================================

  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },


  // =====================================================
  // FALLBACK
  // =====================================================

  {
    path: '**',
    redirectTo: 'login'
  }

];