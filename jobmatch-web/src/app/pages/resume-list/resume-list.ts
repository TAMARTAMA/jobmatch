import { Component, OnInit, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Resumes } from '../../services/resume';
import { Resume } from '../../models/resume';

@Component({
  imports: [RouterLink, DatePipe],
  selector: 'app-resume-list',
  styleUrl: './resume-list.css',
  templateUrl: './resume-list.html',
})
export class ResumeList implements OnInit {
  private resumeService = inject(Resumes);

  protected resumes = signal<Resume[]>([]);
  protected loading = signal(true);
  protected error = signal<string | null>(null);

  ngOnInit(): void {
    this.resumeService.getAll().subscribe({
      next: (list) => {
        this.resumes.set(list);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('לא הצלחתי לטעון קורות חיים. האם השרת רץ?');
        this.loading.set(false);
      },
    });
  }

  remove(resume: Resume): void {
    if (!confirm(`למחוק את "${resume.title}"?`)) return;
    this.resumeService.delete(resume.id).subscribe({
      next: () => this.resumes.update((list) => list.filter((r) => r.id !== resume.id)),
      error: () => this.error.set('המחיקה נכשלה'),
    });
  }
}