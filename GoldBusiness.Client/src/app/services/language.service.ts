import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export interface Language {
  code: string;
  name: string;
  flag: string;
}

@Injectable({
  providedIn: 'root'
})
export class LanguageService {
  private readonly STORAGE_KEY = 'selectedLanguage';

  // Idiomas soportados (deben coincidir con tu API)
  public readonly availableLanguages: Language[] = [
    { code: 'es', name: 'Español', flag: '🇪🇸 -' },
    { code: 'en', name: 'English', flag: '🇺🇸 -' },
    { code: 'fr', name: 'Français', flag: '🇫🇷 -' }
  ];

  // BehaviorSubject para el idioma actual
  private currentLanguageSubject: BehaviorSubject<string>;
  public currentLanguage$: Observable<string>;

  constructor() {
    // Cargar idioma guardado o usar español por defecto
    const savedLanguage = localStorage.getItem(this.STORAGE_KEY) || 'es';
    this.currentLanguageSubject = new BehaviorSubject<string>(savedLanguage);
    this.currentLanguage$ = this.currentLanguageSubject.asObservable();
  }

  /**
   * Obtener el idioma actual
   */
  getCurrentLanguage(): string {
    return this.currentLanguageSubject.value;
  }

  /**
   * Cambiar el idioma
   */
  setLanguage(languageCode: string): void {
    if (this.availableLanguages.some(lang => lang.code === languageCode)) {
      localStorage.setItem(this.STORAGE_KEY, languageCode);
      this.currentLanguageSubject.next(languageCode);
      console.log('✅ Idioma cambiado a:', languageCode);
    } else {
      console.error('❌ Idioma no soportado:', languageCode);
    }
  }

  /**
   * Obtener información del idioma actual
   */
  getCurrentLanguageInfo(): Language {
    const code = this.getCurrentLanguage();
    return this.availableLanguages.find(lang => lang.code === code)
      || this.availableLanguages[0];
  }
}
