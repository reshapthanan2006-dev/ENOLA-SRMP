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

import {
  Vacancy,
  VacancyRequest
} from '../../models/vacancy.model';

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
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {

    const idText =
      this.route.snapshot.paramMap.get('id');

    if (idText) {

      const vacancyId = Number(idText);

      if (!Number.isNaN(vacancyId)) {
        this.isEditMode = true;
        this.editingVacancyId = vacancyId;
        this.loadVacancyForEdit(vacancyId);
      }
    }
  }

  loadVacancyForEdit(vacancyId: number): void {

    const savedVacanciesText =
      localStorage.getItem('member3Vacancies');

    if (!savedVacanciesText) {
      this.router.navigate([
        '/employer/vacancies'
      ]);
      return;
    }

    const vacancies: Vacancy[] =
      JSON.parse(savedVacanciesText);

    const vacancy =
      vacancies.find(
        item => item.jobVacancyId === vacancyId
      );

    if (!vacancy) {
      this.router.navigate([
        '/employer/vacancies'
      ]);
      return;
    }

    this.vacancyForm.setValue({
      title: vacancy.title,
      description: vacancy.description,
      requiredSkills: vacancy.requiredSkills,
      requiredExperience: vacancy.requiredExperience,
      requiredEducation: vacancy.requiredEducation,
      location: vacancy.location
    });
  }

  saveVacancy(): void {

    if (this.vacancyForm.invalid) {
      this.vacancyForm.markAllAsTouched();
      return;
    }

    const vacancyRequest: VacancyRequest =
      this.vacancyForm.getRawValue();

    const savedVacanciesText =
      localStorage.getItem('member3Vacancies');

    let vacancies: Vacancy[] = [];

    if (savedVacanciesText) {
      vacancies = JSON.parse(savedVacanciesText);
    }

    if (
      this.isEditMode &&
      this.editingVacancyId !== null
    ) {

      const vacancyIndex =
        vacancies.findIndex(
          vacancy =>
            vacancy.jobVacancyId ===
            this.editingVacancyId
        );

      if (vacancyIndex === -1) {
        return;
      }

      const existingVacancy =
        vacancies[vacancyIndex];

      const updatedVacancy: Vacancy = {
        ...existingVacancy,
        title: vacancyRequest.title,
        description: vacancyRequest.description,
        requiredSkills: vacancyRequest.requiredSkills,
        requiredExperience:
          vacancyRequest.requiredExperience,
        requiredEducation:
          vacancyRequest.requiredEducation,
        location: vacancyRequest.location
      };

      vacancies[vacancyIndex] =
        updatedVacancy;

    } else {

      const nextId =
        vacancies.length > 0
          ? Math.max(
              ...vacancies.map(
                vacancy =>
                  vacancy.jobVacancyId
              )
            ) + 1
          : 1;

      const newVacancy: Vacancy = {
        jobVacancyId: nextId,
        title: vacancyRequest.title,
        description: vacancyRequest.description,
        requiredSkills:
          vacancyRequest.requiredSkills,
        requiredExperience:
          vacancyRequest.requiredExperience,
        requiredEducation:
          vacancyRequest.requiredEducation,
        location: vacancyRequest.location,
        employerId: 1,
        isOpen: true,
        createdAt: new Date().toISOString()
      };

      vacancies.push(newVacancy);
    }

    localStorage.setItem(
      'member3Vacancies',
      JSON.stringify(vacancies)
    );

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