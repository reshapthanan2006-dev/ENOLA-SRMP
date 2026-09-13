import {
  HttpClient
} from '@angular/common/http';

import {
  Injectable,
  inject
} from '@angular/core';

import {
  Observable
} from 'rxjs';

import {
  environment
} from '../../../../environments/environment';

import {
  ApplicantProfile
} from '../models/applicant-profile.model';

import {
  ApplicantCv
} from '../models/applicant-cv.model';

@Injectable({
  providedIn: 'root'
})
export class ApplicantProfileService {

  private readonly http =
    inject(HttpClient);

  private readonly apiUrl =
    `${environment.apiUrl}/employer/applicants`;

  getApplicantProfile(
    jobVacancyId: number,
    jobSeekerId: number
  ): Observable<ApplicantProfile> {

    return this.http.get<ApplicantProfile>(
      `${this.apiUrl}/vacancy/${jobVacancyId}/jobseeker/${jobSeekerId}/profile`
    );
  }

  getApplicantCv(
    jobVacancyId: number,
    jobSeekerId: number
  ): Observable<ApplicantCv> {

    return this.http.get<ApplicantCv>(
      `${this.apiUrl}/vacancy/${jobVacancyId}/jobseeker/${jobSeekerId}/cv`
    );
  }

  downloadApplicantCv(
    jobVacancyId: number,
    jobSeekerId: number
  ): Observable<Blob> {

    return this.http.get(
      `${this.apiUrl}/vacancy/${jobVacancyId}/jobseeker/${jobSeekerId}/cv/download`,
      {
        responseType: 'blob'
      }
    );
  }
}