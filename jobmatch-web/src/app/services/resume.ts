import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Resume, SaveResume } from '../models/resume';

@Injectable({ providedIn: 'root' })
export class Resumes {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/Resumes`;

  getAll(): Observable<Resume[]> {
    return this.http.get<Resume[]>(this.baseUrl);
  }

  getById(id: number): Observable<Resume> {
    return this.http.get<Resume>(`${this.baseUrl}/${id}`);
  }

  create(resume: SaveResume): Observable<Resume> {
    return this.http.post<Resume>(this.baseUrl, resume);
  }

  update(id: number, resume: SaveResume): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, resume);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}