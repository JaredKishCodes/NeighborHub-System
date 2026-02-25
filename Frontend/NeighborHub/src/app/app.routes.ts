import { Routes } from '@angular/router';
import { Item } from './components/layout/item/item';
import { Dashboard } from './components/layout/dashboard/dashboard';
import { AuthComponent } from './components/auth/auth';
import { authGuard } from './auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/auth', pathMatch: 'full' },
  { path: 'auth', component: AuthComponent },
  { path: 'dashboard', component: Dashboard, canActivate: [authGuard] },
  { path: 'item', component: Item, canActivate: [authGuard] },
];
