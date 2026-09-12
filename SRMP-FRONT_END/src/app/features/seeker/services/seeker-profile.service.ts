import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import {
  SeekerProfile,
  SeekerProfileRequest
} from '../models/seeker-profile.model';

@Injectable({
  providedIn: 'root'
})
export class SeekerProfileService {
  private readonly apiUrl = `${environment.apiUrl}/jobseeker/profile`;

  constructor(private http: HttpClient) { }

  getProfile(): Observable<SeekerProfile> {
    return this.http.get<SeekerProfile>(this.apiUrl);
  }

  createProfile(request: SeekerProfileRequest): Observable<SeekerProfile> {
    return this.http.post<SeekerProfile>(this.apiUrl, request);
  }

  updateProfile(request: SeekerProfileRequest): Observable<SeekerProfile> {
    return this.http.put<SeekerProfile>(this.apiUrl, request);
  }
}
