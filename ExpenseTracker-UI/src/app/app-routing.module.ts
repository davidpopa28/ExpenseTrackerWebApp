import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePageComponent } from './home-page/home-page.component';
import { LoginPageComponent } from './login-page/login-page.component';
import { RegisterPageComponent } from './register-page/register-page.component';
import { RecordsPageComponent } from './records-page/records-page.component';
import { AccountsPageComponent } from './accounts-page/accounts-page.component';
import { AnalyticsPageComponent } from './analytics-page/analytics-page.component';

const routes: Routes = [
  { path: 'home', component: HomePageComponent },
  { path: 'login', component: LoginPageComponent },
  { path: 'register', component: RegisterPageComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'records', component: RecordsPageComponent},
  { path: 'accounts', component: AccountsPageComponent},
  { path: 'analytics', component: AnalyticsPageComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
