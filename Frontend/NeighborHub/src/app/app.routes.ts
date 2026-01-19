import { Routes } from '@angular/router';
import { Item } from './components/layout/item/item';


export const routes: Routes = [

    { path: '', redirectTo: '/home', pathMatch: 'full' },
    { path: 'item', component: Item }
];
