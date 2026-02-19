import { Pipe, PipeTransform } from '@angular/core';
import { LanguageService } from '../services/language.service';

@Pipe({
  name: 'localizedDate',
  pure: false
})
export class LocalizedDatePipe implements PipeTransform {
  constructor(private languageService: LanguageService) { }

  transform(value: Date | string, format: 'short' | 'medium' | 'long' = 'short'): string {
    if (!value) return '';

    const date = new Date(value);
    const lang = this.languageService.getCurrentLanguage();

    // Mapeo de idiomas a locales específicos
    const localeMap: { [key: string]: string } = {
      'es': 'es-ES',
      'en': 'en-US',
      'fr': 'fr-FR'
    };

    const locale = localeMap[lang] || 'es-ES';

    // Configurar opciones según el formato y el idioma
    let options: Intl.DateTimeFormatOptions;

    switch (format) {
      case 'short':
        // Solo fecha en formato corto
        if (lang === 'en') {
          // EE.UU.: MM/DD/YYYY
          options = {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit'
          };
        } else {
          // ES y FR: DD/MM/YYYY
          options = {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit'
          };
        }
        break;

      case 'medium':
        // Fecha con nombre de mes corto
        options = {
          year: 'numeric',
          month: 'short',
          day: 'numeric'
        };
        break;

      case 'long':
        // Fecha completa con nombre de mes largo
        options = {
          year: 'numeric',
          month: 'long',
          day: 'numeric'
        };
        break;
    }

    // Formatear la fecha
    const formatter = new Intl.DateTimeFormat(locale, options);
    let formattedDate = formatter.format(date);

    // Para formato short en inglés, asegurar orden MM/DD/YYYY
    if (format === 'short' && lang === 'en') {
      const parts = formatter.formatToParts(date);
      const month = parts.find(p => p.type === 'month')?.value || '';
      const day = parts.find(p => p.type === 'day')?.value || '';
      const year = parts.find(p => p.type === 'year')?.value || '';
      formattedDate = `${month}/${day}/${year}`;
    }

    // Para formato short en español y francés, asegurar orden DD/MM/YYYY
    if (format === 'short' && (lang === 'es' || lang === 'fr')) {
      const parts = formatter.formatToParts(date);
      const day = parts.find(p => p.type === 'day')?.value || '';
      const month = parts.find(p => p.type === 'month')?.value || '';
      const year = parts.find(p => p.type === 'year')?.value || '';
      formattedDate = `${day}/${month}/${year}`;
    }

    return formattedDate;
  }
}
