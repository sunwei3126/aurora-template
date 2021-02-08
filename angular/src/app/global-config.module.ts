import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { throwIfAlreadyLoaded } from '@core';
import { DelonMockModule } from '@delon/mock';
import { AlainThemeModule } from '@delon/theme';
import { AlainConfig, ALAIN_CONFIG } from '@delon/util/config';

import { DelonACLModule } from '@delon/acl';

const alainConfig: AlainConfig = {
  st: {
    modal: { size: 'lg' },
    sortReName: { ascend: 'asc', descend: 'desc' },
    res: { reName: { list: 'items', total: 'totalCount' } },
    page: { front: false, showQuickJumper: true, showSize: true },
    req: { type: 'skip', reName: { skip: 'skipCount', limit: 'maxResultCount' } },
    multiSort: { global: true, key: 'sorting', nameSeparator: ' ', separator: ',', keepEmptyKey: false }
  },
  pageHeader: {
    home: '首页',
    syncTitle: true,
    autoTitle: false,
    recursiveBreadcrumb: true
  },
  auth: {
    store_key: 'access_token',
    login_url: '/passport/login',
    ignores: [/\/.well-known/, /\/connect\/token/, /\/api\/abp\/application-configuration/]
  }
};

const alainModules = [AlainThemeModule.forRoot(), DelonACLModule.forRoot(), DelonMockModule.forRoot()];
const alainProvides = [{ provide: ALAIN_CONFIG, useValue: alainConfig }];

import { environment } from '@env/environment';
import * as MOCK_DATA from '../../_mock';
if (!environment.production) {
  alainConfig.mock = { data: MOCK_DATA };
}

/**
 * 若需要[路由复用](https://ng-alain.com/components/reuse-tab)需要：
 * 1、在 `shared-delon.module.ts` 导入 `ReuseTabModule` 模块
 * 2、注册 `RouteReuseStrategy`
 * 3、在 `src/app/layout/default/default.component.html` 修改：
 *  ```html
 *  <section class="alain-default__content">
 *    <reuse-tab #reuseTab></reuse-tab>
 *    <router-outlet (activate)="reuseTab.activate($event)"></router-outlet>
 *  </section>
 *  ```
 */
// import { RouteReuseStrategy } from '@angular/router';
// import { ReuseTabService, ReuseTabStrategy } from '@delon/abc/reuse-tab';
// alainProvides.push({
//   provide: RouteReuseStrategy,
//   useClass: ReuseTabStrategy,
//   deps: [ReuseTabService],
// } as any);

import { NzConfig, NZ_CONFIG } from 'ng-zorro-antd/core/config';

const ngZorroConfig: NzConfig = {};

const zorroProvides = [{ provide: NZ_CONFIG, useValue: ngZorroConfig }];

@NgModule({
  imports: [...alainModules]
})
export class GlobalConfigModule {
  constructor(@Optional() @SkipSelf() parentModule: GlobalConfigModule) {
    throwIfAlreadyLoaded(parentModule, 'GlobalConfigModule');
  }

  static forRoot(): ModuleWithProviders<GlobalConfigModule> {
    return {
      ngModule: GlobalConfigModule,
      providers: [...alainProvides, ...zorroProvides]
    };
  }
}
