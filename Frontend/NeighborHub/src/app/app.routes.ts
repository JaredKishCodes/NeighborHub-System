import { Routes } from '@angular/router';
import { Item } from './components/layout/item/item';
import { Dashboard } from './components/layout/dashboard/dashboard';
import { AuthComponent } from './components/auth/auth';
import { authGuard } from './auth.guard';
import { Layout } from './components/layout/layout';
import { MyBookings } from './components/layout/my-bookings/my-bookings';
import { MyLendings } from './components/layout/my-lendings/my-lendings';

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
      { path: 'my-bookings', component: MyBookings },
      { path: 'my-lendings', component: MyLendings },
    ]
  },
];
