import { Component, OnInit } from '@angular/core';
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

import { VacancyRequest } from '../../models/vacancy.model';
import { VacancyService } from '../../services/vacancy.service';

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

  isEditMode = false;

  editingVacancyId: number | null = null;

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

    requiredExperience: new FormControl(0, {
      nonNullable: true,
      validators: [
        Validators.min(0)
      ]
    }),

    requiredEducation: new FormControl('', {
      nonNullable: true
    }),

    location: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.maxLength(100)
      ]
    })

  });

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private vacancyService: VacancyService
  ) { }

  ngOnInit(): void {

    const idText =
      this.route.snapshot.paramMap.get('id');

    if (idText) {

      const vacancyId =
        Number(idText);

      if (!Number.isNaN(vacancyId)) {

        this.isEditMode = true;

        this.editingVacancyId =
          vacancyId;

        this.loadVacancyForEdit(
          vacancyId
        );
      }
    }
  }

  loadVacancyForEdit(
    vacancyId: number
  ): void {

    const vacancy =
      this.vacancyService.getVacancyById(
        vacancyId
      );

    if (!vacancy) {

      this.router.navigate([
        '/employer/vacancies'
      ]);

      return;
    }

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
  }

  saveVacancy(): void {

    if (this.vacancyForm.invalid) {

      this.vacancyForm.markAllAsTouched();

      return;
    }

    const vacancyRequest: VacancyRequest =
      this.vacancyForm.getRawValue();

    if (
      this.isEditMode &&
      this.editingVacancyId !== null
    ) {

      const updatedVacancy =
        this.vacancyService.updateVacancy(
          this.editingVacancyId,
          vacancyRequest
        );

      if (!updatedVacancy) {
        return;
      }

    } else {

      this.vacancyService.createVacancy(
        vacancyRequest
      );
    }

    this.router.navigate([
      '/employer/vacancies'
    ]);
  }

  cancelForm(): void {

    this.router.navigate([
      '/employer/vacancies'
    ]);
  }

}