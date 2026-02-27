import { Routes } from '@angular/router';
import { Item } from './components/layout/item/item';
import { Dashboard } from './components/layout/dashboard/dashboard';
import { AuthComponent } from './components/auth/auth';
import { authGuard } from './auth.guard';
import { Layout } from './components/layout/layout';

export const routes: Routes = [
  { path: '', redirectTo: '/auth', pathMatch: 'full' },
  { path: 'auth', component: AuthComponent },
  {
    path: '',
    component: Layout, // The parent shell with sidebar/navbar
    canActivate: [authGuard],
    children: [
      { path: 'dashboard', component: Dashboard },
      { path: 'item', component: Item },
    ]
  },
];
