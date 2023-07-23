import { MainComponent } from './../../modules/home/components/main/main.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from 'src/app/modules/users/components/login/login.component';
import { LoginActivate } from '../../sharedFeatures/services/login-activate.service';

const routes: Routes = [
  {
    path: 'users',
    loadChildren: () =>
      import('src/app/modules/users/users.module').then(m => m.UsersModule),
  },

 
 
  {
    path: 'home',
    loadChildren: () =>
      import('src/app/modules/home/home.module').then(
        m => m.HomeModule
      ),
  },
  {
    path: 'settings',
    loadChildren: () =>
      import('src/app/modules/settings/settings.module').then(
        m => m.SettingsModule
      ),
  },
  { path: 'login', component: LoginComponent, canActivate: [LoginActivate] },

  { path: '', pathMatch: 'full', redirectTo: '/home' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
