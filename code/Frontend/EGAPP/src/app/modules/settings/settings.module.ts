import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { AccountComponent } from './components/account/account.component';
import { SharedCommonModuleModule } from 'src/app/applicationFeatures/shared-common-module/shared-common-module.module';
import { SharedComponentModule } from 'src/app/applicationFeatures/shared-components/shared-components.module';
import { SettingsRoutingModule } from './settings-routing.module';



@NgModule({
  declarations: [
    ChangePasswordComponent,
    AccountComponent
  ],
  imports: [
    CommonModule,
    SettingsRoutingModule,
    SharedComponentModule,
    SharedCommonModuleModule
  ]
})
export class SettingsModule { }
