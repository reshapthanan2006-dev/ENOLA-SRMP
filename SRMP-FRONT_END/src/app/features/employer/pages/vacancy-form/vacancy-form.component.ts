import {
  Component,
  DestroyRef,
  OnInit,
  inject
} from '@angular/core';

import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  ActivatedRoute,
  Router
} from '@angular/router';

import {
  HttpErrorResponse
} from '@angular/common/http';

import {
  takeUntilDestroyed
} from '@angular/core/rxjs-interop';

import {
  VacancyRequest
} from '../../models/vacancy.model';

import {
  VacancyService
} from '../../services/vacancy.service';

@Component({
  selector: 'app-vacancy-form',
  standalone: true,
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './vacancy-form.component.html',
  styleUrl: './vacancy-form.component.css'
})
export class VacancyFormComponent implements OnInit {

  private readonly router =
    inject(Router);

  private readonly route =
    inject(ActivatedRoute);

  private readonly vacancyService =
    inject(VacancyService);

  private readonly destroyRef =
    inject(DestroyRef);

  isEditMode = false;

  editingVacancyId: number | null =
    null;

  isLoading = false;

  isSaving = false;

  errorMessage = '';

  vacancyForm = new FormGroup({

    title: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.maxLength(150)
      ]
    }),

    description: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.maxLength(1000)
      ]
    }),

    requiredSkills: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required
      ]
    }),

    requiredExperience:
      new FormControl(0, {
        nonNullable: true,
        validators: [
          Validators.min(0)
        ]
      }),

    requiredEducation:
      new FormControl('', {
        nonNullable: true
      }),

    location:
      new FormControl('', {
        nonNullable: true,
        validators: [
          Validators.maxLength(100)
        ]
      })

  });

  ngOnInit(): void {

    const idText =
      this.route.snapshot
        .paramMap
        .get('id');

    if (!idText) {
      return;
    }

    const vacancyId =
      Number(idText);

    if (
      Number.isNaN(vacancyId)
    ) {
      return;
    }

    this.isEditMode = true;

    this.editingVacancyId =
      vacancyId;

    this.loadVacancyForEdit(
      vacancyId
    );
  }

  loadVacancyForEdit(
    vacancyId: number
  ): void {

    this.isLoading = true;

    this.errorMessage = '';

    this.vacancyService
      .getVacancyById(vacancyId)
      .pipe(
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe({

        next: vacancy => {

          this.isLoading = false;

          this.vacancyForm.setValue({

            title:
              vacancy.title,

            description:
              vacancy.description,

            requiredSkills:
              vacancy.requiredSkills,

            requiredExperience:
              vacancy.requiredExperience,

            requiredEducation:
              vacancy.requiredEducation,

            location:
              vacancy.location

          });
        },

        error: (
          error: HttpErrorResponse
        ) => {

          this.isLoading = false;

          this.errorMessage =
            this.getErrorMessage(error);

          window.alert(
            this.errorMessage
          );

          this.router.navigate([
            '/employer/vacancies'
          ]);
        }

      });
  }

  saveVacancy(): void {

    if (
      this.vacancyForm.invalid ||
      this.isSaving
    ) {

      this.vacancyForm
        .markAllAsTouched();

      return;
    }

    this.isSaving = true;

    this.errorMessage = '';

    const vacancyRequest:
      VacancyRequest =
        this.vacancyForm
          .getRawValue();

    if (
      this.isEditMode &&
      this.editingVacancyId !== null
    ) {

      this.vacancyService
        .updateVacancy(
          this.editingVacancyId,
          vacancyRequest
        )
        .pipe(
          takeUntilDestroyed(
            this.destroyRef
          )
        )
        .subscribe({

          next: () => {

            this.isSaving = false;

            this.router.navigate([
              '/employer/vacancies'
            ]);
          },

          error: (
            error: HttpErrorResponse
          ) => {

            this.isSaving = false;

            this.handleSaveError(
              error
            );
          }

        });

      return;
    }

    this.vacancyService
      .createVacancy(
        vacancyRequest
      )
      .pipe(
        takeUntilDestroyed(
          this.destroyRef
        )
      )
      .subscribe({

        next: () => {

          this.isSaving = false;

          this.router.navigate([
            '/employer/vacancies'
          ]);
        },

        error: (
          error: HttpErrorResponse
        ) => {

          this.isSaving = false;

          this.handleSaveError(
            error
          );
        }

      });
  }

  cancelForm(): void {

    this.router.navigate([
      '/employer/vacancies'
    ]);
  }

  private handleSaveError(
    error: HttpErrorResponse
  ): void {

    this.errorMessage =
      this.getErrorMessage(error);

    window.alert(
      this.errorMessage
    );
  }

  private getErrorMessage(
    error: HttpErrorResponse
  ): string {

    if (error.status === 401) {
      return 'Please sign in as an employer.';
    }

    if (error.status === 403) {
      return 'You are not allowed to manage this vacancy.';
    }

    if (error.status === 404) {
      return 'Vacancy not found.';
    }

    if (
      typeof error.error === 'string' &&
      error.error.trim()
    ) {
      return error.error;
    }

    if (
      error.error?.message
    ) {
      return error.error.message;
    }

    return this.isEditMode
      ? 'Unable to update vacancy. Please try again.'
      : 'Unable to create vacancy. Please try again.';
  }
}