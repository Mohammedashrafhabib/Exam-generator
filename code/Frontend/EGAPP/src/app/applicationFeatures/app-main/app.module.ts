import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './components/app-main/app.component';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { FooterComponent } from './components/layouts/footer/footer.component';
import { MainContentComponent } from './components/layouts/main-content/main-content.component';
import { NavBarComponent } from './components/layouts/nav-bar/nav-bar.component';
import { LoginComponent } from 'src/app/modules/users/components/login/login.component';
import { LoaderModule } from '../loader/loader.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedComponentModule } from '../shared-components/shared-components.module';
import { RtlDirective } from 'src/app/sharedFeatures/directives/rtl.directive';
import { SettingsModule } from '../../modules/settings/settings.module';

@NgModule({
  declarations: [
    AppComponent,
    FooterComponent,
    MainContentComponent,
    NavBarComponent,
    LoginComponent,
    RtlDirective,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    LoaderModule,
    SharedComponentModule,
    ReactiveFormsModule,
    FormsModule,
    ToastrModule.forRoot({
      closeButton: true,
      easeTime: 500,
      enableHtml: true,
      progressBar: true,
      progressAnimation: 'increasing',
    }),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: createTranslateLoader,
        deps: [HttpClient],
      },
    }),
    BrowserAnimationsModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}

export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
