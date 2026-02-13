import { Component, OnInit } from '@angular/core';
import { LanguageService, Language } from '../../services/language.service';

@Component({
  selector: 'app-language-selector',
  templateUrl: './language-selector.component.html',
  styleUrls: ['./language-selector.component.css']
})
export class LanguageSelectorComponent implements OnInit {
  availableLanguages: Language[] = [];
  currentLanguage: Language;

  constructor(private languageService: LanguageService) {
    this.availableLanguages = this.languageService.availableLanguages;
    this.currentLanguage = this.languageService.getCurrentLanguageInfo();
  }

  ngOnInit(): void {
    // Suscribirse a cambios de idioma
    this.languageService.currentLanguage$.subscribe(langCode => {
      this.currentLanguage = this.languageService.getCurrentLanguageInfo();
    });
  }

  changeLanguage(languageCode: string): void {
    this.languageService.setLanguage(languageCode);
  }
}
