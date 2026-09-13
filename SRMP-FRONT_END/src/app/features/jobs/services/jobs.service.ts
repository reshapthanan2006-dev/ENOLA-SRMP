import { Injectable } from '@angular/core';

import { Job } from '../models/job.model';

@Injectable({
  providedIn: 'root'
})
export class JobsService {

  private readonly storageKey =
    'member3Vacancies';

  getAllJobs(): Job[] {

    const savedJobs =
      localStorage.getItem(this.storageKey);

    if (!savedJobs) {
      return [];
    }

    return JSON.parse(savedJobs);
  }

  getOpenJobs(): Job[] {

    return this.getAllJobs().filter(
      job => job.isOpen
    );
  }

  getJobById(
    jobVacancyId: number
  ): Job | null {

    return (
      this.getAllJobs().find(
        job =>
          job.jobVacancyId === jobVacancyId
      ) ?? null
    );
  }

  searchJobs(
    keyword: string,
    location: string,
    minExperience: number | null
  ): Job[] {

    const normalizedKeyword =
      keyword.trim().toLowerCase();

    const normalizedLocation =
      location.trim().toLowerCase();

    return this.getOpenJobs().filter(
      job => {

        const matchesKeyword =
          !normalizedKeyword ||
          job.title
            .toLowerCase()
            .includes(normalizedKeyword) ||
          job.description
            .toLowerCase()
            .includes(normalizedKeyword) ||
          job.requiredSkills
            .toLowerCase()
            .includes(normalizedKeyword);

        const matchesLocation =
          !normalizedLocation ||
          job.location
            .toLowerCase()
            .includes(normalizedLocation);

        const matchesExperience =
          minExperience === null ||
          job.requiredExperience >=
            minExperience;

        return (
          matchesKeyword &&
          matchesLocation &&
          matchesExperience
        );
      }
    );
  }
}