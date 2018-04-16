import { Component } from '@angular/core';
import { Http } from '@angular/http';
import { ItemsComponent } from './components/items/items.component';
import { ItemComponent } from './components/item/item.component';
import { SettingsService } from './services/settings.service';
import { JasminService } from './services/jasmin.service';
import { LoginService } from './services/login.service';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  private loggedIn: boolean = false;

  /**
     * Creates an instance of AppComponent. 
     * 
     * Inject the proxy service here to setup the communication layer 
     * between the web app and the hosting enviroment.  
     * 
     * @param {OAuthService} oAuthService 
     * @param {SettingsService} settingsService 
     * 
     * @memberOf AppComponent
     */
  constructor(private loginService: LoginService, private settingsService: SettingsService, private jasminService: JasminService) {
  }

  ngOnInit() {

    this.initSettingsService();

    this.initLoginService();
  }

  ngOnDestroy() {
  }

  private initSettingsService() {
    if (this.settingsService!=null) {
      this.settingsService.getSettings();
    }
  }

  private initLoginService() {
      this.loginService.init();

      if (!this.loginService.loggedIn()) {
        this.loginService.login();
        this.loggedIn = true;
      }
      else {
        this.loggedIn = true;
      }  
  }

  private login() {
    this.loginService.login();
    this.loggedIn = true;
  }

  private logout() {
    this.loginService.logout();
    this.loggedIn = false;
  }
}
