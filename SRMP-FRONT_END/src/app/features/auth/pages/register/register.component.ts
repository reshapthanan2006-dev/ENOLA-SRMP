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

import {
  RegisterRequest,
  RegistrationRole
} from '../../../../core/models/register-request';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly destroyRef = inject(DestroyRef);

  readonly RegistrationRole = RegistrationRole;

  isSubmitting = false;
  errorMessage = '';

  registerForm = this.formBuilder.nonNullable.group({
    fullName: [
      '',
      [
        Validators.required,
        Validators.maxLength(100)
      ]
    ],

    email: [
      '',
      [
        Validators.required,
        Validators.email,
        Validators.maxLength(150)
      ]
    ],

    password: [
      '',
      [
        Validators.required,
        Validators.minLength(6)
      ]
    ],

    role: [
      RegistrationRole.JobSeeker,
      Validators.required
    ]
  });

  onSubmit(): void {

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    const formValue =
      this.registerForm.getRawValue();

    const request: RegisterRequest = {
      fullName: formValue.fullName,
      email: formValue.email,
      password: formValue.password,
      role: formValue.role
    };

    this.authService
      .register(request)
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
            'Unable to create your account. Please try again.';
        }
      });
  }
}