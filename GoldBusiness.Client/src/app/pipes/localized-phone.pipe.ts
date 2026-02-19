import { Pipe, PipeTransform } from '@angular/core';
import { LanguageService } from '../services/language.service';

@Pipe({
  name: 'localizedPhone',
  pure: false
})
export class LocalizedPhonePipe implements PipeTransform {
  constructor(private languageService: LanguageService) { }

  transform(value: string | null | undefined): string {
    if (!value) return '';

    // Limpiar el número (solo dígitos)
    const cleaned = value.replace(/\D/g, '');

    if (cleaned.length === 0) return value;

    const lang = this.languageService.getCurrentLanguage();

    switch (lang) {
      case 'en':
        return this.formatUSPhone(cleaned);

      case 'es':
        // Detectar si es Cuba o España
        return this.formatESPhone(cleaned);

      case 'fr':
        return this.formatFRPhone(cleaned);

      default:
        return value;
    }
  }

  private formatUSPhone(cleaned: string): string {
    // Formato EE.UU.: (XXX) XXX-XXXX
    if (cleaned.length === 10) {
      return `(${cleaned.slice(0, 3)}) ${cleaned.slice(3, 6)}-${cleaned.slice(6)}`;
    }
    // Con código de país: +1 (XXX) XXX-XXXX
    if (cleaned.length === 11 && cleaned.startsWith('1')) {
      return `+1 (${cleaned.slice(1, 4)}) ${cleaned.slice(4, 7)}-${cleaned.slice(7)}`;
    }
    return this.addSpaces(cleaned, 3);
  }

  private formatESPhone(cleaned: string): string {
    // Detectar si es Cuba (+53) o España (+34)

    // Cuba: +53 5XXX XXXX (8 dígitos móvil) o +53 XX XXXXXX (fijo)
    if (cleaned.startsWith('53') && cleaned.length === 10) {
      // Cuba con código de país
      if (cleaned[2] === '5') {
        // Móvil cubano: +53 5XXX XXXX
        return `+53 ${cleaned.slice(2, 6)} ${cleaned.slice(6)}`;
      } else {
        // Fijo cubano: +53 XX XXXXXX
        return `+53 ${cleaned.slice(2, 4)} ${cleaned.slice(4)}`;
      }
    }

    // Cuba sin código de país (8 dígitos)
    if (cleaned.length === 8) {
      if (cleaned.startsWith('5')) {
        // Móvil: 5XXX XXXX
        return `${cleaned.slice(0, 4)} ${cleaned.slice(4)}`;
      } else {
        // Fijo: XX XXXXXX
        return `${cleaned.slice(0, 2)} ${cleaned.slice(2)}`;
      }
    }

    // España: +34 XXX XXX XXX (9 dígitos)
    if (cleaned.startsWith('34') && cleaned.length === 11) {
      // España con código de país
      return `+34 ${cleaned.slice(2, 5)} ${cleaned.slice(5, 8)} ${cleaned.slice(8)}`;
    }

    // España sin código de país (9 dígitos)
    if (cleaned.length === 9) {
      return `${cleaned.slice(0, 3)} ${cleaned.slice(3, 6)} ${cleaned.slice(6)}`;
    }

    // Formato internacional genérico
    if (cleaned.length > 11) {
      return `+${cleaned.slice(0, 2)} ${cleaned.slice(2, 5)} ${cleaned.slice(5, 8)} ${cleaned.slice(8)}`;
    }

    // Fallback
    return this.addSpaces(cleaned, 3);
  }

  private formatFRPhone(cleaned: string): string {
    // Formato Francia: XX XX XX XX XX (10 dígitos)
    if (cleaned.length === 10) {
      return `${cleaned.slice(0, 2)} ${cleaned.slice(2, 4)} ${cleaned.slice(4, 6)} ${cleaned.slice(6, 8)} ${cleaned.slice(8)}`;
    }
    // Con código de país: +33 X XX XX XX XX
    if (cleaned.startsWith('33') && cleaned.length === 11) {
      return `+33 ${cleaned.slice(2, 3)} ${cleaned.slice(3, 5)} ${cleaned.slice(5, 7)} ${cleaned.slice(7, 9)} ${cleaned.slice(9)}`;
    }
    return this.addSpaces(cleaned, 2);
  }

  private addSpaces(cleaned: string, groupSize: number): string {
    return cleaned.match(new RegExp(`.{1,${groupSize}}`, 'g'))?.join(' ') || cleaned;
  }
}
