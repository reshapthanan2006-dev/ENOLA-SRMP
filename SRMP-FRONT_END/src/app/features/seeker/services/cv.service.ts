import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import { SeekerCv } from '../models/cv.model';

@Injectable({
  providedIn: 'root'
})
export class CvService {
  private readonly apiUrl = `${environment.apiUrl}/jobseeker/cv`;

  constructor(private http: HttpClient) { }

  getCv(): Observable<SeekerCv> {
    return this.http.get<SeekerCv>(this.apiUrl);
  }

  uploadCv(file: File): Observable<SeekerCv> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<SeekerCv>(`${this.apiUrl}/upload`, formData);
  }

  downloadCv(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/download`, {
      responseType: 'blob'
    });
  }

  deleteCv(): Observable<{ message: string }> {
    return this.http.delete<{ message: string }>(this.apiUrl);
  }
}
