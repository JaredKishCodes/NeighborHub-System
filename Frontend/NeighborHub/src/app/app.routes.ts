import { Routes } from '@angular/router';
import { Item } from './components/layout/item/item';
import { Dashboard } from './components/layout/dashboard/dashboard';

export const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: Dashboard },
  { path: 'item', component: Item },
];
