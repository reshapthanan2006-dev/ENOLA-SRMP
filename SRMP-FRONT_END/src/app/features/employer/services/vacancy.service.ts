import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import {
  Observable,
  map,
  switchMap
} from 'rxjs';

import { environment } from '../../../../environments/environment';

import {
  Vacancy,
  VacancyRequest
} from '../models/vacancy.model';

@Injectable({
  providedIn: 'root'
})
export class VacancyService {

  private readonly http =
    inject(HttpClient);

  private readonly apiUrl =
    `${environment.apiUrl}/JobVacancy`;

  getVacancies(): Observable<Vacancy[]> {

    return this.http.get<Vacancy[]>(
      `${this.apiUrl}/my`
    );
  }

  getVacancyById(
    vacancyId: number
  ): Observable<Vacancy> {

    return this.http.get<Vacancy>(
      `${this.apiUrl}/${vacancyId}`
    );
  }

  getOpenVacancies(): Observable<Vacancy[]> {

    return this.getVacancies().pipe(
      map(
        vacancies =>
          vacancies.filter(
            vacancy => vacancy.isOpen
          )
      )
    );
  }

  createVacancy(
    request: VacancyRequest
  ): Observable<Vacancy> {

    return this.http.post<Vacancy>(
      this.apiUrl,
      request
    );
  }

  updateVacancy(
    vacancyId: number,
    request: VacancyRequest
  ): Observable<Vacancy> {

    return this.getVacancyById(
      vacancyId
    ).pipe(

      switchMap(
        existingVacancy => {

          const payload: Vacancy = {
            ...existingVacancy,
            ...request
          };

          return this.http.put<Vacancy>(
            `${this.apiUrl}/${vacancyId}`,
            payload
          );
        }
      )

    );
  }

  closeVacancy(
    vacancyId: number
  ): Observable<{ message: string }> {

    return this.http.put<{ message: string }>(
      `${this.apiUrl}/${vacancyId}/close`,
      {}
    );
  }
}