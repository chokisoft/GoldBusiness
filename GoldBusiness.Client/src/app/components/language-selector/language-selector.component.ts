import { Component, Input, OnInit } from '@angular/core';
import { LanguageService, Language } from '../../services/language.service';

@Component({
  selector: 'app-language-selector',
  templateUrl: './language-selector.component.html',
  styleUrls: ['./language-selector.component.css']
})
export class LanguageSelectorComponent implements OnInit {
  @Input() label: string = 'Seleccionar idioma';
  @Input() forceCompact: boolean = false;

  selectedLanguageCode: string = 'es';
  availableLanguages: Language[] = []; // ✅ Inicializar como array vacío
  compact: boolean = false;

  constructor(private languageService: LanguageService) {}

  ngOnInit(): void {
    // ✅ Asignar los idiomas disponibles DESPUÉS de que el servicio esté inyectado
    this.availableLanguages = this.languageService.availableLanguages;
    this.selectedLanguageCode = this.languageService.getCurrentLanguage();
    this.compact = this.forceCompact;

    // Suscribirse a cambios de idioma
    this.languageService.currentLanguage$.subscribe(lang => {
      this.selectedLanguageCode = lang;
    });
  }

  changeLanguage(languageCode: string): void {
    this.languageService.setLanguage(languageCode);
  }

  getCurrentLanguageFlag(): string {
    const currentLang = this.availableLanguages.find(
      lang => lang.code === this.selectedLanguageCode
    );
    return currentLang?.flag || 'assets/flags/es.svg';
  }
}
