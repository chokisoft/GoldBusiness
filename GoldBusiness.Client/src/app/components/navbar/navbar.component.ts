import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService, CurrentUser } from '../../services/auth.service';
import { TranslationService } from '../../services/translation.service';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit, OnDestroy {
  currentUser$: Observable<CurrentUser | null>;
  private languageSubscription?: Subscription;

  // Traducciones dinámicas
  translations = {
    inicio: '',
    acerca: '',
    logout: '',
    logoutConfirm: ''
  };

  constructor(
    private authService: AuthService,
    private translationService: TranslationService
  ) {
    this.currentUser$ = this.authService.currentUser$;
  }

  ngOnInit(): void {
    // Cargar traducciones iniciales
    this.loadTranslations();

    // Suscribirse a cambios de idioma
    this.languageSubscription = this.translationService.translations$.subscribe(() => {
      this.loadTranslations();
    });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  private loadTranslations(): void {
    this.translations = {
      inicio: this.translationService.translate('header.inicio'),
      acerca: this.translationService.translate('header.acerca'),
      logout: this.translationService.translate('header.logout'),
      logoutConfirm: this.translationService.translate('header.logoutConfirm')
    };
  }

  logout(): void {
    if (confirm(this.translations.logoutConfirm)) {
      this.authService.logout();
    }
  }
}
