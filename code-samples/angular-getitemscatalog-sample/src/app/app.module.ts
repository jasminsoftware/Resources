import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule, Http } from '@angular/http';
import { OAuthModule } from 'angular2-oauth2';

import { AppComponent } from './app.component';
import { ItemsComponent } from './components/items/items.component';
import { ItemComponent } from './components/item/item.component';

import { OAuthService } from 'angular2-oauth2/oauth-service';
import { SettingsService } from './services/settings.service';
import { JasminService } from './services/jasmin.service';
import { LoginService } from './services/login.service';

@NgModule({
  declarations: [
    AppComponent,
    ItemsComponent,
    ItemComponent
  ],
  imports: [
    BrowserModule,
    HttpModule
  ],
  providers: [OAuthService, SettingsService, JasminService, LoginService],
  bootstrap: [AppComponent]
})
export class AppModule { }

