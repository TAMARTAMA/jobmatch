import { Component, OnInit, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { JobPostings } from '../../services/job-postings';
import { JobPosting } from '../../models/job-posting';
import { JobAnalysis, JobSkill } from '../../models/job-analysis';
import { HttpErrorResponse } from '@angular/common/http';
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
  protected skills = signal<JobSkill[]>([]);
  protected analysis = signal<JobAnalysis | null>(null);
  protected analyzing = signal(false);
  protected error = signal<string | null>(null);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.jobService.getById(id).subscribe({
      next: (job) => this.job.set(job),
      error: () => this.error.set('המשרה לא נמצאה'),
    });
    this.jobService.getSkills(id).subscribe({
      next: (skills) => this.skills.set(skills),
    });
  }

  protected requiredSkills(): JobSkill[] {
    return this.skills().filter((s) => s.isRequired);
  }

  protected niceSkills(): JobSkill[] {
    return this.skills().filter((s) => !s.isRequired);
  }

  analyze(): void {
    const job = this.job();
    if (!job) return;
    this.analyzing.set(true);
    this.error.set(null);
    this.jobService.analyze(job.id).subscribe({
      next: (result) => {
        this.analysis.set(result);
        this.skills.set([
          ...result.requiredSkills.map((name: string) => ({ name, isRequired: true })),
          ...result.niceToHaveSkills.map((name: string) => ({ name, isRequired: false })),
        ]);
        this.analyzing.set(false);
      },
      error: (err: HttpErrorResponse) => {
        this.error.set('הניתוח נכשל: ' + (err.error?.detail ?? err.message));
        this.analyzing.set(false);
      },
    });
  }
}