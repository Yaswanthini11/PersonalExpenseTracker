import { Routes } from '@angular/router';

import { Login } from './features/auth/pages/login/login';
import { Register } from './features/auth/pages/register/register';
import { Dashboard } from './features/dashboard/pages/dashboard/dashboard';
import { Categories } from './features/categories/pages/categories/categories';
import { Expenses } from './features/expenses/pages/expenses/expenses';
import { authGuard } from './core/guards/auth.guard';
import { Budgets } from './features/budgets/pages/budgets/budgets';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
    },
    {
        path: 'login',
        component: Login
    },
    {
        path: 'register',
        component: Register
    },
    {
        path: 'dashboard',
        component: Dashboard,
        canActivate: [authGuard]
    },
    {
        path: 'categories',
        component: Categories,
        canActivate: [authGuard]
    },
    {
    path: 'expenses',
    component: Expenses,
    canActivate: [authGuard]
},
{
    path: 'budgets',
    component: Budgets,
    canActivate: [authGuard]
},

    {
        path: '**',
        redirectTo: 'login'
    }
];