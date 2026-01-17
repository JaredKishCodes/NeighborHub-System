import { Routes } from '@angular/router';
import { Resource } from './components/resource/resource';

export const routes: Routes = [

    { path: '', redirectTo: '/home', pathMatch: 'full' },

   { path: 'resource', component: Resource }
];
