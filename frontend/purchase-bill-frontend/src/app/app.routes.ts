import { Routes } from '@angular/router';
import { Login } from './features/login/login';
import { PurchaseBill } from './features/purchase-bill/purchase-bill';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'purchase-bill', component: PurchaseBill },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
];
