import { NgModule } from '@angular/core';
import { SharedModule } from '@shared';

import { PassportRoutingModule } from './passport-routing.module';

import { UserLoginComponent } from './login/login.component';

const COMPONENTS = [UserLoginComponent];

@NgModule({
  imports: [SharedModule, PassportRoutingModule],
  declarations: [...COMPONENTS]
})
export class PassportModule {}
