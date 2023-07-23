
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from 'src/app/sharedFeatures/services/auth-guard.service';
import { AccountComponent } from './components/account/account.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', component: AccountComponent },
  { path: 'changePassword', pathMatch: 'full', component: ChangePasswordComponent },
  { path: 'Account', pathMatch: 'full', component: AccountComponent },



];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SettingsRoutingModule {}
