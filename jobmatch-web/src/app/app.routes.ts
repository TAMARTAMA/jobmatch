import { Routes } from '@angular/router';
import { JobList } from './pages/job-list/job-list';
import { JobForm } from './pages/job-form/job-form';
import { JobDetail } from './pages/job-detail/job-detail';
import { ResumeList } from './pages/resume-list/resume-list';
import { ResumeForm } from './pages/resume-form/resume-form';

export const routes: Routes = [
  { path: '', component: JobList },
  { path: 'jobs/new', component: JobForm },
  { path: 'jobs/:id', component: JobDetail },
  { path: 'resumes', component: ResumeList },
  { path: 'resumes/new', component: ResumeForm },
  { path: 'resumes/:id/edit', component: ResumeForm },
  { path: '**', redirectTo: '' },
];