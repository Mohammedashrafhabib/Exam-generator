import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UsersRoutingModule } from './users-routing.module';
import { FormGroupDirective, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './components/login/login.component';
import { SharedComponentModule } from '../../applicationFeatures/shared-components/shared-components.module';
import { TranslateModule } from '@ngx-translate/core';
import { LoaderModule } from 'src/app/applicationFeatures/loader/loader.module';
import { SharedCommonModuleModule } from 'src/app/applicationFeatures/shared-common-module/shared-common-module.module';

@NgModule({
  declarations: [
    
  ],
  imports: [
    CommonModule,
    UsersRoutingModule,
    SharedComponentModule,
    SharedCommonModuleModule
  ],
})
export class UsersModule {}
