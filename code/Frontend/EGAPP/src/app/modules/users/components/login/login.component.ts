import { Component, OnInit, OnDestroy, ChangeDetectorRef, ChangeDetectionStrategy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { NotificationService } from '../../../../sharedFeatures/services/notification.service';
import { UserLoggedIn } from '../../../../sharedFeatures/models/user-login.model';
import { PageTitleService } from '../../../../sharedFeatures/services/page-title.service';
import { Login } from '../../models/login';
import { LoginService } from '../../services/login.service';
import { LanguageService } from 'src/app/sharedFeatures/services/language';
import { UserViewModel } from '../../models/user-model';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit, OnDestroy {
  language!: string;
  loginForm!: FormGroup;
  subscriptionTranslate!: Subscription;
  currentUser: UserLoggedIn | null = null;
  subscriptions: Subscription[] = [];
  editForm_signup!: FormGroup;


 
  model!: UserViewModel;
  domainUrl: string = this.userService.domainUrl;


  login:boolean=true;
  isDisabled: boolean;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private translateService: TranslateService,
    public languageService: LanguageService,
    private notificationService: NotificationService,
    private loginService: LoginService,
    private pageTitle: PageTitleService,
    private userService: UserService,

    private changeDetectorRef:ChangeDetectorRef
   
  ) {}
  changeLang(lang: string) {
    this.translateService.use(lang);
    localStorage.setItem('currentLang', lang);
    this.languageService.setLanguage(lang);
  }
  ngOnInit(): void {
    this.setPageTitle();
    this.buildForm();
    this.buildForm_signup();

    this.setLanguageSubscriber();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  submit(): void {
    if (this.loginForm.valid) {
      let loginModel: Login = {
        userName: this.loginForm.controls['username'].value,
        password: this.loginForm.controls['password'].value,
      };
      this.subscriptions.push(
        this.loginService.login(loginModel).subscribe(
          (res: any) => {
            this.currentUser = res;
            localStorage.setItem('currentUser', JSON.stringify(res));
            this.router.navigate(['/']);
          },
          (error: any) => {
            
            if (error.status == 400) {
              let key = 'error.400';
              this.translateService.get([key]).subscribe(res => {
                this.notificationService.showErrorTranslated(`${res[key]}`, '');
              });
            } else if (error.status == 55) {
              let key = 'error.55';
              this.translateService.get([key]).subscribe(res => {
                this.notificationService.showErrorTranslated(`${res[key]}`, '');
              });
            } else {
              this.notificationService.showErrorTranslated(
                'error.shared.operationFailed',
                ''
              );
            }
          },
          () => {}
        )
      );
    } else {
      const loginFormFormKeys = Object.keys(this.loginForm.controls);
      loginFormFormKeys.forEach(control => {
        this.loginForm.controls[control].markAsTouched();
      });
    }
  }

  buildForm(): void {
    this.loginForm = this.fb.group({
      username: [
        null,
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(200),
        ],
      ],
      password: [null, [Validators.required, Validators.maxLength(200)]],
    });
  }
  setLanguageSubscriber(): void {
    this.language = this.translateService.currentLang;
    this.subscriptionTranslate = this.translateService.onLangChange.subscribe(
      val => {
        this.language = val.lang;
        this.setPageTitle();
      },
      error => {},
      () => {}
    );
    this.subscriptions.push(this.subscriptionTranslate);
  }
  setPageTitle(): void {
    this.pageTitle.setTitleTranslated(`login`);
  }
  goToHome() {

    this.router.navigate(['/']);
  
}
  submit_signup(): void {
    if (this.editForm_signup.valid) {
      let user: UserViewModel = this.collectModel_signup();

        this.subscriptions.push(
          this.userService.add(user).subscribe(
            (res: any) => {
           
              this.notificationService.showSuccess(
                `data Saved Successfuly`,
                ''
              );
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
                  'error.' + error.error.code,
                  ''
                );
              }
            }
          )
        );
      
    } else {
      this.isDisabled = false;
      const loginFormFormKeys = Object.keys(this.editForm_signup.controls);
      loginFormFormKeys.forEach(control => {
        this.editForm_signup.controls[control].markAsTouched();
      });
    }
  }


  collectModel_signup(): UserViewModel {
    let user = new UserViewModel();
    user.username = this.editForm_signup?.controls['email'].value;
    user.firstName = this.editForm_signup?.controls['firstName'].value;
    user.lastName = this.editForm_signup?.controls['lastName'].value;
    user.email = this.editForm_signup?.controls['email'].value;
   
    return user;
  }
 
  buildForm_signup(): void {
    this.editForm_signup = this.fb.group({
      //username: ['', [Validators.required, Validators.minLength(2)]],
      lastName: [null, [Validators.required, Validators.maxLength(200)]],
      firstName: [null, [Validators.required, Validators.maxLength(200)]],
      
      email: [
        null,
        [Validators.required, Validators.email, Validators.maxLength(200)],
      ],
     
    });
  }

  setPageTitle_signup(): void {
    this.pageTitle.setTitleTranslated(`Signe Up`);
  }
  

  getControl(controlName: string): any {
    return this.loginForm?.controls[controlName];
  }
}
