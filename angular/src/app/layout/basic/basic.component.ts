import { Component } from '@angular/core';
import { LayoutDefaultOptions } from '@delon/theme/layout-default';

@Component({
  selector: 'layout-basic',
  template: `
    <layout-default [options]="options" [content]="contentTpl">
      <layout-default-header-item direction="right" hidden="mobile">
        <header-fullscreen layout-default-header-item-trigger></header-fullscreen>
      </layout-default-header-item>
      <layout-default-header-item direction="right">
        <header-i18n layout-default-header-item-trigger></header-i18n>
      </layout-default-header-item>
      <layout-default-header-item direction="right">
        <header-user></header-user>
      </layout-default-header-item>
      <ng-template #contentTpl>
        <router-outlet></router-outlet>
      </ng-template>
    </layout-default>
  `
})
export class LayoutBasicComponent {
  options: LayoutDefaultOptions = {
    logoExpanded: `./assets/logo-full.svg`,
    logoCollapsed: `./assets/logo.svg`
  };
}
