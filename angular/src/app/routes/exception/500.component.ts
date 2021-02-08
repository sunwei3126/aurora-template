import { Component } from '@angular/core';

@Component({
  selector: 'exception-500',
  template: ` <exception type="500" style="min-height: 500px; height: 80%; user-select: none;" desc="抱歉，服务器出错了！"></exception> `
})
export class Exception500Component {}
