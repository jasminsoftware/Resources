import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { OAuthService } from 'angular2-oauth2/oauth-service';
import { SettingsService } from '../services/settings.service';

/**
 * The jasmin interaction service class.
 * 
 * This service provides the enviroment settings for the application.
 * Default values are development settings.
 * 
 * @export
 * @class JasminService
 */
@Injectable()
export class JasminService {

    /**
     * Creates an instance of SettingsService.
     * @param {OAuthService} oAuthService 
     * @param {SettingsService} settingsService 
     * 
     * @memberOf SettingsService
     */
    public constructor(private oAuthService: OAuthService, private settingsService: SettingsService) {
    }

    public getItems(calback): void {
        let uriParts: Array<string> = new Array<string>();
        uriParts[0] = this.settingsService.tenantKey;
        uriParts[1] = this.settingsService.organizationKey;
        uriParts[2] = "salesCore";
        uriParts[3] = "salesItems";

        var getItemsUrl = this.formatString(this.settingsService.jasminApiUri, uriParts);

        var xhttp = new XMLHttpRequest();
        xhttp.open("GET", getItemsUrl, true);
        xhttp.setRequestHeader("Content-type", "application/json");
        xhttp.setRequestHeader("Authorization", "Bearer " + this.oAuthService.getAccessToken());
        xhttp.onload = function (e) {
            if (xhttp.readyState === 4) {
                if (xhttp.status === 200) {
                    if (xhttp.responseText != null && xhttp.responseText != '') {
                        calback(JSON.parse(xhttp.responseText));
                    }
                } else {
                    console.error(xhttp.statusText);
                }
            }
        };
        xhttp.onerror = function (e) {
            console.error(xhttp.statusText);
        };
        xhttp.send(null);
    }

    private formatString(input: string, args: Array<string>) {
        for (var i = 0; i < args.length; i++) {
            var regEx = new RegExp("\\{" + i + "\\}", "gm");
            input = input.replace(regEx, args[i]);
        }

        return input;
    }
}