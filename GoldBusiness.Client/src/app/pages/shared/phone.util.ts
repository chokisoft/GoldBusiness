import { AbstractControl, ValidatorFn } from '@angular/forms';

/**
 * Shared phone utilities used by forms and pipes.
 * - normalizePhone: strip visual chars (+, space, (), -, .)
 * - phoneValidator: returns an Angular ValidatorFn that validates against a provided regex applied to the normalized value
 * - PHONE_MAX_LENGTH: consistent max length for phone inputs
 * - formatPhoneNumber / isValidPhoneNumber: kept for backward compatibility (presentation helpers)
 */

export const PHONE_MAX_LENGTH = 50;

export function normalizePhone(raw?: string | null): string {
  if (!raw) return '';
  const hasPlus = String(raw).trim().startsWith('+');
  const stripped = String(raw).replace(/[\s\-\(\)\.]/g, '').replace(/^\+/, '');
  return hasPlus ? `+${stripped}` : stripped;
}

// and keep phoneValidator using a different internal normalizer that removes '+'
function normalizePhoneForValidation(raw?: string | null): string {
  if (!raw) return '';
  return String(raw).replace(/[\s\-\(\)\.\+]/g, '');
}

export function phoneValidator(regexString?: string): ValidatorFn {
  return (c: AbstractControl) => {
    const v = c.value;
    if (!v) return null;
    if (!regexString) return null;
    try {
      const re = new RegExp(regexString);
      const normalized = normalizePhoneForValidation(v);
      return re.test(normalized) ? null : { telefonoInvalid: true };
    } catch {
      // If server provides an invalid regex, do not block user
      return null;
    }
  };
}

/**
 * Presentation helpers (existing behavior kept)
 */
export function formatPhoneNumber(phone: string): string {
  const cleaned = ('' + phone).replace(/\D/g, '');
  if (cleaned.length !== 10) {
    return phone;
  }
  const match = cleaned.match(/^(\d{3})(\d{3})(\d{4})$/);
  if (match) {
    return `(${match[1]}) ${match[2]}-${match[3]}`;
  }
  return phone;
}

export function isValidPhoneNumber(phone: string): boolean {
  const cleaned = ('' + phone).replace(/\D/g, '');
  return cleaned.length === 10;
}
