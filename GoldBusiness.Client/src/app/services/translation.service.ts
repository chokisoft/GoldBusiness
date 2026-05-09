import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { LanguageService } from './language.service';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';

/**
 * Interfaz para las traducciones multiidioma.
 */
interface Translations {
  [key: string]: {
    [lang: string]: string;
  };
}

@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  private translationsSubject = new BehaviorSubject<string>('');
  public translations$ = this.translationsSubject.asObservable();

  // Contenedor que se poblara cargando los JSON desde assets/i18n
  private translations: Translations = {};

  constructor(private languageService: LanguageService, private http: HttpClient) {
    const initialLang = this.languageService.getCurrentLanguage();
    if (initialLang) {
      this.loadLanguage(initialLang);
    }

    this.languageService.currentLanguage$.subscribe(lang => {
      if (lang) {
        this.loadLanguage(lang);
      }
    });
  }

  private loadLanguage(lang: string): void {
    const path = `assets/i18n/${lang}.json`;
    this.http.get<Record<string, string>>(path).pipe(
      catchError(() => of(null))
    ).subscribe(json => {
      if (!json) {
        // Si no hay JSON, no sobreescribir el contenedor (evita duplicidad)
        console.warn(`No se encontraron traducciones en: ${path}`);
        this.translationsSubject.next(Date.now().toString());
        return;
      }

      Object.keys(json).forEach(key => {
        if (!this.translations[key]) {
          this.translations[key] = {};
        }
        this.translations[key][lang] = json[key];
      });

      this.translationsSubject.next(Date.now().toString());
    });
  }

  translate(key: string, params: any[] = []): string {
    const currentLang = this.languageService.getCurrentLanguage();
    const translation = this.translations[key]?.[currentLang] ?? key;

    // Inyectar año por defecto para login.footer si no hay params
    if (key === 'login.footer' && (!params || params.length === 0)) {
      params = [new Date().getFullYear()];
    }

    if (params && params.length > 0) {
      return String(translation).replace(/{(\d+)}/g, (match, index) => {
        const i = parseInt(index, 10);
        return params[i] !== undefined ? String(params[i]) : match;
      });
    }

    return translation;
  }

  getTranslationsFor(prefix: string): { [key: string]: string } {
    const currentLang = this.languageService.getCurrentLanguage();
    const result: { [key: string]: string } = {};

    Object.keys(this.translations).forEach(key => {
      if (key.startsWith(prefix)) {
        const shortKey = key.replace(`${prefix}.`, '');
        result[shortKey] = this.translations[key][currentLang] ?? key;
      }
    });

    return result;
  }
}
