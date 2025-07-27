import { Routes } from '@angular/router';
import { EcsServiceListComponent } from './components/ecs-service-list/ecs-service-list.component';

export const routes: Routes = [
  { path: '', component: EcsServiceListComponent },
  { path: '**', redirectTo: '' }
];
