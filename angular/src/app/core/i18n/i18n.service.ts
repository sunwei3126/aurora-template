import { Platform } from '@angular/cdk/platform';
import { registerLocaleData } from '@angular/common';
import { Injectable } from '@angular/core';

import { AlainI18NService, DelonLocaleService, SettingsService } from '@delon/theme';
import { TranslateService } from '@ngx-translate/core';
import { NzSafeAny } from 'ng-zorro-antd/core/types';
import { NzI18nService } from 'ng-zorro-antd/i18n';
import { BehaviorSubject, Observable } from 'rxjs';
import { filter } from 'rxjs/operators';

import ngEn from '@angular/common/locales/en';
import ngZhHans from '@angular/common/locales/zh-Hans';
import ngZhHant from '@angular/common/locales/zh-Hant';

import { en_US as delonEn, zh_CN as delonZhHans, zh_TW as delonZhHant } from '@delon/theme';
import { enUS as dfEn, zhCN as dfZhHans, zhTW as dfZhHant } from 'date-fns/locale';
import { en_US as zorroEn, zh_CN as zorroZhHans, zh_TW as zorroZhHant } from 'ng-zorro-antd/i18n';

interface LangData {
  abbr: string;
  text: string;
  ng: NzSafeAny;
  zorro: NzSafeAny;
  date: NzSafeAny;
  delon: NzSafeAny;
}

const DEFAULT = 'zh-Hans';
const LANGUAGES: { [key: string]: LangData } = {
  'zh-Hans': {
    text: '简体中文',
    ng: ngZhHans,
    zorro: zorroZhHans,
    date: dfZhHans,
    delon: delonZhHans,
    abbr: '🇨🇳'
  },
  'zh-Hant': {
    text: '繁体中文',
    ng: ngZhHant,
    zorro: zorroZhHant,
    date: dfZhHant,
    delon: delonZhHant,
    abbr: '🇭🇰'
  },
  en: {
    text: 'English',
    ng: ngEn,
    zorro: zorroEn,
    date: dfEn,
    delon: delonEn,
    abbr: '🇬🇧'
  }
};

@Injectable({ providedIn: 'root' })
export class I18NService implements AlainI18NService {
  private _default = DEFAULT;
  private change$ = new BehaviorSubject<string | null>(null);

  private _languages = Object.keys(LANGUAGES).map((code) => {
    const item = LANGUAGES[code];
    return { code, text: item.text, abbr: item.abbr };
  });

  constructor(
    private settings: SettingsService,
    private nzI18nService: NzI18nService,
    private delonLocaleService: DelonLocaleService,
    private translate: TranslateService,
    private platform: Platform
  ) {
    const languages = this._languages.map((item) => item.code);
    translate.addLangs(languages);

    const defaultLan = this.getDefaultLang();
    if (languages.includes(defaultLan)) this._default = defaultLan;

    this.updateLangData(this._default);
  }

  private getDefaultLang(): string {
    if (!this.platform.isBrowser) return DEFAULT;
    if (this.settings.layout.lang) return this.settings.layout.lang;
    return (navigator.languages ? navigator.languages[0] : null) || navigator.language;
  }

  private updateLangData(lang: string): void {
    const item = LANGUAGES[lang];
    registerLocaleData(item.ng);
    this.nzI18nService.setLocale(item.zorro);
    this.nzI18nService.setDateLocale(item.date);
    this.delonLocaleService.setLocale(item.delon);
  }

  get change(): Observable<string> {
    return this.change$.asObservable().pipe(filter((w) => w != null)) as Observable<string>;
  }

  use(lang: string): void {
    lang = lang || this.translate.getDefaultLang();
    if (this.currentLang === lang) {
      return;
    }
    this.updateLangData(lang);
    this.translate.use(lang).subscribe(() => this.change$.next(lang));
  }

  getLangs(): Array<{ code: string; text: string; abbr: string }> {
    return this._languages;
  }

  fanyi(key: string, interpolateParams?: {}): any {
    return this.translate.instant(key, interpolateParams);
  }

  get defaultLang(): string {
    return this._default;
  }

  get currentLang(): string {
    return this.translate.currentLang || this.translate.getDefaultLang() || this._default;
  }
}
