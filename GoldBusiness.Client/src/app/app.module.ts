import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { provideHttpClient, withInterceptorsFromDi, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TestConnectionComponent } from './components/test-connection/test-connection.component';
import { LanguageSelectorComponent } from './components/language-selector/language-selector.component';
import { LoginComponent } from './components/login/login.component';

// Layout
import { NavbarComponent } from './components/navbar/navbar.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { MainLayoutComponent } from './components/layout/main-layout.component';

// GrupoCuenta
import { GrupoCuentaListComponent } from './pages/grupo-cuenta/grupo-cuenta-list/grupo-cuenta-list.component';
import { GrupoCuentaFormComponent } from './pages/grupo-cuenta/grupo-cuenta-form/grupo-cuenta-form.component';

// SubGrupoCuenta
import { SubGrupoCuentaListComponent } from './pages/subgrupo-cuenta/subgrupo-cuenta-list/subgrupo-cuenta-list.component';
import { SubGrupoCuentaFormComponent } from './pages/subgrupo-cuenta/subgrupo-cuenta-form/subgrupo-cuenta-form.component';

// Cuenta
import { CuentaListComponent } from './pages/cuenta/cuenta-list/cuenta-list.component';
import { CuentaFormComponent } from './pages/cuenta/cuenta-form/cuenta-form.component';

// Servicios
import { ApiService } from './services/api.service';
import { AuthService } from './services/auth.service';
import { LanguageService } from './services/language.service';
import { TranslationService } from './services/translation.service';
import { GrupoCuentaService } from './services/grupo-cuenta.service';
import { SubGrupoCuentaService } from './services/subgrupo-cuenta.service';
import { CuentaService } from './services/cuenta.service';

// Interceptors
import { LanguageInterceptor } from './interceptors/language.interceptor';
import { AuthInterceptor } from './interceptors/auth.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    TestConnectionComponent,
    LanguageSelectorComponent,
    LoginComponent,
    NavbarComponent,
    SidebarComponent,
    MainLayoutComponent,
    GrupoCuentaListComponent,
    GrupoCuentaFormComponent,
    SubGrupoCuentaListComponent,
    SubGrupoCuentaFormComponent,
    CuentaListComponent,
    CuentaFormComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LanguageInterceptor,
      multi: true
    },
    ApiService,
    AuthService,
    LanguageService,
    TranslationService,
    GrupoCuentaService,
    SubGrupoCuentaService,
    CuentaService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
