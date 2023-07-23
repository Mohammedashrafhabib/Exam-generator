import { ErrorCode } from './../../../../sharedFeatures/enum/errorCode-mode.enum';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { ChangePasswordViewModel } from 'src/app/modules/users/models/change-password-view-model';
import { UserViewModel } from 'src/app/modules/users/models/user-model';
import { UserService } from 'src/app/modules/users/services/user.service';
import { UserLoggedIn } from 'src/app/sharedFeatures/models/user-login.model';
import { CurrentUserService } from 'src/app/sharedFeatures/services/current-user.service';
import { NotificationService } from 'src/app/sharedFeatures/services/notification.service';
import { PageTitleService } from 'src/app/sharedFeatures/services/page-title.service';
import { ValidationService } from 'src/app/sharedFeatures/services/validation.service';
import { UserDetailModel } from './../../../users/models/user-details-model';
import { Component, OnInit } from '@angular/core';

@Component({
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {

  currentUser: UserLoggedIn | null = null;
  editForm!: FormGroup;
  subscriptionTranslate!: Subscription;
  subscriptions: Subscription[] = [];

  id!: number;
  model!: UserViewModel;
  domainUrl: string = this.userService.domainUrl;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private translateService: TranslateService,
    private notificationService: NotificationService,
    private userService: UserService,
    private pageTitle: PageTitleService,
    private currentUserService: CurrentUserService,
    private validationService: ValidationService
  ) {}

  ngOnInit(): void {
    this.buildForm();
    this.currentUser = this.currentUserService.getCurrentUser();
    this.id=this.currentUser?.id
this.getuser()
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }
  goToHome() {

    this.router.navigate(['/']);
  
}
getuser(){
  this.subscriptions.push(
    this.userService.getById(this.id).subscribe(
      (res: any) => {
        this.model = res;
        this.bindModelToForm();
      },
      (error: any) => {
        if (error.status == 70) {
          let key = 'error.auth';
          this.translateService.get([key]).subscribe(res => {
            this.notificationService.showErrorTranslated(
              `${res[key]}`,
              ''
            );
          });
          this.goToHome();
        } else {
          this.notificationService.showErrorTranslated(
            'error.shared.operationFailed',
            ''
          );
        }
      }
    )
  );
}


  submit(): void {
    debugger;
    if (this.editForm.valid) {
      let user: UserViewModel = this.collectModel();

        this.subscriptions.push(
          this.userService.update(user).subscribe(
            (res: any) => {
              this.notificationService.showSuccess(
                `data Saved Successfuly`,
                ''
              );
           
            },
            (error: any) => {
              
              if (error.status == 400) {
                let key = 'error.400';
                this.translateService.get([key]).subscribe(res => {
                  this.notificationService.showErrorTranslated(
                    `${res[key]}`,
                    ''
                  );
                });
              } else {
                this.notificationService.showErrorTranslated(
                  `${error.error.mesage}`,
                  ''
                );
              }
            }
          )
        );
      
    } else {
      const loginFormFormKeys = Object.keys(this.editForm.controls);
      loginFormFormKeys.forEach(control => {
        this.editForm.controls[control].markAsTouched();
      });
    }
  }

  collectModel(): UserViewModel {
    let user = new UserViewModel();
    user.id = this.currentUser?.id;
    user.firstName = this.editForm?.controls['firstname'].value;
    user.lastName = this.editForm?.controls['lastname'].value;
    user.email = this.editForm?.controls['email'].value;
    return user;
  }
  bindModelToForm() {
    debugger
    this.editForm?.controls['firstname']?.setValue(this.model.firstName);
    this.editForm?.controls['lastname']?.setValue(this.model.lastName);
    this.editForm?.controls['email']?.setValue(
      this.model.email
    );
  }
  buildForm(): void {
    this.editForm = this.fb.group({
      firstname: ['', [Validators.required, Validators.maxLength(200)]],
      lastname: [
        '',
        [Validators.required, Validators.maxLength(200)],
      ],
      email: [
        '',
        [
          Validators.required,
          Validators.maxLength(250),
         
        ],
      ],
    });
  }


}
