import { Component, OnInit, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { JobPostings } from '../../services/job-postings';
import { JOB_STATUSES, JobPosting, JobStatus } from '../../models/job-posting';
@Component({
  imports: [RouterLink, DatePipe],
  selector: 'app-job-list',
  styleUrl: './job-list.css',
  templateUrl: './job-list.html',
})

export class JobList implements OnInit {
  private jobService = inject(JobPostings);

  protected jobs = signal<JobPosting[]>([]);
  protected loading = signal(true);
  protected error = signal<string | null>(null);
  protected statuses = JOB_STATUSES;

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.jobService.getAll().subscribe({
      next: (jobs) => {
        this.jobs.set(jobs);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('לא הצלחתי לטעון משרות. האם השרת רץ?');
        this.loading.set(false);
      },
    });
  }

  changeStatus(job: JobPosting, status: string): void {
    this.jobService.updateStatus(job.id, status as JobStatus).subscribe({
      next: () => this.jobs.update((list) =>
        list.map((j) => (j.id === job.id ? { ...j, status: status as JobStatus } : j))),
      error: () => this.error.set('עדכון הסטטוס נכשל'),
    });
  }

  remove(job: JobPosting): void {
    this.jobService.delete(job.id).subscribe({
      next: () => this.jobs.update((list) => list.filter((j) => j.id !== job.id)),
      error: () => this.error.set('המחיקה נכשלה'),
    });
  }
}