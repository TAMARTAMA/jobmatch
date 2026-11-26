import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { JobPostings } from '../../services/job-postings';

@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-job-form',
  styleUrl: './job-form.css',
  templateUrl: './job-form.html',
})
export class JobForm {
  private fb = inject(FormBuilder);
  private jobService = inject(JobPostings);
  private router = inject(Router);

  protected saving = signal(false);
  protected error = signal<string | null>(null);

  protected form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    company: ['', Validators.maxLength(200)],
    location: ['', Validators.maxLength(100)],
    description: ['', Validators.required],
    url: ['', Validators.maxLength(500)],
  });

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const v = this.form.getRawValue();
    this.saving.set(true);
    this.jobService
      .create({
        title: v.title,
        company: v.company || null,
        location: v.location || null,
        description: v.description,
        url: v.url || null,
      })
      .subscribe({
        next: (job) => this.router.navigate(['/jobs', job.id]),
        error: () => {
          this.error.set('השמירה נכשלה');
          this.saving.set(false);
        },
      });
  }
}