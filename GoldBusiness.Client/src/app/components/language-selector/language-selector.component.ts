import { Component, OnInit, OnDestroy } from '@angular/core';
import { LanguageService, Language } from '../../services/language.service';
import { TranslationService } from '../../services/translation.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-language-selector',
  templateUrl: './language-selector.component.html',
  styleUrls: ['./language-selector.component.css']
})
export class LanguageSelectorComponent implements OnInit, OnDestroy {
  availableLanguages: Language[] = [];
  selectedLanguageCode: string = 'es'; // ← NUEVA propiedad para ngModel
  label: string = '';

  private languageSubscription?: Subscription;

  constructor(
    private languageService: LanguageService,
    private translate: TranslationService
  ) {
    this.availableLanguages = this.languageService.availableLanguages;
    this.selectedLanguageCode = this.languageService.getCurrentLanguage(); // ← Usar código directamente
    console.log('🎨 LanguageSelector inicializado con idioma:', this.selectedLanguageCode);
  }

  ngOnInit(): void {
    // Cargar label inicial
    this.loadLabel();

    // Suscribirse a cambios de idioma
    this.languageSubscription = this.languageService.currentLanguage$.subscribe(langCode => {
      console.log('🔄 LanguageSelector: Idioma cambiado a', langCode);
      this.selectedLanguageCode = langCode; // ← Actualizar el código seleccionado
      this.loadLabel();
    });
  }

  ngOnDestroy(): void {
    // Cancelar suscripción para evitar memory leaks
    this.languageSubscription?.unsubscribe();
    console.log('🧹 LanguageSelector destruido');
  }

  /**
   * Cargar el label traducido
   */
  private loadLabel(): void {
    this.label = this.translate.t('language.label');
    console.log('📝 Label actualizado:', this.label);
  }

  /**
   * Cambiar el idioma seleccionado
   */
  changeLanguage(languageCode: string): void {
    console.log('🌍 Usuario cambió idioma a:', languageCode);
    this.languageService.setLanguage(languageCode);
  }
}
