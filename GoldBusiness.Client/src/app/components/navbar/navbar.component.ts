import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService, CurrentUser } from '../../services/auth.service';
import { TranslationService } from '../../services/translation.service';
import { Observable, Subscription } from 'rxjs';
import { SidebarService } from '../../services/sidebar.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit, OnDestroy {
  currentUser$: Observable<CurrentUser | null>;
  private languageSubscription?: Subscription;
  private sidebarSubscription?: Subscription;
  isCollapsed = false;

  translations = {
    title: '',
    subtitle: '',
    inicio: '',
    acerca: '',
    logout: '',
    logoutConfirm: ''
  };

  constructor(
    private authService: AuthService,
    private translationService: TranslationService,
    private sidebarService: SidebarService
  ) {
    this.currentUser$ = this.authService.currentUser$;
  }

  ngOnInit(): void {
    this.loadTranslations();
    this.languageSubscription = this.translationService.translations$.subscribe(() => this.loadTranslations());
    this.sidebarSubscription = this.sidebarService.collapsed$.subscribe(c => this.isCollapsed = c);
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
    this.sidebarSubscription?.unsubscribe();
  }

  private loadTranslations(): void {
    this.translations = {
      title: this.translationService.translate('login.title'),
      subtitle: this.translationService.translate('login.subtitle'),
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

  toggleMobileSidebar(): void {
    this.sidebarService.toggleMobile();
  }

  toggleDesktopSidebar(): void {
    this.sidebarService.toggle();
  }
}
