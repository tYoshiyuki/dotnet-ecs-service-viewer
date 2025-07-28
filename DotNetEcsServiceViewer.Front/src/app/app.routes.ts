import { Routes } from '@angular/router';
import { EcsServiceList } from './components/ecs-service-list/ecs-service-list';

export const routes: Routes = [
    { path: '', component: EcsServiceList },
    { path: '**', redirectTo: '' }
];
