import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/from';
import 'rxjs/add/observable/empty';

/**
 * The settings service class.
 * 
 * This service provides the enviroment settings for the application.
 * Default values are development settings.
 * 
 * @export
 * @class SettingsService
 */
@Injectable()
export class SettingsService {

    public appBaseUri: string = "<Preencher com endereço onde está publicado este cliente ex: http://myhostservice:4300>";
    public tenantKey: string = "<Preencher com o campo 'Conta' (obtido na altura da criação da subscrição) ex: 10001>";
    public organizationKey: string = "<Preencher com o campo 'alias' (obtido na altura da criação da subscrição) ex: 10001-0350>";
    public jasminApiUri: string = "http://my.jasminsoftware.com/api/{0}/{1}/{2}/{3}";
    public identityServerClientId: string = "<Preencher com o campo 'Nome da Aplicação' (obtido na altura da criação da subscrição) ex: 10001>";
    public identityServerScope: string = "openid application";
    public identityRedirectUri: string = "<Preencher com endereço onde está publicado este cliente ex: http://myhostservice:4300>";
    public identityServerUri: string = "https://identity.primaverabss.com";
    public identityLogoutUri: string = "https://identity.primaverabss.com";

    /**
     * The product name that we're migrating.
     * 
     * @type {string}
     * @memberOf SettingsService
     */
    public productName: string = 'JASMIN CUSTOMERS';

    /**
     * The server query response observale.
     * 
     * @private
     * @type {Observable<any>}
     * @memberof SettingsService
     */
    private serverQueryResponseObservale: Observable<any>;

    /**
     * Creates an instance of SettingsService.
     * @param {Http} http 
     * 
     * @memberOf SettingsService
     */
    public constructor(private http: Http) { }

    public getSettings(): Observable<any> {

        // TODO: load settings from config file
        return null;
    }
}