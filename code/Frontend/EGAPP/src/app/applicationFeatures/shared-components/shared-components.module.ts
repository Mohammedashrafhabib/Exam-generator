import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ControlErrorDisplayComponent } from './components/control-error-display/control-error-display.component';
import { SharedCcomponentRoutingModule } from './shared-components-routing.module';
import { TranslateModule } from '@ngx-translate/core';
import { PaginationComponent } from './components/pagination/pagination.component';
import { DeleteModalComponent } from './components/delete-modal/delete-modal.component';

@NgModule({
  declarations: [
    ControlErrorDisplayComponent,
    PaginationComponent,
    DeleteModalComponent,
  ],
  imports: [CommonModule, TranslateModule, SharedCcomponentRoutingModule],
  exports: [
    ControlErrorDisplayComponent,
    PaginationComponent,
    DeleteModalComponent,
  ],
})
export class SharedComponentModule {}
