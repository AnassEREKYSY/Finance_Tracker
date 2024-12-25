import { Routes } from '@angular/router';
import { StartComponent } from './component/start/start.component';
import { LoginComponent } from './component/login/login.component';
import { RegisterComponent } from './component/register/register.component';
import { HomeComponent } from './component/home/home.component';
import { BudgetsComponent } from './component/budgets/budgets.component';
import { TransactionsComponent } from './component/transactions/transactions.component';
import { AddUpdateTransactionComponent } from './component/transactions/add-update-transaction/add-update-transaction.component';
import { CategoriesComponent } from './component/categories/categories.component';
import { AddUpdateCategoryComponent } from './component/categories/add-update-category/add-update-category.component';
import { AddUpdateBudgetComponent } from './component/budgets/add-update-budget/add-update-budget.component';
import { AuthGuard } from './guards/auth.guard';
import { BudgetReportComponent } from './component/budgets/budget-report/budget-report.component';

export const routes: Routes = [
    { path: '', component: StartComponent },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'home', component: HomeComponent },
    { path: 'budgets', component: BudgetsComponent , canActivate: [AuthGuard] },
    { path: 'budgets/addUpdate', component: AddUpdateBudgetComponent , canActivate: [AuthGuard] },
    { path: 'budgets/report', component: BudgetReportComponent , canActivate: [AuthGuard] },
    { path: 'transactions', component: TransactionsComponent , canActivate: [AuthGuard] },
    { path: 'transactions/addUpdate', component: AddUpdateTransactionComponent , canActivate: [AuthGuard] },
    { path: 'categories', component: CategoriesComponent , canActivate: [AuthGuard] },
    { path: 'categories/addUpdate', component: AddUpdateCategoryComponent , canActivate: [AuthGuard] },
];
