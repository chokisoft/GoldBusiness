import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { LanguageService } from './language.service';

/**
 * Interfaz para las traducciones multiidioma.
 * Cada clave de traducción contiene un objeto con los idiomas disponibles.
 */
interface Translations {
  [key: string]: {
    [lang: string]: string;
  };
}

/**
 * Servicio de traducción para la aplicación GoldBusiness.
 * 
 * Proporciona traducciones en múltiples idiomas:
 * - Español (es)
 * - Inglés (en)
 * - Francés (fr)
 * - Portugués (pt)
 * - Alemán (de)
 * 
 * @example
 * // Uso básico
 * const saveText = translationService.translate('common.save');
 * 
 * // Con parámetros
 * const maxLength = translationService.translate('validation.maxLength', [50]);
 * 
 * // Obtener traducciones de un módulo
 * const grupoCuentaTranslations = translationService.getTranslationsFor('grupoCuenta');
 */
@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  /** Subject para notificar cambios en las traducciones */
  private translationsSubject = new BehaviorSubject<string>('');

  /** Observable para suscribirse a cambios en las traducciones */
  public translations$ = this.translationsSubject.asObservable();

  /**
   * Diccionario de traducciones organizadas por secciones.
   * Idiomas soportados: es, en, fr, pt, de
   */
  private translations: Translations = {
    // ═══════════════════════════════════════════════════════════
    // 🔐 LOGIN - Pantalla de inicio de sesión
    // ═══════════════════════════════════════════════════════════
    'login.formTitle': {
      'es': 'Acceso al sistema',
      'en': 'System Access',
      'fr': 'Accès au système',
      'pt': 'Acesso ao sistema',
      'de': 'Systemzugang'
    },
    'login.formSubtitle': {
      'es': 'Ingrese sus credenciales',
      'en': 'Enter your credentials',
      'fr': 'Entrez vos identifiants',
      'pt': 'Digite suas credenciais',
      'de': 'Geben Sie Ihre Anmeldedaten ein'
    },
    'login.title': {
      'es': 'GoldBusiness',
      'en': 'GoldBusiness',
      'fr': 'GoldBusiness',
      'pt': 'GoldBusiness',
      'de': 'GoldBusiness'
    },
    'login.subtitle': {
      'es': 'Sistema de Gestión Empresarial',
      'en': 'Enterprise Management System',
      'fr': 'Système de Gestion d\'Entreprise',
      'pt': 'Sistema de Gestão Empresarial',
      'de': 'Unternehmensführungssystem'
    },
    'login.username': {
      'es': 'Usuario o correo',
      'en': 'Username or email',
      'fr': 'Nom d\'utilisateur ou email',
      'pt': 'Usuário ou email',
      'de': 'Benutzername oder E-Mail'
    },
    'login.password': {
      'es': 'Contraseña',
      'en': 'Password',
      'fr': 'Mot de passe',
      'pt': 'Senha',
      'de': 'Passwort'
    },
    'login.usernamePlaceholder': {
      'es': 'Ingrese su usuario o correo',
      'en': 'Enter your username or email',
      'fr': 'Entrez votre nom d\'utilisateur ou email',
      'pt': 'Digite seu usuário ou email',
      'de': 'Geben Sie Ihren Benutzernamen oder E-Mail ein'
    },
    'login.passwordPlaceholder': {
      'es': 'Ingrese su contraseña',
      'en': 'Enter your password',
      'fr': 'Entrez votre mot de passe',
      'pt': 'Digite sua senha',
      'de': 'Geben Sie Ihr Passwort ein'
    },
    'login.submit': {
      'es': '🚀 Iniciar Sesión',
      'en': '🚀 Sign In',
      'fr': '🚀 Se Connecter',
      'pt': '🚀 Entrar',
      'de': '🚀 Anmelden'
    },
    'login.loading': {
      'es': 'Iniciando sesión...',
      'en': 'Signing in...',
      'fr': 'Connexion en cours...',
      'pt': 'Entrando...',
      'de': 'Anmelden...'
    },
    'login.forgotPassword': {
      'es': '¿Olvidó su contraseña?',
      'en': 'Forgot your password?',
      'fr': 'Mot de passe oublié?',
      'pt': 'Esqueceu sua senha?',
      'de': 'Passwort vergessen?'
    },
    'login.googleSignIn': {
      'es': 'Entrar con Google',
      'en': 'Sign in with Google',
      'fr': 'Se connecter avec Google',
      'pt': 'Entrar com Google',
      'de': 'Mit Google anmelden'
    },
    'login.showPassword': {
      'es': 'Mostrar contraseña',
      'en': 'Show password',
      'fr': 'Afficher le mot de passe',
      'pt': 'Mostrar senha',
      'de': 'Passwort anzeigen'
    },
    'login.hidePassword': {
      'es': 'Ocultar contraseña',
      'en': 'Hide password',
      'fr': 'Masquer le mot de passe',
      'pt': 'Ocultar senha',
      'de': 'Passwort verbergen'
    },
    'login.errorGeneral': {
      'es': 'Usuario o contraseña incorrectos. Por favor, intente nuevamente.',
      'en': 'Incorrect username or password. Please try again.',
      'fr': 'Nom d\'utilisateur ou mot de passe incorrect. Veuillez réessayer.',
      'pt': 'Usuário ou senha incorretos. Por favor, tente novamente.',
      'de': 'Falscher Benutzername oder Passwort. Bitte versuchen Sie es erneut.'
    },
    'login.footer': {
      'es': '\u00A9 {0} - Chokisoft Soluciones Tecnológicas',
      'en': '\u00A9 {0} - Chokisoft Technology Solutions',
      'fr': '\u00A9 {0} - Chokisoft Solutions Technologiques',
      'pt': '\u00A9 {0} - Chokisoft Soluções Tecnológicas',
      'de': '\u00A9 {0} - Chokisoft Technologielösungen'
    },

    // Errores de autenticación con Google OAuth
    'login.errorGoogleUserNotFound': {
      'es': '❌ Usuario no encontrado. Debe ser creado previamente por un administrador.',
      'en': '❌ User not found. Must be created by an administrator first.',
      'fr': '❌ Utilisateur introuvable. Doit être créé par un administrateur d\'abord.',
      'pt': '❌ Usuário não encontrado. Deve ser criado previamente por um administrador.',
      'de': '❌ Benutzer nicht gefunden. Muss zuerst von einem Administrator erstellt werden.'
    },
    'login.errorGoogleProviderNotAllowed': {
      'es': '❌ Su cuenta no está configurada para Google. Use usuario y contraseña.',
      'en': '❌ Your account is not configured for Google. Use username and password.',
      'fr': '❌ Votre compte n\'est pas configuré pour Google. Utilisez nom d\'utilisateur et mot de passe.',
      'pt': '❌ Sua conta não está configurada para Google. Use usuário e senha.',
      'de': '❌ Ihr Konto ist nicht für Google konfiguriert. Verwenden Sie Benutzername und Passwort.'
    },
    'login.errorGoogleUserInactive': {
      'es': '❌ Su cuenta está inactiva. Contacte al administrador.',
      'en': '❌ Your account is inactive. Contact the administrator.',
      'fr': '❌ Votre compte est inactif. Contactez l\'administrateur.',
      'pt': '❌ Sua conta está inativa. Contate o administrador.',
      'de': '❌ Ihr Konto ist inaktiv. Kontaktieren Sie den Administrator.'
    },
    'login.errorGoogleEmailNotFound': {
      'es': '❌ No se pudo obtener el email desde Google.',
      'en': '❌ Could not retrieve email from Google.',
      'fr': '❌ Impossible de récupérer l\'email depuis Google.',
      'pt': '❌ Não foi possível obter o email do Google.',
      'de': '❌ E-Mail konnte nicht von Google abgerufen werden.'
    },
    'login.errorGoogleTokenFailed': {
      'es': '❌ Error al generar token de autenticación.',
      'en': '❌ Error generating authentication token.',
      'fr': '❌ Erreur lors de la génération du jeton d\'authentification.',
      'pt': '❌ Erro ao gerar token de autenticação.',
      'de': '❌ Fehler beim Generieren des Authentifizierungstokens.'
    },
    'login.errorGoogleRemoteFailure': {
      'es': '❌ Error de comunicación con Google. Intente nuevamente.',
      'en': '❌ Google communication error. Please try again.',
      'fr': '❌ Erreur de communication avec Google. Veuillez réessayer.',
      'pt': '❌ Erro de comunicação com o Google. Tente novamente.',
      'de': '❌ Google-Kommunikationsfehler. Bitte versuchen Sie es erneut.'
    },
    'login.errorGoogleInternalError': {
      'es': '❌ Error interno del servidor. Contacte al soporte técnico.',
      'en': '❌ Internal server error. Contact technical support.',
      'fr': '❌ Erreur interne du serveur. Contactez le support technique.',
      'pt': '❌ Erro interno do servidor. Contate o suporte técnico.',
      'de': '❌ Interner Serverfehler. Kontaktieren Sie den technischen Support.'
    },
    'login.errorGoogleUserCreationFailed': {
      'es': '❌ Error al crear el usuario. Contacte al administrador.',
      'en': '❌ Error creating user. Contact the administrator.',
      'fr': '❌ Erreur lors de la création de l\'utilisateur. Contactez l\'administrateur.',
      'pt': '❌ Erro ao criar usuário. Contate o administrador.',
      'de': '❌ Fehler beim Erstellen des Benutzers. Kontaktieren Sie den Administrator.'
    },
    'login.errorGoogleGeneric': {
      'es': '❌ Error de autenticación con Google',
      'en': '❌ Google authentication error',
      'fr': '❌ Erreur d\'authentification Google',
      'pt': '❌ Erro de autenticação com Google',
      'de': '❌ Google-Authentifizierungsfehler'
    },
    'login.errorGoogleCompleteLogin': {
      'es': '❌ No se pudo completar el inicio de sesión con Google.',
      'en': '❌ Could not complete Google sign-in.',
      'fr': '❌ Impossible de terminer la connexion Google.',
      'pt': '❌ Não foi possível completar o login com Google.',
      'de': '❌ Google-Anmeldung konnte nicht abgeschlossen werden.'
    },
    // ═══════════════════════════════════════════════════════════
    // 🧪 TEST CONNECTION - Pruebas de conectividad
    // ═══════════════════════════════════════════════════════════
    'test.title': {
      'es': 'Prueba de Conexión',
      'en': 'Connection Test',
      'fr': 'Test de Connexion',
      'pt': 'Teste de Conexão',
      'de': 'Verbindungstest'
    },
    'test.configTitle': {
      'es': 'Configuración',
      'en': 'Configuration',
      'fr': 'Configuration',
      'pt': 'Configuração',
      'de': 'Konfiguration'
    },
    'test.connectionTitle': {
      'es': 'Conexión con el Backend',
      'en': 'Backend Connection',
      'fr': 'Connexion Backend',
      'pt': 'Conexão com o Backend',
      'de': 'Backend-Verbindung'
    },
    'test.testButton': {
      'es': 'Probar Conexión',
      'en': 'Test Connection',
      'fr': 'Tester Connexion',
      'pt': 'Testar Conexão',
      'de': 'Verbindung testen'
    },
    'test.testing': {
      'es': 'Probando...',
      'en': 'Testing...',
      'fr': 'Test en cours...',
      'pt': 'Testando...',
      'de': 'Teste läuft...'
    },
    'test.successTitle': {
      'es': 'Respuesta del Servidor',
      'en': 'Server Response',
      'fr': 'Réponse du Serveur',
      'pt': 'Resposta do Servidor',
      'de': 'Serverantwort'
    },
    'test.errorLabel': {
      'es': 'Error',
      'en': 'Error',
      'fr': 'Erreur',
      'pt': 'Erro',
      'de': 'Fehler'
    },

    // ═══════════════════════════════════════════════════════════
    // 🌍 IDIOMA / LANGUAGE - Selector de idioma
    // ═══════════════════════════════════════════════════════════
    'language.label': {
      'es': 'Idioma',
      'en': 'Language',
      'fr': 'Langue',
      'pt': 'Idioma',
      'de': 'Sprache'
    },

    // ═══════════════════════════════════════════════════════════
    // 📌 HEADER / NAVBAR - Barra de navegación superior
    // ═══════════════════════════════════════════════════════════
    'header.environment.production': {
      'es': 'Producción',
      'en': 'Production',
      'fr': 'Production',
      'pt': 'Produção',
      'de': 'Produktion'
    },
    'header.environment.development': {
      'es': 'Desarrollo',
      'en': 'Development',
      'fr': 'Développement',
      'pt': 'Desenvolvimento',
      'de': 'Entwicklung'
    },
    'header.inicio': {
      'es': 'Inicio',
      'en': 'Home',
      'fr': 'Accueil',
      'pt': 'Início',
      'de': 'Startseite'
    },
    'header.acerca': {
      'es': 'Acerca de',
      'en': 'About',
      'fr': 'À propos',
      'pt': 'Sobre',
      'de': 'Über'
    },
    'header.logout': {
      'es': 'Cerrar Sesión',
      'en': 'Logout',
      'fr': 'Déconnexion',
      'pt': 'Sair',
      'de': 'Abmelden'
    },
    'header.logoutConfirm': {
      'es': '¿Está seguro que desea cerrar sesión?',
      'en': 'Are you sure you want to logout?',
      'fr': 'Êtes-vous sûr de vouloir vous déconnecter?',
      'pt': 'Tem certeza que deseja sair?',
      'de': 'Sind Sie sicher, dass Sie sich abmelden möchten?'
    },

    // ═══════════════════════════════════════════════════════════
    // 🗂️ SIDEBAR - Menú lateral de navegación
    // ═══════════════════════════════════════════════════════════
    'sidebar.nomencladores': {
      'es': 'Nomencladores',
      'en': 'Nomenclators',
      'fr': 'Nomenclateurs',
      'pt': 'Nomenclaturas',
      'de': 'Nomenklatoren'
    },
    'sidebar.planCuentas': {
      'es': 'Plan de Cuentas',
      'en': 'Chart of Accounts',
      'fr': 'Plan Comptable',
      'pt': 'Plano de Contas',
      'de': 'Kontenplan'
    },
    'sidebar.terceros': {
      'es': 'Terceros',
      'en': 'Third Parties',
      'fr': 'Tiers',
      'pt': 'Terceiros',
      'de': 'Dritte'
    },
    'sidebar.organizacion': {
      'es': 'Organización',
      'en': 'Organization',
      'fr': 'Organisation',
      'pt': 'Organização',
      'de': 'Organisation'
    },
    'sidebar.clasificador': {
      'es': 'Clasificador',
      'en': 'Classifier',
      'fr': 'Classificateur',
      'pt': 'Classificador',
      'de': 'Klassifikator'
    },
    'sidebar.operaciones': {
      'es': 'Operaciones',
      'en': 'Operations',
      'fr': 'Opérations',
      'pt': 'Operações',
      'de': 'Operationen'
    },
    'sidebar.producto': {
      'es': 'Producto',
      'en': 'Product',
      'fr': 'Produit',
      'pt': 'Produto',
      'de': 'Produkt'
    },
    'sidebar.configuracion': {
      'es': 'Configuración',
      'en': 'Configuration',
      'fr': 'Configuration',
      'pt': 'Configuração',
      'de': 'Konfiguration'
    },
    'sidebar.negocio': {
      'es': 'Negocio',
      'en': 'Business',
      'fr': 'Entreprise',
      'pt': 'Negócio',
      'de': 'Geschäft'
    },
    'sidebar.usuarios': {
      'es': 'Usuarios',
      'en': 'Users',
      'fr': 'Utilisateurs',
      'pt': 'Usuários',
      'de': 'Benutzer'
    },
    'sidebar.expandMenu': {
      'es': 'Expandir menú',
      'en': 'Expand menu',
      'fr': 'Développer le menu',
      'pt': 'Expandir menu',
      'de': 'Menü erweitern'
    },
    'sidebar.collapseMenu': {
      'es': 'Colapsar menú',
      'en': 'Collapse menu',
      'fr': 'Réduire le menu',
      'pt': 'Recolher menu',
      'de': 'Menü einklappen'
    },
    'sidebar.testConnection': {
      'es': 'Prueba de Conexión',
      'en': 'Connection Test',
      'fr': 'Test de Connexion',
      'pt': 'Teste de Conexão',
      'de': 'Verbindungstest'
    },

    // ═══════════════════════════════════════════════════════════
    // 👥 TERCEROS
    // ═══════════════════════════════════════════════════════════
    'proveedores.title': {
      'es': 'Proveedores',
      'en': 'Suppliers',
      'pt': 'Fornecedores',
      'de': 'Lieferanten',
      'fr': 'Fournisseurs'
    },
    'proveedores.subtitle': {
      'es': 'Gestión de proveedores',
      'en': 'Suppliers management',
      'pt': 'Gestão de Fornecedores',
      'de': 'Verwaltung de Lieferanten',
      'fr': 'Gestion des fournisseurs'
    },
    'proveedores.newTitle': {
      'es': 'Nuevo Proveedor',
      'en': 'New Supplier',
      'pt': 'Novo Fornecedor',
      'de': 'Neu Lieferant',
      'fr': 'Nouveau Fournisseur'
    },
    'proveedores.editTitle': {
      'es': 'Editar Proveedor',
      'en': 'Edit Supplier',
      'pt': 'Editar Fornecedor',
      'de': 'Bearbeiten Lieferant',
      'fr': 'Modifier Fournisseur'
    },
    'proveedores.detailTitle': {
      'es': 'Detalle del Proveedor',
      'en': 'Supplier Details',
      'pt': 'Detalhe del Fornecedor',
      'de': 'Detail del Lieferant',
      'fr': 'Détails du Fournisseur'
    },
    'proveedores.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'proveedores.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'pt': 'Nome',
      'de': 'Name',
      'fr': 'Nom'
    },
    'proveedores.rfc': {
      'es': 'RFC/CIF',
      'en': 'Tax ID',
      'pt': 'RFC/CIF',
      'de': 'RFC/CIF',
      'fr': 'Numéro de TVA'
    },
    'proveedores.telefono': {
      'es': 'Teléfono',
      'en': 'Phone',
      'pt': 'Telefone',
      'de': 'Telefon',
      'fr': 'Téléphone'
    },
    'proveedores.email': {
      'es': 'Email',
      'en': 'Email',
      'pt': 'Email',
      'de': 'E-Mail',
      'fr': 'Email'
    },
    'proveedores.direccion': {
      'es': 'Dirección',
      'en': 'Address',
      'pt': 'Endereço',
      'de': 'Adresse',
      'fr': 'Adresse'
    },
    'proveedores.iva': {
      'es': 'IVA',
      'en': 'VAT',
      'pt': 'IVA',
      'de': 'IVA',
      'fr': 'TVA'
    },
    'proveedores.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'pt': 'Descrição',
      'de': 'Beschreibung',
      'fr': 'Description'
    },
    'proveedores.codigoPlaceholder': {
      'es': 'Ingrese código (5 dígitos)',
      'en': 'Enter code (5 chars)',
      'pt': 'Ingrese Código (5 dígitos)',
      'de': 'Ingrese Code (5 dígitos)',
      'fr': 'Entrez le code (5 caractères)'
    },
    'proveedores.descripcionPlaceholder': {
      'es': 'Ej: Nombre del proveedor',
      'en': 'Ex: Supplier name',
      'pt': 'Ex: Nome do Fornecedor',
      'de': 'z.B.: Kundenname',
      'fr': 'Ex: Nom du fournisseur'
    },
    'proveedores.direccionPlaceholder': {
      'es': 'Ej: Calle y número',
      'en': 'Ex: Street and number',
      'pt': 'Ej: Calle y número',
      'de': 'Ej: Calle y número',
      'fr': 'Ex: Rue et numéro'
    },
    'proveedores.telefonoPlaceholder': {
      'es': 'Ej: +34 912 345 678',
      'en': 'Ex: +34 912 345 678',
      'pt': 'Ej: +34 912 345 678',
      'de': 'Ej: +34 912 345 678',
      'fr': 'Ex: +33 1 23 45 67 89'
    },
    'proveedores.emailPlaceholder': {
      'es': 'Ej: proveedor@empresa.com',
      'en': 'Ex: supplier@company.com',
      'pt': 'Ej: Fornecedor@Empresa.com',
      'de': 'Ej: Lieferant@Unternehmen.com',
      'fr': 'Ex: fournisseur@entreprise.com'
    },
    'proveedores.ivaPlaceholder': {
      'es': 'Ej: 21.00',
      'en': 'Ex: 21.00',
      'pt': 'Ej: 21.00',
      'de': 'Ej: 21.00',
      'fr': 'Ex: 21.00'
    },
    'proveedores.pais': {
      'es': 'País',
      'en': 'Country',
      'pt': 'País',
      'de': 'Land',
      'fr': 'Pays'
    },
    'proveedores.provincia': {
      'es': 'Provincia',
      'en': 'Province',
      'pt': 'Província',
      'de': 'Provinz',
      'fr': 'Province'
    },
    'proveedores.municipio': {
      'es': 'Municipio',
      'en': 'Municipality',
      'pt': 'Município',
      'de': 'Gemeinde',
      'fr': 'Municipalité'
    },
    'proveedores.codigoPostal': {
      'es': 'Código Postal',
      'en': 'Postal Code',
      'pt': 'Código Postal',
      'de': 'Postleitzahl',
      'fr': 'Code Postal'
    },
    'proveedores.email1': {
      'es': 'Email Principal',
      'en': 'Primary Email',
      'pt': 'Email Principal',
      'de': 'E-Mail Principal',
      'fr': 'Email Principal'
    },
    'proveedores.email2': {
      'es': 'Email Secundario',
      'en': 'Secondary Email',
      'pt': 'Email Secundario',
      'de': 'E-Mail Secundario',
      'fr': 'Email Secondaire'
    },
    'proveedores.telefono1': {
      'es': 'Teléfono Principal',
      'en': 'Primary Phone',
      'pt': 'Telefone Principal',
      'de': 'Telefon Principal',
      'fr': 'Téléphone Principal'
    },
    'proveedores.telefono2': {
      'es': 'Teléfono Secundario',
      'en': 'Secondary Phone',
      'pt': 'Telefone Secundario',
      'de': 'Telefon Secundario',
      'fr': 'Téléphone Secondaire'
    },
    'proveedores.nif': {
      'es': 'NIF/CIF',
      'en': 'Tax ID',
      'pt': 'NIF/CIF',
      'de': 'NIF/CIF',
      'fr': 'Numéro fiscal'
    },
    'proveedor.noPaisesDisponibles': {
      'es': 'No hay países disponibles.',
      'en': 'No countries available.',
      'pt': 'Não hay países disponibles.',
      'de': 'Nein hay países disponibles.',
      'fr': 'Aucun pays disponible.'
    },
    'proveedor.selectPaisToSeeProvincias': {
      'es': 'Seleccione un país para ver provincias.',
      'en': 'Select a country to see provinces.',
      'pt': 'Seleccione un País para Ver provincias.',
      'de': 'Seleccione un Land para Anzeigen provincias.',
      'fr': 'Sélectionnez un pays pour voir les provinces.'
    },
    'proveedor.selectProvinciaToSeeMunicipios': {
      'es': 'Seleccione una provincia para ver municipios.',
      'en': 'Select a province to see municipalities.',
      'pt': 'Seleccione una Província para Ver municipios.',
      'de': 'Seleccione una Provinz para Anzeigen municipios.',
      'fr': 'Sélectionnez une province pour voir les municipalités.'
    },
    'proveedor.selectMunicipioToSeeCodigosPostales': {
      'es': 'Seleccione un municipio para ver códigos postales.',
      'en': 'Select a municipality to see postal codes.',
      'pt': 'Seleccione un Município para Ver códigos postales.',
      'de': 'Seleccione un Gemeinde para Anzeigen códigos postales.',
      'fr': 'Sélectionnez une municipalité pour voir les codes postaux.'
    },
    'proveedor.selectCodigoPostalHelp': {
      'es': 'Seleccione un código postal.',
      'en': 'Select a postal code.',
      'pt': 'Seleccione un Código Postal.',
      'de': 'Seleccione un Code postal.',
      'fr': 'Sélectionnez un code postal.'
    },
    'proveedor.provinciaHelp': {
      'es': 'Provincia o estado donde se encuentra el proveedor.',
      'en': 'Province or state where the supplier is located.',
      'pt': 'Província o estado donde se encuentra el Fornecedor.',
      'de': 'Provinz o estado donde se encuentra el Lieferant.',
      'fr': 'Province ou état où se trouve le fournisseur.'
    },
    'proveedor.municipioHelp': {
      'es': 'Municipio o localidad donde se encuentra el proveedor.',
      'en': 'Municipality or locality where the supplier is located.',
      'pt': 'Município o localidad donde se encuentra el Fornecedor.',
      'de': 'Gemeinde o localidad donde se encuentra el Lieferant.',
      'fr': 'Municipalité ou localité où se trouve le fournisseur.'
    },
    'proveedor.codigoPostalHelp': {
      'es': 'Código postal del proveedor.',
      'en': 'Supplier postal code.',
      'pt': 'Código Postal del Fornecedor.',
      'de': 'Code postal del Lieferant.',
      'fr': 'Code postal du fournisseur.'
    },
    'proveedores.codigoHelp': {
      'es': 'Ingrese el código único del proveedor (5 caracteres).',
      'en': 'Enter the unique supplier code (5 characters).',
      'pt': 'Ingrese el Código único del Fornecedor (5 caracteres).',
      'de': 'Ingrese el Code único del Lieferant (5 caracteres).',
      'fr': 'Entrez le code fournisseur unique (5 caractères).'
    },
    'proveedores.descripcionHelp': {
      'es': 'Ingrese el nombre completo o razón social del proveedor.',
      'en': 'Enter the full name or company name of the supplier.',
      'pt': 'Ingrese el Nome completo o razón social del Fornecedor.',
      'de': 'Ingrese el Name completo o razón social del Lieferant.',
      'fr': 'Entrez le nom complet ou la raison sociale du fournisseur.'
    },
    'proveedores.nifHelp': {
      'es': 'Número de identificación fiscal del proveedor (opcional).',
      'en': 'Supplier tax identification number (optional).',
      'pt': 'Número de identificación fiscal del Fornecedor (opcional).',
      'de': 'Número de identificación fiscal del Lieferant (opcional).',
      'fr': 'Numéro d\'identification fiscale du fournisseur (optionnel).'
    },
    'proveedores.ivaHelp': {
      'es': 'Porcentaje de IVA aplicable al proveedor (0-99.99%).',
      'en': 'VAT percentage applicable to the supplier (0-99.99%).',
      'pt': 'Porcentaje de IVA aplicable al Fornecedor (0-99.99%).',
      'de': 'Porcentaje de IVA aplicable al Lieferant (0-99.99%).',
      'fr': 'Pourcentage de TVA applicable au fournisseur (0-99.99%).'
    },
    'proveedores.ibanHelp': {
      'es': 'Código IBAN de la cuenta bancaria del proveedor.',
      'en': 'IBAN code of the supplier\'s bank account.',
      'pt': 'Código IBAN de la Conta bancaria del Fornecedor.',
      'de': 'Code IBAN de la Konto bancaria del Lieferant.',
      'fr': 'Code IBAN du compte bancaire du fournisseur.'
    },
    'proveedores.bicoSwiftHelp': {
      'es': 'Código BIC/SWIFT del banco del proveedor.',
      'en': 'BIC/SWIFT code of the supplier\'s bank.',
      'pt': 'Código BIC/SWIFT del banco del Fornecedor.',
      'de': 'Code BIC/SWIFT del banco del Lieferant.',
      'fr': 'Code BIC/SWIFT de la banque du fournisseur.'
    },
    'proveedores.direccionHelp': {
      'es': 'Ingrese la dirección completa del proveedor.',
      'en': 'Enter the full address of the supplier.',
      'pt': 'Ingrese la Endereço completa del Fornecedor.',
      'de': 'Ingrese la Adresse completa del Lieferant.',
      'fr': 'Entrez l\'adresse complète du fournisseur.'
    },
    'proveedor.paisHelp': {
      'es': 'Seleccione el país donde se encuentra el proveedor.',
      'en': 'Select the country where the supplier is located.',
      'pt': 'Seleccione el País donde se encuentra el Fornecedor.',
      'de': 'Seleccione el Land donde se encuentra el Lieferant.',
      'fr': 'Sélectionnez le pays où se trouve le fournisseur.'
    },
    'proveedores.email1Help': {
      'es': 'Email principal de contacto del proveedor.',
      'en': 'Primary contact email for the supplier.',
      'pt': 'Email principal de contacto del Fornecedor.',
      'de': 'E-Mail principal de contacto del Lieferant.',
      'fr': 'Email de contact principal du fournisseur.'
    },
    'proveedores.email2Help': {
      'es': 'Email secundario de contacto (opcional).',
      'en': 'Secondary contact email (optional).',
      'pt': 'Email secundario de contacto (opcional).',
      'de': 'E-Mail secundario de contacto (opcional).',
      'fr': 'Email de contact secondaire (optionnel).'
    },
    'proveedores.telefono1Help': {
      'es': 'Teléfono principal de contacto (incluya prefijo del país).',
      'en': 'Primary contact phone (include country prefix).',
      'pt': 'Telefone principal de contacto (incluya prefijo del País).',
      'de': 'Telefon principal de contacto (incluya prefijo del Land).',
      'fr': 'Téléphone de contact principal (inclure l\'indicatif du pays).'
    },
    'proveedores.telefono2Help': {
      'es': 'Teléfono secundario de contacto (opcional).',
      'en': 'Secondary contact phone (optional).',
      'pt': 'Telefone secundario de contacto (opcional).',
      'de': 'Telefon secundario de contacto (opcional).',
      'fr': 'Téléphone de contact secondaire (optionnel).'
    },
    'proveedores.webHelp': {
      'es': 'Sitio web del proveedor (opcional, use https://).',
      'en': 'Supplier website (optional, use https://).',
      'fr': 'Site web du fournisseur (optionnel, use https://).',
      'pt': 'Sitio web del Fornecedor (opcional, use https://).',
      'de': 'Sitio web del Lieferant (opcional, use https://).'
    },
    'proveedores.fax1Help': {
      'es': 'Número de fax principal (opcional).',
      'en': 'Primary fax number (optional).',
      'pt': 'Número de fax principal (opcional).',
      'de': 'Número de fax principal (opcional).',
      'fr': 'Numéro de fax principal (optionnel).'
    },
    'proveedores.fax2Help': {
      'es': 'Número de fax secundario (opcional).',
      'en': 'Secondary fax number (optional).',
      'pt': 'Número de fax secundario (opcional).',
      'de': 'Número de fax secundario (opcional).',
      'fr': 'Numéro de fax secondaire (optionnel).'
    },
    'proveedores.telefonoInvalido': {
      'es': 'Teléfono no válido para el país seleccionado',
      'en': 'Invalid phone for selected country',
      'pt': 'Telefone Não válido para el País seleccionado',
      'de': 'Telefon Nein válido para el Land seleccionado',
      'fr': 'Téléphone invalide pour le pays sélectionné'
    },
    'proveedores.placeholderIban': {
      'es': 'Ej: ES91 2100 0418 4502 0005 1332',
      'en': 'Ex: ES91 2100 0418 4502 0005 1332',
      'pt': 'Ej: ES91 2100 0418 4502 0005 1332',
      'de': 'Ej: ES91 2100 0418 4502 0005 1332',
      'fr': 'Ex: FR14 2004 1010 0505 0001 3M02 606'
    },
    'proveedores.placeholderBicoSwift': {
      'es': 'Ej: CAIXESBBXXX',
      'en': 'Ex: CAIXESBBXXX',
      'pt': 'Ej: CAIXESBBXXX',
      'de': 'Ej: CAIXESBBXXX',
      'fr': 'Ex: BNPAFRPPXXX'
    },
    'proveedores.placeholderWeb': {
      'es': 'Ej: https://www.proveedor.com',
      'en': 'Ex: https://www.supplier.com',
      'pt': 'Ej: https://www.Fornecedor.com',
      'de': 'Ej: https://www.Lieferant.com',
      'fr': 'Ex: https://www.fournisseur.fr'
    },
    'proveedores.placeholderFax': {
      'es': 'Ej: +34 912 345 678',
      'en': 'Ex: +34 912 345 678',
      'pt': 'Ej: +34 912 345 678',
      'de': 'Ej: +34 912 345 678',
      'fr': 'Ex: +33 1 23 45 67 89'
    },
    'proveedores.placeholderDireccion': {
      'es': 'Ej: Calle y número',
      'en': 'Ex: Street and number',
      'pt': 'Ej: Calle y número',
      'de': 'Ej: Calle y número',
      'fr': 'Ex: Rue et numéro'
    },
    'proveedores.identificadorFiscal': {
      'es': 'Identificador Fiscal',
      'en': 'Tax ID',
      'pt': 'Identificador Fiscal',
      'de': 'Identificador Fiscal',
      'fr': 'Identifiant Fiscal'
    },
    'proveedores.identificadorFiscalHelp': {
      'es': 'Número de identificación fiscal del proveedor (NIF, CIF, VAT, etc.).',
      'en': 'Supplier tax identification number (NIF, CIF, VAT, etc.).',
      'fr': 'Numéro d\'identification fiscale du fournisseur (NIF, CIF, TVA, etc.).',
      'pt': 'Número de identificación fiscal del Fornecedor (NIF, CIF, VAT, etc.).',
      'de': 'Número de identificación fiscal del Lieferant (NIF, CIF, VAT, etc.).'
    },
    'proveedores.placeholderIdentificadorFiscal': {
      'es': 'Ej: 12345678A',
      'en': 'Ex: 12345678A',
      'pt': 'Ej: 12345678A',
      'de': 'Ej: 12345678A',
      'fr': 'Ex: 12345678A'
    },
    'proveedores.tasaIva': {
      'es': 'Tasa IVA (%)',
      'en': 'VAT Rate (%)',
      'pt': 'Tasa IVA (%)',
      'de': 'Tasa IVA (%)',
      'fr': 'Taux TVA (%)'
    },
    'proveedores.tasaIvaHelp': {
      'es': 'Tasa de IVA aplicable al proveedor (0-99.99%). Ej: 21 para 21%.',
      'en': 'VAT rate applicable to the supplier (0-99.99%). Ex: 21 for 21%.',
      'fr': 'Taux de TVA applicable au fournisseur (0-99.99%). Ex: 21 pour 21%.',
      'pt': 'Tasa de IVA aplicable al Fornecedor (0-99.99%). Ej: 21 para 21%.',
      'de': 'Tasa de IVA aplicable al Lieferant (0-99.99%). Ej: 21 para 21%.'
    },
    'proveedores.tasaIvaPlaceholder': {
      'es': 'Ej: 21.00',
      'en': 'Ex: 21.00',
      'pt': 'Ej: 21.00',
      'de': 'Ej: 21.00',
      'fr': 'Ex: 21.00'
    },
    'proveedores.telefonoHelp': {
      'es': 'Teléfono de contacto del proveedor (incluya prefijo internacional si aplica).',
      'en': 'Supplier contact phone (include international prefix if applicable).',
      'pt': 'Telefone de contacto del Fornecedor (incluya prefijo internacional si aplica).',
      'de': 'Telefon de contacto del Lieferant (incluya prefijo internacional si aplica).',
      'fr': 'Téléphone de contact du fournisseur (inclure l\'indicatif international si applicable).'
    },
    'proveedores.emailHelp': {
      'es': 'Email de contacto del proveedor.',
      'en': 'Supplier contact email.',
      'pt': 'Email de contacto del Fornecedor.',
      'de': 'E-Mail de contacto del Lieferant.',
      'fr': 'Email de contact du fournisseur.'
    },
    'proveedores.iban': {
      'es': 'IBAN',
      'en': 'IBAN',
      'pt': 'IBAN',
      'de': 'IBAN',
      'fr': 'IBAN'
    },
    'proveedores.bicoSwift': {
      'es': 'BIC/SWIFT',
      'en': 'BIC/SWIFT',
      'pt': 'BIC/SWIFT',
      'de': 'BIC/SWIFT',
      'fr': 'BIC/SWIFT'
    },
    'proveedores.web': {
      'es': 'Sitio Web',
      'en': 'Website',
      'pt': 'Sitio Web',
      'de': 'Sitio Web',
      'fr': 'Site Web'
    },
    'proveedores.fax1': {
      'es': 'Fax Principal',
      'en': 'Primary Fax',
      'pt': 'Fax Principal',
      'de': 'Fax Principal',
      'fr': 'Fax Principal'
    },
    'proveedores.fax2': {
      'es': 'Fax Secundario',
      'en': 'Secondary Fax',
      'pt': 'Fax Secundario',
      'de': 'Fax Secundario',
      'fr': 'Fax Secondaire'
    },
    'proveedores.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'pt': 'Informação Principal',
      'de': 'Hauptinformation',
      'fr': 'Informations Principales'
    },
    'proveedores.infoContacto': {
      'es': 'Información de Contacto',
      'en': 'Contact Information',
      'pt': 'Informação de Contacto',
      'de': 'Information de Contacto',
      'fr': 'Informations de Contact'
    },
    'proveedores.infoUbicacion': {
      'es': 'Información de Ubicación',
      'en': 'Location Information',
      'pt': 'Informação de Ubicación',
      'de': 'Information de Ubicación',
      'fr': 'Informations de Localisation'
    },
    'proveedores.infoBancaria': {
      'es': 'Información Bancaria',
      'en': 'Banking Information',
      'pt': 'Informação Bancaria',
      'de': 'Information Bancaria',
      'fr': 'Informations Bancaires'
    },

    'clientes.title': {
      'es': 'Clientes',
      'en': 'Clients',
      'pt': 'Clientes',
      'de': 'Kunden',
      'fr': 'Clients'
    },
    'clientes.subtitle': {
      'es': 'Gestión de clientes',
      'en': 'Clients management',
      'pt': 'Gestão de Clientes',
      'de': 'Kundenverwaltung',
      'fr': 'Gestion des clients'
    },
    'clientes.newTitle': {
      'es': 'Nuevo Cliente',
      'en': 'New Client',
      'pt': 'Novo Cliente',
      'de': 'Neu Kunde',
      'fr': 'Nouveau Client'
    },
    'clientes.editTitle': {
      'es': 'Editar Cliente',
      'en': 'Edit Client',
      'pt': 'Editar Cliente',
      'de': 'Bearbeiten Kunde',
      'fr': 'Modifier Client'
    },
    'clientes.detailTitle': {
      'es': 'Detalle del Cliente',
      'en': 'Client Details',
      'pt': 'Detalhe del Cliente',
      'de': 'Detail del Kunde',
      'fr': 'Détails du Client'
    },
    'clientes.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'pt': 'Informação Principal',
      'de': 'Hauptinformation',
      'fr': 'Informations Principales'
    },
    'clientes.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'clientes.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'pt': 'Nome',
      'de': 'Name',
      'fr': 'Nom'
    },
    'clientes.telefono': {
      'es': 'Teléfono',
      'en': 'Phone',
      'pt': 'Telefone',
      'de': 'Telefon',
      'fr': 'Téléphone'
    },
    'clientes.email': {
      'es': 'Email',
      'en': 'Email',
      'pt': 'Email',
      'de': 'E-Mail',
      'fr': 'Email'
    },
    'clientes.direccion': {
      'es': 'Dirección',
      'en': 'Address',
      'pt': 'Endereço',
      'de': 'Adresse',
      'fr': 'Adresse'
    },
    'clientes.iva': {
      'es': 'IVA',
      'en': 'VAT',
      'pt': 'IVA',
      'de': 'IVA',
      'fr': 'TVA'
    },
    'clientes.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'pt': 'Descrição',
      'de': 'Beschreibung',
      'fr': 'Description'
    },
    'clientes.nif': {
      'es': 'NIF',
      'en': 'NIF',
      'pt': 'NIF',
      'de': 'NIF',
      'fr': 'NIF'
    },
    'clientes.codigoPlaceholder': {
      'es': 'Ingrese código (8 dígitos)',
      'en': 'Enter code (8 chars)',
      'pt': 'Ingrese Código (8 dígitos)',
      'de': 'Ingrese Code (8 dígitos)',
      'fr': 'Entrez le code (8 caractères)'
    },
    'clientes.descripcionPlaceholder': {
      'es': 'Ej: Nombre del cliente o razón social',
      'en': 'Ex: Customer name or company',
      'pt': 'Ej: Nome del Cliente o razón social',
      'de': 'Ej: Name del Kunde o razón social',
      'fr': 'Ex: Nom du client ou raison sociale'
    },
    'clientes.direccionPlaceholder': {
      'es': 'Ej: Calle y número',
      'en': 'Ex: Street and number',
      'pt': 'Ej: Calle y número',
      'de': 'Ej: Calle y número',
      'fr': 'Ex: Rue et numéro'
    },
    'clientes.telefonoPlaceholder': {
      'es': 'Ej: +34 912 345 678',
      'en': 'Ex: +34 912 345 678',
      'pt': 'Ej: +34 912 345 678',
      'de': 'Ej: +34 912 345 678',
      'fr': 'Ex: +33 1 23 45 67 89'
    },
    'clientes.emailPlaceholder': {
      'es': 'Ej: cliente@empresa.com',
      'en': 'Ex: customer@company.com',
      'pt': 'Ej: Cliente@Empresa.com',
      'de': 'Ej: Kunde@Unternehmen.com',
      'fr': 'Ex: client@entreprise.com'
    },
    'clientes.ivaPlaceholder': {
      'es': 'Ej: 21.00',
      'en': 'Ex: 21.00',
      'pt': 'Ej: 21.00',
      'de': 'Ej: 21.00',
      'fr': 'Ex: 21.00'
    },
    "clientes.placeholderCodigo": {
      "es": "Ingrese código (8 dígitos)",
      "en": "Enter code (8 chars)",
      "fr": "Entrez le code (8 caractères)",
      "pt": "Digite código (8 dígitos)",
      "de": "Code eingeben (8 Zeichen)"
    },
    "clientes.placeholderDescripcion": {
      "es": "Ej: Nombre del cliente o razón social",
      "en": "Ex: Customer name or company",
      "fr": "Ex: Nom du client ou raison sociale",
      "pt": "Ex: Nome do cliente ou razão social",
      "de": "z.B.: Kundenname oder Firma"
    },
    "clientes.placeholderDireccion": {
      "es": "Ej: Calle y número",
      "en": "Ex: Street and number",
      "fr": "Ex: Rue et numéro",
      "pt": "Ex: Rua e número",
      "de": "z.B.: Straße und Nummer"
    },
    "clientes.placeholderEmail": {
      "es": "Ej: cliente@empresa.com",
      "en": "Ex: customer@company.com",
      "fr": "Ex: client@entreprise.com",
      "pt": "Ex: cliente@empresa.com",
      "de": "z.B.: kunde@firma.com"
    },
    "clientes.placeholderNif": {
      "es": "Ej: 12345678A",
      "en": "Ex: 12345678A",
      "fr": "Ex: 12345678A",
      "pt": "Ex: 12345678A",
      "de": "z.B.: 12345678A"
    },
    "clientes.placeholderTelefono": {
      "es": "Ej: +34 912 345 678",
      "en": "Ex: +34 912 345 678",
      "fr": "Ex: +33 1 23 45 67 89",
      "pt": "Ex: +351 21 234 5678",
      "de": "z.B.: +49 30 1234 5678"
    },
    'clientes.pais': {
      'es': 'País',
      'en': 'Country',
      'pt': 'País',
      'de': 'Land',
      'fr': 'Pays'
    },
    'clientes.provincia': {
      'es': 'Provincia',
      'en': 'Province',
      'pt': 'Província',
      'de': 'Provinz',
      'fr': 'Province'
    },
    'clientes.municipio': {
      'es': 'Municipio',
      'en': 'Municipality',
      'pt': 'Município',
      'de': 'Gemeinde',
      'fr': 'Municipalité'
    },
    'clientes.codigoPostal': {
      'es': 'Código Postal',
      'en': 'Postal Code',
      'pt': 'Código Postal',
      'de': 'Postleitzahl',
      'fr': 'Code Postal'
    },
    'clientes.email1': {
      'es': 'Email Principal',
      'en': 'Primary Email',
      'pt': 'Email Principal',
      'de': 'E-Mail Principal',
      'fr': 'Email Principal'
    },
    'clientes.email2': {
      'es': 'Email Secundario',
      'en': 'Secondary Email',
      'pt': 'Email Secundario',
      'de': 'E-Mail Secundario',
      'fr': 'Email Secondaire'
    },
    'clientes.telefono1': {
      'es': 'Teléfono Principal',
      'en': 'Primary Phone',
      'pt': 'Telefone Principal',
      'de': 'Telefon Principal',
      'fr': 'Téléphone Principal'
    },
    'clientes.telefono2': {
      'es': 'Teléfono Secundario',
      'en': 'Secondary Phone',
      'pt': 'Telefone Secundario',
      'de': 'Telefon Secundario',
      'fr': 'Téléphone Secondaire'
    },
    'cliente.noPaisesDisponibles': {
      'es': 'No hay países disponibles.',
      'en': 'No countries available.',
      'pt': 'Não hay países disponibles.',
      'de': 'Nein hay países disponibles.',
      'fr': 'Aucun pays disponible.'
    },
    'cliente.selectPaisToSeeProvincias': {
      'es': 'Seleccione un país para ver provincias.',
      'en': 'Select a country to see provinces.',
      'pt': 'Seleccione un País para Ver provincias.',
      'de': 'Seleccione un Land para Anzeigen provincias.',
      'fr': 'Sélectionnez un pays pour voir les provinces.'
    },
    'cliente.selectProvinciaToSeeMunicipios': {
      'es': 'Seleccione una provincia para ver municipios.',
      'en': 'Select a province to see municipalities.',
      'pt': 'Seleccione una Província para Ver municipios.',
      'de': 'Seleccione una Provinz para Anzeigen municipios.',
      'fr': 'Sélectionnez une province pour voir les municipalités.'
    },
    'cliente.selectMunicipioToSeeCodigosPostales': {
      'es': 'Seleccione un municipio para ver códigos postales.',
      'en': 'Select a municipality to see postal codes.',
      'pt': 'Seleccione un Município para Ver códigos postales.',
      'de': 'Seleccione un Gemeinde para Anzeigen códigos postales.',
      'fr': 'Sélectionnez une municipalité pour voir les codes postaux.'
    },
    'cliente.provinciaHelp': {
      'es': 'Provincia o estado donde se encuentra el cliente.',
      'en': 'Province or state where the customer is located.',
      'pt': 'Província o estado donde se encuentra el Cliente.',
      'de': 'Provinz o estado donde se encuentra el Kunde.',
      'fr': 'Province ou état où se trouve le client.'
    },
    'cliente.municipioHelp': {
      'es': 'Municipio o localidad donde se encuentra el cliente.',
      'en': 'Municipality or locality where the customer is located.',
      'pt': 'Município o localidad donde se encuentra el Cliente.',
      'de': 'Gemeinde o localidad donde se encuentra el Kunde.',
      'fr': 'Municipalité ou localité où se trouve le client.'
    },
    'cliente.codigoPostalHelp': {
      'es': 'Código postal del cliente.',
      'en': 'Customer postal code.',
      'pt': 'Código Postal del Cliente.',
      'de': 'Code postal del Kunde.',
      'fr': 'Code postal du client.'
    },
    'clientes.codigoHelp': {
      'es': 'Ingrese el código único del cliente (8 dígitos numéricos).',
      'en': 'Enter the unique customer code (8 numeric digits).',
      'pt': 'Ingrese el Código único del Cliente (8 dígitos numéricos).',
      'de': 'Ingrese el Code único del Kunde (8 dígitos numéricos).',
      'fr': 'Entrez le code client unique (8 chiffres numériques).'
    },
    'clientes.descripcionHelp': {
      'es': 'Ingrese el nombre completo o razón social del cliente.',
      'en': 'Enter the full name or company name of the customer.',
      'pt': 'Ingrese el Nome completo o razón social del Cliente.',
      'de': 'Ingrese el Name completo o razón social del Kunde.',
      'fr': 'Entrez le nom complet ou la raison sociale du client.'
    },
    'clientes.nifHelp': {
      'es': 'Número de identificación fiscal del cliente (opcional).',
      'en': 'Customer tax identification number (optional).',
      'pt': 'Número de identificación fiscal del Cliente (opcional).',
      'de': 'Número de identificación fiscal del Kunde (opcional).',
      'fr': 'Numéro d\'identification fiscale du client (optionnel).'
    },
    'clientes.ivaHelp': {
      'es': 'Porcentaje de IVA aplicable al cliente (0-99.99%).',
      'en': 'VAT percentage applicable to the customer (0-99.99%).',
      'pt': 'Porcentaje de IVA aplicable al Cliente (0-99.99%).',
      'de': 'Porcentaje de IVA aplicable al Kunde (0-99.99%).',
      'fr': 'Pourcentage de TVA applicable au client (0-99.99%).'
    },
    'clientes.ibanHelp': {
      'es': 'Código IBAN de la cuenta bancaria del cliente.',
      'en': 'IBAN code of the customer\'s bank account.',
      'pt': 'Código IBAN de la Conta bancaria del Cliente.',
      'de': 'Code IBAN de la Konto bancaria del Kunde.',
      'fr': 'Code IBAN du compte bancaire du client.'
    },
    'clientes.bicoSwiftHelp': {
      'es': 'Código BIC/SWIFT del banco del cliente.',
      'en': 'BIC/SWIFT code of the customer\'s bank.',
      'pt': 'Código BIC/SWIFT del banco del Cliente.',
      'de': 'Code BIC/SWIFT del banco del Kunde.',
      'fr': 'Code BIC/SWIFT de la banque du client.'
    },
    'clientes.direccionHelp': {
      'es': 'Ingrese la dirección completa del cliente.',
      'en': 'Enter the full address of the customer.',
      'pt': 'Ingrese la Endereço completa del Cliente.',
      'de': 'Ingrese la Adresse completa del Kunde.',
      'fr': 'Entrez l\'adresse complète du client.'
    },
    'cliente.paisHelp': {
      'es': 'Seleccione el país donde se encuentra el cliente.',
      'en': 'Select the country where the customer is located.',
      'pt': 'Seleccione el País donde se encuentra el Cliente.',
      'de': 'Seleccione el Land donde se encuentra el Kunde.',
      'fr': 'Sélectionnez le pays où se trouve le client.'
    },
    'clientes.email1Help': {
      'es': 'Email principal de contacto del cliente.',
      'en': 'Primary contact email for the customer.',
      'pt': 'Email principal de contacto del Cliente.',
      'de': 'E-Mail principal de contacto del Kunde.',
      'fr': 'Email de contact principal du client.'
    },
    'clientes.email2Help': {
      'es': 'Email secundario de contacto (opcional).',
      'en': 'Secondary contact email (optional).',
      'pt': 'Email secundario de contacto (opcional).',
      'de': 'E-Mail secundario de contacto (opcional).',
      'fr': 'Email de contact secondaire (optionnel).'
    },
    'clientes.telefono1Help': {
      'es': 'Teléfono principal de contacto (incluya prefijo del país).',
      'en': 'Primary contact phone (include country prefix).',
      'pt': 'Telefone principal de contacto (incluya prefijo del País).',
      'de': 'Telefon principal de contacto (incluya prefijo del Land).',
      'fr': 'Téléphone de contact principal (inclure l\'indicatif du pays).'
    },
    'clientes.telefono2Help': {
      'es': 'Teléfono secundario de contacto (opcional).',
      'en': 'Secondary contact phone (optional).',
      'pt': 'Telefone secundario de contacto (opcional).',
      'de': 'Telefon secundario de contacto (opcional).',
      'fr': 'Téléphone de contact secondaire (optionnel).'
    },
    'clientes.webHelp': {
      'es': 'Sitio web del cliente (opcional, use https://).',
      'en': 'Customer website (optional, use https://).',
      'pt': 'Sitio web del Cliente (opcional, use https://).',
      'de': 'Sitio web del Kunde (opcional, use https://).',
      'fr': 'Site web du client (optionnel, utilisez https://).'
    },
    'clientes.fax1Help': {
      'es': 'Número de fax principal (opcional).',
      'en': 'Primary fax number (optional).',
      'pt': 'Número de fax principal (opcional).',
      'de': 'Número de fax principal (opcional).',
      'fr': 'Numéro de fax principal (optionnel).'
    },
    'clientes.placeholderIban': {
      'es': 'Ej: ES91 2100 0418 4502 0005 1332',
      'en': 'Ex: ES91 2100 0418 4502 0005 1332',
      'pt': 'Ej: ES91 2100 0418 4502 0005 1332',
      'de': 'Ej: ES91 2100 0418 4502 0005 1332',
      'fr': 'Ex: FR14 2004 1010 0505 0001 3M02 606'
    },
    'clientes.placeholderBicoSwift': {
      'es': 'Ej: CAIXESBBXXX',
      'en': 'Ex: CAIXESBBXXX',
      'pt': 'Ej: CAIXESBBXXX',
      'de': 'Ej: CAIXESBBXXX',
      'fr': 'Ex: BNPAFRPPXXX'
    },
    'clientes.placeholderWeb': {
      'es': 'Ej: https://www.cliente.com',
      'en': 'Ex: https://www.customer.com',
      'pt': 'Ej: https://www.Cliente.com',
      'de': 'Ej: https://www.Kunde.com',
      'fr': 'Ex: https://www.client.fr'
    },
    'clientes.iban': {
      'es': 'IBAN',
      'en': 'IBAN',
      'pt': 'IBAN',
      'de': 'IBAN',
      'fr': 'IBAN'
    },
    'clientes.bicoSwift': {
      'es': 'BIC/SWIFT',
      'en': 'BIC/SWIFT',
      'pt': 'BIC/SWIFT',
      'de': 'BIC/SWIFT',
      'fr': 'BIC/SWIFT'
    },
    'clientes.web': {
      'es': 'Sitio Web',
      'en': 'Website',
      'pt': 'Sitio Web',
      'de': 'Sitio Web',
      'fr': 'Site Web'
    },
    'clientes.fax1': {
      'es': 'Fax Principal',
      'en': 'Primary Fax',
      'pt': 'Fax Principal',
      'de': 'Fax Principal',
      'fr': 'Fax Principal'
    },
    'clientes.fax2': {
      'es': 'Fax Secundario',
      'en': 'Secondary Fax',
      'pt': 'Fax Secundario',
      'de': 'Fax Secundario',
      'fr': 'Fax Secondaire'
    },
    'clientes.infoContacto': {
      'es': 'Información de Contacto',
      'en': 'Contact Information',
      'pt': 'Informação de Contacto',
      'de': 'Information de Contacto',
      'fr': 'Informations de Contact'
    },
    'clientes.infoUbicacion': {
      'es': 'Información de Ubicación',
      'en': 'Location Information',
      'pt': 'Informação de Ubicación',
      'de': 'Information de Ubicación',
      'fr': 'Informations de Localisation'
    },
    'clientes.infoBancaria': {
      'es': 'Información Bancaria',
      'en': 'Banking Information',
      'pt': 'Informação Bancaria',
      'de': 'Information Bancaria',
      'fr': 'Informations Bancaires'
    },
    'clientes.fax2Help': {
      'es': 'Número de fax secundario (opcional).',
      'en': 'Secondary fax number (optional).',
      'pt': 'Número de fax secundario (opcional).',
      'de': 'Número de fax secundario (opcional).',
      'fr': 'Numéro de fax secondaire (optionnel).'
    },
    'clientes.telefonoInvalido': {
      'es': 'Teléfono no válido para el país seleccionado',
      'en': 'Invalid phone for selected country',
      'pt': 'Telefone Não válido para el País seleccionado',
      'de': 'Telefon Nein válido para el Land seleccionado',
      'fr': 'Téléphone invalide pour le pays sélectionné'
    },
    'clientes.placeholderFax': {
      'es': 'Ej: +34 912 345 678',
      'en': 'Ex: +34 912 345 678',
      'pt': 'Ej: +34 912 345 678',
      'de': 'Ej: +34 912 345 678',
      'fr': 'Ex: +33 1 23 45 67 89'
    },
    'clientes.identificadorFiscal': {
      'es': 'Identificador Fiscal',
      'en': 'Tax ID',
      'pt': 'Identificador Fiscal',
      'de': 'Identificador Fiscal',
      'fr': 'Identifiant Fiscal'
    },
    'clientes.identificadorFiscalHelp': {
      'es': 'Número de identificación fiscal del cliente (NIF, CIF, VAT, etc.).',
      'en': 'Customer tax identification number (NIF, CIF, VAT, etc.).',
      'pt': 'Número de identificación fiscal del Cliente (NIF, CIF, VAT, etc.).',
      'de': 'Número de identificación fiscal del Kunde (NIF, CIF, VAT, etc.).',
      'fr': 'Numéro d\'identification fiscale du client (NIF, CIF, TVA, etc.).'
    },
    'clientes.placeholderIdentificadorFiscal': {
      'es': 'Ej: 12345678A',
      'en': 'Ex: 12345678A',
      'pt': 'Ej: 12345678A',
      'de': 'Ej: 12345678A',
      'fr': 'Ex: 12345678A'
    },
    'clientes.tasaIva': {
      'es': 'Tasa IVA (%)',
      'en': 'VAT Rate (%)',
      'pt': 'Tasa IVA (%)',
      'de': 'Tasa IVA (%)',
      'fr': 'Taux TVA (%)'
    },
    'clientes.tasaIvaHelp': {
      'es': 'Tasa de IVA aplicable al cliente (0-99.99%). Ej: 21 para 21%.',
      'en': 'VAT rate applicable to the customer (0-99.99%). Ex: 21 for 21%.',
      'pt': 'Tasa de IVA aplicable al Cliente (0-99.99%). Ej: 21 para 21%.',
      'de': 'Tasa de IVA aplicable al Kunde (0-99.99%). Ej: 21 para 21%.',
      'fr': 'Taux de TVA applicable au client (0-99,99%). Ex: 21 pour 21%.'
    },
    'clientes.tasaIvaPlaceholder': {
      'es': 'Ej: 21.00',
      'en': 'Ex: 21.00',
      'pt': 'Ej: 21.00',
      'de': 'Ej: 21.00',
      'fr': 'Ex: 21.00'
    },
    'clientes.telefonoHelp': {
      'es': 'Teléfono de contacto del cliente (incluya prefijo internacional si aplica).',
      'en': 'Customer contact phone (include international prefix if applicable).',
      'pt': 'Telefone de contacto del Cliente (incluya prefijo internacional si aplica).',
      'de': 'Telefon de contacto del Kunde (incluya prefijo internacional si aplica).',
      'fr': 'Téléphone de contact du client (inclure l\'indicatif international si applicable).'
    },
    'clientes.emailHelp': {
      'es': 'Email de contacto del cliente.',
      'en': 'Customer contact email.',
      'pt': 'Email de contacto del Cliente.',
      'de': 'E-Mail de contacto del Kunde.',
      'fr': 'Email de contact du client.'
    },

    // ═══════════════════════════════════════════════════════════
    // 🏢 ORGANIZACIÓN
    // ═══════════════════════════════════════════════════════════
    'establecimiento.title': {
      'es': 'Establecimientos',
      'en': 'Establishments',
      'pt': 'Establecimientos',
      'de': 'Establecimientos',
      'fr': 'Établissements'
    },
    'establecimiento.subtitle': {
      'es': 'Gestión de establecimientos',
      'en': 'Establishments management',
      'pt': 'Gestão de establecimientos',
      'de': 'Verwaltung de establecimientos',
      'fr': 'Gestion des établissements'
    },
    'establecimiento.newTitle': {
      'es': 'Nuevo Establecimiento',
      'en': 'New Establishment',
      'pt': 'Novo Establecimiento',
      'de': 'Neu Establecimiento',
      'fr': 'Nouvel Établissement'
    },
    'establecimiento.editTitle': {
      'es': 'Editar Establecimiento',
      'en': 'Edit Establishment',
      'pt': 'Editar Establecimiento',
      'de': 'Bearbeiten Establecimiento',
      'fr': 'Modifier Établissement'
    },
    'establecimiento.detailTitle': {
      'es': 'Detalle del Establecimiento',
      'en': 'Establishment Details',
      'pt': 'Detalhe del Establecimiento',
      'de': 'Detail del Establecimiento',
      'fr': 'Détails de l\'Établissement'
    },
    'establecimiento.detailSubtitle': {
      'es': 'Visualización completa de la información del establecimiento',
      'en': 'Complete view of establishment information',
      'pt': 'Visualización completa de la Informação del establecimiento',
      'de': 'Visualización completa de la Information del establecimiento',
      'fr': 'Vue complète des informations de l\'établissement'
    },
    'establecimiento.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'pt': 'Informação Principal',
      'de': 'Hauptinformation',
      'fr': 'Informations Principales'
    },
    'establecimiento.infoContacto': {
      'es': 'Información de Contacto',
      'en': 'Contact Information',
      'pt': 'Informação de Contacto',
      'de': 'Information de Contacto',
      'fr': 'Informations de Contact'
    },
    'establecimiento.infoUbicacion': {
      'es': 'Información de Ubicación',
      'en': 'Location Information',
      'pt': 'Informação de Ubicación',
      'de': 'Information de Ubicación',
      'fr': 'Informations de Localisation'
    },
    'establecimiento.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'establecimiento.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'pt': 'Nome',
      'de': 'Name',
      'fr': 'Nom'
    },
    'establecimiento.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'pt': 'Descrição',
      'de': 'Beschreibung',
      'fr': 'Description'
    },
    'establecimiento.telefono': {
      'es': 'Teléfono',
      'en': 'Phone',
      'pt': 'Telefone',
      'de': 'Telefon',
      'fr': 'Téléphone'
    },
    'establecimiento.email': {
      'es': 'Email',
      'en': 'Email',
      'pt': 'Email',
      'de': 'E-Mail',
      'fr': 'Email'
    },
    'establecimiento.direccion': {
      'es': 'Dirección',
      'en': 'Address',
      'pt': 'Endereço',
      'de': 'Adresse',
      'fr': 'Adresse'
    },
    'establecimiento.localidad': {
      'es': 'Localidad',
      'en': 'Locality',
      'pt': 'Localidad',
      'de': 'Localidad',
      'fr': 'Localité'
    },
    'establecimiento.codigoPlaceholder': {
      'es': 'Ej: EST001',
      'en': 'Ex: EST001',
      'pt': 'Ej: EST001',
      'de': 'Ej: EST001',
      'fr': 'Ex: EST001'
    },
    'establecimiento.nombrePlaceholder': {
      'es': 'Ej: Sucursal Centro',
      'en': 'Ex: Downtown Branch',
      'pt': 'Ej: Sucursal Centro',
      'de': 'Ej: Sucursal Centro',
      'fr': 'Ex: Succursale Centre-Ville'
    },
    'establecimiento.descripcionPlaceholder': {
      'es': 'Ej: Sucursal principal en el centro de la ciudad',
      'en': 'Ex: Main branch in downtown',
      'pt': 'Ej: Sucursal principal en el centro de la ciudad',
      'de': 'Ej: Sucursal principal en el centro de la ciudad',
      'fr': 'Ex: Succursale principale au centre-ville'
    },
    'establecimiento.direccionPlaceholder': {
      'es': 'Ej: Calle Principal 123',
      'en': 'Ex: Main Street 123',
      'pt': 'Ej: Calle Principal 123',
      'de': 'Ej: Calle Principal 123',
      'fr': 'Ex: Rue Principale 123'
    },
    'establecimiento.telefonoPlaceholder': {
      'es': 'Ej: +52 55 1234 5678',
      'en': 'Ex: +52 55 1234 5678',
      'pt': 'Ej: +52 55 1234 5678',
      'de': 'Ej: +52 55 1234 5678',
      'fr': 'Ex: +52 55 1234 5678'
    },
    'establecimiento.emailPlaceholder': {
      'es': 'Ej: establecimiento@empresa.com',
      'en': 'Ex: establishment@company.com',
      'pt': 'Ej: establecimiento@Empresa.com',
      'de': 'Ej: establecimiento@Unternehmen.com',
      'fr': 'Ex: etablissement@entreprise.com'
    },
    'establecimiento.negocio': {
      'es': 'Negocio',
      'en': 'Business',
      'pt': 'Negócio',
      'de': 'Geschäft',
      'fr': 'Entreprise'
    },
    'establecimiento.pais': {
      'es': 'País',
      'en': 'Country',
      'pt': 'País',
      'de': 'Land',
      'fr': 'Pays'
    },
    'establecimiento.provincia': {
      'es': 'Provincia',
      'en': 'Province',
      'pt': 'Província',
      'de': 'Provinz',
      'fr': 'Province'
    },
    'establecimiento.municipio': {
      'es': 'Municipio',
      'en': 'Municipality',
      'pt': 'Município',
      'de': 'Gemeinde',
      'fr': 'Municipalité'
    },
    'establecimiento.codigoPostal': {
      'es': 'Código Postal',
      'en': 'Postal Code',
      'pt': 'Código Postal',
      'de': 'Postleitzahl',
      'fr': 'Code Postal'
    },
    'establecimiento.activo': {
      'es': 'Activo',
      'en': 'Active',
      'pt': 'Ativo',
      'de': 'Aktiv',
      'fr': 'Actif'
    },
    'establecimiento.activoHelp': {
      'es': 'Indica si el establecimiento está activo.',
      'en': 'Indicates if the establishment is active.',
      'pt': 'Indica si el establecimiento está Ativo.',
      'de': 'Indica si el establecimiento está Aktiv.',
      'fr': 'Indique si l’établissement est actif.'
    },
    'localidad.title': {
      'es': 'Localidades',
      'en': 'Localities',
      'pt': 'Localidades',
      'de': 'Localidades',
      'fr': 'Localités'
    },
    'localidad.subtitle': {
      'es': 'Gestión de localidades',
      'en': 'Localities management',
      'pt': 'Gestão de localidades',
      'de': 'Verwaltung de localidades',
      'fr': 'Gestion des localités'
    },
    'localidad.newTitle': {
      'es': 'Nueva Localidad',
      'en': 'New Locality',
      'pt': 'Nova Localidad',
      'de': 'Neue Localidad',
      'fr': 'Nouvelle Localité'
    },
    'localidad.editTitle': {
      'es': 'Editar Localidad',
      'en': 'Edit Locality',
      'pt': 'Editar Localidad',
      'de': 'Bearbeiten Localidad',
      'fr': 'Modifier Localité'
    },
    'localidad.detailTitle': {
      'es': 'Detalle de Localidad',
      'en': 'Locality Details',
      'pt': 'Detalhe de Localidad',
      'de': 'Detail de Localidad',
      'fr': 'Détails de la Localité'
    },
    'localidad.detailSubtitle': {
      'es': 'Visualización completa de la información de la localidad',
      'en': 'Complete view of locality information',
      'pt': 'Visualización completa de la Informação de la localidad',
      'de': 'Visualización completa de la Information de la localidad',
      'fr': 'Vue complète des informations de la localité'
    },
    'localidad.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'pt': 'Informação Principal',
      'de': 'Hauptinformation',
      'fr': 'Informations Principales'
    },
    'localidad.establecimiento': {
      'es': 'Establecimiento',
      'en': 'Establishment',
      'pt': 'Establecimiento',
      'de': 'Establecimiento',
      'fr': 'Établissement'
    },
    'localidad.almacen': {
      'es': 'Almacén',
      'en': 'Warehouse',
      'pt': 'Almacén',
      'de': 'Almacén',
      'fr': 'Entrepôt'
    },
    'localidad.cuentasContables': {
      'es': 'Cuentas Contables',
      'en': 'Accounting Accounts',
      'pt': 'Contas Contables',
      'de': 'Konten Contables',
      'fr': 'Comptes Comptables'
    },
    'localidad.cuentasContablesSubtitle': {
      'es': 'Configuración de cuentas para inventario, costos, ventas y devoluciones.',
      'en': 'Account settings for inventory, costs, sales, and returns.',
      'pt': 'Configuração de Contas para inventario, costos, ventas y devoluciones.',
      'de': 'Konfiguration de Konten para inventario, costos, ventas y devoluciones.',
      'fr': 'Configuration des comptes pour les stocks, coûts, ventes et retours.'
    },
    'localidad.cuentasInventarioCosto': {
      'es': 'Inventario y Costo',
      'en': 'Inventory and Cost',
      'pt': 'Inventario y Costo',
      'de': 'Inventario y Costo',
      'fr': 'Stock et Coût'
    },
    'localidad.cuentasVentaDevolucion': {
      'es': 'Venta y Devolución',
      'en': 'Sales and Returns',
      'pt': 'Venta y Devolución',
      'de': 'Venta y Devolución',
      'fr': 'Vente et Retour'
    },
    'localidad.cuentaInventario': {
      'es': 'Cuenta de Inventario',
      'en': 'Inventory Account',
      'pt': 'Conta de Inventario',
      'de': 'Konto de Inventario',
      'fr': 'Compte de Stock'
    },
    'localidad.cuentaCosto': {
      'es': 'Cuenta de Costo',
      'en': 'Cost Account',
      'pt': 'Conta de Costo',
      'de': 'Konto de Costo',
      'fr': 'Compte de Coût'
    },
    'localidad.cuentaVenta': {
      'es': 'Cuenta de Venta',
      'en': 'Sales Account',
      'pt': 'Conta de Venta',
      'de': 'Konto de Venta',
      'fr': 'Compte de Vente'
    },
    'localidad.cuentaDevolucion': {
      'es': 'Cuenta de Devolución',
      'en': 'Returns Account',
      'pt': 'Conta de Devolución',
      'de': 'Konto de Devolución',
      'fr': 'Compte de Retour'
    },
    'localidad.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'localidad.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'pt': 'Nome',
      'de': 'Name',
      'fr': 'Nom'
    },
    'localidad.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'pt': 'Descrição',
      'de': 'Beschreibung',
      'fr': 'Description'
    },
    'localidad.municipio': {
      'es': 'Municipio',
      'en': 'Municipality',
      'pt': 'Município',
      'de': 'Gemeinde',
      'fr': 'Municipalité'
    },
    'localidad.codigoPostal': {
      'es': 'Código Postal',
      'en': 'Postal Code',
      'pt': 'Código Postal',
      'de': 'Postleitzahl',
      'fr': 'Code Postal'
    },
    'localidad.codigoPlaceholder': {
      'es': 'Ej: LOC001',
      'en': 'Ex: LOC001',
      'pt': 'Ej: LOC001',
      'de': 'Ej: LOC001',
      'fr': 'Ex: LOC001'
    },
    'localidad.nombrePlaceholder': {
      'es': 'Ej: Centro Histórico',
      'en': 'Ex: Historic Center',
      'pt': 'Ej: Centro Histórico',
      'de': 'Ej: Centro Histórico',
      'fr': 'Ex: Centre Historique'
    },

    'moneda.title': {
      'es': 'Monedas',
      'en': 'Currencies',
      'pt': 'Monedas',
      'de': 'Monedas',
      'fr': 'Devises'
    },
    'moneda.subtitle': {
      'es': 'Gestión de monedas',
      'en': 'Currencies management',
      'pt': 'Gestão de monedas',
      'de': 'Verwaltung de monedas',
      'fr': 'Gestion des devises'
    },
    'moneda.newTitle': {
      'es': 'Nueva Moneda',
      'en': 'New Currency',
      'pt': 'Nova Moneda',
      'de': 'Neue Moneda',
      'fr': 'Nouvelle Devise'
    },
    'moneda.editTitle': {
      'es': 'Editar Moneda',
      'en': 'Edit Currency',
      'pt': 'Editar Moneda',
      'de': 'Bearbeiten Moneda',
      'fr': 'Modifier Devise'
    },
    'moneda.detailTitle': {
      'es': 'Detalle de Moneda',
      'en': 'Currency Details',
      'pt': 'Detalhe de Moneda',
      'de': 'Detail de Moneda',
      'fr': 'Détails de la Devise'
    },
    'moneda.detailSubtitle': {
      'es': 'Visualización completa de la información de la moneda',
      'en': 'Complete view of currency information',
      'pt': 'Visualización completa de la Informação de la moneda',
      'de': 'Visualización completa de la Information de la moneda',
      'fr': 'Vue complète des informations de la devise'
    },
    'moneda.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'pt': 'Informação Principal',
      'de': 'Hauptinformation',
      'fr': 'Informations Principales'
    },
    'moneda.infoTasaCambio': {
      'es': 'Información de Tasa de Cambio',
      'en': 'Exchange Rate Information',
      'pt': 'Informação de Tasa de Cambio',
      'de': 'Information de Tasa de Cambio',
      'fr': 'Informations de Taux de Change'
    },
    'moneda.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'moneda.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'pt': 'Nome',
      'de': 'Name',
      'fr': 'Nom'
    },
    'moneda.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'pt': 'Descrição',
      'de': 'Beschreibung',
      'fr': 'Description'
    },
    'moneda.simbolo': {
      'es': 'Símbolo',
      'en': 'Symbol',
      'pt': 'Simmbolo',
      'de': 'Jambolo',
      'fr': 'Symbole'
    },
    'moneda.cambio': {
      'es': 'Tasa de Cambio',
      'en': 'Exchange Rate',
      'pt': 'Tasa de Cambio',
      'de': 'Tasa de Cambio',
      'fr': 'Taux de Change'
    },
    'moneda.codigoPlaceholder': {
      'es': 'Ej: USD, EUR, MXN',
      'en': 'Ex: USD, EUR, MXN',
      'pt': 'Ej: USD, EUR, MXN',
      'de': 'Ej: USD, EUR, MXN',
      'fr': 'Ex: USD, EUR, MXN'
    },
    'moneda.nombrePlaceholder': {
      'es': 'Ej: Dólar Estadounidense',
      'en': 'Ex: US Dollar',
      'pt': 'Ej: Dólar Estadounidense',
      'de': 'Ej: Dólar Estadounidense',
      'fr': 'Ex: Dollar Américain'
    },
    'moneda.descripcionPlaceholder': {
      'es': 'Ej: Moneda oficial de Estados Unidos',
      'en': 'Ex: Official currency of United States',
      'pt': 'Ej: Moneda oficial de Estados Unidos',
      'de': 'Ej: Moneda oficial de Estados Unidos',
      'fr': 'Ex: Monnaie officielle des États-Unis'
    },
    'moneda.simboloPlaceholder': {
      'es': 'Ej: $, €, £',
      'en': 'Ex: $, €, £',
      'pt': 'Ej: $, €, £',
      'de': 'Ej: $, €, £',
      'fr': 'Ex: $, €, £'
    },
    'moneda.cambioPlaceholder': {
      'es': 'Ej: 1.00, 20.50',
      'en': 'Ex: 1.00, 20.50',
      'pt': 'Ej: 1.00, 20.50',
      'de': 'Ej: 1.00, 20.50',
      'fr': 'Ex: 1.00, 20.50'
    },

    'pais.title': {
      'es': 'Países',
      'en': 'Countries',
      'pt': 'Países',
      'de': 'Países',
      'fr': 'Pays'
    },
    'pais.subtitle': {
      'es': 'Gestión de países',
      'en': 'Countries management',
      'pt': 'Gestão de países',
      'de': 'Verwaltung de países',
      'fr': 'Gestion des pays'
    },
    'pais.newTitle': {
      'es': 'Nuevo País',
      'en': 'New Country',
      'pt': 'Novo País',
      'de': 'Neu Land',
      'fr': 'Nouveau Pays'
    },
    'pais.editTitle': {
      'es': 'Editar País',
      'en': 'Edit Country',
      'pt': 'Editar País',
      'de': 'Bearbeiten Land',
      'fr': 'Modifier Pays'
    },
    'pais.detailTitle': {
      'es': 'Detalle de País',
      'en': 'Country Details',
      'pt': 'Detalhe de País',
      'de': 'Detail de Land',
      'fr': 'Détails du Pays'
    },
    'pais.detailSubtitle': {
      'es': 'Visualización completa de la información del país',
      'en': 'Complete view of country information',
      'pt': 'Visualización completa de la Informação del País',
      'de': 'Visualización completa de la Information del Land',
      'fr': 'Vue complète des informations du pays'
    },
    'pais.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'pt': 'Informação Principal',
      'de': 'Hauptinformation',
      'fr': 'Informations Principales'
    },
    'pais.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'pais.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'pt': 'Nome',
      'de': 'Name',
      'fr': 'Nom'
    },
    'pais.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'pt': 'Descrição',
      'de': 'Beschreibung',
      'fr': 'Description'
    },
    'pais.nacionalidad': {
      'es': 'Nacionalidad',
      'en': 'Nationality',
      'pt': 'Nacionalidad',
      'de': 'Nacionalidad',
      'fr': 'Nationalité'
    },
    'pais.codigoPlaceholder': {
      'es': 'Ej: MX, US, ES',
      'en': 'Ex: MX, US, ES',
      'fr': 'Ex: MX, US, ES',
      'pt': 'Ej: MX, US, ES',
      'de': 'Ej: MX, US, ES'
    },
    'pais.nombrePlaceholder': {
      'es': 'Ej: México',
      'en': 'Ex: Mexico',
      'pt': 'Ej: México',
      'de': 'Ej: México',
      'fr': 'Ex: Mexique'
    },
    'pais.descripcionPlaceholder': {
      'es': 'Ej: País de América del Norte',
      'en': 'Ex: Country in North America',
      'pt': 'Ej: País de América del Norte',
      'de': 'Ej: Land de América del Norte',
      'fr': 'Ex: Pays en Amérique du Nord'
    },
    'pais.nacionalidadPlaceholder': {
      'es': 'Ej: Mexicana',
      'en': 'Ex: Mexican',
      'pt': 'Ej: Mexicana',
      'de': 'Ej: Mexicana',
      'fr': 'Ex: Mexicaine'
    },

    'provincia.title': {
      'es': 'Provincias',
      'en': 'Provinces',
      'pt': 'Provincias',
      'de': 'Provincias',
      'fr': 'Provinces'
    },
    'provincia.subtitle': {
      'es': 'Gestión de provincias',
      'en': 'Provinces management',
      'pt': 'Gestão de provincias',
      'de': 'Verwaltung de provincias',
      'fr': 'Gestion des provinces'
    },
    'provincia.newTitle': {
      'es': 'Nueva Provincia',
      'en': 'New Province',
      'pt': 'Nova Província',
      'de': 'Neue Provinz',
      'fr': 'Nouvelle Province'
    },
    'provincia.editTitle': {
      'es': 'Editar Provincia',
      'en': 'Edit Province',
      'pt': 'Editar Província',
      'de': 'Bearbeiten Provinz',
      'fr': 'Modifier Province'
    },
    'provincia.detailTitle': {
      'es': 'Detalle de Provincia',
      'en': 'Province Details',
      'pt': 'Detalhe de Província',
      'de': 'Detail de Provinz',
      'fr': 'Détails de la Province'
    },
    'provincia.detailSubtitle': {
      'es': 'Visualización completa de la información de la provincia',
      'en': 'Complete view of province information',
      'pt': 'Visualización completa de la Informação de la Província',
      'de': 'Visualización completa de la Information de la Provinz',
      'fr': 'Vue complète des informations de la province'
    },
    'provincia.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'pt': 'Informação Principal',
      'de': 'Hauptinformation',
      'fr': 'Informations Principales'
    },
    'provincia.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'provincia.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'pt': 'Nome',
      'de': 'Name',
      'fr': 'Nom'
    },
    'provincia.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'pt': 'Descrição',
      'de': 'Beschreibung',
      'fr': 'Description'
    },
    'provincia.pais': {
      'es': 'País',
      'en': 'Country',
      'pt': 'País',
      'de': 'Land',
      'fr': 'Pays'
    },
    'provincia.codigoPlaceholder': {
      'es': 'Ej: PROV01',
      'en': 'Ex: PROV01',
      'pt': 'Ej: PROV01',
      'de': 'Ej: PROV01',
      'fr': 'Ex: PROV01'
    },
    'provincia.nombrePlaceholder': {
      'es': 'Ej: Ciudad de México',
      'en': 'Ex: Mexico City',
      'pt': 'Ej: Ciudad de México',
      'de': 'Ej: Ciudad de México',
      'fr': 'Ex: Mexico'
    },
    'provincia.descripcionPlaceholder': {
      'es': 'Ej: Capital del país',
      'en': 'Ex: Capital of the country',
      'pt': 'Ej: Capital del País',
      'de': 'Ej: Capital del Land',
      'fr': 'Ex: Capitale du pays'
    },

    'municipio.title': {
      'es': 'Municipios',
      'en': 'Municipalities',
      'pt': 'Municipios',
      'de': 'Municipios',
      'fr': 'Municipalités'
    },
    'municipio.subtitle': {
      'es': 'Gestión de municipios',
      'en': 'Municipalities management',
      'pt': 'Gestão de municipios',
      'de': 'Verwaltung de municipios',
      'fr': 'Gestion des municipalités'
    },
    'municipio.newTitle': {
      'es': 'Nuevo Municipio',
      'en': 'New Municipality',
      'pt': 'Novo Município',
      'de': 'Neu Gemeinde',
      'fr': 'Nouvelle Municipalité'
    },
    'municipio.editTitle': {
      'es': 'Editar Municipio',
      'en': 'Edit Municipality',
      'pt': 'Editar Município',
      'de': 'Bearbeiten Gemeinde',
      'fr': 'Modifier Municipalité'
    },
    'municipio.detailTitle': {
      'es': 'Detalle de Municipio',
      'en': 'Municipality Details',
      'pt': 'Detalhe de Município',
      'de': 'Detail de Gemeinde',
      'fr': 'Détails de la Municipalité'
    },
    'municipio.detailSubtitle': {
      'es': 'Visualización completa de la información del municipio',
      'en': 'Complete view of municipality information',
      'pt': 'Visualización completa de la Informação del Município',
      'de': 'Visualización completa de la Information del Gemeinde',
      'fr': 'Vue complète des informations de la municipalité'
    },
    'municipio.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'pt': 'Informação Principal',
      'de': 'Hauptinformation',
      'fr': 'Informations Principales'
    },
    'municipio.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'municipio.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'pt': 'Nome',
      'de': 'Name',
      'fr': 'Nom'
    },
    'municipio.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'pt': 'Descrição',
      'de': 'Beschreibung',
      'fr': 'Description'
    },
    'municipio.provincia': {
      'es': 'Provincia',
      'en': 'Province',
      'pt': 'Província',
      'de': 'Provinz',
      'fr': 'Province'
    },
    'municipio.codigoPlaceholder': {
      'es': 'Ej: MUN001',
      'en': 'Ex: MUN001',
      'pt': 'Ej: MUN001',
      'de': 'Ej: MUN001',
      'fr': 'Ex: MUN001'
    },
    'municipio.nombrePlaceholder': {
      'es': 'Ej: Benito Juárez',
      'en': 'Ex: Benito Juarez',
      'pt': 'Ej: Benito Juárez',
      'de': 'Ej: Benito Juárez',
      'fr': 'Ex: Benito Juarez'
    },
    'municipio.descripcionPlaceholder': {
      'es': 'Ej: Municipio central de la provincia',
      'en': 'Ex: Central municipality of the province',
      'pt': 'Ej: Município central de la Província',
      'de': 'Ej: Gemeinde central de la Provinz',
      'fr': 'Ex: Municipalité centrale de la province'
    },

    'codigoPostal.title': {
      'es': 'Códigos Postales',
      'en': 'Postal Codes',
      'pt': 'Códigos Postales',
      'de': 'Códigos Postales',
      'fr': 'Codes Postaux'
    },
    'codigoPostal.subtitle': {
      'es': 'Gestión de códigos postales',
      'en': 'Postal codes management',
      'pt': 'Gestão de códigos postales',
      'de': 'Verwaltung de códigos postales',
      'fr': 'Gestion des codes postaux'
    },
    'codigoPostal.newTitle': {
      'es': 'Nuevo Código Postal',
      'en': 'New Postal Code',
      'pt': 'Novo Código Postal',
      'de': 'Neu Code Postal',
      'fr': 'Nouveau Code Postal'
    },
    'codigoPostal.editTitle': {
      'es': 'Editar Código Postal',
      'en': 'Edit Postal Code',
      'pt': 'Editar Código Postal',
      'de': 'Bearbeiten Code Postal',
      'fr': 'Modifier Code Postal'
    },
    'codigoPostal.detailTitle': {
      'es': 'Detalle de Código Postal',
      'en': 'Postal Code Details',
      'pt': 'Detalhe de Código Postal',
      'de': 'Detail de Code Postal',
      'fr': 'Détails du Code Postal'
    },
    'codigoPostal.detailSubtitle': {
      'es': 'Visualización completa de la información del código postal',
      'en': 'Complete view of postal code information',
      'pt': 'Visualización completa de la Informação del Código Postal',
      'de': 'Visualización completa de la Information del Code postal',
      'fr': 'Vue complète des informations du code postal'
    },
    'codigoPostal.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'pt': 'Informação Principal',
      'de': 'Hauptinformation',
      'fr': 'Informations Principales'
    },
    'codigoPostal.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'codigoPostal.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'pt': 'Descrição',
      'de': 'Beschreibung',
      'fr': 'Description'
    },
    'codigoPostal.pais': {
      'es': 'País',
      'en': 'Country',
      'pt': 'País',
      'de': 'Land',
      'fr': 'Pays'
    },
    'codigoPostal.provincia': {
      'es': 'Provincia',
      'en': 'Province',
      'pt': 'Província',
      'de': 'Provinz',
      'fr': 'Province'
    },
    'codigoPostal.municipio': {
      'es': 'Municipio',
      'en': 'Municipality',
      'pt': 'Município',
      'de': 'Gemeinde',
      'fr': 'Municipalité'
    },
    'codigoPostal.selectPaisFirst': {
      'es': 'Seleccione un país primero',
      'en': 'Select a country first',
      'pt': 'Seleccione un País primero',
      'de': 'Seleccione un Land primero',
      'fr': 'Sélectionnez d\'abord un pays'
    },
    'codigoPostal.selectProvinciaFirst': {
      'es': 'Seleccione una provincia primero',
      'en': 'Select a province first',
      'pt': 'Seleccione una Província primero',
      'de': 'Seleccione una Provinz primero',
      'fr': 'Sélectionnez d\'abord une province'
    },
    'codigoPostal.paisDescripcion': {
      'es': 'País',
      'en': 'Country',
      'pt': 'País',
      'de': 'Land',
      'fr': 'Pays'
    },
    'codigoPostal.provinciaDescripcion': {
      'es': 'Provincia',
      'en': 'Province',
      'pt': 'Província',
      'de': 'Provinz',
      'fr': 'Province'
    },
    'codigoPostal.municipioDescripcion': {
      'es': 'Municipio',
      'en': 'Municipality',
      'pt': 'Município',
      'de': 'Gemeinde',
      'fr': 'Municipalité'
    },
    'codigoPostal.codigoPlaceholder': {
      'es': 'Ej: 03100',
      'en': 'Ex: 03100',
      'pt': 'Ej: 03100',
      'de': 'Ej: 03100',
      'fr': 'Ex: 03100'
    },
    'codigoPostal.descripcionPlaceholder': {
      'es': 'Ej: Colonia Del Valle Centro',
      'en': 'Ex: Del Valle Centro Neighborhood',
      'pt': 'Ej: Colonia Del Valle Centro',
      'de': 'Ej: Colonia Del Valle Centro',
      'fr': 'Ex: Quartier Del Valle Centro'
    },

    // ═══════════════════════════════════════════════════════════
    // 📋 CLASIFICADOR
    // ═══════════════════════════════════════════════════════════
    'linea.title': {
      'es': 'Líneas',
      'en': 'Lines',
      'pt': 'Líneas',
      'de': 'Líneas',
      'fr': 'Lignes'
    },
    'linea.subtitle': {
      'es': 'Gestión de líneas',
      'en': 'Lines management',
      'pt': 'Gestão de líneas',
      'de': 'Verwaltung de líneas',
      'fr': 'Gestion des lignes'
    },
    'linea.newTitle': {
      'es': 'Nueva Línea',
      'en': 'New Line',
      'pt': 'Nova Línea',
      'de': 'Neue Línea',
      'fr': 'Nouvelle Ligne'
    },
    'linea.editTitle': {
      'es': 'Editar Línea',
      'en': 'Edit Line',
      'pt': 'Editar Línea',
      'de': 'Bearbeiten Línea',
      'fr': 'Modifier Ligne'
    },
    'linea.detailTitle': {
      'es': 'Detalle de Línea',
      'en': 'Line Details',
      'pt': 'Detalhe de Línea',
      'de': 'Detail de Línea',
      'fr': 'Détails de la Ligne'
    },
    'linea.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'linea.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'pt': 'Nome',
      'de': 'Name',
      'fr': 'Nom'
    },
    'linea.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'pt': 'Descrição',
      'de': 'Beschreibung',
      'fr': 'Description'
    },
    'linea.descripcionPlaceholder': {
      'es': 'Ej: Alimentos, Bebidas',
      'en': 'Ex: Food, Beverages',
      'pt': 'Ej: Alimentos, Bebidas',
      'de': 'Ej: Alimentos, Bebidas',
      'fr': 'Ex: Aliments, Boissons'
    },

    'subLinea.title': {
      'es': 'Sublíneas',
      'en': 'Sublines',
      'pt': 'Sublíneas',
      'de': 'Sublíneas',
      'fr': 'Sous-lignes'
    },
    'subLinea.subtitle': {
      'es': 'Gestión de sublíneas',
      'en': 'Sublines management',
      'pt': 'Gestão de sublíneas',
      'de': 'Verwaltung de sublíneas',
      'fr': 'Gestion des sous-lignes'
    },
    'subLinea.newTitle': {
      'es': 'Nueva Sublínea',
      'en': 'New Subline',
      'pt': 'Nova Sublínea',
      'de': 'Neue Sublínea',
      'fr': 'Nouvelle Sous-ligne'
    },
    'subLinea.editTitle': {
      'es': 'Editar Sublínea',
      'en': 'Edit Subline',
      'pt': 'Editar Sublínea',
      'de': 'Bearbeiten Sublínea',
      'fr': 'Modifier Sous-ligne'
    },
    'subLinea.detailTitle': {
      'es': 'Detalle de Sublínea',
      'en': 'Subline Details',
      'pt': 'Detalhe de Sublínea',
      'de': 'Detail de Sublínea',
      'fr': 'Détails de la Sous-ligne'
    },
    'subLinea.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'subLinea.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'pt': 'Nome',
      'de': 'Name',
      'fr': 'Nom'
    },
    'subLinea.linea': {
      'es': 'Línea',
      'en': 'Line',
      'pt': 'Línea',
      'de': 'Línea',
      'fr': 'Ligne'
    },
    'subLinea.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'pt': 'Descrição',
      'de': 'Beschreibung',
      'fr': 'Description'
    },
    'sub-linea.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'sub-linea.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'pt': 'Descrição',
      'de': 'Beschreibung',
      'fr': 'Description'
    },
    'sub-linea.lineaId': {
      'es': 'Línea',
      'en': 'Line',
      'pt': 'Línea',
      'de': 'Línea',
      'fr': 'Ligne'
    },
    'sub-linea.editTitle': {
      'es': 'Editar Sublínea',
      'en': 'Edit Subline',
      'pt': 'Editar Sublínea',
      'de': 'Bearbeiten Sublínea',
      'fr': 'Modifier Sous-ligne'
    },
    'sub-linea.newTitle': {
      'es': 'Nueva Sublínea',
      'en': 'New Subline',
      'pt': 'Nova Sublínea',
      'de': 'Neue Sublínea',
      'fr': 'Nouvelle Sous-ligne'
    },
    'sub-linea.descripcionPlaceholder': {
      'es': 'Ej: Confecciones Exteriores',
      'en': 'Ex: Outdoor Clothing',
      'pt': 'Ej: Confecciones Exteriores',
      'de': 'Ej: Confecciones Exteriores',
      'fr': 'Ex: Vêtements d\'Extérieur'
    },

    'unidadMedida.title': {
      'es': 'Unidades de Medida',
      'en': 'Units of Measure',
      'pt': 'Unidades de Medida',
      'de': 'Unidades de Medida',
      'fr': 'Unités de Mesure'
    },
    'unidadMedida.subtitle': {
      'es': 'Gestión de unidades de medida',
      'en': 'Units of measure management',
      'pt': 'Gestão de unidades de medida',
      'de': 'Verwaltung de unidades de medida',
      'fr': 'Gestion des unités de mesure'
    },
    'unidadMedida.newTitle': {
      'es': 'Nueva Unidad de Medida',
      'en': 'New Unit of Measure',
      'pt': 'Nova Unidad de Medida',
      'de': 'Neue Unidad de Medida',
      'fr': 'Nouvelle Unité de Mesure'
    },
    'unidadMedida.editTitle': {
      'es': 'Editar Unidad de Medida',
      'en': 'Edit Unit of Measure',
      'pt': 'Editar Unidad de Medida',
      'de': 'Bearbeiten Unidad de Medida',
      'fr': 'Modifier Unité de Mesure'
    },
    'unidadMedida.detailTitle': {
      'es': 'Detalle de Unidad de Medida',
      'en': 'Unit of Measure Details',
      'pt': 'Detalhe de Unidad de Medida',
      'de': 'Detail de Unidad de Medida',
      'fr': 'Détails de l\'Unité de Mesure'
    },
    'unidadMedida.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'unidadMedida.abreviatura': {
      'es': 'Abreviatura',
      'en': 'Abbreviation',
      'pt': 'Abreviatura',
      'de': 'Abreviatura',
      'fr': 'Abréviation'
    },
    'unidadMedida.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'pt': 'Descrição',
      'de': 'Beschreibung',
      'fr': 'Description'
    },
    'unidadMedida.descripcionPlaceholder': {
      'es': 'Ej: Unidad, Kilogramo, Litro',
      'en': 'Ex: Unit, Kilogram, Liter',
      'fr': 'Ex: Unité, Kilogramme, Litre',
      'pt': 'Ej: Unidad, Kilogramo, Litro',
      'de': 'Ej: Unidad, Kilogramo, Litro'
    },
    'unidadMedida.codigoPlaceholder': {
      'es': 'Ej: UNO',
      'en': 'Ex: UNE',
      'pt': 'Ej: UNO',
      'de': 'Ej: UNO',
      'fr': 'Ex: UNÉ'
    },

    // ═══════════════════════════════════════════════════════════
    // 🔄 OPERACIONES
    // ═══════════════════════════════════════════════════════════
    'transaccion.title': {
      'es': 'Transacciones',
      'en': 'Transactions',
      'pt': 'Transacciones',
      'de': 'Transacciones',
      'fr': 'Transactions'
    },
    'transaccion.subtitle': {
      'es': 'Gestión de transacciones',
      'en': 'Transactions management',
      'pt': 'Gestão de transacciones',
      'de': 'Verwaltung de transacciones',
      'fr': 'Gestion des transactions'
    },
    'transaccion.newTitle': {
      'es': 'Nueva Transacción',
      'en': 'New Transaction',
      'pt': 'Nova Transacción',
      'de': 'Neue Transacción',
      'fr': 'Nouvelle Transaction'
    },
    'transaccion.editTitle': {
      'es': 'Editar Transacción',
      'en': 'Edit Transaction',
      'pt': 'Editar Transacción',
      'de': 'Bearbeiten Transacción',
      'fr': 'Modifier Transaction'
    },
    'transaccion.detailTitle': {
      'es': 'Detalle de Transacción',
      'en': 'Transaction Details',
      'pt': 'Detalhe de Transacción',
      'de': 'Detail de Transacción',
      'fr': 'Détails de la Transaction'
    },
    'transaccion.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'transaccion.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'pt': 'Nome',
      'de': 'Name',
      'fr': 'Nom'
    },
    'transaccion.tipo': {
      'es': 'Tipo',
      'en': 'Type',
      'pt': 'Tipo',
      'de': 'Tipo',
      'fr': 'Type'
    },
    'transaccion.ingreso': {
      'es': 'Ingreso',
      'en': 'Income',
      'pt': 'Ingreso',
      'de': 'Ingreso',
      'fr': 'Revenu'
    },
    'transaccion.egreso': {
      'es': 'Egreso',
      'en': 'Expense',
      'pt': 'Egreso',
      'de': 'Egreso',
      'fr': 'Dépense'
    },

    'conceptoAjuste.title': {
      'es': 'Conceptos de Ajuste',
      'en': 'Adjustment Concepts',
      'pt': 'Conceptos de Ajuste',
      'de': 'Conceptos de Ajuste',
      'fr': 'Concepts d\'Ajustement'
    },
    'conceptoAjuste.subtitle': {
      'es': 'Gestión de conceptos de ajuste',
      'en': 'Adjustment concepts management',
      'pt': 'Gestão de conceptos de ajuste',
      'de': 'Verwaltung de conceptos de ajuste',
      'fr': 'Gestion des concepts d\'ajustement'
    },
    'conceptoAjuste.newTitle': {
      'es': 'Nuevo Concepto de Ajuste',
      'en': 'New Adjustment Concept',
      'pt': 'Novo Concepto de Ajuste',
      'de': 'Neu Concepto de Ajuste',
      'fr': 'Nouveau Concept d\'Ajustement'
    },
    'conceptoAjuste.editTitle': {
      'es': 'Editar Concepto de Ajuste',
      'en': 'Edit Adjustment Concept',
      'pt': 'Editar Concepto de Ajuste',
      'de': 'Bearbeiten Concepto de Ajuste',
      'fr': 'Modifier Concept d\'Ajustement'
    },
    'conceptoAjuste.detailTitle': {
      'es': 'Detalle de Concepto de Ajuste',
      'en': 'Adjustment Concept Details',
      'pt': 'Detalhe de Concepto de Ajuste',
      'de': 'Detail de Concepto de Ajuste',
      'fr': 'Détails du Concept d\'Ajustement'
    },
    'conceptoAjuste.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'conceptoAjuste.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'pt': 'Nome',
      'de': 'Name',
      'fr': 'Nom'
    },
    'conceptoAjuste.tipo': {
      'es': 'Tipo de Ajuste',
      'en': 'Adjustment Type',
      'pt': 'Tipo de Ajuste',
      'de': 'Tipo de Ajuste',
      'fr': 'Type d\'Ajustement'
    },
    'conceptoAjuste.aumento': {
      'es': 'Aumento',
      'en': 'Increase',
      'pt': 'Aumento',
      'de': 'Aumento',
      'fr': 'Augmentation'
    },
    'conceptoAjuste.disminucion': {
      'es': 'Disminución',
      'en': 'Decrease',
      'pt': 'Disminución',
      'de': 'Disminución',
      'fr': 'Diminution'
    },

    // ═══════════════════════════════════════════════════════════
    // 📦 PRODUCTO
    // ═══════════════════════════════════════════════════════════
    'producto.title': {
      'es': 'Productos',
      'en': 'Products',
      'pt': 'Produtos',
      'de': 'Produkte',
      'fr': 'Produits'
    },
    'producto.subtitle': {
      'es': 'Gestión de productos',
      'en': 'Products management',
      'pt': 'Gestão de Produtos',
      'de': 'Verwaltung de Produkte',
      'fr': 'Gestion des produits'
    },
    'producto.newTitle': {
      'es': 'Nuevo Producto',
      'en': 'New Product',
      'pt': 'Novo Produto',
      'de': 'Neu Produkt',
      'fr': 'Nouveau Produit'
    },
    'producto.editTitle': {
      'es': 'Editar Producto',
      'en': 'Edit Product',
      'pt': 'Editar Produto',
      'de': 'Bearbeiten Produkt',
      'fr': 'Modifier Produit'
    },
    'producto.detailTitle': {
      'es': 'Detalle de Producto',
      'en': 'Product Details',
      'pt': 'Detalhe de Produto',
      'de': 'Detail de Produkt',
      'fr': 'Détails du Produit'
    },
    'producto.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'producto.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'pt': 'Nome',
      'de': 'Name',
      'fr': 'Nom'
    },
    'producto.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'pt': 'Descrição',
      'de': 'Beschreibung',
      'fr': 'Description'
    },
    'producto.precio': {
      'es': 'Precio',
      'en': 'Price',
      'pt': 'Precio',
      'de': 'Precio',
      'fr': 'Prix'
    },
    'producto.costo': {
      'es': 'Costo',
      'en': 'Cost',
      'pt': 'Costo',
      'de': 'Costo',
      'fr': 'Coût'
    },
    'producto.stock': {
      'es': 'Stock',
      'en': 'Stock',
      'pt': 'Stock',
      'de': 'Stock',
      'fr': 'Stock'
    },
    'producto.unidadMedida': {
      'es': 'Unidad de Medida',
      'en': 'Unit of Measure',
      'pt': 'Unidad de Medida',
      'de': 'Unidad de Medida',
      'fr': 'Unité de Mesure'
    },
    'producto.linea': {
      'es': 'Línea',
      'en': 'Line',
      'pt': 'Línea',
      'de': 'Línea',
      'fr': 'Ligne'
    },
    'producto.sublinea': {
      'es': 'Sublínea',
      'en': 'Subline',
      'pt': 'Sublínea',
      'de': 'Sublínea',
      'fr': 'Sous-ligne'
    },

    // ═══════════════════════════════════════════════════════════
    // 📊 DASHBOARD
    // ═══════════════════════════════════════════════════════════
    'dashboard.title': {
      'es': 'Panel de Control',
      'en': 'Dashboard',
      'pt': 'Panel de Control',
      'de': 'Panel de Control',
      'fr': 'Tableau de Bord'
    },
    'dashboard.subtitle': {
      'es': 'Bienvenido a GoldBusiness ERP',
      'en': 'Welcome to GoldBusiness ERP',
      'pt': 'Bienvenido a GoldBusiness ERP',
      'de': 'Bienvenido a GoldBusiness ERP',
      'fr': 'Bienvenue à GoldBusiness ERP'
    },
    'dashboard.viewReports': {
      'es': 'Ver Reportes',
      'en': 'View Reports',
      'pt': 'Ver Reportes',
      'de': 'Anzeigen Reportes',
      'fr': 'Voir Rapports'
    },
    'dashboard.totalAccounts': {
      'es': 'Total de Cuentas',
      'en': 'Total Accounts',
      'pt': 'Total de Contas',
      'de': 'Total de Konten',
      'fr': 'Total des Comptes'
    },
    'dashboard.activeUsers': {
      'es': 'Usuarios Activos',
      'en': 'Active Users',
      'pt': 'Usuários Activos',
      'de': 'Benutzer Activos',
      'fr': 'Utilisateurs Actifs'
    },
    'dashboard.accountGroups': {
      'es': 'Grupos de Cuenta',
      'en': 'Account Groups',
      'pt': 'Grupos de Conta',
      'de': 'Gruppen de Konto',
      'fr': 'Groupes de Comptes'
    },
    'dashboard.pendingTasks': {
      'es': 'Tareas Pendientes',
      'en': 'Pending Tasks',
      'pt': 'Tareas Pendientes',
      'de': 'Tareas Pendientes',
      'fr': 'Tâches en Attente'
    },
    'dashboard.recentActivities': {
      'es': 'Actividades Recientes',
      'en': 'Recent Activities',
      'pt': 'Actividades Recientes',
      'de': 'Actividades Recientes',
      'fr': 'Activités Récentes'
    },
    'dashboard.quickAccess': {
      'es': 'Acceso Rápido',
      'en': 'Quick Access',
      'pt': 'Acceso Rápido',
      'de': 'Acceso Rápido',
      'fr': 'Accès Rapide'
    },
    'dashboard.monthlyTrends': {
      'es': 'Tendencias Mensuales',
      'en': 'Monthly Trends',
      'pt': 'Tendencias Mensuales',
      'de': 'Tendencias Mensuales',
      'fr': 'Tendances Mensuelles'
    },
    'dashboard.accountsDistribution': {
      'es': 'Distribución de Cuentas',
      'en': 'Accounts Distribution',
      'pt': 'Distribución de Contas',
      'de': 'Distribución de Konten',
      'fr': 'Distribution des Comptes'
    },
    'dashboard.chartComingSoon': {
      'es': 'Gráfico próximamente...',
      'en': 'Chart coming soon...',
      'pt': 'Gráfico próximamente...',
      'de': 'Gráfico próximamente...',
      'fr': 'Graphique à venir...'
    },
    'dashboard.timeAgo': {
      'es': 'hace {0}',
      'en': '{0} ago',
      'fr': 'il y a {0}',
      'pt': 'há {0}',
      'de': 'vor {0}'
    },
    'dashboard.activity.accountCreated': {
      'es': 'Cuenta creada:',
      'en': 'Account created:',
      'pt': 'Conta creada:',
      'de': 'Konto creada:',
      'fr': 'Compte créé:'
    },
    'dashboard.activity.accountModified': {
      'es': 'Cuenta modificada:',
      'en': 'Account modified:',
      'pt': 'Conta modificada:',
      'de': 'Konto modificada:',
      'fr': 'Compte modifié:'
    },
    'dashboard.activity.configUpdated': {
      'es': 'Configuración actualizada:',
      'en': 'Configuration updated:',
      'pt': 'Configuração actualizada:',
      'de': 'Konfiguration actualizada:',
      'fr': 'Configuration mise à jour:'
    },
    'dashboard.quickLinks.newAccount': {
      'es': 'Nueva Cuenta',
      'en': 'New Account',
      'pt': 'Nova Conta',
      'de': 'Neue Konto',
      'fr': 'Nouveau Compte'
    },
    'dashboard.quickLinks.viewAccounts': {
      'es': 'Ver Cuentas',
      'en': 'View Accounts',
      'pt': 'Ver Contas',
      'de': 'Anzeigen Konten',
      'fr': 'Voir Comptes'
    },
    'dashboard.quickLinks.configuration': {
      'es': 'Configuración',
      'en': 'Configuration',
      'pt': 'Configuração',
      'de': 'Konfiguration',
      'fr': 'Configuration'
    },
    'dashboard.quickLinks.reports': {
      'es': 'Reportes',
      'en': 'Reports',
      'pt': 'Reportes',
      'de': 'Reportes',
      'fr': 'Rapports'
    },
    'dashboard.time.minutes': {
      'es': 'hace {0} minutos',
      'en': '{0} minutes ago',
      'fr': 'il y a {0} minutes',
      'pt': 'há {0} minutos',
      'de': 'vor {0} Minuten'
    },
    'dashboard.time.oneHour': {
      'es': 'hace 1 hora',
      'en': '1 hour ago',
      'pt': 'hace 1 hora',
      'de': 'hace 1 hora',
      'fr': 'il y a 1 heure'
    },
    'dashboard.time.hours': {
      'es': 'hace {0} horas',
      'en': '{0} hours ago',
      'fr': 'il y a {0} heures',
      'pt': 'há {0} horas',
      'de': 'vor {0} Stunden'
    },
    'dashboard.time.oneDay': {
      'es': 'hace 1 día',
      'en': '1 day ago',
      'pt': 'hace 1 día',
      'de': 'hace 1 día',
      'fr': 'il y a 1 jour'
    },
    'dashboard.time.days': {
      'es': 'hace {0} días',
      'en': '{0} days ago',
      'fr': 'il y a {0} jours',
      'pt': 'há {0} dias',
      'de': 'vor {0} Tagen'
    },

    // ═══════════════════════════════════════════════════════════
    // 📋 COMÚN - Botones y Acciones
    // ═══════════════════════════════════════════════════════════
    'common.new': {
      'es': 'Nuevo',
      'en': 'New',
      'pt': 'Novo',
      'de': 'Neu',
      'fr': 'Nouveau'
    },
    'common.edit': {
      'es': 'Editar',
      'en': 'Edit',
      'pt': 'Editar',
      'de': 'Bearbeiten',
      'fr': 'Modifier'
    },
    'common.delete': {
      'es': 'Eliminar',
      'en': 'Delete',
      'pt': 'Excluir',
      'de': 'Löschen',
      'fr': 'Supprimer'
    },
    'common.view': {
      'es': 'Ver detalles',
      'en': 'View details',
      'pt': 'Ver detalhes',
      'de': 'Details anzeigen',
      'fr': 'Voir détails'
    },
    'common.save': {
      'es': 'Guardar',
      'en': 'Save',
      'pt': 'Salvar',
      'de': 'Speichern',
      'fr': 'Enregistrer'
    },
    'common.cancel': {
      'es': 'Cancelar',
      'en': 'Cancel',
      'pt': 'Cancelar',
      'de': 'Abbrechen',
      'fr': 'Annuler'
    },
    'common.back': {
      'es': 'Volver',
      'en': 'Back',
      'pt': 'Voltar',
      'de': 'Zurück',
      'fr': 'Retour'
    },
    'common.search': {
      'es': 'Buscar',
      'en': 'Search',
      'pt': 'Buscar',
      'de': 'Suchen',
      'fr': 'Rechercher'
    },
    'common.actions': {
      'es': 'Acciones',
      'en': 'Actions',
      'pt': 'Ações',
      'de': 'Aktionen',
      'fr': 'Actions'
    },
    'common.noData': {
      'es': 'No hay datos disponibles',
      'en': 'No data available',
      'pt': 'Não hay datos disponibles',
      'de': 'Nein hay datos disponibles',
      'fr': 'Aucune donnée disponible'
    },
    'common.ifNotChecked': {
      'es': '(si no está marcado)',
      'en': '(if not checked)',
      'pt': '(si Não está marcado)',
      'de': '(si Nein está marcado)',
      'fr': '(si non coché)'
    },
    'common.information': {
      'es': 'Información',
      'en': 'Information',
      'pt': 'Informação',
      'de': 'Information',
      'fr': 'Information'
    },
    'common.createdAt': {
      'es': 'Fecha de Creación',
      'en': 'Created At',
      'pt': 'Data de Criação',
      'de': 'Erstellungsdatum',
      'fr': 'Date de Création'
    },
    'common.updatedAt': {
      'es': 'Última Actualización',
      'en': 'Updated At',
      'pt': 'Última Atualização',
      'de': 'Zuletzt aktualisiert',
      'fr': 'Dernière Mise à Jour'
    },
    'common.id': {
      'es': 'ID',
      'en': 'ID',
      'pt': 'ID',
      'de': 'ID',
      'fr': 'ID'
    },
    'common.status': {
      'es': 'Estado',
      'en': 'Status',
      'pt': 'Estado',
      'de': 'Estado',
      'fr': 'Statut'
    },
    'common.active': {
      'es': 'Activo',
      'en': 'Active',
      'pt': 'Ativo',
      'de': 'Aktiv',
      'fr': 'Actif'
    },
    'common.yes': {
      'es': 'Sí',
      'en': 'Yes',
      'pt': 'Sim',
      'de': 'Ja',
      'fr': 'Oui'
    },
    'common.no': {
      'es': 'No',
      'en': 'No',
      'pt': 'Não',
      'de': 'Nein',
      'fr': 'Non'
    },
    'common.inactive': {
      'es': 'Inactivo',
      'en': 'Inactive',
      'pt': 'Inativo',
      'de': 'Inaktiv',
      'fr': 'Inactif'
    },
    'common.show': {
      'es': 'Mostrar',
      'en': 'Show',
      'pt': 'Mostrar',
      'de': 'Mostrar',
      'fr': 'Afficher'
    },
    'common.showing': {
      'es': 'Mostrando',
      'en': 'Showing',
      'pt': 'Mostrando',
      'de': 'Mostrando',
      'fr': 'Affichage'
    },
    'common.of': {
      'es': 'de',
      'en': 'of',
      'pt': 'de',
      'de': 'de',
      'fr': 'sur'
    },
    'common.previous': {
      'es': 'Anterior',
      'en': 'Previous',
      'pt': 'Anterior',
      'de': 'Vorherige',
      'fr': 'Précédent'
    },
    'common.next': {
      'es': 'Siguiente',
      'en': 'Next',
      'pt': 'Próximo',
      'de': 'Nächste',
      'fr': 'Suivant'
    },
    'common.noResults': {
      'es': 'No se encontraron resultados',
      'en': 'No results found',
      'pt': 'Não se encontraron resultados',
      'de': 'Nein se encontraron resultados',
      'fr': 'Aucun résultat trouvé'
    },
    'common.retry': {
      'es': 'Reintentar',
      'en': 'Retry',
      'pt': 'Reintentar',
      'de': 'Reintentar',
      'fr': 'Réessayer'
    },
    'common.saving': {
      'es': 'Guardando...',
      'en': 'Saving...',
      'pt': 'Salvando...',
      'de': 'Speichern...',
      'fr': 'Enregistrement...'
    },
    'common.deleting': {
      'es': 'Eliminando...',
      'en': 'Deleting...',
      'pt': 'Excluindo...',
      'de': 'Löschen...',
      'fr': 'Suppression...'
    },
    'common.processing': {
      'es': 'Procesando...',
      'en': 'Processing...',
      'pt': 'Processando...',
      'de': 'Verarbeitung...',
      'fr': 'Traitement...'
    },
    'common.select': {
      'es': 'Seleccione',
      'en': 'Select',
      'pt': 'Seleccione',
      'de': 'Seleccione',
      'fr': 'Sélectionner'
    },
    'common.loading': {
      'es': 'Cargando',
      'en': 'Loading',
      'pt': 'Carregando',
      'de': 'Laden',
      'fr': 'Chargement'
    },
    'common.emailPlaceholder': {
      'es': 'Ej: ejemplo@empresa.com',
      'en': 'Ex: example@company.com',
      'pt': 'Ej: ejemplo@Empresa.com',
      'de': 'Ej: ejemplo@Unternehmen.com',
      'fr': 'Ex: exemple@entreprise.com'
    },
    'common.telefonoPlaceholder': {
      'es': 'Ej: +52 55 1234 5678',
      'en': 'Ex: +52 55 1234 5678',
      'pt': 'Ej: +52 55 1234 5678',
      'de': 'Ej: +52 55 1234 5678',
      'fr': 'Ex: +52 55 1234 5678'
    },
    'common.webPlaceholder': {
      'es': 'Ej: www.empresa.com',
      'en': 'Ex: www.company.com',
      'pt': 'Ej: www.Empresa.com',
      'de': 'Ej: www.Unternehmen.com',
      'fr': 'Ex: www.entreprise.com'
    },
    'common.fillRequired': {
      'es': 'Rellena los campos obligatorios (*)',
      'en': 'Please complete the required fields (*)',
      'pt': 'Rellena los campos obligatorios (*)',
      'de': 'Rellena los campos obligatorios (*)',
      'fr': 'Veuillez remplir les champs obligatoires (*)'
    },
    'common.fillRequiredHint': {
      'es': 'Rellena los campos obligatorios (*) y revisa los textos de ayuda antes de guardar.',
      'en': 'Complete the required fields (*) and review help texts before saving.',
      'pt': 'Rellena los campos obligatorios (*) y revisa los textos de ayuda antes de Salvar.',
      'de': 'Rellena los campos obligatorios (*) y revisa los textos de ayuda antes de Speichern.',
      'fr': 'Remplissez les champs obligatoires (*) et vérifiez les textes d’aide avant d’enregistrer.'
    },

    // ═══════════════════════════════════════════════════════════
    // 📁 GRUPO CUENTA
    // ═══════════════════════════════════════════════════════════
    'grupoCuenta.title': {
      'es': 'Grupos de Cuenta',
      'en': 'Account Groups',
      'pt': 'Grupos de Conta',
      'de': 'Gruppen de Konto',
      'fr': 'Groupes de Comptes'
    },
    'grupoCuenta.subtitle': {
      'es': 'Nivel superior del plan de cuentas',
      'en': 'Top level of chart of accounts',
      'pt': 'Nivel superior del plan de Contas',
      'de': 'Nivel superior del plan de Konten',
      'fr': 'Niveau supérieur du plan comptable'
    },
    'grupoCuenta.newTitle': {
      'es': 'Nuevo Grupo de Cuenta',
      'en': 'New Account Group',
      'pt': 'Novo Grupo de Conta',
      'de': 'Neu Gruppe de Konto',
      'fr': 'Nouveau Groupe de Comptes'
    },
    'grupoCuenta.editTitle': {
      'es': 'Editar Grupo de Cuenta',
      'en': 'Edit Account Group',
      'pt': 'Editar Grupo de Conta',
      'de': 'Bearbeiten Gruppe de Konto',
      'fr': 'Modifier Groupe de Comptes'
    },
    'grupoCuenta.detailTitle': {
      'es': 'Detalle del Grupo de Cuenta',
      'en': 'Account Group Details',
      'pt': 'Detalhe del Grupo de Conta',
      'de': 'Detail del Gruppe de Konto',
      'fr': 'Détails du Groupe de Comptes'
    },
    'grupoCuenta.detailSubtitle': {
      'es': 'Visualización completa de la información del grupo de cuenta',
      'en': 'Complete view of account group information',
      'pt': 'Visualización completa de la Informação del Grupo de Conta',
      'de': 'Visualización completa de la Information del Gruppe de Konto',
      'fr': 'Vue complète des informations du groupe de comptes'
    },
    'grupoCuenta.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'pt': 'Informação Principal',
      'de': 'Hauptinformation',
      'fr': 'Informations Principales'
    },
    'grupoCuenta.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'grupoCuenta.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'pt': 'Nome',
      'de': 'Name',
      'fr': 'Nom'
    },
    'grupoCuenta.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'pt': 'Descrição',
      'de': 'Beschreibung',
      'fr': 'Description'
    },
    'grupoCuenta.codigoPlaceholder': {
      'es': 'Ej: 01',
      'en': 'Ex: 01',
      'pt': 'Ej: 01',
      'de': 'Ej: 01',
      'fr': 'Ex: 01'
    },
    'grupoCuenta.descripcionPlaceholder': {
      'es': 'Ej: Activo, Pasivo, Patrimonio',
      'en': 'Ex: Asset, Liability, Equity',
      'fr': 'Ex: Actif, Passif, Capitaux Propres',
      'pt': 'Ej: Ativo, Pasivo, Patrimonio',
      'de': 'Ej: Aktiv, Pasivo, Patrimonio'
    },
    'grupoCuenta.infoBasica': {
      'es': 'Información General',
      'en': 'General Information',
      'pt': 'Informação Geral',
      'de': 'Allgemeine Informationen',
      'fr': 'Informations Générales'
    },
    "grupoCuenta.codigoHelp": {
      "es": "Ingrese el código numérico del grupo (2 dígitos).",
      "en": "Enter the numeric group code (2 digits).",
      "fr": "Entrez le code numérique du groupe (2 chiffres).",
      "pt": "Digite o código numérico do grupo (2 dígitos).",
      "de": "Geben Sie den numerischen Gruppencode ein (2 Ziffern)."
    },
    "grupoCuenta.descripcionHelp": {
      "es": "Ingrese una breve descripción del grupo de cuenta.",
      "en": "Enter a brief description of the account group.",
      "fr": "Entrez une brève description du groupe de comptes.",
      "pt": "Digite uma breve descrição do grupo de contas.",
      "de": "Geben Sie eine kurze Beschreibung der Kontogruppe ein."
    },
    "grupoCuenta.placeholderCodigo": {
      "es": "Ej: 01",
      "en": "Ex: 01",
      "fr": "Ex: 01",
      "pt": "Ex: 01",
      "de": "z.B.: 01"
    },
    "grupoCuenta.placeholderDescripcion": {
      "es": "Ej: Activo, Pasivo, Patrimonio",
      "en": "Ex: Asset, Liability, Equity",
      "fr": "Ex: Actif, Passif, Capitaux Propres",
      "pt": "Ex: Ativo, Passivo, Patrimônio",
      "de": "z.B.: Aktiva, Passiva, Eigenkapital"
    },
    'grupoCuenta.noGruposDisponibles': {
      'es': 'No hay grupos disponibles.',
      'en': 'No groups available.',
      'pt': 'Não hay Grupos disponibles.',
      'de': 'Nein hay Gruppen disponibles.',
      'fr': 'Aucun groupe disponible.'
    },
    'grupoCuenta.grupoHelp': {
      'es': 'Seleccione el grupo principal para la cuenta.',
      'en': 'Select the top-level group for the account.',
      'pt': 'Seleccione el Grupo principal para la Conta.',
      'de': 'Seleccione el Gruppe principal para la Konto.',
      'fr': 'Sélectionnez le groupe principal pour le compte.'
    },

    // ═══════════════════════════════════════════════════════════
    // 📂 SUBGRUPO CUENTA
    // ═══════════════════════════════════════════════════════════
    'subGrupoCuenta.title': {
      'es': 'SubGrupos de Cuenta',
      'en': 'Account Subgroups',
      'pt': 'Subgrupos de Conta',
      'de': 'Untergruppen de Konto',
      'fr': 'Sous-groupes de Comptes'
    },
    'subGrupoCuenta.subtitle': {
      'es': 'Segundo nivel del plan de cuentas',
      'en': 'Second level of chart of accounts',
      'pt': 'Segundo nivel del plan de Contas',
      'de': 'Segundo nivel del plan de Konten',
      'fr': 'Deuxième niveau du plan comptable'
    },
    'subGrupoCuenta.newTitle': {
      'es': 'Nuevo SubGrupo de Cuenta',
      'en': 'New Account Subgroup',
      'pt': 'Novo Subgrupo de Conta',
      'de': 'Neu Untergruppe de Konto',
      'fr': 'Nouveau Sous-groupe de Comptes'
    },
    'subGrupoCuenta.editTitle': {
      'es': 'Editar SubGrupo de Cuenta',
      'en': 'Edit Account Subgroup',
      'pt': 'Editar Subgrupo de Conta',
      'de': 'Bearbeiten Untergruppe de Konto',
      'fr': 'Modifier Sous-groupe de Comptes'
    },
    'subGrupoCuenta.detailTitle': {
      'es': 'Detalle del SubGrupo de Cuenta',
      'en': 'Account Subgroup Details',
      'pt': 'Detalhe del Subgrupo de Conta',
      'de': 'Detail del Untergruppe de Konto',
      'fr': 'Détails du Sous-groupe de Comptes'
    },
    'subGrupoCuenta.detailSubtitle': {
      'es': 'Visualización completa de la información del subgrupo de cuenta',
      'en': 'Complete view of account subgroup information',
      'pt': 'Visualización completa de la Informação del Subgrupo de Conta',
      'de': 'Visualización completa de la Information del Untergruppe de Konto',
      'fr': 'Vue complète des informations du sous-groupe de comptes'
    },
    'subGrupoCuenta.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'pt': 'Informação Principal',
      'de': 'Hauptinformation',
      'fr': 'Informations Principales'
    },
    'subGrupoCuenta.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'subGrupoCuenta.subgrupoHelp': {
      'es': 'Seleccione el subgrupo aplicable para la cuenta.',
      'en': 'Select the applicable subgroup for the account.',
      'pt': 'Seleccione el Subgrupo aplicable para la Conta.',
      'de': 'Seleccione el Untergruppe aplicable para la Konto.',
      'fr': 'Sélectionnez le sous-groupe applicable pour le compte.'
    },
    'subGrupoCuenta.grupoCuenta': {
      'es': 'Grupo de Cuenta',
      'en': 'Account Group',
      'pt': 'Grupo de Conta',
      'de': 'Gruppe de Konto',
      'fr': 'Groupe de Comptes'
    },
    'subGrupoCuenta.tipo': {
      'es': 'Tipo',
      'en': 'Type',
      'pt': 'Tipo',
      'de': 'Tipo',
      'fr': 'Type'
    },
    'subGrupoCuenta.deudora': {
      'es': 'Deudora',
      'en': 'Debit',
      'pt': 'Deudora',
      'de': 'Deudora',
      'fr': 'Débiteur'
    },
    'subGrupoCuenta.acreedora': {
      'es': 'Acreedora',
      'en': 'Credit',
      'pt': 'Acreedora',
      'de': 'Acreedora',
      'fr': 'Créditeur'
    },
    'subGrupoCuenta.estado': {
      'es': 'Estado',
      'en': 'Status',
      'pt': 'Estado',
      'de': 'Estado',
      'fr': 'Statut'
    },
    'subGrupoCuenta.codigoPlaceholder': {
      'es': 'Ej: 01001',
      'en': 'Ex: 01001',
      'pt': 'Ej: 01001',
      'de': 'Ej: 01001',
      'fr': 'Ex: 01001'
    },
    'subGrupoCuenta.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'pt': 'Descrição',
      'de': 'Beschreibung',
      'fr': 'Description'
    },
    'subGrupoCuenta.descripcionPlaceholder': {
      'es': 'Ej: Activo Circulante, Efectivo y Equivalentes',
      'en': 'Ex: Current Assets, Cash and Equivalents',
      'fr': 'Ex: Actif Circulant, Trésorerie et Équivalents',
      'pt': 'Ej: Ativo Circulante, Efectivo y Equivalentes',
      'de': 'Ej: Aktiv Circulante, Efectivo y Equivalentes'
    },
    'subGrupoCuenta.codigoUsuario': {
      'es': 'Código (3 dígitos)',
      'en': 'Code (3 digits)',
      'pt': 'Código (3 dígitos)',
      'de': 'Code (3 dígitos)',
      'fr': 'Code (3 chiffres)'
    },
    'subGrupoCuenta.codigoUsuarioPlaceholder': {
      'es': 'Ej: 001',
      'en': 'Ex: 001',
      'pt': 'Ej: 001',
      'de': 'Ej: 001',
      'fr': 'Ex: 001'
    },
    'subGrupoCuenta.codigoUsuarioHelp': {
      'es': 'Ingrese los últimos 3 dígitos del código',
      'en': 'Enter the last 3 digits of the code',
      'pt': 'Ingrese los últimos 3 dígitos del Código',
      'de': 'Ingrese los últimos 3 dígitos del Code',
      'fr': 'Entrez les 3 derniers chiffres du code'
    },
    'subGrupoCuenta.codigoCompleto': {
      'es': 'Código Completo',
      'en': 'Full Code',
      'pt': 'Código Completo',
      'de': 'Code Completo',
      'fr': 'Code Complet'
    },
    'subGrupoCuenta.codigoCompletoPlaceholder': {
      'es': 'Se generará automáticamente',
      'en': 'Will be generated automatically',
      'pt': 'Se generará automáticamente',
      'de': 'Se generará automáticamente',
      'fr': 'Sera généré automatiquement'
    },
    'subGrupoCuenta.codigoCompletoHelp': {
      'es': 'Código del grupo + sus 3 dígitos = Código completo',
      'en': 'Group code + your 3 digits = Full code',
      'pt': 'Código del Grupo + sus 3 dígitos = Código completo',
      'de': 'Code del Gruppe + sus 3 dígitos = Code completo',
      'fr': 'Code du groupe + vos 3 chiffres = Code complet'
    },
    'subGrupoCuenta.prefijoSeleccionado': {
      'es': 'Prefijo seleccionado',
      'en': 'Selected prefix',
      'pt': 'Prefijo seleccionado',
      'de': 'Prefijo seleccionado',
      'fr': 'Préfixe sélectionné'
    },
    "subGrupoCuenta.descripcionHelp": {
      "es": "Ingrese una breve descripción del subgrupo de cuenta.",
      "en": "Enter a brief description of the account subgroup.",
      "fr": "Entrez une brève description du sous-groupe de comptes.",
      "pt": "Digite uma breve descrição do subgrupo de contas.",
      "de": "Geben Sie eine kurze Beschreibung der Kontountergruppe ein."
    },
    "subGrupoCuenta.deudoraHelp": {
      "es": "Marque si el subgrupo afecta cuentas deudoras (débitos).",
      "en": "Check if the subgroup affects debit accounts.",
      "fr": "Cochez si le sous-groupe affecte les comptes débiteurs.",
      "pt": "Marque se o subgrupo afeta contas devedoras (débitos).",
      "de": "Markieren Sie, wenn die Untergruppe Sollkonten betrifft."
    },
    "subGrupoCuenta.placeholderCodigoCompleto": {
      "es": "Se generará automáticamente",
      "en": "Will be generated automatically",
      "fr": "Sera généré automatiquement",
      "pt": "Será gerado automaticamente",
      "de": "Wird automatisch generiert"
    },
    "subGrupoCuenta.placeholderCodigoUsuario": {
      "es": "Ej: 001",
      "en": "Ex: 001",
      "fr": "Ex: 001",
      "pt": "Ex: 001",
      "de": "z.B.: 001"
    },
    "subGrupoCuenta.placeholderDescripcion": {
      "es": "Ej: Activo Circulante, Efectivo y Equivalentes",
      "en": "Ex: Current Assets, Cash and Equivalents",
      "fr": "Ex: Actif Circulant, Trésorerie et Équivalents",
      "pt": "Ex: Ativo Circulante, Caixa e Equivalentes",
      "de": "z.B.: Umlaufvermögen, Bargeld und Äquivalente"
    },
    "subGrupoCuenta.selectGrupoHelp": {
      "es": "Seleccione el grupo para generar el prefijo del subgrupo.",
      "en": "Select the group to generate the subgroup prefix.",
      "fr": "Sélectionnez le groupe pour générer le préfixe du sous-groupe.",
      "pt": "Selecione o grupo para gerar o prefixo do subgrupo.",
      "de": "Wählen Sie die Gruppe aus, um das Untergruppenpräfix zu generieren."
    },
    'subGrupoCuenta.noSubgruposDisponibles': {
      'es': 'No hay subgrupos disponibles.',
      'en': 'No subgroups available.',
      'pt': 'Não hay Subgrupos disponibles.',
      'de': 'Nein hay Untergruppen disponibles.',
      'fr': 'Aucun sous-groupe disponible.'
    },

    // ═══════════════════════════════════════════════════════════
    // 📄 CUENTA
    // ═══════════════════════════════════════════════════════════
    'cuenta.title': {
      'es': 'Cuentas Contables',
      'en': 'Accounting Accounts',
      'pt': 'Contas Contables',
      'de': 'Buchungskonten',
      'fr': 'Comptes Comptables'
    },
    'cuenta.subtitle': {
      'es': 'Nivel más detallado del plan de cuentas',
      'en': 'Most detailed level of chart of accounts',
      'pt': 'Nivel más detallado del plan de Contas',
      'de': 'Nivel más detallado del plan de Konten',
      'fr': 'Niveau le plus détaillé du plan comptable'
    },
    'cuenta.newTitle': {
      'es': 'Nueva Cuenta',
      'en': 'New Account',
      'pt': 'Nova Conta',
      'de': 'Neue Konto',
      'fr': 'Nouveau Compte'
    },
    'cuenta.editTitle': {
      'es': 'Editar Cuenta',
      'en': 'Edit Account',
      'pt': 'Editar Conta',
      'de': 'Bearbeiten Konto',
      'fr': 'Modifier Compte'
    },
    'cuenta.detailTitle': {
      'es': 'Detalle de la Cuenta',
      'en': 'Account Details',
      'pt': 'Detalhe de la Conta',
      'de': 'Detail de la Konto',
      'fr': 'Détails du Compte'
    },
    'cuenta.detailSubtitle': {
      'es': 'Visualización completa de la información de la cuenta',
      'en': 'Complete view of account information',
      'pt': 'Visualización completa de la Informação de la Conta',
      'de': 'Visualización completa de la Information de la Konto',
      'fr': 'Vue complète des informations du compte'
    },
    'cuenta.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'pt': 'Informação Principal',
      'de': 'Hauptinformation',
      'fr': 'Informations Principales'
    },
    'cuenta.codigo': {
      'es': 'Código',
      'en': 'Code',
      'pt': 'Código',
      'de': 'Code',
      'fr': 'Code'
    },
    'cuenta.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'pt': 'Descrição',
      'de': 'Beschreibung',
      'fr': 'Description'
    },
    'cuenta.subGrupoCuenta': {
      'es': 'SubGrupo de Cuenta',
      'en': 'Account Subgroup',
      'pt': 'Subgrupo de Conta',
      'de': 'Untergruppe de Konto',
      'fr': 'Sous-groupe de Comptes'
    },
    'cuenta.grupoCuenta': {
      'es': 'Grupo de Cuenta',
      'en': 'Account Group',
      'pt': 'Grupo de Conta',
      'de': 'Gruppe de Konto',
      'fr': 'Groupe de Comptes'
    },
    'cuenta.selectGrupoToSeeSubgrupos': {
      'es': 'Seleccione un grupo para ver subgrupos.',
      'en': 'Select a group to see subgroups.',
      'pt': 'Seleccione un Grupo para Ver Subgrupos.',
      'de': 'Seleccione un Gruppe para Anzeigen Untergruppen.',
      'fr': 'Sélectionnez un groupe pour voir les sous-groupes.'
    },
    "cuenta.descripcionHelp": {
      "es": "Ingrese una breve descripción para la cuenta contable.",
      "en": "Enter a brief description for the accounting account.",
      "fr": "Entrez une brève description du compte comptable.",
      "pt": "Digite uma breve descrição para a conta contábil.",
      "de": "Geben Sie eine kurze Beschreibung für das Buchungskonto ein."
    },
    "cuenta.placeholderDescripcion": {
      "es": "Ej: Efectivo en Caja, Bancos Moneda Nacional",
      "en": "Ex: Cash on Hand, National Currency Banks",
      "fr": "Ex: Espèces en Caisse, Banques en Monnaie Nationale",
      "pt": "Ex: Dinheiro em Caixa, Bancos Moeda Nacional",
      "de": "z.B.: Bargeld in Kasse, Banken Landeswährung"
    },
    'cuenta.negocio': {
      'es': 'Negocio',
      'en': 'Business',
      'pt': 'Negócio',
      'de': 'Geschäft',
      'fr': 'Entreprise'
    },
    'cuenta.negocioHelp': {
      'es': 'Seleccione el negocio (configuración del sistema) asociado a la cuenta.',
      'en': 'Select the business (system configuration) associated with the account.',
      'pt': 'Seleccione el Negócio (Configuração del Sistema) asociado a la Conta.',
      'de': 'Seleccione el Geschäft (Konfiguration del System) asociado a la Konto.',
      'fr': 'Sélectionnez l’entreprise (configuration système) associée au compte.'
    },
    'cuenta.selectNegocio': {
      'es': 'Seleccione un negocio primero',
      'en': 'Select a business first',
      'pt': 'Seleccione un Negócio primero',
      'de': 'Seleccione un Geschäft primero',
      'fr': 'Sélectionnez d\'abord une entreprise'
    },
    'cuenta.noNegociosDisponibles': {
      'es': 'No hay negocios disponibles.',
      'en': 'No businesses available.',
      'pt': 'Não hay negocios disponibles.',
      'de': 'Nein hay negocios disponibles.',
      'fr': 'Aucune entreprise disponible.'
    },
    'cuenta.codigoUsuario': {
      'es': 'Código (3 Dígitos)',
      'en': 'Code (3 Digits)',
      'pt': 'Código (3 Dígitos)',
      'de': 'Code (3 Dígitos)',
      'fr': 'Code (3 Chiffres)'
    },
    'cuenta.codigoUsuarioPlaceholder': {
      'es': 'Ej: 001',
      'en': 'Ex: 001',
      'pt': 'Ej: 001',
      'de': 'Ej: 001',
      'fr': 'Ex: 001'
    },
    'cuenta.codigoUsuarioHelp': {
      'es': 'Ingrese los últimos 3 dígitos del código',
      'en': 'Enter the last 3 digits of the code',
      'pt': 'Ingrese los últimos 3 dígitos del Código',
      'de': 'Ingrese los últimos 3 dígitos del Code',
      'fr': 'Entrez les 3 derniers chiffres du code'
    },
    'cuenta.codigoCompleto': {
      'es': 'Código Completo',
      'en': 'Full Code',
      'pt': 'Código Completo',
      'de': 'Code Completo',
      'fr': 'Code Complet'
    },
    'cuenta.codigoCompletoPlaceholder': {
      'es': 'Se generará automáticamente',
      'en': 'Will be generated automatically',
      'pt': 'Se generará automáticamente',
      'de': 'Se generará automáticamente',
      'fr': 'Sera généré automatiquement'
    },
    'cuenta.codigoCompletoHelp': {
      'es': 'Código del subgrupo + sus 3 dígitos = Código completo',
      'en': 'Subgroup code + your 3 digits = Full code',
      'pt': 'Código del Subgrupo + sus 3 dígitos = Código completo',
      'de': 'Code del Untergruppe + sus 3 dígitos = Code completo',
      'fr': 'Code du sous-groupe + vos 3 chiffres = Code complet'
    },
    'cuenta.prefijoSeleccionado': {
      'es': 'Prefijo Seleccionado',
      'en': 'Selected Prefix',
      'pt': 'Prefijo Seleccionado',
      'de': 'Prefijo Seleccionado',
      'fr': 'Préfixe Sélectionné'
    },

    // Ayudas para selects padre y placeholders
    'subLinea.noLineasDisponibles': {
      'es': 'No hay líneas disponibles.',
      'en': 'No lines available.',
      'pt': 'Não hay líneas disponibles.',
      'de': 'Nein hay líneas disponibles.',
      'fr': 'Aucune ligne disponible.'
    },
    'subGrupoCuenta.noGruposDisponibles': {
      'es': 'No hay grupos disponibles.',
      'en': 'No groups available.',
      'pt': 'Não hay Grupos disponibles.',
      'de': 'Nein hay Gruppen disponibles.',
      'fr': 'Aucun groupe disponible.'
    },
    'conceptoAjuste.noCuentasDisponibles': {
      'es': 'No hay cuentas disponibles.',
      'en': 'No accounts available.',
      'pt': 'Não hay Contas disponibles.',
      'de': 'Nein hay Konten disponibles.',
      'fr': 'Aucun compte disponible.'
    },
    'proveedores.placeholderNif': {
      'es': 'Ej: 12345678A',
      'en': 'Ex: 12345678A',
      'pt': 'Ej: 12345678A',
      'de': 'Ej: 12345678A',
      'fr': 'Ex: 12345678A'
    },
    'proveedores.placeholderIva': {
      'es': 'Ej: 21.00',
      'en': 'Ex: 21.00',
      'pt': 'Ej: 21.00',
      'de': 'Ej: 21.00',
      'fr': 'Ex: 21.00'
    },
    "proveedores.placeholderCodigo": {
      "es": "Ingrese código (5 dígitos)",
      "en": "Enter code (5 chars)",
      "fr": "Entrez le code (5 caractères)",
      "pt": "Digite código (5 dígitos)",
      "de": "Code eingeben (5 Zeichen)"
    },
    "proveedores.placeholderDescripcion": {
      "es": "Ej: Nombre del proveedor",
      "en": "Ex: Supplier name",
      "fr": "Ex: Nom du fournisseur",
      "pt": "Ex: Nome do fornecedor",
      "de": "z.B.: Lieferantenname"
    },
    "proveedores.placeholderEmail": {
      "es": "Ej: cliente@empresa.com",
      "en": "Ex: customer@company.com",
      "fr": "Ex: client@entreprise.com",
      "pt": "Ex: cliente@empresa.com",
      "de": "z.B.: kunde@firma.com"
    },
    "proveedores.placeholderTelefono": {
      "es": "Ej: +34 912 345 678",
      "en": "Ex: +34 912 345 678",
      "fr": "Ex: +33 1 23 45 67 89",
      "pt": "Ex: +351 21 234 5678",
      "de": "z.B.: +49 30 1234 5678"
    },

    'clientes.placeholderIva': {
      'es': 'Ej: 21.00',
      'en': 'Ex: 21.00',
      'pt': 'Ej: 21.00',
      'de': 'Ej: 21.00',
      'fr': 'Ex: 21.00'
    },

    'establecimiento.selectPaisToSeeProvincias': {
      'es': 'Seleccione un país para ver provincias.',
      'en': 'Select a country to see provinces.',
      'pt': 'Seleccione un País para Ver provincias.',
      'de': 'Seleccione un Land para Anzeigen provincias.',
      'fr': 'Sélectionnez un pays pour voir les provinces.'
    },
    'establecimiento.selectProvinciaToSeeMunicipios': {
      'es': 'Seleccione una provincia para ver municipios.',
      'en': 'Select a province to see municipalities.',
      'pt': 'Seleccione una Província para Ver municipios.',
      'de': 'Seleccione una Provinz para Anzeigen municipios.',
      'fr': 'Sélectionnez une province pour voir les municipalités.'
    },
    'establecimiento.selectMunicipioToSeeCodigosPostales': {
      'es': 'Seleccione un municipio para ver códigos postales.',
      'en': 'Select a municipality to see postal codes.',
      'pt': 'Seleccione un Município para Ver códigos postales.',
      'de': 'Seleccione un Gemeinde para Anzeigen códigos postales.',
      'fr': 'Sélectionnez une municipalité pour voir les codes postaux.'
    },
    'establecimiento.selectCodigoPostalHelp': {
      'es': 'Seleccione un código postal disponible.',
      'en': 'Select an available postal code.',
      'pt': 'Seleccione un Código Postal disponible.',
      'de': 'Seleccione un Code postal disponible.',
      'fr': 'Sélectionnez un code postal disponible.'
    },
    'establecimiento.noPaisesDisponibles': {
      'es': 'No hay países disponibles.',
      'en': 'No countries available.',
      'pt': 'Não hay países disponibles.',
      'de': 'Nein hay países disponibles.',
      'fr': 'Aucun pays disponible.'
    },
    'provincia.selectPaisToSeeProvincias': {
      'es': 'Seleccione un país para ver provincias.',
      'en': 'Select a country to see provinces.',
      'pt': 'Seleccione un País para Ver provincias.',
      'de': 'Seleccione un Land para Anzeigen provincias.',
      'fr': 'Sélectionnez un pays pour voir les provinces.'
    },
    'provincia.noPaisesDisponibles': {
      'es': 'No hay países disponibles.',
      'en': 'No countries available.',
      'pt': 'Não hay países disponibles.',
      'de': 'Nein hay países disponibles.',
      'fr': 'Aucun pays disponible.'
    },
    'municipio.selectProvinciaToSeeMunicipios': {
      'es': 'Seleccione una provincia para ver municipios.',
      'en': 'Select a province to see municipalities.',
      'pt': 'Seleccione una Província para Ver municipios.',
      'de': 'Seleccione una Provinz para Anzeigen municipios.',
      'fr': 'Sélectionnez une province pour voir les municipalités.'
    },
    'codigoPostal.selectPaisToSeeProvincias': {
      'es': 'Seleccione un país para ver provincias.',
      'en': 'Select a country to see provinces.',
      'pt': 'Seleccione un País para Ver provincias.',
      'de': 'Seleccione un Land para Anzeigen provincias.',
      'fr': 'Sélectionnez un pays pour voir les provinces.'
    },
    'codigoPostal.selectProvinciaToSeeMunicipios': {
      'es': 'Seleccione una provincia para ver municipios.',
      'en': 'Select a province to see municipalities.',
      'pt': 'Seleccione una Província para Ver municipios.',
      'de': 'Seleccione una Provinz para Anzeigen municipios.',
      'fr': 'Sélectionnez une province pour voir les municipalités.'
    },
    'codigoPostal.selectMunicipioToSeeCodigosPostales': {
      'es': 'Seleccione un municipio para ver códigos postales.',
      'en': 'Select a municipality to see postal codes.',
      'pt': 'Seleccione un Município para Ver códigos postales.',
      'de': 'Seleccione un Gemeinde para Anzeigen códigos postales.',
      'fr': 'Sélectionnez une municipalité pour voir les codes postaux.'
    },
    'codigoPostal.noPaisesDisponibles': {
      'es': 'No hay países disponibles.',
      'en': 'No countries available.',
      'pt': 'Não hay países disponibles.',
      'de': 'Nein hay países disponibles.',
      'fr': 'Aucun pays disponible.'
    },
    'cuenta.descripcionPlaceholder': {
      'es': 'Ej: Efectivo en Caja, Bancos Moneda Nacional',
      'en': 'Ex: Cash on Hand, National Currency Banks',
      'fr': 'Ex: Espèces en Caisse,  Banques en Monnaie Nationale',
      'pt': 'Ej: Efectivo en Caja, Bancos Moneda Nacional',
      'de': 'Ej: Efectivo en Caja, Bancos Moneda Nacional'
    },

    // ═══════════════════════════════════════════════════════════
    // 🏢 CONFIGURACIÓN DEL NEGOCIO
    // ═══════════════════════════════════════════════════════════
    'systemConfig.title': {
      'es': 'Configuración del Sistema',
      'en': 'System Configuration',
      'pt': 'Configuração del Sistema',
      'de': 'Konfiguration del System',
      'fr': 'Configuration du Système'
    },
    'systemConfig.subtitle': {
      'es': 'Gestión de configuración general y licencias',
      'en': 'General configuration and license management',
      'pt': 'Gestão de Configuração general y licencias',
      'de': 'Verwaltung de Konfiguration general y licencias',
      'fr': 'Configuration générale et gestion des licences'
    },
    'systemConfig.newTitle': {
      'es': 'Nueva Configuración',
      'en': 'New Configuration',
      'pt': 'Nova Configuração',
      'de': 'Neue Konfiguration',
      'fr': 'Nouvelle Configuration'
    },
    'systemConfig.editTitle': {
      'es': 'Editar Configuración del Sistema',
      'en': 'Edit System Configuration',
      'pt': 'Editar Configuração del Sistema',
      'de': 'Bearbeiten Konfiguration del System',
      'fr': 'Modifier Configuration du Système'
    },
    'systemConfig.detailTitle': {
      'es': 'Detalle de Configuración del Sistema',
      'en': 'System Configuration Details',
      'pt': 'Detalhe de Configuração del Sistema',
      'de': 'Detail de Konfiguration del System',
      'fr': 'Détails de Configuration du Système'
    },
    'systemConfig.detailSubtitle': {
      'es': 'Visualización completa de la configuración del sistema',
      'en': 'Complete view of system configuration',
      'pt': 'Visualización completa de la Configuração del Sistema',
      'de': 'Visualización completa de la Konfiguration del System',
      'fr': 'Vue complète de la configuration du système'
    },
    'systemConfig.infoSistema': {
      'es': 'Información del Sistema',
      'en': 'System Information',
      'pt': 'Informação del Sistema',
      'de': 'Information del System',
      'fr': 'Informations du Système'
    },
    'systemConfig.infoNegocio': {
      'es': 'Información del Negocio',
      'en': 'Business Information',
      'pt': 'Informação del Negócio',
      'de': 'Information del Geschäft',
      'fr': 'Informations de l\'Entreprise'
    },
    'systemConfig.codigoSistema': {
      'es': 'Código del Sistema',
      'en': 'System Code',
      'pt': 'Código del Sistema',
      'de': 'Code del System',
      'fr': 'Code Système'
    },
    'systemConfig.licencia': {
      'es': 'Licencia',
      'en': 'License',
      'pt': 'Licencia',
      'de': 'Licencia',
      'fr': 'Licence'
    },
    'systemConfig.nombreNegocio': {
      'es': 'Nombre del Negocio',
      'en': 'Business Name',
      'pt': 'Nome del Negócio',
      'de': 'Name del Geschäft',
      'fr': 'Nom de l\'Entreprise'
    },
    'systemConfig.direccion': {
      'es': 'Dirección',
      'en': 'Address',
      'pt': 'Endereço',
      'de': 'Adresse',
      'fr': 'Adresse'
    },
    'systemConfig.municipio': {
      'es': 'Municipio',
      'en': 'Municipality',
      'pt': 'Município',
      'de': 'Gemeinde',
      'fr': 'Municipalité'
    },
    'systemConfig.provincia': {
      'es': 'Provincia',
      'en': 'Province',
      'pt': 'Província',
      'de': 'Provinz',
      'fr': 'Province'
    },
    'systemConfig.pais': {
      'es': 'País',
      'en': 'Country',
      'pt': 'País',
      'de': 'Land',
      'fr': 'Pays'
    },
    'systemConfig.codigoPostal': {
      'es': 'Código Postal',
      'en': 'Postal Code',
      'pt': 'Código Postal',
      'de': 'Postleitzahl',
      'fr': 'Code Postal'
    },
    'systemConfig.email': {
      'es': 'Email',
      'en': 'Email',
      'pt': 'Email',
      'de': 'E-Mail',
      'fr': 'Email'
    },
    'systemConfig.telefono': {
      'es': 'Teléfono',
      'en': 'Phone',
      'pt': 'Telefone',
      'de': 'Telefon',
      'fr': 'Téléphone'
    },
    'systemConfig.web': {
      'es': 'Sitio Web',
      'en': 'Website',
      'pt': 'Sitio Web',
      'de': 'Sitio Web',
      'fr': 'Site Web'
    },
    'systemConfig.imagen': {
      'es': 'URL Logo/Imagen',
      'en': 'Logo/Image URL',
      'pt': 'URL Logo/Imagen',
      'de': 'URL Logo/Imagen',
      'fr': 'URL Logo/Image'
    },
    'systemConfig.caducidad': {
      'es': 'Fecha de Caducidad',
      'en': 'Expiration Date',
      'pt': 'Fecha de Caducidad',
      'de': 'Fecha de Caducidad',
      'fr': 'Date d\'Expiration'
    },
    'systemConfig.estado': {
      'es': 'Estado',
      'en': 'Status',
      'pt': 'Estado',
      'de': 'Estado',
      'fr': 'Statut'
    },
    'systemConfig.cuentaPagar': {
      'es': 'Cuenta Por Pagar',
      'en': 'Accounts Payable',
      'pt': 'Conta Por Pagar',
      'de': 'Konto Por Pagar',
      'fr': 'Comptes Fournisseurs'
    },
    'systemConfig.cuentaCobrar': {
      'es': 'Cuenta Por Cobrar',
      'en': 'Accounts Receivable',
      'pt': 'Conta Por Cobrar',
      'de': 'Konto Por Cobrar',
      'fr': 'Comptes Clients'
    },
    'systemConfig.infoBasica': {
      'es': 'Información Básica',
      'en': 'Basic Information',
      'pt': 'Informação Básica',
      'de': 'Grundinformation',
      'fr': 'Informations de Base'
    },
    'systemConfig.infoContacto': {
      'es': 'Información de Contacto',
      'en': 'Contact Information',
      'pt': 'Informação de Contacto',
      'de': 'Information de Contacto',
      'fr': 'Informations de Contact'
    },
    'systemConfig.cuentasContables': {
      'es': 'Cuentas Contables Predeterminadas',
      'en': 'Default Accounting Accounts',
      'pt': 'Contas Contables Predeterminadas',
      'de': 'Konten Contables Predeterminadas',
      'fr': 'Comptes Comptables par Défaut'
    },
    'systemConfig.estadoLicencia': {
      'es': 'Estado de Licencia',
      'en': 'License Status',
      'pt': 'Estado de Licencia',
      'de': 'Estado de Licencia',
      'fr': 'Statut de Licence'
    },
    'systemConfig.vigente': {
      'es': 'Vigente',
      'en': 'Valid',
      'pt': 'Vigente',
      'de': 'Vigente',
      'fr': 'Valide'
    },
    'systemConfig.porVencer': {
      'es': 'Por Vencer',
      'en': 'Expiring Soon',
      'pt': 'Por Vencer',
      'de': 'Por Vencer',
      'fr': 'Expirant Bientôt'
    },
    'systemConfig.vencida': {
      'es': 'Vencida',
      'en': 'Expired',
      'pt': 'Vencida',
      'de': 'Vencida',
      'fr': 'Expiré'
    },
    'systemConfig.diasRestantes': {
      'es': 'días restantes',
      'en': 'days remaining',
      'pt': 'días restantes',
      'de': 'días restantes',
      'fr': 'jours restants'
    },
    'systemConfig.personaContacto': {
      'es': 'Persona de Contacto',
      'en': 'Contact Person',
      'pt': 'Persona de Contacto',
      'de': 'Persona de Contacto',
      'fr': 'Personne de Contact'
    },
    'systemConfig.formaJuridica': {
      'es': 'Forma Jurídica',
      'en': 'Legal Form',
      'pt': 'Forma Jurídica',
      'de': 'Forma Jurídica',
      'fr': 'Forme Juridique'
    },
    'systemConfig.activo': {
      'es': 'Activo',
      'en': 'Active',
      'pt': 'Ativo',
      'de': 'Aktiv',
      'fr': 'Actif'
    },
    'systemConfig.selectPaisToSeeProvincias': {
      'es': 'Seleccione un país para ver provincias.',
      'en': 'Select a country to see provinces.',
      'pt': 'Seleccione un País para Ver provincias.',
      'de': 'Seleccione un Land para Anzeigen provincias.',
      'fr': 'Sélectionnez un pays pour voir les provinces.'
    },
    'systemConfig.selectProvinciaToSeeMunicipios': {
      'es': 'Seleccione una provincia para ver municipios.',
      'en': 'Select a province to see municipalities.',
      'pt': 'Seleccione una Província para Ver municipios.',
      'de': 'Seleccione una Provinz para Anzeigen municipios.',
      'fr': 'Sélectionnez une province pour voir les municipalités.'
    },
    'systemConfig.selectMunicipioToSeeCodigosPostales': {
      'es': 'Seleccione un municipio para ver códigos postales.',
      'en': 'Select a municipality to see postal codes.',
      'pt': 'Seleccione un Município para Ver códigos postales.',
      'de': 'Seleccione un Gemeinde para Anzeigen códigos postales.',
      'fr': 'Sélectionnez une municipalité pour voir les codes postaux.'
    },
    'systemConfig.noPaisesDisponibles': {
      'es': 'No hay países disponibles.',
      'en': 'No countries available.',
      'pt': 'Não hay países disponibles.',
      'de': 'Nein hay países disponibles.',
      'fr': 'Aucun pays disponible.'
    },
    'systemConfig.activoHelp': {
      'es': 'Indica si el negocio está activo.',
      'en': 'Indicates whether the business is active.',
      'pt': 'Indica si el Negócio está Ativo.',
      'de': 'Indica si el Geschäft está Aktiv.',
      'fr': 'Indique si l’entreprise est active.'
    },
    'systemConfig.codigoHelp': {
      'es': 'Ingrese 3 caracteres alfanuméricos (ej: GOL-ERP-001)',
      'en': 'Enter 3 alphanumeric characters (e.g. GOL-ERP-001)',
      'pt': 'Ingrese 3 caracteres alfanuméricos (ej: GOL-ERP-001)',
      'de': 'Ingrese 3 caracteres alfanuméricos (ej: GOL-ERP-001)',
      'fr': 'Entrez 3 caractères alphanumériques (p.ex. GOL-ERP-001)'
    },
    'systemConfig.formaJuridicaHelp': {
      'es': 'Seleccione la forma jurídica del negocio.',
      'en': 'Select the legal form of the business.',
      'pt': 'Seleccione la forma jurídica del Negócio.',
      'de': 'Seleccione la forma jurídica del Geschäft.',
      'fr': 'Sélectionnez la forme juridique de l’entreprise.'
    },
    'systemConfig.placeholderPersonaContacto': {
      'es': 'Nombre de la persona de contacto',
      'en': 'Contact person name',
      'pt': 'Nome de la persona de contacto',
      'de': 'Name de la persona de contacto',
      'fr': 'Nom de la personne de contact'
    },
    'systemConfig.personaContactoHelp': {
      'es': 'Nombre de la persona responsable para comunicaciones y soporte.',
      'en': 'Name of the person responsible for communications and support.',
      'pt': 'Nome de la persona responsable para comunicaciones y soporte.',
      'de': 'Name de la persona responsable para comunicaciones y soporte.',
      'fr': 'Nom de la personne responsable des communications et du support.'
    },
    'systemConfig.caducidadHelp': {
      'es': 'Fecha en la que la licencia dejará de ser válida.',
      'en': 'Date when the license will expire.',
      'pt': 'Fecha en la que la licencia dejará de ser válida.',
      'de': 'Fecha en la que la licencia dejará de ser válida.',
      'fr': 'Date à laquelle la licence expirera.'
    },
    'systemConfig.licenciaHelp': {
      'es': 'Código de licencia entregado por el proveedor; necesario para activar el sistema.',
      'en': 'License code provided by the vendor; required to activate the system.',
      'pt': 'Código de licencia entregado por el Fornecedor; necesario para activar el Sistema.',
      'de': 'Code de licencia entregado por el Lieferant; necesario para activar el System.',
      'fr': 'Code de licence fourni par le fournisseur ; requis pour activer le système.'
    },
    'systemConfig.nombreNegocioHelp': {
      'es': 'Nombre legal o comercial del negocio (visible en documentos).',
      'en': 'Legal or trade name of the business (visible on documents).',
      'pt': 'Nome legal o comercial del Negócio (visible en documentos).',
      'de': 'Name legal o comercial del Geschäft (visible en documentos).',
      'fr': 'Nom légal ou commercial de l’entreprise (visible sur les documents).'
    },
    'systemConfig.emailHelp': {
      'es': 'Email principal para notificaciones y contacto.',
      'en': 'Primary email for notifications and contact.',
      'pt': 'Email principal para notificaciones y contacto.',
      'de': 'E-Mail principal para notificaciones y contacto.',
      'fr': 'Email principal pour les notifications et le contact.'
    },
    'systemConfig.telefonoHelp': {
      'es': 'Teléfono de contacto (incluya prefijo internacional si aplica).',
      'en': 'Contact phone (include international prefix if applicable).',
      'pt': 'Telefone de contacto (incluya prefijo internacional si aplica).',
      'de': 'Telefon de contacto (incluya prefijo internacional si aplica).',
      'fr': 'Téléphone de contact (inclure l’indicatif international si applicable).'
    },
    'systemConfig.webHelp': {
      'es': 'Sitio web oficial del negocio (use https://).',
      'en': 'Official business website (use https://).',
      'pt': 'Sitio web oficial del Negócio (use https://).',
      'de': 'Sitio web oficial del Geschäft (use https://).',
      'fr': 'Site web officiel de l’entreprise (utilisez https://).'
    },
    'systemConfig.imagenHelp': {
      'es': 'URL pública del logo o nombre del archivo subido. Máx. 2MB.',
      'en': 'Public URL of the logo or uploaded file name. Max 2MB.',
      'pt': 'URL pública del logo o Nome del archivo subido. Máx. 2MB.',
      'de': 'URL pública del logo o Name del archivo subido. Máx. 2MB.',
      'fr': 'URL publique du logo ou nom du fichier téléchargé. Max 2Mo.'
    },
    'systemConfig.paisHelp': {
      'es': 'País donde está localizado el negocio.',
      'en': 'Country where the business is located.',
      'pt': 'País donde está localizado el Negócio.',
      'de': 'Land donde está localizado el Geschäft.',
      'fr': 'Pays où se trouve l’entreprise.'
    },
    'systemConfig.provinciaHelp': {
      'es': 'Provincia o estado del negocio.',
      'en': 'Province or state of the business.',
      'pt': 'Província o estado del Negócio.',
      'de': 'Provinz o estado del Geschäft.',
      'fr': 'Province ou État de l’entreprise.'
    },
    'systemConfig.municipioHelp': {
      'es': 'Municipio o localidad del negocio.',
      'en': 'Municipality or locality of the business.',
      'pt': 'Município o localidad del Negócio.',
      'de': 'Gemeinde o localidad del Geschäft.',
      'fr': 'Municipalité ou localité de l’entreprise.'
    },
    'systemConfig.codigoPostalHelp': {
      'es': 'Código postal aplicable al domicilio del negocio.',
      'en': 'Postal code applicable to the business address.',
      'pt': 'Código Postal aplicable al domicilio del Negócio.',
      'de': 'Code postal aplicable al domicilio del Geschäft.',
      'fr': 'Code postal applicable à l’adresse de l’entreprise.'
    },
    'systemConfig.direccionHelp': {
      'es': 'Ingrese la dirección completa del negocio.',
      'en': 'Enter the full business address.',
      'pt': 'Ingrese la Endereço completa del Negócio.',
      'de': 'Ingrese la Adresse completa del Geschäft.',
      'fr': "Entrez l'adresse complète de l'entreprise."
    },
    'systemConfig.cuentaPagarHelp': {
      'es': 'Cuenta contable predeterminada para registrar las cuentas por pagar.',
      'en': 'Default accounting account for recording accounts payable.',
      'pt': 'Conta contable predeterminada para registrar las Contas por pagar.',
      'de': 'Konto contable predeterminada para registrar las Konten por pagar.',
      'fr': 'Compte comptable par défaut pour enregistrer les comptes fournisseurs.'
    },
    'systemConfig.cuentaCobrarHelp': {
      'es': 'Cuenta contable predeterminada para registrar las cuentas por cobrar.',
      'en': 'Default accounting account for recording accounts receivable.',
      'pt': 'Conta contable predeterminada para registrar las Contas por cobrar.',
      'de': 'Konto contable predeterminada para registrar las Konten por cobrar.',
      'fr': 'Compte comptable par défaut pour enregistrer les comptes clients.'
    },

    // ═══════════════════════════════════════════════════════════
    // 🏢 ESTABLECIMIENTO
    // ═══════════════════════════════════════════════════════════
    'establecimiento.negocioHelp': {
      'es': 'Seleccione el negocio al que pertenece el establecimiento.',
      'en': 'Select the business this establishment belongs to.',
      'pt': 'Seleccione el Negócio al que pertenece el establecimiento.',
      'de': 'Seleccione el Geschäft al que pertenece el establecimiento.',
      'fr': 'Sélectionnez l’entreprise à laquelle appartient l’établissement.'
    },
    'establecimiento.codigoHelp': {
      'es': 'Ingrese el código único del establecimiento.',
      'en': 'Enter the unique code for the establishment.',
      'pt': 'Ingrese el Código único del establecimiento.',
      'de': 'Ingrese el Code único del establecimiento.',
      'fr': 'Entrez le code unique de l’établissement.'
    },
    'establecimiento.descripcionHelp': {
      'es': 'Ingrese una breve descripción del establecimiento.',
      'en': 'Enter a brief description of the establishment.',
      'pt': 'Ingrese una breve Descrição del establecimiento.',
      'de': 'Ingrese una breve Beschreibung del establecimiento.',
      'fr': 'Entrez une brève description de l’établissement.'
    },
    'establecimiento.direccionHelp': {
      'es': 'Ingrese la dirección completa del establecimiento.',
      'en': 'Enter the full address of the establishment.',
      'pt': 'Ingrese la Endereço completa del establecimiento.',
      'de': 'Ingrese la Adresse completa del establecimiento.',
      'fr': 'Entrez l’adresse complète de l’établissement.'
    },
    'establecimiento.telefonoHelp': {
      'es': 'Ingrese el número de teléfono de contacto.',
      'en': 'Enter the contact phone number.',
      'pt': 'Ingrese el número de Telefone de contacto.',
      'de': 'Ingrese el número de Telefon de contacto.',
      'fr': 'Entrez le numéro de téléphone de contact.'
    },
    'establecimiento.paisHelp': {
      'es': 'Seleccione un país para ver provincias.',
      'en': 'Select a country to see provinces.',
      'pt': 'Seleccione un País para Ver provincias.',
      'de': 'Seleccione un Land para Anzeigen provincias.',
      'fr': 'Sélectionnez un pays pour voir les provinces.'
    },
    'establecimiento.provinciaHelp': {
      'es': 'Seleccione una provincia para ver municipios.',
      'en': 'Select a province to see municipalities.',
      'pt': 'Seleccione una Província para Ver municipios.',
      'de': 'Seleccione una Provinz para Anzeigen municipios.',
      'fr': 'Sélectionnez une province pour voir les municipalités.'
    },
    'establecimiento.municipioHelp': {
      'es': 'Seleccione un municipio para ver códigos postales.',
      'en': 'Select a municipality to see postal codes.',
      'pt': 'Seleccione un Município para Ver códigos postales.',
      'de': 'Seleccione un Gemeinde para Anzeigen códigos postales.',
      'fr': 'Sélectionnez une municipalité pour voir les codes postaux.'
    },
    'establecimiento.codigoPostalHelp': {
      'es': 'Seleccione un código postal disponible.',
      'en': 'Select an available postal code.',
      'pt': 'Seleccione un Código Postal disponible.',
      'de': 'Seleccione un Code postal disponible.',
      'fr': 'Sélectionnez un code postal disponible.'
    },
    'establecimiento.placeholderCodigo': {
      'es': 'Ej: EST001',
      'en': 'Ex: EST001',
      'pt': 'Ej: EST001',
      'de': 'Ej: EST001',
      'fr': 'Ex: EST001'
    },
    'establecimiento.placeholderDescripcion': {
      'es': 'Ej: Sucursal Central',
      'en': 'Ex: Main Branch',
      'pt': 'Ej: Sucursal Central',
      'de': 'Ej: Sucursal Central',
      'fr': 'Ex: Succursale Principale'
    },
    'establecimiento.placeholderDireccion': {
      'es': 'Ej: Calle 123, Ciudad',
      'en': 'Ex: 123 Street, City',
      'fr': 'Ex: 123 Rue,  Ville',
      'pt': 'Ej: Calle 123, Ciudad',
      'de': 'Ej: Calle 123, Ciudad'
    },
    'establecimiento.placeholderTelefono': {
      'es': 'Ej: +34 912 345 678',
      'en': 'Ex: +34 912 345 678',
      'pt': 'Ej: +34 912 345 678',
      'de': 'Ej: +34 912 345 678',
      'fr': 'Ex: +33 1 23 45 67 89'
    },

    // ═══════════════════════════════════════════════════════════
    // 🏢 LOCALIDAD
    // ═══════════════════════════════════════════════════════════
    'localidad.establecimientoHelp': {
      'es': 'Seleccione un establecimiento para ver localidades.',
      'en': 'Select an establishment to see localities.',
      'pt': 'Seleccione un establecimiento para Ver localidades.',
      'de': 'Seleccione un establecimiento para Anzeigen localidades.',
      'fr': 'Sélectionnez un établissement pour voir les localités.'
    },
    'localidad.descripcionPlaceholder': {
      'es': 'Ej: Zona Industrial',
      'en': 'Ex: Industrial Area',
      'pt': 'Ej: Zona Industrial',
      'de': 'Ej: Zona Industrial',
      'fr': 'Ex: Zone Industrielle'
    },

    // ═══════════════════════════════════════════════════════════
    // 👤 USUARIO
    // ═══════════════════════════════════════════════════════════
    'usuario.placeholderUserName': {
      'es': 'Ej: jdoe',
      'en': 'Ex: jdoe',
      'pt': 'Ej: jdoe',
      'de': 'Ej: jdoe',
      'fr': 'Ex: jdoe'
    },
    'usuario.placeholderFullName': {
      'es': 'Ej: Juan Pérez',
      'en': 'Ex: John Smith',
      'pt': 'Ej: Juan Pérez',
      'de': 'Ej: Juan Pérez',
      'fr': 'Ex: Jean Dupont'
    },
    'usuario.placeholderEmail': {
      'es': 'Ej: usuario@empresa.com',
      'en': 'Ex: user@company.com',
      'pt': 'Ej: Usuário@Empresa.com',
      'de': 'Ej: Benutzer@Unternehmen.com',
      'fr': 'Ex: utilisateur@entreprise.com'
    },
    'usuario.authProviderHelp': {
      'es': 'Seleccione el método de autenticación.',
      'en': 'Select the authentication method.',
      'pt': 'Seleccione el método de autenticación.',
      'de': 'Seleccione el método de autenticación.',
      'fr': 'Sélectionnez la méthode d’authentification.'
    },
    'usuario.isActiveHelp': {
      'es': 'Indica si el usuario está activo o inactivo.',
      'en': 'Indicates if the user is active or inactive.',
      'pt': 'Indica si el Usuário está Ativo o Inativo.',
      'de': 'Indica si el Benutzer está Aktiv o Inaktiv.',
      'fr': 'Indique si l’utilisateur est actif ou inactif.'
    },

    // ═══════════════════════════════════════════════════════════
    // ⚙️ SYSTEM CONFIGURATION
    // ═══════════════════════════════════════════════════════════
    'systemConfig.placeholderCodigoSistema': {
      'es': 'Ej: GOL-ERP-001',
      'en': 'Ex: GOL-ERP-001',
      'pt': 'Ej: GOL-ERP-001',
      'de': 'Ej: GOL-ERP-001',
      'fr': 'Ex: GOL-ERP-001'
    },
    'systemConfig.placeholderLicencia': {
      'es': 'Código de licencia',
      'en': 'License code',
      'pt': 'Código de licencia',
      'de': 'Code de licencia',
      'fr': 'Code de licence'
    },
    'systemConfig.placeholderNombreNegocio': {
      'es': 'Nombre completo del negocio',
      'en': 'Full business name',
      'pt': 'Nome completo del Negócio',
      'de': 'Name completo del Geschäft',
      'fr': 'Nom complet de l’entreprise'
    },
    'systemConfig.placeholderDireccion': {
      'es': 'Dirección completa del negocio',
      'en': 'Full business address',
      'pt': 'Endereço completa del Negócio',
      'de': 'Adresse completa del Geschäft',
      'fr': 'Adresse complète de l’entreprise'
    },
    'systemConfig.logoUpload': {
      'es': 'Subir Logo',
      'en': 'Upload Logo',
      'pt': 'Subir Logo',
      'de': 'Subir Logo',
      'fr': 'Télécharger Logo'
    },
    'systemConfig.logoCurrentFile': {
      'es': 'Archivo actual',
      'en': 'Current file',
      'pt': 'Archivo actual',
      'de': 'Archivo actual',
      'fr': 'Fichier actuel'
    },
    'systemConfig.logoSelectFile': {
      'es': 'Seleccionar archivo de logo',
      'en': 'Select logo file',
      'pt': 'Seleccionar archivo de logo',
      'de': 'Seleccionar archivo de logo',
      'fr': 'Sélectionner fichier logo'
    },
    'systemConfig.logoChooseFile': {
      'es': 'Elegir Archivo',
      'en': 'Choose File',
      'pt': 'Elegir Archivo',
      'de': 'Elegir Archivo',
      'fr': 'Choisir Fichier'
    },
    'systemConfig.logoRemove': {
      'es': 'Quitar',
      'en': 'Remove',
      'pt': 'Quitar',
      'de': 'Quitar',
      'fr': 'Retirer'
    },
    'systemConfig.logoFormatHint': {
      'es': 'PNG, JPG, GIF o WEBP (máx. 2MB)',
      'en': 'PNG, JPG, GIF or WEBP (max 2MB)',
      'fr': 'PNG, JPG, GIF ou WEBP (max 2Mo)',
      'pt': 'PNG, JPG, GIF o WEBP (máx. 2MB)',
      'de': 'PNG, JPG, GIF o WEBP (máx. 2MB)'
    },
    'systemConfig.logoInvalidFormat': {
      'es': 'Formato no válido. Use PNG, JPG, GIF o WEBP.',
      'en': 'Invalid format. Use PNG, JPG, GIF or WEBP.',
      'fr': 'Format invalide. Utilisez PNG, JPG, GIF ou WEBP.',
      'pt': 'Formato Não válido. Use PNG, JPG, GIF o WEBP.',
      'de': 'Formato Nein válido. Use PNG, JPG, GIF o WEBP.'
    },
    'systemConfig.logoSizeExceeded': {
      'es': 'El archivo no puede superar 2MB.',
      'en': 'File cannot exceed 2MB.',
      'pt': 'El archivo Não puede superar 2MB.',
      'de': 'El archivo Nein puede superar 2MB.',
      'fr': 'Le fichier ne peut pas dépasser 2Mo.'
    },
    'systemConfig.logoUploadError': {
      'es': 'Error al subir el logo',
      'en': 'Error uploading logo',
      'pt': 'Erro al subir el logo',
      'de': 'Fehler al subir el logo',
      'fr': 'Erreur lors du téléchargement du logo'
    },
    'systemConfig.noLogo': {
      'es': 'Sin logo configurado',
      'en': 'No logo configured',
      'pt': 'Sin logo configurado',
      'de': 'Sin logo configurado',
      'fr': 'Aucun logo configuré'
    },
    'systemConfig.logoPreview': {
      'es': 'Vista previa del logo',
      'en': 'Logo preview',
      'pt': 'Vista previa del logo',
      'de': 'Vista previa del logo',
      'fr': 'Aperçu du logo'
    },

    // ═══════════════════════════════════════════════════════════
    // 🕒 AUDITORÍA
    // ═══════════════════════════════════════════════════════════
    'audit.title': {
      'es': 'Auditoría',
      'en': 'Audit',
      'pt': 'Auditoría',
      'de': 'Auditoría',
      'fr': 'Audit'
    },
    'audit.createdBy': {
      'es': 'por',
      'en': 'by',
      'pt': 'por',
      'de': 'por',
      'fr': 'par'
    },
    'audit.createdAt': {
      'es': 'Creado',
      'en': 'Created',
      'pt': 'Criado',
      'de': 'Erstellt',
      'fr': 'Créé'
    },
    'audit.modifiedBy': {
      'es': 'por',
      'en': 'by',
      'pt': 'por',
      'de': 'por',
      'fr': 'par'
    },
    'audit.modifiedAt': {
      'es': 'Última modificación',
      'en': 'Last modified',
      'pt': 'Última modificación',
      'de': 'Última modificación',
      'fr': 'Dernière modification'
    },

    // ═══════════════════════════════════════════════════════════
    // ⚠️ VALIDACIONES Y ERRORES
    // ═══════════════════════════════════════════════════════════
    'validation.required': {
      'es': 'Este campo es requerido',
      'en': 'This field is required',
      'pt': 'Este campo es requerido',
      'de': 'Este campo es requerido',
      'fr': 'Ce champ est obligatoire'
    },
    'validation.maxLength': {
      'es': 'Máximo {0} caracteres',
      'en': 'Maximum {0} characters',
      'fr': 'Maximum {0} caractères',
      'pt': 'Máximo {0} caracteres',
      'de': 'Maximal {0} Zeichen'
    },
    'validation.email': {
      'es': 'Email no válido',
      'en': 'Invalid email',
      'pt': 'Email Não válido',
      'de': 'E-Mail Nein válido',
      'fr': 'Email invalide'
    },
    'validation.usernameRequired': {
      'es': '⚠️ El usuario es obligatorio',
      'en': '⚠️ Username is required',
      'pt': '⚠️ El Usuário es obligatorio',
      'de': '⚠️ El Benutzer es obligatorio',
      'fr': '⚠️ Le nom d\'utilisateur est obligatoire'
    },
    'validation.usernameMinLength': {
      'es': '⚠️ El usuario debe tener al menos 3 caracteres',
      'en': '⚠️ Username must be at least 3 characters',
      'pt': '⚠️ El Usuário debe tener al menos 3 caracteres',
      'de': '⚠️ El Benutzer debe tener al menos 3 caracteres',
      'fr': '⚠️ Le nom d\'utilisateur doit comporter au moins 3 caractères'
    },
    'validation.passwordRequired': {
      'es': '⚠️ La contraseña es obligatoria',
      'en': '⚠️ Password is required',
      'pt': '⚠️ La Senha es obligatoria',
      'de': '⚠️ La Passwort es obligatoria',
      'fr': '⚠️ Le mot de passe est obligatoire'
    },
    'validation.passwordMinLength': {
      'es': '⚠️ La contraseña debe tener al menos 8 caracteres',
      'en': '⚠️ Password must be at least 8 characters',
      'pt': '⚠️ La Senha debe tener al menos 8 caracteres',
      'de': '⚠️ La Passwort debe tener al menos 8 caracteres',
      'fr': '⚠️ Le mot de passe doit comporter au moins 8 caractères'
    },
    'validation.codigo5Digitos': {
      'es': 'El código debe ser un número de 5 dígitos',
      'en': 'Code must be a 5-digit number',
      'pt': 'El Código debe ser un número de 5 dígitos',
      'de': 'El Code debe ser un número de 5 dígitos',
      'fr': 'Le code doit être un nombre à 5 chiffres'
    },
    'validation.codigo3Digitos': {
      'es': 'El código debe ser un número de 3 dígitos',
      'en': 'Code must be a 3-digit number',
      'pt': 'El Código debe ser un número de 3 dígitos',
      'de': 'El Code debe ser un número de 3 dígitos',
      'fr': 'Le code doit être un nombre à 3 chiffres'
    },
    'validation.codigo2Digitos': {
      'es': 'El código debe ser un número de 2 dígitos',
      'en': 'Code must be a 2-digit number',
      'pt': 'El Código debe ser un número de 2 dígitos',
      'de': 'El Code debe ser un número de 2 dígitos',
      'fr': 'Le code doit être un nombre à 2 chiffres'
    },
    'validation.numeric': {
      'es': 'El código debe ser numérico',
      'en': 'Code must be numeric',
      'pt': 'El Código debe ser numérico',
      'de': 'El Code debe ser numérico',
      'fr': 'Le code doit être numérique'
    },
    'validation.alphanumeric': {
      'es': 'El código debe ser alfanumérico',
      'en': 'Code must be alphanumeric',
      'pt': 'El Código debe ser alfanumérico',
      'de': 'El Code debe ser alfanumérico',
      'fr': 'Le code doit être alphanumérique'
    },
    'validation.length': {
      'es': 'Longitud inválida',
      'en': 'Invalid length',
      'pt': 'Longitud inválida',
      'de': 'Longitud inválida',
      'fr': 'Longueur invalide'
    },
    'validation.invalid': {
      'es': 'Valor inválido',
      'en': 'Invalid value',
      'pt': 'Valor inválido',
      'de': 'Valor inválido',
      'fr': 'Valeur invalide'
    },
    'validation.ivaRequired': {
      'es': 'El IVA es obligatorio',
      'en': 'VAT is required',
      'pt': 'El IVA es obligatorio',
      'de': 'El IVA es obligatorio',
      'fr': 'La TVA est obligatoire'
    },
    'validation.ivaInteger': {
      'es': 'El IVA debe ser un número entero',
      'en': 'VAT must be an integer',
      'pt': 'El IVA debe ser un número entero',
      'de': 'El IVA debe ser un número entero',
      'fr': 'La TVA doit être un entier'
    },
    'validation.ivaRange': {
      'es': 'El IVA debe estar entre 0 y 100',
      'en': 'VAT must be between 0 and 100',
      'pt': 'El IVA debe estar entre 0 y 100',
      'de': 'El IVA debe estar entre 0 y 100',
      'fr': 'La TVA doit être comprise entre 0 et 100'
    },
    'validation.codigo8Digitos': {
      'es': 'El código debe ser numérico de 8 dígitos (00000000-99999999)',
      'en': 'Code must be an 8-digit number (00000000-99999999)',
      'pt': 'El Código debe ser numérico de 8 dígitos (00000000-99999999)',
      'de': 'El Code debe ser numérico de 8 dígitos (00000000-99999999)',
      'fr': 'Le code doit être un nombre à 8 chiffres (00000000-99999999)'
    },

    'error.loading': {
      'es': 'Error al cargar los datos',
      'en': 'Error loading data',
      'pt': 'Erro al cargar los datos',
      'de': 'Fehler al cargar los datos',
      'fr': 'Erreur lors du chargement des données'
    },
    'error.saving': {
      'es': 'Error al guardar',
      'en': 'Error saving',
      'pt': 'Erro al Salvar',
      'de': 'Fehler al Speichern',
      'fr': 'Erreur lors de l\'enregistrement'
    },
    'error.deleting': {
      'es': 'Error al eliminar',
      'en': 'Error deleting',
      'pt': 'Erro al Excluir',
      'de': 'Fehler al Löschen',
      'fr': 'Erreur lors de la suppression'
    },
    'confirm.delete': {
      'es': '¿Está seguro que desea eliminar?',
      'en': 'Are you sure you want to delete?',
      'pt': '¿Está seguro que desea Excluir?',
      'de': '¿Está seguro que desea Löschen?',
      'fr': 'Êtes-vous sûr de vouloir supprimer?'
    },
    'fiscal.title': {
      'es': 'Datos Fiscales',
      'en': 'Tax Information',
      'pt': 'Datos Fiscales',
      'de': 'Datos Fiscales',
      'fr': 'Informations Fiscales'
    },
    'fiscal.titleEmpresa': {
      'es': 'Datos Fiscales de la Empresa',
      'en': 'Company Tax Information',
      'pt': 'Datos Fiscales de la Empresa',
      'de': 'Datos Fiscales de la Unternehmen',
      'fr': 'Informations Fiscales de l\'Entreprise'
    },
    'fiscal.tipoIdentificadorFiscal': {
      'es': 'Tipo de Identificación Fiscal',
      'en': 'Tax ID Type',
      'pt': 'Tipo de Identificación Fiscal',
      'de': 'Tipo de Identificación Fiscal',
      'fr': 'Type d\'Identifiant Fiscal'
    },
    'fiscal.regimenFiscal': {
      'es': 'Régimen Fiscal',
      'en': 'Tax Regime',
      'pt': 'Régimen Fiscal',
      'de': 'Régimen Fiscal',
      'fr': 'Régime Fiscal'
    },
    'fiscal.identificadorFiscal': {
      'es': 'Identificador Fiscal',
      'en': 'Tax ID',
      'pt': 'Identificador Fiscal',
      'de': 'Identificador Fiscal',
      'fr': 'Identifiant Fiscal'
    },
    'fiscal.tasaIvaDefecto': {
      'es': 'Tasa IVA por Defecto (%)',
      'en': 'Default VAT Rate (%)',
      'pt': 'Tasa IVA por Defecto (%)',
      'de': 'Tasa IVA por Defecto (%)',
      'fr': 'Taux TVA par Défaut (%)'
    },
    'fiscal.registradaIva': {
      'es': 'Registrada para IVA',
      'en': 'Registered for VAT',
      'pt': 'Registrada para IVA',
      'de': 'Registrada para IVA',
      'fr': 'Enregistrée pour TVA'
    },
    'fiscal.ivaInternacional': {
      'es': 'IVA Internacional/Intracomunitario',
      'en': 'International/Intra-Community VAT',
      'pt': 'IVA Internacional/Intracomunitario',
      'de': 'IVA Internacional/Intracomunitario',
      'fr': 'TVA Internationale/Intracommunautaire'
    },
    'fiscal.exentoIva': {
      'es': 'Exento de IVA',
      'en': 'VAT Exempt',
      'pt': 'Exento de IVA',
      'de': 'Exento de IVA',
      'fr': 'Exempté de TVA'
    },
    'fiscal.exentoIvaHelp': {
      'es': 'Marque si está exento de IVA/TVA.',
      'en': 'Check if exempt from VAT.',
      'pt': 'Marque si está exento de IVA/TVA.',
      'de': 'Marque si está exento de IVA/TVA.',
      'fr': 'Cochez si exempté de TVA.'
    },
    'fiscal.extranjero': {
      'es': 'Extranjero',
      'en': 'Foreign',
      'pt': 'Extranjero',
      'de': 'Extranjero',
      'fr': 'Étranger'
    },
    'fiscal.extranjeroHelp': {
      'es': 'Marque si es una entidad extranjera.',
      'en': 'Check if it is a foreign entity.',
      'pt': 'Marque si es una entidad extranjera.',
      'de': 'Marque si es una entidad extranjera.',
      'fr': 'Cochez s\'il s\'agit d\'une entité étrangère.'
    },
    'fiscal.codigoPaisIso': {
      'es': 'Código País ISO',
      'en': 'ISO Country Code',
      'pt': 'Código País ISO',
      'de': 'Code Land ISO',
      'fr': 'Code Pays ISO'
    },
    'fiscal.codigoPaisIsoHelp': {
      'es': 'Código ISO del país (ej: ES para España, FR para Francia, MX para México).',
      'en': 'ISO country code (e.g., ES for Spain, FR for France, MX for Mexico).',
      'fr': 'Code ISO du pays (par ex. ES pour Espagne, FR pour France, MX pour Mexique).',
      'pt': 'Código ISO del País (ej: ES para España, FR para Francia, MX para México).',
      'de': 'Code ISO del Land (ej: ES para España, FR para Francia, MX para México).'
    },
    'fiscal.validarIdentificadorFiscal': {
      'es': 'Validar Identificador Fiscal',
      'en': 'Validate Tax ID',
      'pt': 'Validar Identificador Fiscal',
      'de': 'Validar Identificador Fiscal',
      'fr': 'Valider Identifiant Fiscal'
    },
    'fiscal.validarIdentificadorFiscalHelp': {
      'es': 'Marque para validar el identificador fiscal automáticamente.',
      'en': 'Check to automatically validate the tax ID.',
      'pt': 'Marque para validar el identificador fiscal automáticamente.',
      'de': 'Marque para validar el identificador fiscal automáticamente.',
      'fr': 'Cochez pour valider automatiquement l\'identifiant fiscal.'
    },
    'fiscal.inversionSujetoPasivo': {
      'es': 'Inversión del Sujeto Pasivo',
      'en': 'Reverse Charge',
      'pt': 'Inversión del Sujeto Pasivo',
      'de': 'Inversión del Sujeto Pasivo',
      'fr': 'Autoliquidation'
    },
    'fiscal.inversionSujetoPasivoHelp': {
      'es': 'Marque si aplica inversión del sujeto pasivo en operaciones.',
      'en': 'Check if reverse charge applies to operations.',
      'pt': 'Marque si aplica inversión del sujeto pasivo en operaciones.',
      'de': 'Marque si aplica inversión del sujeto pasivo en operaciones.',
      'fr': 'Cochez si l\'autoliquidation s\'applique aux opérations.'
    },
    'fiscal.identificadorFiscalHelp': {
      'es': 'Número de identificación fiscal del negocio (NIF, CIF, VAT, etc.). Opcional si no aplica.',
      'en': 'Business tax identification number (NIF, CIF, VAT, etc.). Optional if not applicable.',
      'fr': 'Numéro d\'identification fiscale de l\'entreprise (NIF, CIF, TVA, etc.). Optionnel si non applicable.',
      'pt': 'Número de identificación fiscal del Negócio (NIF, CIF, VAT, etc.). Opcional si Não aplica.',
      'de': 'Número de identificación fiscal del Geschäft (NIF, CIF, VAT, etc.). Opcional si Nein aplica.'
    },
    'fiscal.tipoIdentificadorFiscalHelp': {
      'es': 'Seleccione el tipo de identificación fiscal que corresponda a su negocio.',
      'en': 'Select the type of tax identification that applies to your business.',
      'pt': 'Seleccione el tipo de identificación fiscal que corresponda a su Negócio.',
      'de': 'Seleccione el tipo de identificación fiscal que corresponda a su Geschäft.',
      'fr': 'Sélectionnez le type d\'identification fiscale applicable à votre entreprise.'
    },
    'fiscal.regimenFiscalHelp': {
      'es': 'Régimen fiscal aplicable al negocio (General, Simplificado, etc.).',
      'en': 'Tax regime applicable to the business (General, Simplified, etc.).',
      'fr': 'Régime fiscal applicable à l\'entreprise (Général, Simplifié, etc.).',
      'pt': 'Régimen fiscal aplicable al Negócio (General, Simplificado, etc.).',
      'de': 'Régimen fiscal aplicable al Geschäft (General, Simplificado, etc.).'
    },
    'fiscal.tasaIvaDefectoHelp': {
      'es': 'Tasa de IVA predeterminada para operaciones del negocio (0-99.99%). Ej: 21 para 21%.',
      'en': 'Default VAT rate for business operations (0-99.99%). Ex: 21 for 21%.',
      'fr': 'Taux de TVA par défaut pour les opérations commerciales (0-99.99%). Ex: 21 pour 21%.',
      'pt': 'Tasa de IVA predeterminada para operaciones del Negócio (0-99.99%). Ej: 21 para 21%.',
      'de': 'Tasa de IVA predeterminada para operaciones del Geschäft (0-99.99%). Ej: 21 para 21%.'
    },
    'fiscal.registradaIvaHelp': {
      'es': 'Marque si el negocio está registrado para IVA/TVA.',
      'en': 'Check if the business is registered for VAT.',
      'pt': 'Marque si el Negócio está registrado para IVA/TVA.',
      'de': 'Marque si el Geschäft está registrado para IVA/TVA.',
      'fr': 'Cochez si l\'entreprise est enregistrée pour la TVA.'
    },
    'fiscal.ivaInternacionalHelp': {
      'es': 'Marque si el negocio realiza operaciones intracomunitarias o internacionales con IVA.',
      'en': 'Check if the business performs intra-community or international VAT operations.',
      'pt': 'Marque si el Negócio realiza operaciones intracomunitarias o internacionales con IVA.',
      'de': 'Marque si el Geschäft realiza operaciones intracomunitarias o internacionales con IVA.',
      'fr': 'Cochez si l\'entreprise effectue des opérations intracommunautaires ou internationales avec TVA.'
    },
    'fiscal.placeholderTipoIdentificadorFiscal': {
      'es': 'Seleccione tipo',
      'en': 'Select type',
      'pt': 'Seleccione tipo',
      'de': 'Seleccione tipo',
      'fr': 'Sélectionner type'
    },
    'fiscal.placeholderRegimenFiscal': {
      'es': 'Seleccione régimen',
      'en': 'Select regime',
      'pt': 'Seleccione régimen',
      'de': 'Seleccione régimen',
      'fr': 'Sélectionner régime'
    },
    'fiscal.placeholderCodigoPaisIso': {
      'es': 'ES, FR, MX...',
      'en': 'ES, FR, MX...',
      'fr': 'ES, FR, MX...',
      'pt': 'ES, FR, MX...',
      'de': 'ES, FR, MX...'
    },
    'fiscal.placeholderIdentificadorFiscal': {
      'es': 'Ej: 12345678A, A12345678',
      'en': 'Ex: 12345678A, A12345678',
      'fr': 'Ex: 12345678A, A12345678',
      'pt': 'Ej: 12345678A, A12345678',
      'de': 'Ej: 12345678A, A12345678'
    },
    'fiscal.placeholderTasaIva': {
      'es': 'Ej: 21.00',
      'en': 'Ex: 21.00',
      'pt': 'Ej: 21.00',
      'de': 'Ej: 21.00',
      'fr': 'Ex: 21.00'
    }

  };

  /**
   * Constructor del servicio de traducción.
   * @param languageService Servicio de gestión de idiomas
   */
  constructor(private languageService: LanguageService) {
    // Suscribirse a cambios de idioma para notificar a los componentes
    this.languageService.currentLanguage$.subscribe(() => {
      this.translationsSubject.next(Date.now().toString());
    });
  }

  /**
   * Obtiene la traducción de una clave en el idioma actual.
   * 
   * @param key - Clave de traducción (ej: 'common.save', 'login.title')
   * @param params - Array de parámetros opcionales para reemplazar en el texto.
   *                 Se reemplazan usando la sintaxis {0}, {1}, etc.
   * @returns Texto traducido en el idioma actual. Si no existe la traducción, devuelve la clave.
   * 
   * @example
   * // Traducción simple
   * translate('common.save') // => 'Guardar' (en español)
   * 
   * // Traducción con parámetros
   * translate('validation.maxLength', [50]) // => 'Máximo 50 caracteres'
   * 
   * // Footer con año automático
   * translate('login.footer') // => '© 2024 - Chokisoft Soluciones Tecnológicas'
   */
  translate(key: string, params: any[] = []): string {
    const currentLang = this.languageService.getCurrentLanguage();
    const translation = this.translations[key]?.[currentLang] || key;

    // Si se solicita login.footer y no hay parámetros, inyectar el año actual automáticamente
    if (key === 'login.footer' && (!params || params.length === 0)) {
      params = [new Date().getFullYear()];
    }

    // Reemplazar parámetros si existen (ej: {0}, {1})
    if (params.length > 0) {
      return translation.replace(/{(\d+)}/g, (match, index) => {
        return params[index] !== undefined ? String(params[index]) : match;
      });
    }

    return translation;
  }

  /**
   * Obtiene todas las traducciones que comienzan con un prefijo específico.
   * 
   * @param prefix - Prefijo de las claves (ej: 'common', 'grupoCuenta', 'login')
   * @returns Objeto con las traducciones filtradas donde la clave es el sufijo después del prefijo
   * 
   * @example
   * // Obtener todas las traducciones comunes
   * getTranslationsFor('common')
   * // Retorna: { save: 'Guardar', cancel: 'Cancelar', ... }
   * 
   * // Obtener traducciones de grupo cuenta
   * getTranslationsFor('grupoCuenta')
   * // Retorna: { title: 'Grupos de Cuenta', subtitle: '...', ... }
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
