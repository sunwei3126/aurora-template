import { Component } from '@angular/core';

@Component({
  selector: 'exception-404',
  template: ` <exception type="404" style="min-height: 500px; height: 80%; user-select: none;" desc="抱歉，您访问的页面不存在！"></exception> `
})
export class Exception404Component {}
