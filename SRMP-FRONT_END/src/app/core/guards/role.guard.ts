import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router
} from '@angular/router';

import {
  AuthService
} from '../auth/auth.service';

import {
  UserRole
} from '../models/auth-response';

export const roleGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot
) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const user =
    authService.getCurrentUser();

  if (!user) {
    return router.createUrlTree(['/login']);
  }

  const allowedRoles =
    route.data['roles'] as UserRole[] | undefined;

  if (
    !allowedRoles ||
    allowedRoles.length === 0
  ) {
    return true;
  }

  if (allowedRoles.includes(user.role)) {
    return true;
  }

  return router.createUrlTree(['/']);
};