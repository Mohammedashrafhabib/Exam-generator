import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './components/main/main.component';
import { homeRoutingModule } from './home-routing.module';
import { StartComponent } from './components/start/start.component';
import { GenrateComponent } from './components/genrate/genrate.component';
import { SharedCommonModuleModule } from 'src/app/applicationFeatures/shared-common-module/shared-common-module.module';
import { SharedComponentModule } from 'src/app/applicationFeatures/shared-components/shared-components.module';
import { ContactComponent } from './components/contact/contact.component';
import { TermsComponent } from './components/terms/terms.component';



@NgModule({
  declarations: [
    MainComponent,
    StartComponent,
    GenrateComponent,
    ContactComponent,
    TermsComponent
  ],
  imports: [
    CommonModule,
    homeRoutingModule,
    SharedComponentModule,
    SharedCommonModuleModule,
  ]
})
export class HomeModule { }
