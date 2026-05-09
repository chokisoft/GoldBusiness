import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

// Layout Components (Core - always loaded)
import { NavbarComponent } from './components/navbar/navbar.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { MainLayoutComponent } from './components/layout/main-layout.component';

// SharedModule (con pipes compartidos)
import { SharedModule } from './shared/shared.module';

// Core Services
import { ApiService } from './services/api.service';
import { AuthService } from './services/auth.service';
import { LanguageService } from './services/language.service';
import { TranslationService } from './services/translation.service';
import { SidebarService } from './services/sidebar.service';

// Interceptors
import { LanguageInterceptor } from './interceptors/language.interceptor';
import { AuthInterceptor } from './interceptors/auth.interceptor';

@NgModule({
  declarations: [
    // ============================================
    // 🏠 CORE COMPONENTS (Always loaded)
    // ============================================
    AppComponent,
    LoginComponent,
    DashboardComponent,
    
    // ============================================
    // 🎨 LAYOUT COMPONENTS
    // ============================================
    NavbarComponent,
    SidebarComponent,
    MainLayoutComponent,
  ],
  imports: [
    // ============================================
    // 📦 ANGULAR CORE MODULES
    // ============================================
    BrowserModule,
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    
    // ============================================
    // 🔧 SHARED MODULE (con pipes compartidos)
    // ============================================
    SharedModule,
    
    // ============================================
    // 🚀 APP ROUTING (with Lazy Loading)
    // ============================================
    AppRoutingModule,
  ],
  providers: [
    // ============================================
    // 🌐 HTTP CLIENT WITH INTERCEPTORS
    // ============================================
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
    
    // ============================================
    // 🔧 CORE SERVICES
    // ============================================
    ApiService,
    AuthService,
    LanguageService,
    TranslationService,
    SidebarService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
