import { Directive, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LanguageService } from '../services/language';

@Directive({
  selector: '[EGRtl]',
})
export class RtlDirective implements OnInit {
  constructor(
    private elem: ElementRef,
    private renderer: Renderer2,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    this.languageService.language.subscribe((lang: any) => {
      this.setRtlDirection(lang);
    });
  }
  setRtlDirection(lang: string): void {
    lang === 'ar'
      ? this.renderer.addClass(this.elem.nativeElement, 'rtl')
      : this.renderer.removeClass(this.elem.nativeElement, 'rtl');
  }
}
