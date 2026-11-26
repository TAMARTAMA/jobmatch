export type JobStatus = 'Saved' | 'Applied' | 'Interview' | 'Rejected' | 'Offer';

export const JOB_STATUSES: JobStatus[] = ['Saved', 'Applied', 'Interview', 'Rejected', 'Offer'];

export interface JobPosting {
    id: number;
    title: string;
    company: string | null;
    location: string | null;
    description: string;
    url: string | null;
    status: JobStatus;
    createdAt: string;
}
  
export interface CreateJobPosting {
    title: string;
    company: string | null;
    location: string | null;
    description: string;
    url: string | null;
}