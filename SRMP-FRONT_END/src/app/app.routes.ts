import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/pages/login/login.component')
        .then(component => component.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./features/auth/pages/register/register.component')
        .then(component => component.RegisterComponent)
  },
  {
    path: 'seeker',
    loadChildren: () =>
      import('./features/seeker/seeker.routes')
        .then(routes => routes.seekerRoutes)
  },
  {
    path: 'employer',
    loadChildren: () =>
      import('./features/employer/employer.routes')
        .then(routes => routes.employerRoutes)
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'login'
  }
];