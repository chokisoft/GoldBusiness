import { Component, OnInit, OnDestroy } from '@angular/core';
import { environment } from '../environments/environment.development';
import { AuthService, CurrentUser } from './services/auth.service';
import { TranslationService } from './services/translation.service';
import { LanguageService } from './services/language.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'GoldBusiness ERP';

  isProduction = environment.production;
  apiUrl = environment.apiUrl;
  apiVersion = environment.apiVersion;
  currentYear = new Date().getFullYear();

  currentUser: CurrentUser | null = null;
  private languageSubscription?: Subscription;
  private userSubscription?: Subscription;

  // Traducciones
  translations = {
    environmentBadge: '',
    logout: '',
    footer: ''
  };

  constructor(
    public authService: AuthService,
    public translate: TranslationService,
    private languageService: LanguageService
  ) {
    // Forzar la inicialización del idioma al cargar la app
    const currentLang = this.languageService.getCurrentLanguage();
    console.log('🌍 AppComponent: Aplicación iniciada con idioma:', currentLang);
  }

  ngOnInit(): void {
    console.log('🌍 Entorno:', this.isProduction ? 'Producción' : 'Desarrollo');
    console.log('🔗 API URL:', this.apiUrl);

    // Suscribirse al usuario actual
    this.userSubscription = this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
      console.log('👤 Usuario actual:', user);
    });

    // Cargar traducciones iniciales
    this.loadTranslations();

    // ✅ CORREGIDO: Suscribirse directamente a languageService en lugar de translate
    this.languageSubscription = this.languageService.currentLanguage$.subscribe(langCode => {
      console.log('🔄 AppComponent: Idioma cambiado a', langCode);
      this.loadTranslations();
    });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
    this.userSubscription?.unsubscribe();
    console.log('🧹 AppComponent destruido');
  }

  private loadTranslations(): void {
    const envKey = this.isProduction ? 'production' : 'development';
    this.translations = {
      environmentBadge: this.translate.t(`header.environment.${envKey}`),
      logout: this.translate.t('header.logout'),
      footer: this.translate.t('login.footer')
    };
    console.log('📝 AppComponent: Traducciones cargadas para idioma:', this.languageService.getCurrentLanguage());
  }

  logout(): void {
    const confirmMessage = this.translate.t('header.logoutConfirm');
    if (confirm(confirmMessage)) {
      this.authService.logout();
    }
  }
}
