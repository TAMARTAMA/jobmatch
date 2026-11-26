import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { CreateResume, Resume } from '../models/resume';

@Injectable({
  providedIn: 'root',
})
export class Resumes {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/Resumes`;

  getAll(): Observable<Resume[]> {
    return this.http.get<Resume[]>(this.baseUrl);
  }

  getById(id: number): Observable<Resume> {
    return this.http.get<Resume>(`${this.baseUrl}/${id}`);
  }
 
  create(resume: CreateResume): Observable<Resume> {
    return this.http.post<Resume>(this.baseUrl, resume);
  }

  update(id: number, resume: CreateResume): Observable<Resume> {
    return this.http.put<Resume>(`${this.baseUrl}/${id}`, resume);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
