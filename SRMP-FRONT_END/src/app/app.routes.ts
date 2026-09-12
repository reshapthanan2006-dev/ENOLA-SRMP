import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'seeker',
    loadChildren: () =>
      import('./features/seeker/seeker.routes')
        .then(routes => routes.seekerRoutes)
  }
];