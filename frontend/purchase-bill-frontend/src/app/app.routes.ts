import { Routes } from '@angular/router';
import { Login } from './features/login/login';
import { PurchaseBill } from './features/purchase-bill/purchase-bill';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'purchase-bill', component: PurchaseBill, canActivate: [authGuard] },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
];
