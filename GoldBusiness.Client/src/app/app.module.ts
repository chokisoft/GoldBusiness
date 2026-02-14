import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { provideHttpClient, withInterceptorsFromDi, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TestConnectionComponent } from './components/test-connection/test-connection.component';
import { LanguageSelectorComponent } from './components/language-selector/language-selector.component';
import { LoginComponent } from './components/login/login.component';

// Servicios
import { ApiService } from './services/api.service';
import { AuthService } from './services/auth.service';
import { LanguageService } from './services/language.service';
import { TranslationService } from './services/translation.service';

// Interceptor
import { LanguageInterceptor } from './interceptors/language.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    TestConnectionComponent,
    LanguageSelectorComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LanguageInterceptor,
      multi: true
    },
    ApiService,
    AuthService,
    LanguageService,
    TranslationService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
