import { Component, OnInit, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { JobPostings } from '../../services/job-postings';
import { JobPosting } from '../../models/job-posting';

@Component({
  imports: [RouterLink, DatePipe],
  selector: 'app-job-detail',
  styleUrl: './job-detail.css',
  templateUrl: './job-detail.html',
})
export class JobDetail implements OnInit {
  private route = inject(ActivatedRoute);
  private jobService = inject(JobPostings);

  protected job = signal<JobPosting | null>(null);
  protected error = signal<string | null>(null);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.jobService.getById(id).subscribe({
      next: (job) => this.job.set(job),
      error: () => this.error.set('המשרה לא נמצאה'),
    });
  }
}