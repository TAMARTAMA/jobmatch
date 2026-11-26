
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { CreateJobPosting, JobPosting, JobStatus } from '../models/job-posting';
import { JobAnalysis, JobSkill } from '../models/job-analysis';


@Injectable({
  providedIn: 'root'
})
export class JobPostings {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/JobPostings`;

  getAll(): Observable<JobPosting[]> {
    return this.http.get<JobPosting[]>(this.baseUrl);
  }

  getById(id: number): Observable<JobPosting> {
    return this.http.get<JobPosting>(`${this.baseUrl}/${id}`);
  }

  create(job: CreateJobPosting): Observable<JobPosting> {
    return this.http.post<JobPosting>(this.baseUrl, job);
  }

  updateStatus(id: number, status: JobStatus): Observable<void> {
    return this.http.patch<void>(`${this.baseUrl}/${id}/status`, { status });
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  getSkills(id: number): Observable<JobSkill[]> {
    return this.http.get<JobSkill[]>(`${this.baseUrl}/${id}/skills`);
  }

  analyze(id: number): Observable<JobAnalysis> {
    return this.http.post<JobAnalysis>(`${this.baseUrl}/${id}/analyze`, {});
  }
}
