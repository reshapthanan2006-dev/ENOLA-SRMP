import { Routes } from '@angular/router';

export const notificationRoutes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import(
        './pages/notification-list/notification-list.component'
      ).then(
        (m) => m.NotificationListComponent
      )
  }
];