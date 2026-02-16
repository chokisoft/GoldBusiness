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
    // ═══════════════════════════════════════════════════════════
    // 🔐 LOGIN
    // ═══════════════════════════════════════════════════════════
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
    'login.errorGeneral': {
      'es': 'Usuario o contraseña incorrectos. Por favor, intente nuevamente.',
      'en': 'Incorrect username or password. Please try again.',
      'fr': 'Nom d\'utilisateur ou mot de passe incorrect. Veuillez réessayer.'
    },
    'login.footer': {
      'es': '© 2026 Chokisoft - GoldBusiness ERP',
      'en': '© 2026 Chokisoft - GoldBusiness ERP',
      'fr': '© 2026 Chokisoft - GoldBusiness ERP'
    },

    // ═══════════════════════════════════════════════════════════
    // 🧪 TEST CONNECTION
    // ═══════════════════════════════════════════════════════════
    'test.title': {
      'es': 'Prueba de Conexión',
      'en': 'Connection Test',
      'fr': 'Test de Connexion'
    },
    'test.configTitle': {
      'es': 'Configuración',
      'en': 'Configuration',
      'fr': 'Configuration'
    },
    'test.connectionTitle': {
      'es': 'Conexión con el Backend',
      'en': 'Backend Connection',
      'fr': 'Connexion Backend'
    },
    'test.testButton': {
      'es': 'Probar Conexión',
      'en': 'Test Connection',
      'fr': 'Tester Connexion'
    },
    'test.testing': {
      'es': 'Probando...',
      'en': 'Testing...',
      'fr': 'Test en cours...'
    },
    'test.successTitle': {
      'es': 'Respuesta del Servidor',
      'en': 'Server Response',
      'fr': 'Réponse du Serveur'
    },
    'test.errorLabel': {
      'es': 'Error',
      'en': 'Error',
      'fr': 'Erreur'
    },

    // ═══════════════════════════════════════════════════════════
    // 🌍 IDIOMA / LANGUAGE
    // ═══════════════════════════════════════════════════════════
    'language.label': {
      'es': 'Idioma',
      'en': 'Language',
      'fr': 'Langue'
    },

    // ═══════════════════════════════════════════════════════════
    // 📌 HEADER / NAVBAR
    // ═══════════════════════════════════════════════════════════
    'header.environment.production': {
      'es': 'Producción',
      'en': 'Production',
      'fr': 'Production'
    },
    'header.environment.development': {
      'es': 'Desarrollo',
      'en': 'Development',
      'fr': 'Développement'
    },
    'header.inicio': {
      'es': 'Inicio',
      'en': 'Home',
      'fr': 'Accueil'
    },
    'header.acerca': {
      'es': 'Acerca de',
      'en': 'About',
      'fr': 'À propos'
    },
    'header.logout': {
      'es': 'Cerrar Sesión',
      'en': 'Logout',
      'fr': 'Déconnexion'
    },
    'header.logoutConfirm': {
      'es': '¿Está seguro que desea cerrar sesión?',
      'en': 'Are you sure you want to logout?',
      'fr': 'Êtes-vous sûr de vouloir vous déconnecter?'
    },
    // ═══════════════════════════════════════════════════════════
    // 🗂️ SIDEBAR
    // ═══════════════════════════════════════════════════════════
    'sidebar.nomencladores': {
      'es': 'Nomencladores',
      'en': 'Nomenclators',
      'fr': 'Nomenclateurs'
    },
    'sidebar.planCuentas': {
      'es': 'Plan de Cuentas',
      'en': 'Chart of Accounts',
      'fr': 'Plan Comptable'
    },
    'sidebar.gestionFinanciera': {
      'es': 'Gestión Financiera',
      'en': 'Financial Management',
      'fr': 'Gestion Financière'
    },
    'sidebar.ubicaciones': {
      'es': 'Ubicaciones',
      'en': 'Locations',
      'fr': 'Emplacements'
    },
    'sidebar.terceros': {
      'es': 'Terceros',
      'en': 'Third Parties',
      'fr': 'Tiers'
    },
    'sidebar.productos': {
      'es': 'Productos',
      'en': 'Products',
      'fr': 'Produits'
    },
    'sidebar.configuracion': {
      'es': 'Configuración',
      'en': 'Configuration',
      'fr': 'Configuration'
    },
    'sidebar.negocio': {
      'es': 'Negocio',
      'en': 'Business',
      'fr': 'Entreprise'
    },
    'sidebar.usuarios': {
      'es': 'Usuarios',
      'en': 'Users',
      'fr': 'Utilisateurs'
    },

    // ═══════════════════════════════════════════════════════════
    // 📋 COMÚN - Botones y Acciones
    // ═══════════════════════════════════════════════════════════
    'common.new': {
      'es': 'Nuevo',
      'en': 'New',
      'fr': 'Nouveau'
    },
    'common.edit': {
      'es': 'Editar',
      'en': 'Edit',
      'fr': 'Modifier'
    },
    'common.delete': {
      'es': 'Eliminar',
      'en': 'Delete',
      'fr': 'Supprimer'
    },
    'common.view': {
      'es': 'Ver detalles',
      'en': 'View details',
      'fr': 'Voir détails'
    },
    'common.save': {
      'es': 'Guardar',
      'en': 'Save',
      'fr': 'Enregistrer'
    },
    'common.cancel': {
      'es': 'Cancelar',
      'en': 'Cancel',
      'fr': 'Annuler'
    },
    'common.back': {
      'es': 'Volver',
      'en': 'Back',
      'fr': 'Retour'
    },
    'common.search': {
      'es': 'Buscar',
      'en': 'Search',
      'fr': 'Rechercher'
    },
    'common.actions': {
      'es': 'Acciones',
      'en': 'Actions',
      'fr': 'Actions'
    },
    'common.loading': {
      'es': 'Cargando...',
      'en': 'Loading...',
      'fr': 'Chargement...'
    },
    'common.saving': {
      'es': 'Guardando...',
      'en': 'Saving...',
      'fr': 'Enregistrement...'
    },
    'common.noData': {
      'es': 'No hay datos disponibles',
      'en': 'No data available',
      'fr': 'Aucune donnée disponible'
    },
    'common.select': {
      'es': 'Seleccionar',
      'en': 'Select',
      'fr': 'Sélectionner'
    },
    'common.ifNotChecked': {
      'es': '(si no está marcado)',
      'en': '(if not checked)',
      'fr': '(si non coché)'
    },

    // ═══════════════════════════════════════════════════════════
    // 📁 GRUPO CUENTA
    // ═══════════════════════════════════════════════════════════
    'grupoCuenta.title': {
      'es': 'Grupos de Cuenta',
      'en': 'Account Groups',
      'fr': 'Groupes de Comptes'
    },
    'grupoCuenta.subtitle': {
      'es': 'Nivel superior del plan de cuentas',
      'en': 'Top level of chart of accounts',
      'fr': 'Niveau supérieur du plan comptable'
    },
    'grupoCuenta.newTitle': {
      'es': 'Nuevo Grupo de Cuenta',
      'en': 'New Account Group',
      'fr': 'Nouveau Groupe de Comptes'
    },
    'grupoCuenta.editTitle': {
      'es': 'Editar Grupo de Cuenta',
      'en': 'Edit Account Group',
      'fr': 'Modifier Groupe de Comptes'
    },
    'grupoCuenta.detailTitle': {
      'es': 'Detalle del Grupo de Cuenta',
      'en': 'Account Group Details',
      'fr': 'Détails du Groupe de Comptes'
    },
    'grupoCuenta.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'grupoCuenta.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
    },
    'grupoCuenta.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'grupoCuenta.activo': {
      'es': 'Activo',
      'en': 'Active',
      'fr': 'Actif'
    },
    'grupoCuenta.infoBasica': {
      'es': 'Información General',
      'en': 'General Information',
      'fr': 'Informations Générales'
    },

    // ═══════════════════════════════════════════════════════════
    // 📂 SUBGRUPO CUENTA
    // ═══════════════════════════════════════════════════════════
    'subgrupoCuenta.title': {
      'es': 'SubGrupos de Cuenta',
      'en': 'Account Subgroups',
      'fr': 'Sous-groupes de Comptes'
    },
    'subgrupoCuenta.subtitle': {
      'es': 'Segundo nivel del plan de cuentas',
      'en': 'Second level of chart of accounts',
      'fr': 'Deuxième niveau du plan comptable'
    },
    'subgrupoCuenta.newTitle': {
      'es': 'Nuevo SubGrupo de Cuenta',
      'en': 'New Account Subgroup',
      'fr': 'Nouveau Sous-groupe de Comptes'
    },
    'subgrupoCuenta.editTitle': {
      'es': 'Editar SubGrupo de Cuenta',
      'en': 'Edit Account Subgroup',
      'fr': 'Modifier Sous-groupe de Comptes'
    },
    'subgrupoCuenta.detailTitle': {
      'es': 'Detalle del SubGrupo de Cuenta',
      'en': 'Account Subgroup Details',
      'fr': 'Détails du Sous-groupe de Comptes'
    },
    'subgrupoCuenta.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'subgrupoCuenta.grupoCuenta': {
      'es': 'Grupo de Cuenta',
      'en': 'Account Group',
      'fr': 'Groupe de Comptes'
    },
    'subgrupoCuenta.tipo': {
      'es': 'Tipo',
      'en': 'Type',
      'fr': 'Type'
    },
    'subgrupoCuenta.deudora': {
      'es': 'Deudora',
      'en': 'Debit',
      'fr': 'Débiteur'
    },
    'subgrupoCuenta.acreedora': {
      'es': 'Acreedora',
      'en': 'Credit',
      'fr': 'Créditeur'
    },
    'subgrupoCuenta.estado': {
      'es': 'Estado',
      'en': 'Status',
      'fr': 'Statut'
    },

    // ═══════════════════════════════════════════════════════════
    // 📄 CUENTA
    // ═══════════════════════════════════════════════════════════
    'cuenta.title': {
      'es': 'Cuentas Contables',
      'en': 'Accounting Accounts',
      'fr': 'Comptes Comptables'
    },
    'cuenta.subtitle': {
      'es': 'Nivel más detallado del plan de cuentas',
      'en': 'Most detailed level of chart of accounts',
      'fr': 'Niveau le plus détaillé du plan comptable'
    },
    'cuenta.newTitle': {
      'es': 'Nueva Cuenta',
      'en': 'New Account',
      'fr': 'Nouveau Compte'
    },
    'cuenta.editTitle': {
      'es': 'Editar Cuenta',
      'en': 'Edit Account',
      'fr': 'Modifier Compte'
    },
    'cuenta.detailTitle': {
      'es': 'Detalle de la Cuenta',
      'en': 'Account Details',
      'fr': 'Détails du Compte'
    },
    'cuenta.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'cuenta.subgrupoCuenta': {
      'es': 'SubGrupo de Cuenta',
      'en': 'Account Subgroup',
      'fr': 'Sous-groupe de Comptes'
    },

    // ═══════════════════════════════════════════════════════════
    // 🏢 CONFIGURACIÓN DEL NEGOCIO
    // ═══════════════════════════════════════════════════════════
    'systemConfig.title': {
      'es': 'Configuración del Sistema',
      'en': 'System Configuration',
      'fr': 'Configuration du Système'
    },
    'systemConfig.subtitle': {
      'es': 'Gestión de configuración general y licencias',
      'en': 'General configuration and license management',
      'fr': 'Configuration générale et gestion des licences'
    },
    'systemConfig.newTitle': {
      'es': 'Nueva Configuración',
      'en': 'New Configuration',
      'fr': 'Nouvelle Configuration'
    },
    'systemConfig.editTitle': {
      'es': 'Editar Configuración del Sistema',
      'en': 'Edit System Configuration',
      'fr': 'Modifier Configuration du Système'
    },
    'systemConfig.detailTitle': {
      'es': 'Detalle de Configuración del Sistema',
      'en': 'System Configuration Details',
      'fr': 'Détails de Configuration du Système'
    },
    'systemConfig.codigoSistema': {
      'es': 'Código del Sistema',
      'en': 'System Code',
      'fr': 'Code Système'
    },
    'systemConfig.licencia': {
      'es': 'Licencia',
      'en': 'License',
      'fr': 'Licence'
    },
    'systemConfig.nombreNegocio': {
      'es': 'Nombre del Negocio',
      'en': 'Business Name',
      'fr': 'Nom de l\'Entreprise'
    },
    'systemConfig.direccion': {
      'es': 'Dirección',
      'en': 'Address',
      'fr': 'Adresse'
    },
    'systemConfig.municipio': {
      'es': 'Municipio',
      'en': 'Municipality',
      'fr': 'Municipalité'
    },
    'systemConfig.provincia': {
      'es': 'Provincia',
      'en': 'Province',
      'fr': 'Province'
    },
    'systemConfig.codPostal': {
      'es': 'Código Postal',
      'en': 'Postal Code',
      'fr': 'Code Postal'
    },
    'systemConfig.email': {
      'es': 'Email',
      'en': 'Email',
      'fr': 'Email'
    },
    'systemConfig.telefono': {
      'es': 'Teléfono',
      'en': 'Phone',
      'fr': 'Téléphone'
    },
    'systemConfig.web': {
      'es': 'Sitio Web',
      'en': 'Website',
      'fr': 'Site Web'
    },
    'systemConfig.imagen': {
      'es': 'URL Logo/Imagen',
      'en': 'Logo/Image URL',
      'fr': 'URL Logo/Image'
    },
    'systemConfig.caducidad': {
      'es': 'Fecha de Caducidad',
      'en': 'Expiration Date',
      'fr': 'Date d\'Expiration'
    },
    'systemConfig.cuentaPagar': {
      'es': 'Cuenta Por Pagar',
      'en': 'Accounts Payable',
      'fr': 'Comptes Fournisseurs'
    },
    'systemConfig.cuentaCobrar': {
      'es': 'Cuenta Por Cobrar',
      'en': 'Accounts Receivable',
      'fr': 'Comptes Clients'
    },
    'systemConfig.infoBasica': {
      'es': 'Información Básica',
      'en': 'Basic Information',
      'fr': 'Informations de Base'
    },
    'systemConfig.infoContacto': {
      'es': 'Información de Contacto',
      'en': 'Contact Information',
      'fr': 'Informations de Contact'
    },
    'systemConfig.cuentasContables': {
      'es': 'Cuentas Contables Predeterminadas',
      'en': 'Default Accounting Accounts',
      'fr': 'Comptes Comptables par Défaut'
    },
    'systemConfig.estadoLicencia': {
      'es': 'Estado de Licencia',
      'en': 'License Status',
      'fr': 'Statut de Licence'
    },
    'systemConfig.vigente': {
      'es': 'Vigente',
      'en': 'Valid',
      'fr': 'Valide'
    },
    'systemConfig.porVencer': {
      'es': 'Por Vencer',
      'en': 'Expiring Soon',
      'fr': 'Expirant Bientôt'
    },
    'systemConfig.vencida': {
      'es': 'Vencida',
      'en': 'Expired',
      'fr': 'Expiré'
    },
    'systemConfig.diasRestantes': {
      'es': 'días restantes',
      'en': 'days remaining',
      'fr': 'jours restants'
    },

    // ═══════════════════════════════════════════════════════════
    // 🕒 AUDITORÍA
    // ═══════════════════════════════════════════════════════════
    'audit.title': {
      'es': 'Auditoría',
      'en': 'Audit',
      'fr': 'Audit'
    },
    'audit.createdBy': {
      'es': 'por',
      'en': 'by',
      'fr': 'par'
    },
    'audit.createdAt': {
      'es': 'Creado',
      'en': 'Created',
      'fr': 'Créé'
    },
    'audit.modifiedBy': {
      'es': 'por',
      'en': 'by',
      'fr': 'par'
    },
    'audit.modifiedAt': {
      'es': 'Última modificación',
      'en': 'Last modified',
      'fr': 'Dernière modification'
    },

    // ═══════════════════════════════════════════════════════════
    // ⚠️ VALIDACIONES Y ERRORES
    // ═══════════════════════════════════════════════════════════
    'validation.required': {
      'es': 'Este campo es requerido',
      'en': 'This field is required',
      'fr': 'Ce champ est obligatoire'
    },
    'validation.maxLength': {
      'es': 'Máximo {0} caracteres',
      'en': 'Maximum {0} characters',
      'fr': 'Maximum {0} caractères'
    },
    'validation.email': {
      'es': 'Email no válido',
      'en': 'Invalid email',
      'fr': 'Email invalide'
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
    'error.loading': {
      'es': 'Error al cargar los datos',
      'en': 'Error loading data',
      'fr': 'Erreur lors du chargement des données'
    },
    'error.saving': {
      'es': 'Error al guardar',
      'en': 'Error saving',
      'fr': 'Erreur lors de l\'enregistrement'
    },
    'error.deleting': {
      'es': 'Error al eliminar',
      'en': 'Error deleting',
      'fr': 'Erreur lors de la suppression'
    },
    'confirm.delete': {
      'es': '¿Está seguro que desea eliminar?',
      'en': 'Are you sure you want to delete?',
      'fr': 'Êtes-vous sûr de vouloir supprimer?'
    }
  };

  constructor(private languageService: LanguageService) {
    // Suscribirse a cambios de idioma para notificar a los componentes
    this.languageService.currentLanguage$.subscribe(() => {
      this.translationsSubject.next(Date.now().toString());
    });
  }

  /**
   * Obtener traducción por clave
   * @param key - Clave de traducción (ej: 'common.save')
   * @param params - Parámetros opcionales para reemplazar en el texto (ej: [10] para 'Máximo {0} caracteres')
   * @returns Texto traducido en el idioma actual
   */
  translate(key: string, params: any[] = []): string {
    const currentLang = this.languageService.getCurrentLanguage();
    const translation = this.translations[key]?.[currentLang] || key;

    // Reemplazar parámetros si existen (ej: {0}, {1})
    if (params.length > 0) {
      return translation.replace(/{(\d+)}/g, (match, index) => {
        return params[index] !== undefined ? params[index] : match;
      });
    }

    return translation;
  }

  /**
   * Obtener todas las traducciones de un prefijo específico
   * @param prefix - Prefijo de las claves (ej: 'common', 'grupoCuenta')
   * @returns Objeto con las traducciones filtradas
   */
  getTranslationsFor(prefix: string): { [key: string]: string } {
    const currentLang = this.languageService.getCurrentLanguage();
    const result: { [key: string]: string } = {};

    Object.keys(this.translations).forEach(key => {
      if (key.startsWith(prefix)) {
        const shortKey = key.replace(`${prefix}.`, '');
        result[shortKey] = this.translations[key][currentLang] || key;
      }
    });

    return result;
  }
}
