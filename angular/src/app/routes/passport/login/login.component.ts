import { Component, Inject } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { StartupService } from '@core';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { SettingsService, _HttpClient } from '@delon/theme';
import { environment } from '@env/environment';
import { OAuthService } from 'angular-oauth2-oidc';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'passport-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less']
})
export class UserLoginComponent {
  constructor(
    fb: FormBuilder,
    private router: Router,
    private startupSrv: StartupService,
    @Inject(DA_SERVICE_TOKEN) private tokenService: ITokenService,
    private settingsService: SettingsService,
    private oauthService: OAuthService,
    public msgSrv: NzMessageService,
    public http: _HttpClient
  ) {
    this.form = fb.group({
      username: [null, [Validators.required]],
      password: [null, [Validators.required]],
      remember: [true]
    });
    this.oauthService.configure({
      issuer: environment.AUTH_SERVER,
      scope: environment.AURORA_HOST_WEB.SCOPE,
      clientId: environment.AURORA_HOST_WEB.CLIENT_ID,
      dummyClientSecret: environment.AURORA_HOST_WEB.CLIENT_SECRET
    });
  }

  get username(): AbstractControl {
    return this.form.controls.username;
  }
  get password(): AbstractControl {
    return this.form.controls.password;
  }

  error = '';
  form: FormGroup;

  async submit(): Promise<any> {
    this.error = '';
    this.username.markAsDirty();
    this.username.updateValueAndValidity();
    this.password.markAsDirty();
    this.password.updateValueAndValidity();
    if (this.username.invalid || this.password.invalid) return;

    try {
      if (!this.oauthService.discoveryDocumentLoaded) await this.oauthService.loadDiscoveryDocument();
      const tokenResponse = await this.oauthService.fetchTokenUsingPasswordFlow(this.username.value, this.password.value);
      this.tokenService.set({ token: tokenResponse.access_token, expired: tokenResponse.expires_in * 1000 });
      this.startupSrv.load().then(() => this.router.navigateByUrl('/'));
    } catch (e) {
      if (e.error) {
        this.error = e.error.error_description ? e.error.error_description : e.error.error;
      } else {
        this.error = e.message;
      }
    }
  }
}
