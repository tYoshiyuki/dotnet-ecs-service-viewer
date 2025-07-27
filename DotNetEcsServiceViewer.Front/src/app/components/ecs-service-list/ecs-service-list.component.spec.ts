import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EcsServiceListComponent } from './ecs-service-list.component';

describe('EcsServiceListComponent', () => {
  let component: EcsServiceListComponent;
  let fixture: ComponentFixture<EcsServiceListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EcsServiceListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EcsServiceListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
