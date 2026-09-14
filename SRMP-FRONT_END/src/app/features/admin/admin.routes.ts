import { Routes } from '@angular/router';

export const adminRoutes: Routes = [
  {
    path: '',

    loadComponent: () =>
      import(
        '../../layout/admin-layout/admin-layout.component'
      ).then(
        (m) => m.AdminLayoutComponent
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
        path: 'users',
        loadComponent: () =>
          import(
            './pages/users/users.component'
          ).then(
            (m) => m.UsersComponent
          )
      }
    ]
  }
];