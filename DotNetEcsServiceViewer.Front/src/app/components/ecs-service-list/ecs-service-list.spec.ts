import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EcsServiceList } from './ecs-service-list';

describe('EcsServiceList', () => {
  let component: EcsServiceList;
  let fixture: ComponentFixture<EcsServiceList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EcsServiceList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EcsServiceList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
