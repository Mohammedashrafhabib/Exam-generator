import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TranslateService } from '@ngx-translate/core';
import { BaseHttpServiceService } from 'src/app/sharedFeatures/services/base-http-service.service';
import { CurrentUserService } from 'src/app/sharedFeatures/services/current-user.service';
import { Observable } from 'rxjs';
import { contextModel } from '../models/context.model';
import { ResultModel } from '../models/result.model';

@Injectable({
  providedIn: 'root'
})
export class GenerateService extends BaseHttpServiceService {
  private controller: string = `${this.baseUrl}/ExamGenrator/`;
  constructor(
    http: HttpClient,
    translateService: TranslateService,
    currentUserService: CurrentUserService
  ) {
    super(http, translateService, currentUserService);
  }
  genrate(context: contextModel): Observable<any> {
    let url: string = `${this.controller}Genarate`;
    return this.postData<ResultModel>(url, context);
  }
}
