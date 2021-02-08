import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ACLService } from '@delon/acl';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { ALAIN_I18N_TOKEN, MenuService, SettingsService, TitleService, _HttpClient } from '@delon/theme';
import { TranslateService } from '@ngx-translate/core';
import { I18NService } from '../i18n/i18n.service';

import { NzIconService } from 'ng-zorro-antd/icon';
import { ICONS } from '../../../style-icons';
import { ICONS_AUTO } from '../../../style-icons-auto';
import MENUS from '../menus';

@Injectable()
export class StartupService {
  constructor(
    iconSrv: NzIconService,
    private router: Router,
    private aclService: ACLService,
    private httpClient: _HttpClient,
    private menuService: MenuService,
    private titleService: TitleService,
    private translate: TranslateService,
    private settingService: SettingsService,
    @Inject(ALAIN_I18N_TOKEN) private i18n: I18NService,
    @Inject(DA_SERVICE_TOKEN) private tokenService: ITokenService
  ) {
    iconSrv.addIcon(...ICONS_AUTO, ...ICONS);
  }

  private viaHttp(resolve: any, reject: any): void {
    this.httpClient.get('/api/abp/application-configuration').subscribe(
      (res: any) => {
        const currentUser = res.currentUser;
        const currentTenant = res.currentTenant;

        if (currentUser.isAuthenticated) this.settingService.setUser({ name: currentUser.name, email: currentUser.email, avatar: './assets/tmp/img/avatar.jpg' });
        this.aclService.setAbility(Object.keys(res.auth.grantedPolicies));
        this.menuService.add(MENUS);

        this.settingService.setApp({ name: 'AURORA', description: currentTenant.name || '' });
        this.settingService.setData('config', res);
        this.settingService.setLayout('lang', res.localization.currentCulture.name);

        this.titleService.suffix = 'AURORA' + (currentTenant.name ? ' - ' + currentTenant.name : '');

        resolve({});
      },
      async () => {
        await this.router.navigateByUrl('/exception/500', { replaceUrl: true });
        resolve(null);
      }
    );
  }

  load(): Promise<any> {
    return new Promise((resolve, reject) => {
      this.viaHttp(resolve, reject);
    });
  }
}
