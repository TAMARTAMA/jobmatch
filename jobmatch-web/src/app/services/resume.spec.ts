import { TestBed } from '@angular/core/testing';

import { Resumes } from './resume';

describe('Resume', () => {
  let service: Resumes;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Resumes);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
