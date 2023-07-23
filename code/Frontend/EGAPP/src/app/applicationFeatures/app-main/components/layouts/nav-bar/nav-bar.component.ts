import { NotificationService } from './../../../../../sharedFeatures/services/notification.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { LanguageService } from 'src/app/sharedFeatures/services/language';
import { UserService } from 'src/app/modules/users/services/user.service';
import { Language } from 'src/app/sharedFeatures/models/language.enum';
import { UserLoggedIn } from 'src/app/sharedFeatures/models/user-login.model';
import { CurrentUserService } from '../../../../../sharedFeatures/services/current-user.service';


@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
})
export class NavBarComponent implements OnInit {
  lang: string = 'ar';
  currentUser: UserLoggedIn | null = null;
  domainUrl: string = this._UsersService.domainUrl;
  isButtonEnable:boolean =true;


  constructor(
    private currentUserService: CurrentUserService,
    private router: Router,
    private notificationService: NotificationService,
    private _UsersService: UserService,
    private translateService: TranslateService,
    public languageService: LanguageService,
    private _currentUserService: CurrentUserService
  ) {}

  ngOnInit(): void {
    this.lang = this.translateService.currentLang;
    this.currentUser = this.currentUserService.getCurrentUser();
    // if (this.currentUser == null) {
    //   this.router.navigate(['/login']);
    // }
    
  
    // TODO: avoid attaching events to global dom element
    // this.DismissNotificationMinue();
  }
 
  DismissNotificationMinue() {
    document.querySelector('#content')!.addEventListener('click', event => {
      const notificationCard: any = document.querySelector('#notificationCard');
      const withinBoundaries = event.composedPath().includes(notificationCard);
      if (withinBoundaries) {
        notificationCard.style.top = '71px';
      } else {
        notificationCard.style.top = '-150%';
        document
          .querySelector('.active-notification')
          ?.classList.remove('active-notification');
      }
    });
  }

  goToLink(url: string) {
    if (url != null) {
      if (url?.includes('http')) {
        window.open(url, '_blank');
      } else {
        this.router.navigate([url]);
      }
    }
  }

  userLogout() {
    this.currentUserService.logOut();
    this.router.navigate(['/login']);
  }
  viewUser() {

  //var service = this._currentUserService.getCurrentUser();
   
    let url = '/users/view/' + this.currentUser?.id + '/1';
    this.router.navigate([url]);
  }
  changeLang(lang: string) {
    this.translateService.use(lang);
    localStorage.setItem('currentLang', lang);

    this.languageService.setLanguage(lang);
  }
  UnSeenNotifications: number = 0;
  pageIndex = 0;
  pageSize = 10;
}
