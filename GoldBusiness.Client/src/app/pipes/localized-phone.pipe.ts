import { Pipe, PipeTransform } from '@angular/core';
import { LanguageService } from '../services/language.service';
import { PaisDTO } from '../services/pais.service';

@Pipe({
  name: 'localizedPhone',
  pure: false
})
export class LocalizedPhonePipe implements PipeTransform {
  constructor(private languageService: LanguageService) { }

  /**
   * transform(value, paisOrPattern?)
   * - value: teléfono en texto (puede contener + y otros separadores)
   * - paisOrPattern: puede ser un PaisDTO, o una cadena con el patrón (ej "+34 XXX XX XX XX")
   */
  transform(value: string | null | undefined, paisOrPattern?: PaisDTO | string): string {
    if (!value) return '';

    const cleaned = this.onlyDigits(value);
    if (!cleaned) return value;

    // Si nos pasan un PaisDTO, preferimos usar formatoTelefono o formatoEjemplo
    let pattern: string | undefined;
    if (paisOrPattern) {
      if (typeof paisOrPattern === 'string') {
        pattern = paisOrPattern;
      } else {
        pattern = paisOrPattern.formatoTelefono || paisOrPattern.formatoEjemplo || undefined;
      }
    }

    if (pattern) {
      const formatted = this.formatByPattern(cleaned, pattern);
      if (formatted) return formatted;
      // si falla el formateo por patrón, caemos al formateo por idioma
    }

    // Fallback: comportamiento anterior basado en idioma
    const lang = this.languageService.getCurrentLanguage();
    switch (lang) {
      case 'en':
        return this.formatUSPhone(cleaned);
      case 'es':
        return this.formatESPhone(cleaned);
      case 'fr':
        return this.formatFRPhone(cleaned);
      default:
        return value;
    }
  }

  private onlyDigits(input: string): string {
    return (input || '').replace(/\D/g, '');
  }

  /**
   * Format number using a pattern string.
   * Pattern example: "+34 XXX XX XX XX" or "+1 (XXX) XXX-XXXX"
   *
   * Rules:
   * - 'X' (o 'x') are placeholders to be replaced by digits from cleaned number.
   * - Any other characters are copied as-is.
   * - If pattern contains a numeric prefix (digits before first X), and cleaned starts with same prefix, that prefix is removed from cleaned before filling placeholders (avoids duplicating country code).
   * - If digits remain after filling all placeholders, they are appended (separated by a space).
   * - If digits run out before filling placeholders, placeholders remain unfilled (we stop there).
   */
  private formatByPattern(cleaned: string, pattern: string): string | null {
    if (!pattern) return null;

    // Extract prefix digits from pattern (digits before first X)
    const firstXIdx = pattern.search(/[Xx]/);
    let prefixDigits = '';
    if (firstXIdx > -1) {
      const prefixPart = pattern.slice(0, firstXIdx);
      prefixDigits = prefixPart.replace(/\D/g, '');
    } else {
      // No placeholders - nothing to format
      return null;
    }

    let digits = cleaned;

    // If cleaned starts with prefixDigits, strip it to avoid duplication
    if (prefixDigits && digits.startsWith(prefixDigits)) {
      digits = digits.slice(prefixDigits.length);
    }

    let result = '';
    let digitIndex = 0;

    for (let i = 0; i < pattern.length; i++) {
      const ch = pattern[i];
      if (ch === 'X' || ch === 'x') {
        if (digitIndex < digits.length) {
          result += digits[digitIndex++];
        } else {
          // No more digits: stop inserting placeholders (trim trailing pattern that are placeholders)
          // We'll return partial formatted string (reasonable fallback)
          break;
        }
      } else {
        result += ch;
      }
    }

    // If there are remaining digits, append them separated by space
    if (digitIndex < digits.length) {
      const rest = digits.slice(digitIndex);
      // Append with a space if last char is not whitespace or punctuation
      if (result && !/[ \-\)\(]$/.test(result)) result += ' ';
      result += rest;
    }

    // Clean up possible duplicated spaces
    result = result.replace(/\s+/g, ' ').trim();

    return result || null;
  }

  private formatUSPhone(cleaned: string): string {
    if (cleaned.length === 10) {
      return `(${cleaned.slice(0, 3)}) ${cleaned.slice(3, 6)}-${cleaned.slice(6)}`;
    }
    if (cleaned.length === 11 && cleaned.startsWith('1')) {
      return `+1 (${cleaned.slice(1, 4)}) ${cleaned.slice(4, 7)}-${cleaned.slice(7)}`;
    }
    return this.addSpaces(cleaned, 3);
  }

  private formatESPhone(cleaned: string): string {
    // Cuba +53 and Spain +34 detection and some heuristics
    // Cuba with country code: 53 + 8 or 10 length patterns considered
    if (cleaned.startsWith('53') && (cleaned.length === 10 || cleaned.length === 8)) {
      if (cleaned.length === 10 && cleaned[2] === '5') {
        return `+53 ${cleaned.slice(2, 6)} ${cleaned.slice(6)}`;
      }
      if (cleaned.length === 8) {
        if (cleaned.startsWith('5')) return `${cleaned.slice(0, 4)} ${cleaned.slice(4)}`;
        return `${cleaned.slice(0, 2)} ${cleaned.slice(2)}`;
      }
    }

    // Spain
    if (cleaned.startsWith('34') && cleaned.length >= 11) {
      // +34 ddd ddd ddd
      return `+34 ${cleaned.slice(2, 5)} ${cleaned.slice(5, 8)} ${cleaned.slice(8)}`;
    }
    if (cleaned.length === 9) {
      return `${cleaned.slice(0, 3)} ${cleaned.slice(3, 6)} ${cleaned.slice(6)}`;
    }

    if (cleaned.length > 11) {
      return `+${cleaned.slice(0, 2)} ${cleaned.slice(2, 5)} ${cleaned.slice(5, 8)} ${cleaned.slice(8)}`;
    }

    return this.addSpaces(cleaned, 3);
  }

  private formatFRPhone(cleaned: string): string {
    if (cleaned.length === 10) {
      return `${cleaned.slice(0, 2)} ${cleaned.slice(2, 4)} ${cleaned.slice(4, 6)} ${cleaned.slice(6, 8)} ${cleaned.slice(8)}`;
    }
    if (cleaned.startsWith('33') && cleaned.length === 11) {
      return `+33 ${cleaned.slice(2, 3)} ${cleaned.slice(3, 5)} ${cleaned.slice(5, 7)} ${cleaned.slice(7, 9)} ${cleaned.slice(9)}`;
    }
    return this.addSpaces(cleaned, 2);
  }

  private addSpaces(cleaned: string, groupSize: number): string {
    return cleaned.match(new RegExp(`.{1,${groupSize}}`, 'g'))?.join(' ') || cleaned;
  }
}
