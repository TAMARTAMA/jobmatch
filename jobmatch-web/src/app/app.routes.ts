import { Routes } from '@angular/router';
import { JobList } from './pages/job-list/job-list';
import { JobForm } from './pages/job-form/job-form';
import { JobDetail } from './pages/job-detail/job-detail';

export const routes: Routes = [
  { path: '', component: JobList },
  { path: 'jobs/new', component: JobForm },
  { path: 'jobs/:id', component: JobDetail },
  { path: '**', redirectTo: '' },
];