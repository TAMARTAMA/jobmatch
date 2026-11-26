import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Observable } from 'rxjs';
import { Resumes } from '../../services/resumes';

@Component({
  imports: [ReactiveFormsModule, RouterLink],
  selector: 'app-resume-form',
  styleUrl: './resume-form.css',
  templateUrl: './resume-form.html',
})
export class ResumeForm implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private resumeService = inject(Resumes);

  protected editId = signal<number | null>(null);
  protected saving = signal(false);
  protected error = signal<string | null>(null);

  protected form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    content: ['', Validators.required],
  });

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      this.editId.set(id);
      this.resumeService.getById(id).subscribe({
        next: (r) => this.form.setValue({ title: r.title, content: r.content }),
        error: () => this.error.set('קורות החיים לא נמצאו'),
      });
    }
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.saving.set(true);
    const value = this.form.getRawValue();
    const id = this.editId();
    const request: Observable<unknown> = id === null
      ? this.resumeService.create(value)
      : this.resumeService.update(id, value);

    request.subscribe({
      next: () => this.router.navigate(['/resumes']),
      error: () => {
        this.error.set('השמירה נכשלה');
        this.saving.set(false);
      },
    });
  }
}