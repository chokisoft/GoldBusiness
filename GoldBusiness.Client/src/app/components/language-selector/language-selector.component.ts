import { Component, OnInit, OnDestroy, Input } from '@angular/core';
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
  selectedLanguageCode: string = 'es';
  label: string = '';
  
  // Permite que el componente padre controle si debe ser compacto
  @Input() forceCompact: boolean = false;
  
  compact: boolean = false;

  private languageSubscription?: Subscription;

  constructor(
    private languageService: LanguageService,
    private translationService: TranslationService
  ) {
    this.availableLanguages = this.languageService.availableLanguages;
    this.selectedLanguageCode = this.languageService.getCurrentLanguage();
    console.log('🎨 LanguageSelector inicializado con idioma:', this.selectedLanguageCode);
  }

  ngOnInit(): void {
    this.loadLabel();

    // Determinar modo compacto basado en forceCompact o tamaño de pantalla
    this.updateCompactMode();

    this.languageSubscription = this.languageService.currentLanguage$.subscribe(langCode => {
      console.log('🔄 LanguageSelector: Idioma cambiado a', langCode);
      this.selectedLanguageCode = langCode;
      this.loadLabel();
    });

    window.addEventListener('resize', this.onResize);
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
    window.removeEventListener('resize', this.onResize);
    console.log('🧹 LanguageSelector destruido');
  }

  private onResize = () => {
    this.updateCompactMode();
  }

  private updateCompactMode(): void {
    // Si forceCompact es true, usar compacto en móvil
    // Si forceCompact es false, nunca usar compacto (siempre selector normal)
    if (this.forceCompact) {
      this.compact = window.innerWidth <= 768;
    } else {
      this.compact = false; // Siempre normal
    }
  }

  private loadLabel(): void {
    this.label = this.translationService.translate('language.label');
    console.log('📝 Label actualizado:', this.label);
  }

  changeLanguage(languageCode: string): void {
    console.log('🌍 Usuario cambió idioma a:', languageCode);
    this.languageService.setLanguage(languageCode);
  }

  getCurrentLanguageFlag(): string {
    const lang = this.availableLanguages.find(l => l.code === this.selectedLanguageCode);
    const flag = lang?.flag || '🌐';
    console.log('🚩 getCurrentLanguageFlag - code:', this.selectedLanguageCode, 'flag:', flag);
    return flag;
  }
}
