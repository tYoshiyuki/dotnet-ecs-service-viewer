import { TestBed } from '@angular/core/testing';

import { EcsService } from './ecs.service';

describe('EcsService', () => {
  let service: EcsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EcsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
