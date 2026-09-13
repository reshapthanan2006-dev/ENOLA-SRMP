import { Injectable } from '@angular/core';

import {
  Vacancy,
  VacancyRequest
} from '../models/vacancy.model';

@Injectable({
  providedIn: 'root'
})
export class VacancyService {

  private readonly storageKey =
    'member3Vacancies';

  getVacancies(): Vacancy[] {

    const savedVacancies =
      localStorage.getItem(this.storageKey);

    if (!savedVacancies) {
      return [];
    }

    return JSON.parse(savedVacancies);
  }

  getVacancyById(
    vacancyId: number
  ): Vacancy | null {

    const vacancies =
      this.getVacancies();

    return (
      vacancies.find(
        vacancy =>
          vacancy.jobVacancyId === vacancyId
      ) ?? null
    );
  }

  getOpenVacancies(): Vacancy[] {

    return this.getVacancies().filter(
      vacancy => vacancy.isOpen
    );
  }

  createVacancy(
    request: VacancyRequest
  ): Vacancy {

    const vacancies =
      this.getVacancies();

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
      title: request.title,
      description: request.description,
      requiredSkills: request.requiredSkills,
      requiredExperience:
        request.requiredExperience,
      requiredEducation:
        request.requiredEducation,
      location: request.location,
      employerId: 1,
      isOpen: true,
      createdAt: new Date().toISOString()
    };

    vacancies.push(newVacancy);

    this.saveVacancies(vacancies);

    return newVacancy;
  }

  updateVacancy(
    vacancyId: number,
    request: VacancyRequest
  ): Vacancy | null {

    const vacancies =
      this.getVacancies();

    const vacancyIndex =
      vacancies.findIndex(
        vacancy =>
          vacancy.jobVacancyId === vacancyId
      );

    if (vacancyIndex === -1) {
      return null;
    }

    const existingVacancy =
      vacancies[vacancyIndex];

    const updatedVacancy: Vacancy = {
      ...existingVacancy,
      title: request.title,
      description: request.description,
      requiredSkills: request.requiredSkills,
      requiredExperience:
        request.requiredExperience,
      requiredEducation:
        request.requiredEducation,
      location: request.location
    };

    vacancies[vacancyIndex] =
      updatedVacancy;

    this.saveVacancies(vacancies);

    return updatedVacancy;
  }

  closeVacancy(
    vacancyId: number
  ): boolean {

    const vacancies =
      this.getVacancies();

    const vacancy =
      vacancies.find(
        item =>
          item.jobVacancyId === vacancyId
      );

    if (!vacancy) {
      return false;
    }

    vacancy.isOpen = false;

    this.saveVacancies(vacancies);

    return true;
  }

  private saveVacancies(
    vacancies: Vacancy[]
  ): void {

    localStorage.setItem(
      this.storageKey,
      JSON.stringify(vacancies)
    );
  }
}