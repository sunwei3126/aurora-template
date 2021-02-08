import { NgModule } from '@angular/core';
import { STWidgetRegistry } from '@delon/abc/st';
import { SharedModule } from '@shared';

export const ST_WIDGET_COMPONENTS = [];

@NgModule({
  declarations: ST_WIDGET_COMPONENTS,
  imports: [SharedModule],
  exports: [...ST_WIDGET_COMPONENTS]
})
export class STWidgetModule {
  constructor(widgetRegistry: STWidgetRegistry) {}
}
