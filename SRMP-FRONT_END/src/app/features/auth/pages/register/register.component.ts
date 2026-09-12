import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

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

    this.authService.register(request).subscribe({
      next: () => {
        this.isSubmitting = false;

        this.router.navigate(['/']);
      },

      error: error => {
        this.isSubmitting = false;

        this.errorMessage =
          error?.error?.message ??
          'Unable to create your account. Please try again.';
      }
    });
  }
}