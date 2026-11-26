import { TestBed } from '@angular/core/testing';

import { JobPostings } from './job-postings';

describe('JobPostings', () => {
  let service: JobPostings;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JobPostings);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
