import { Routes } from '@angular/router';
import { StartComponent } from './component/start/start.component';
import { LoginComponent } from './component/login/login.component';
import { RegisterComponent } from './component/register/register.component';
import { HomeComponent } from './component/home/home.component';

export const routes: Routes = [
    { path: '', component: StartComponent },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'home', component: HomeComponent },



];
