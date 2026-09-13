import {
  Component,
  DestroyRef,
  inject
} from '@angular/core';

import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  Router,
  RouterLink
} from '@angular/router';

import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { AuthService } from '../../../../core/auth/auth.service';
import { LoginRequest } from '../../../../core/models/login-request';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly destroyRef = inject(DestroyRef);

  isSubmitting = false;
  errorMessage = '';

  loginForm = this.formBuilder.nonNullable.group({
    email: [
      '',
      [
        Validators.required,
        Validators.email
      ]
    ],

    password: [
      '',
      Validators.required
    ]
  });

  onSubmit(): void {

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    const formValue = this.loginForm.getRawValue();

    const request: LoginRequest = {
      email: formValue.email,
      password: formValue.password
    };

    this.authService
      .login(request)
      .pipe(
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe({
        next: (response) => {

          this.isSubmitting = false;

          switch (response.role) {

            case 'JobSeeker':
              this.router.navigate([
                '/seeker/dashboard'
              ]);
              break;

            case 'Employer':
              this.router.navigate([
                '/employer/dashboard'
              ]);
              break;

            case 'Administrator':
              this.router.navigate([
                '/admin/dashboard'
              ]);
              break;

            default:
              this.router.navigate([
                '/login'
              ]);
          }
        },

        error: (error) => {

          this.isSubmitting = false;

          this.errorMessage =
            error?.error?.message ??
            'Unable to login. Please check your email and password.';
        }
      });
  }
}