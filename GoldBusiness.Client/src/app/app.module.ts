import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TestConnectionComponent } from './components/test-connection/test-connection.component';
import { LanguageSelectorComponent } from './components/language-selector/language-selector.component';

// Servicios
import { ApiService } from './services/api.service';
import { AuthService } from './services/auth.service';
import { LanguageService } from './services/language.service';

@NgModule({
  declarations: [
    AppComponent,
    TestConnectionComponent,
    LanguageSelectorComponent // ← Agregar
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    ApiService,
    AuthService,
    LanguageService // ← Agregar
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
