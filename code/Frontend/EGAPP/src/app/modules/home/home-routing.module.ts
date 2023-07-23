import { GenrateComponent } from './components/genrate/genrate.component';
import { StartComponent } from './components/start/start.component';
import { MainComponent } from './components/main/main.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from 'src/app/sharedFeatures/services/auth-guard.service';
import { ContactComponent } from './components/contact/contact.component';
import { TermsComponent } from './components/terms/terms.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', component: MainComponent },
  { path: 'start', pathMatch: 'full', component: StartComponent },
  { path: 'generate', pathMatch: 'full', component: GenrateComponent },
  { path: 'Contact', pathMatch: 'full', component: ContactComponent },
  { path: 'Terms', pathMatch: 'full', component: TermsComponent },



];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class homeRoutingModule {}
