
import { ChangePasswordViewModel } from './../models/change-password-view-model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { UserLoggedIn } from './../../../sharedFeatures/models/user-login.model';
import { BaseHttpServiceService } from './../../../sharedFeatures/services/base-http-service.service';
import { CurrentUserService } from './../../../sharedFeatures/services/current-user.service';

import { UserViewModel } from '../models/user-model';
import { UserDetailModel } from '../models/user-details-model';
import { UserLightViewModel } from '../models/user-light-view-model.model';


@Injectable({
  providedIn: 'root',
})
export class UserService extends BaseHttpServiceService {
  private controller: string = `${this.baseUrl}/Users/`;
  constructor(
    http: HttpClient,
    translateService: TranslateService,
    currentUserService: CurrentUserService
  ) {
    super(http, translateService, currentUserService);
  }

  add(model: UserViewModel): Observable<UserViewModel> {
    let url: string = `${this.controller}add`;
    return this.postData<UserViewModel>(url, model);
  }

  update(model: UserViewModel): Observable<UserViewModel> {
    let url: string = `${this.controller}update`;
    return this.postData<UserViewModel>(url, model);
  }

  changePassword(model: ChangePasswordViewModel): Observable<any> {
    let url: string = `${this.controller}change-password`;
    return this.postData<ChangePasswordViewModel>(url, model);
  }
  checkPermissionForList(id: any): Observable<boolean> {
    let url: string = `${this.controller}CheckPermissionForList/${id}`;
    return this.getData<boolean>(url);
  }
  

  getById(id: any): Observable<UserViewModel> {
    let url: string = `${this.controller}get/${id}`;
    return this.getData<UserViewModel>(url);
  }

  getDetailsById(id: any): Observable<UserDetailModel> {
    let url: string = `${this.controller}details/${id}`;
    return this.getData<UserDetailModel>(url);
  }
  delete(id: any): Observable<any> {
    let url: string = `${this.controller}delete/${id}`;
    return this.postData<any>(url, null);
  }

 
}
