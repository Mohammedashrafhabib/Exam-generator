import { Component, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { LanguageService } from 'src/app/sharedFeatures/services/language';

import { UserLoggedIn } from '../../../../sharedFeatures/models/user-login.model';
import { CurrentUserService } from '../../../../sharedFeatures/services/current-user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'EGAPP';
  directionRTL: boolean = true;
  isMenuCollapsed: boolean = false;
  isNavigating: boolean = false;
  currentUser: UserLoggedIn | null = null;
  showArrow: boolean = false;

  constructor(
    public translateService: TranslateService,
    private currentUserService: CurrentUserService,
    private router: Router,
    public languageService: LanguageService,
    private activatedRoute: ActivatedRoute,
  ) {
    translateService.addLangs(['en']);
    var currentLang = localStorage.getItem('currentLang');
    if (currentLang != null && currentLang != 'null') {
      this.languageService.setLanguage(currentLang);
      this.translateService.use(currentLang);
    } else {
      this.languageService.setLanguage('en');
      translateService.use('en');
    }
  }

  ngOnInit(): void {
    // this.HubNotifictionService.startConnection();
    // this.HubNotifictionService.addDataSetReviewListener();
    // this.HubNotifictionService.addNEBTReviewListener();
    // this.HubNotifictionService.addChartReviewListener();

    this.router.events.subscribe((e) => {
      this.currentUser = this.currentUserService.getCurrentUser();
      // if (this.currentUser == null) {
      //   this.router.navigate(['/login']);
      // }
      // else {
      //   this.router.navigate(['/users/list']);
      // }
    });
  }

  @HostListener('scroll', ['$event'])
  onWindowScroll(event: any) {
    // do some stuff here when the window is scrolled
    // const verticalOffset = window.pageYOffset
    //       || document.documentElement.scrollTop
    //       || document.body.scrollTop || 0;

    // if (window.pageYOffset > 10) {
    //   this.showArrow = true;
    // }
    // if (window.pageYOffset <= 10) {
    //   this.showArrow = false;
    // }
  }

  getNavItems() {
   
  }

  goTop() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
  }

  switchLang(lang: string) {
    this.translateService.use(lang);
    localStorage.setItem('currentLang', lang);
  }
}
