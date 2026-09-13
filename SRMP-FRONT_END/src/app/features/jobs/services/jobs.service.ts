import { Injectable, inject } from '@angular/core';

import {
  HttpClient,
  HttpParams
} from '@angular/common/http';

import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';

import { Job } from '../models/job.model';

@Injectable({
  providedIn: 'root'
})
export class JobsService {

  private readonly http =
    inject(HttpClient);

  private readonly apiUrl =
    `${environment.apiUrl}/JobVacancy`;

  getOpenJobs(): Observable<Job[]> {

    return this.searchJobs(
      '',
      '',
      null
    );
  }

  searchJobs(
    keyword: string,
    location: string,
    minExperience: number | null
  ): Observable<Job[]> {

    let params =
      new HttpParams();

    const trimmedKeyword =
      keyword.trim();

    const trimmedLocation =
      location.trim();

    if (trimmedKeyword) {

      params =
        params.set(
          'keyword',
          trimmedKeyword
        );
    }

    if (trimmedLocation) {

      params =
        params.set(
          'location',
          trimmedLocation
        );
    }

    if (minExperience !== null) {

      params =
        params.set(
          'minExperience',
          minExperience.toString()
        );
    }

    return this.http.get<Job[]>(
      `${this.apiUrl}/search`,
      {
        params
      }
    );
  }

  getJobById(
    jobVacancyId: number
  ): Observable<Job> {

    return this.http.get<Job>(
      `${this.apiUrl}/${jobVacancyId}`
    );
  }
}