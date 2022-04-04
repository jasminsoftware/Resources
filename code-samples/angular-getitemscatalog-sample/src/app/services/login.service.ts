import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs/Observable';
import { OAuthService } from 'angular2-oauth2/oauth-service';
import { SettingsService } from '../services/settings.service';

import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/from';
import 'rxjs/add/observable/empty';

/**
 * The login service implementation.
 * 
 * This service provides the api that abstracts authentication and authorization.
 * 
 * @export
 * @class LoginService
 */
@Injectable()
export class LoginService {

    private authCode: string;

    constructor(private http: Http, private oAuthService: OAuthService, private settingsService: SettingsService) {
    }

    public init() {

        // load default configurations.
        let identityServerUrl = this.settingsService.identityServerUri;

        // oauth config
        this.oAuthService.loginUrl = identityServerUrl + "/connect/authorize";
        this.oAuthService.redirectUri = this.settingsService.identityRedirectUri;
        this.oAuthService.clientId = this.settingsService.identityServerClientId;
        this.oAuthService.scope = this.settingsService.identityServerScope;
        this.oAuthService.issuer = identityServerUrl;
        this.oAuthService.logoutUrl = identityServerUrl + "/connect/endsession?id_token_hint={{id_token}}&post_logout_redirect_uri=" + this.settingsService.identityLogoutUri;

        this.oAuthService.setStorage(sessionStorage);
        this.oAuthService.oidc = true;

        this.oAuthService.tryLogin({});
    }

    public loggedIn(): boolean {
        let hasIdToken = this.oAuthService.hasValidIdToken();
        let hasAccessToken = this.oAuthService.hasValidAccessToken();

        return (hasIdToken && hasAccessToken);
    }

    public login() {

        // validate access - are tokens valid?
        let hasIdToken = this.oAuthService.hasValidIdToken();
        let hasAccessToken = this.oAuthService.hasValidAccessToken();

        if (!hasIdToken || !hasAccessToken) {
            this.oAuthService.initImplicitFlow();
        }
    }

    public logout() {
        let hasIdToken = this.oAuthService.hasValidIdToken();
        let hasAccessToken = this.oAuthService.hasValidAccessToken();

        if (hasIdToken || hasAccessToken) {
            this.oAuthService.logOut();
        }
    }
}
