import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { HttpModule, Http } from '@angular/http';
import { OAuthModule } from 'angular2-oauth2';

/* Components */
import { AppComponent } from './app.component';

/* Services */

import { OAuthService } from 'angular2-oauth2/oauth-service';
import { SettingsService } from './services/settings.service';
import { JasminService } from './services/jasmin.service';
import { LoginService } from './services/login.service';
import { CustomersComponent } from './components/customers/customers.component';
import { CustomerComponent } from './components/customer/customer.component';

@NgModule({
  declarations: [
    AppComponent,
    CustomersComponent,
    CustomerComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpModule
  ],
  providers: [OAuthService, SettingsService, JasminService, LoginService],
  bootstrap: [AppComponent]
})
export class AppModule { }
