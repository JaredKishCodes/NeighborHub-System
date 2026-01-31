import { Routes } from '@angular/router';
import { Item } from './components/layout/item/item';
import { CreateItem } from './components/layout/create-item/create-item';


export const routes: Routes = [

    { path: '', redirectTo: '/home', pathMatch: 'full' },
    { path: 'item', component: Item },
    { path: 'create-item', component: CreateItem }
];
