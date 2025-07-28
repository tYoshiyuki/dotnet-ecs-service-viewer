import { TestBed } from '@angular/core/testing';

import { Ecs } from './ecs';

describe('Ecs', () => {
  let service: Ecs;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Ecs);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
