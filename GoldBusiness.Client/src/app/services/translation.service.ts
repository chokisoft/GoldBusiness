import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { LanguageService } from './language.service';

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

  private translations: Translations = {
    // Login Page
    'login.title': {
      'es': 'GoldBusiness ERP',
      'en': 'GoldBusiness ERP',
      'fr': 'GoldBusiness ERP'
    },
    'login.subtitle': {
      'es': 'Sistema de Gestión Empresarial',
      'en': 'Enterprise Management System',
      'fr': 'Système de Gestion d\'Entreprise'
    },
    'login.username': {
      'es': 'Usuario',
      'en': 'Username',
      'fr': 'Nom d\'utilisateur'
    },
    'login.password': {
      'es': 'Contraseña',
      'en': 'Password',
      'fr': 'Mot de passe'
    },
    'login.usernamePlaceholder': {
      'es': 'Ingrese su usuario',
      'en': 'Enter your username',
      'fr': 'Entrez votre nom d\'utilisateur'
    },
    'login.passwordPlaceholder': {
      'es': 'Ingrese su contraseña',
      'en': 'Enter your password',
      'fr': 'Entrez votre mot de passe'
    },
    'login.submit': {
      'es': '🚀 Iniciar Sesión',
      'en': '🚀 Sign In',
      'fr': '🚀 Se Connecter'
    },
    'login.loading': {
      'es': 'Iniciando sesión...',
      'en': 'Signing in...',
      'fr': 'Connexion en cours...'
    },
    'login.forgotPassword': {
      'es': '¿Olvidó su contraseña?',
      'en': 'Forgot your password?',
      'fr': 'Mot de passe oublié?'
    },
    'login.errorGeneral': {
      'es': 'Usuario o contraseña incorrectos. Por favor, intente nuevamente.',
      'en': 'Incorrect username or password. Please try again.',
      'fr': 'Nom d\'utilisateur ou mot de passe incorrect. Veuillez réessayer.'
    },
    'login.showPassword': {
      'es': 'Mostrar contraseña',
      'en': 'Show password',
      'fr': 'Afficher le mot de passe'
    },
    'login.hidePassword': {
      'es': 'Ocultar contraseña',
      'en': 'Hide password',
      'fr': 'Masquer le mot de passe'
    },
    'login.footer': {
      'es': '© 2026 Chokisoft - GoldBusiness ERP',
      'en': '© 2026 Chokisoft - GoldBusiness ERP',
      'fr': '© 2026 Chokisoft - GoldBusiness ERP'
    },

    // Validations
    'validation.required': {
      'es': 'Este campo es obligatorio',
      'en': 'This field is required',
      'fr': 'Ce champ est obligatoire'
    },
    'validation.usernameRequired': {
      'es': '⚠️ El usuario es obligatorio',
      'en': '⚠️ Username is required',
      'fr': '⚠️ Le nom d\'utilisateur est obligatoire'
    },
    'validation.usernameMinLength': {
      'es': '⚠️ El usuario debe tener al menos 3 caracteres',
      'en': '⚠️ Username must be at least 3 characters',
      'fr': '⚠️ Le nom d\'utilisateur doit comporter au moins 3 caractères'
    },
    'validation.passwordRequired': {
      'es': '⚠️ La contraseña es obligatoria',
      'en': '⚠️ Password is required',
      'fr': '⚠️ Le mot de passe est obligatoire'
    },
    'validation.passwordMinLength': {
      'es': '⚠️ La contraseña debe tener al menos 8 caracteres',
      'en': '⚠️ Password must be at least 8 characters',
      'fr': '⚠️ Le mot de passe doit comporter au moins 8 caractères'
    },

    // App Header
    'header.environment.development': {
      'es': '🟢 Desarrollo',
      'en': '🟢 Development',
      'fr': '🟢 Développement'
    },
    'header.environment.production': {
      'es': '🔴 Producción',
      'en': '🔴 Production',
      'fr': '🔴 Production'
    },
    'header.logout': {
      'es': '🚪 Cerrar Sesión',
      'en': '🚪 Sign Out',
      'fr': '🚪 Se Déconnecter'
    },
    'header.logoutConfirm': {
      'es': '¿Está seguro que desea cerrar sesión?',
      'en': 'Are you sure you want to sign out?',
      'fr': 'Êtes-vous sûr de vouloir vous déconnecter?'
    },

    // Language Selector
    'language.label': {
      'es': '🌍 Idioma:',
      'en': '🌍 Language:',
      'fr': '🌍 Langue:'
    },

    // Test Connection Component (← NUEVAS TRADUCCIONES)
    'test.title': {
      'es': '🧪 Prueba de Conexión con la API',
      'en': '🧪 API Connection Test',
      'fr': '🧪 Test de Connexion à l\'API'
    },
    'test.configTitle': {
      'es': '⚙️ Configuración',
      'en': '⚙️ Configuration',
      'fr': '⚙️ Configuration'
    },
    'test.connectionTitle': {
      'es': '🔌 Conexión',
      'en': '🔌 Connection',
      'fr': '🔌 Connexion'
    },
    'test.testButton': {
      'es': '🚀 Probar Conexión',
      'en': '🚀 Test Connection',
      'fr': '🚀 Tester la Connexion'
    },
    'test.testing': {
      'es': '⏳ Probando...',
      'en': '⏳ Testing...',
      'fr': '⏳ Test en cours...'
    },
    'test.successTitle': {
      'es': '✅ Conexión Exitosa',
      'en': '✅ Connection Successful',
      'fr': '✅ Connexion Réussie'
    },
    'test.errorLabel': {
      'es': '❌ Error',
      'en': '❌ Error',
      'fr': '❌ Erreur'
    }
  };

  constructor(private languageService: LanguageService) {
    // Emitir el idioma inicial inmediatamente
    const initialLang = this.languageService.getCurrentLanguage();
    this.translationsSubject.next(initialLang);
    console.log('📚 TranslationService inicializado con idioma:', initialLang);

    // Suscribirse a cambios de idioma
    this.languageService.currentLanguage$.subscribe((langCode) => {
      console.log('🔄 TranslationService: Idioma cambiado a', langCode);
      this.translationsSubject.next(langCode);
    });
  }

  /**
   * Obtener traducción por clave
   */
  translate(key: string): string {
    const currentLang = this.languageService.getCurrentLanguage();
    const translation = this.translations[key];

    if (!translation) {
      console.warn(`Translation key not found: ${key}`);
      return key;
    }

    return translation[currentLang] || translation['es'] || key;
  }

  /**
   * Obtener traducción por clave (alias corto)
   */
  t(key: string): string {
    return this.translate(key);
  }

  /**
   * Obtener múltiples traducciones
   */
  translateMultiple(keys: string[]): { [key: string]: string } {
    const result: { [key: string]: string } = {};
    keys.forEach(key => {
      result[key] = this.translate(key);
    });
    return result;
  }

  /**
   * Verificar si existe una traducción
   */
  hasTranslation(key: string): boolean {
    return !!this.translations[key];
  }
}
