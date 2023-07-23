import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { ChangePasswordViewModel } from 'src/app/modules/users/models/change-password-view-model';
import { UserService } from 'src/app/modules/users/services/user.service';
import { LookupModel } from 'src/app/sharedFeatures/models/lookup-model';
import { UserLoggedIn } from 'src/app/sharedFeatures/models/user-login.model';
import { CurrentUserService } from 'src/app/sharedFeatures/services/current-user.service';
import { NotificationService } from 'src/app/sharedFeatures/services/notification.service';
import { PageTitleService } from 'src/app/sharedFeatures/services/page-title.service';
import { ValidationService } from 'src/app/sharedFeatures/services/validation.service';

@Component({
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {

  currentUser: UserLoggedIn | null = null;
  editForm!: FormGroup;
  subscriptionTranslate!: Subscription;
  subscriptions: Subscription[] = [];

  id!: number;
  model!: ChangePasswordViewModel;
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
    this.setPageTitle();
    this.buildForm();
    this.setLanguageSubscriber();
    this.currentUser = this.currentUserService.getCurrentUser();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }



  submit(): void {
    debugger;
    if (this.editForm.valid) {
      let user: ChangePasswordViewModel = this.collectModel();

        this.subscriptions.push(
          this.userService.changePassword(user).subscribe(
            (res: any) => {
              this.notificationService.showSuccess(
                `data Saved Successfuly`,
                ''
              );
              localStorage.removeItem('currentUser');
              this.router.navigate(['/']);
              this.goToHome();
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
                  'error.shared.oldPasswordWrong',
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

  collectModel(): ChangePasswordViewModel {
    let user = new ChangePasswordViewModel();
    user.userId = this.currentUser?.id;
    user.oldPassword = this.editForm?.controls['oldPassword'].value;
    user.newPassword = this.editForm?.controls['newPassword'].value;
    user.confirmPassword = this.editForm?.controls['confirmPassword'].value;
    return user;
  }
  bindModelToForm() {
    this.editForm?.controls['oldPassword']?.setValue(this.model.oldPassword);
    this.editForm?.controls['newPassword']?.setValue(this.model.newPassword);
    this.editForm?.controls['confirmPassword']?.setValue(
      this.model.confirmPassword
    );
  }
  buildForm(): void {
    this.editForm = this.fb.group({
      oldPassword: ['', [Validators.required, Validators.maxLength(200)]],
      newPassword: [
        '',
        [Validators.required, Validators.maxLength(200), this.shouldNotMatch],
      ],
      confirmPassword: [
        '',
        [
          Validators.required,
          Validators.maxLength(200),
          this.mustMatchPassword,
        ],
      ],
    });
  }

  mustMatchPassword(control: AbstractControl): ValidationErrors | null {
    let newPassword = control.parent?.get('newPassword');
    let confirmPassword = control.parent?.get('confirmPassword');
    if (
      newPassword?.value &&
      confirmPassword?.value &&
      newPassword?.value != confirmPassword?.value
    )
      return { mustMatchPassword: true };
    return null;
  }

  shouldNotMatch(control: AbstractControl): ValidationErrors | null {
    let oldPassword = control.parent?.get('oldPassword');
    let newPassword = control.parent?.get('newPassword');
    if (
      oldPassword?.value &&
      newPassword?.value &&
      newPassword?.value == oldPassword?.value
    )
      return { notMatcholdPassword: true };
    return null;
  }

  setLanguageSubscriber(): void {
    this.subscriptionTranslate = this.translateService.onLangChange.subscribe(
      val => {
        this.setPageTitle();
      },
      error => {},
      () => {}
    );
    this.subscriptions.push(this.subscriptionTranslate);
  }
  setPageTitle(): void {
    this.pageTitle.setTitleTranslated(`ChangePassword`);
  }
  getControl(controlName: string): any {
    return this.editForm?.controls[controlName];
  }
  goToHome() {

      this.router.navigate(['/']);
    
  }
}
