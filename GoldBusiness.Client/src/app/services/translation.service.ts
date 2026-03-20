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
    'login.formTitle': {
      'es': 'Acceso al sistema',
      'en': 'System Access',
      'fr': 'Accès au système'
    },
    'login.formSubtitle': {
      'es': 'Ingrese sus credenciales',
      'en': 'Enter your credentials',
      'fr': 'Entrez vos identifiants'
    },
    'login.title': {
      'es': 'GoldBusiness',
      'en': 'GoldBusiness',
      'fr': 'GoldBusiness'
    },
    'login.subtitle': {
      'es': 'Sistema de Gestión Empresarial',
      'en': 'Enterprise Management System',
      'fr': 'Système de Gestion d\'Entreprise'
    },
    'login.username': {
      'es': 'Usuario o correo',
      'en': 'Username or email',
      'fr': 'Nom d\'utilisateur ou email'
    },
    'login.password': {
      'es': 'Contraseña',
      'en': 'Password',
      'fr': 'Mot de passe'
    },
    'login.usernamePlaceholder': {
      'es': 'Ingrese su usuario o correo',
      'en': 'Enter your username or email',
      'fr': 'Entrez votre nom d\'utilisateur ou email'
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
    'login.googleSignIn': {
      'es': 'Entrar con Google',
      'en': 'Sign in with Google',
      'fr': 'Se connecter avec Google'
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
      'es': '\u00A9 {0} - Chokisoft Soluciones Tecnológicas',
      'en': '\u00A9 {0} - Chokisoft Technology Solutions',
      'fr': '\u00A9 {0} - Chokisoft Solutions Technologiques'
    },
    // ✅ NUEVO: Errores de Google OAuth
    'login.errorGoogleUserNotFound': {
      'es': '❌ Usuario no encontrado. Debe ser creado previamente por un administrador.',
      'en': '❌ User not found. Must be created by an administrator first.',
      'fr': '❌ Utilisateur introuvable. Doit être créé par un administrateur d\'abord.'
    },
    'login.errorGoogleProviderNotAllowed': {
      'es': '❌ Su cuenta no está configurada para Google. Use usuario y contraseña.',
      'en': '❌ Your account is not configured for Google. Use username and password.',
      'fr': '❌ Votre compte n\'est pas configuré pour Google. Utilisez nom d\'utilisateur et mot de passe.'
    },
    'login.errorGoogleUserInactive': {
      'es': '❌ Su cuenta está inactiva. Contacte al administrador.',
      'en': '❌ Your account is inactive. Contact the administrator.',
      'fr': '❌ Votre compte est inactif. Contactez l\'administrateur.'
    },
    'login.errorGoogleEmailNotFound': {
      'es': '❌ No se pudo obtener el email desde Google.',
      'en': '❌ Could not retrieve email from Google.',
      'fr': '❌ Impossible de récupérer l\'email depuis Google.'
    },
    'login.errorGoogleTokenFailed': {
      'es': '❌ Error al generar token de autenticación.',
      'en': '❌ Error generating authentication token.',
      'fr': '❌ Erreur lors de la génération du jeton d\'authentification.'
    },
    'login.errorGoogleRemoteFailure': {
      'es': '❌ Error de comunicación con Google. Intente nuevamente.',
      'en': '❌ Google communication error. Please try again.',
      'fr': '❌ Erreur de communication avec Google. Veuillez réessayer.'
    },
    'login.errorGoogleInternalError': {
      'es': '❌ Error interno del servidor. Contacte al soporte técnico.',
      'en': '❌ Internal server error. Contact technical support.',
      'fr': '❌ Erreur interne du serveur. Contactez le support technique.'
    },
    'login.errorGoogleUserCreationFailed': {
      'es': '❌ Error al crear el usuario. Contacte al administrador.',
      'en': '❌ Error creating user. Contact the administrator.',
      'fr': '❌ Erreur lors de la création de l\'utilisateur. Contactez l\'administrateur.'
    },
    'login.errorGoogleGeneric': {
      'es': '❌ Error de autenticación con Google',
      'en': '❌ Google authentication error',
      'fr': '❌ Erreur d\'authentification Google'
    },
    'login.errorGoogleCompleteLogin': {
      'es': '❌ No se pudo completar el inicio de sesión con Google.',
      'en': '❌ Could not complete Google sign-in.',
      'fr': '❌ Impossible de terminer la connexion Google.'
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
    'sidebar.terceros': {
      'es': 'Terceros',
      'en': 'Third Parties',
      'fr': 'Tiers'
    },
    'sidebar.organizacion': {
      'es': 'Organización',
      'en': 'Organization',
      'fr': 'Organisation'
    },
    'sidebar.clasificador': {
      'es': 'Clasificador',
      'en': 'Classifier',
      'fr': 'Classificateur'
    },
    'sidebar.operaciones': {
      'es': 'Operaciones',
      'en': 'Operations',
      'fr': 'Opérations'
    },
    'sidebar.producto': {
      'es': 'Producto',
      'en': 'Product',
      'fr': 'Produit'
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
    'sidebar.expandMenu': {
      'es': 'Expandir menú',
      'en': 'Expand menu',
      'fr': 'Développer le menu'
    },
    'sidebar.collapseMenu': {
      'es': 'Colapsar menú',
      'en': 'Collapse menu',
      'fr': 'Réduire le menu'
    },
    'sidebar.testConnection': {
      'es': 'Prueba de Conexión',
      'en': 'Connection Test',
      'fr': 'Test de Connexion'
    },

    // ═══════════════════════════════════════════════════════════
    // 👥 TERCEROS
    // ═══════════════════════════════════════════════════════════
    'proveedores.title': {
      'es': 'Proveedores',
      'en': 'Suppliers',
      'fr': 'Fournisseurs'
    },
    'proveedores.subtitle': {
      'es': 'Gestión de proveedores',
      'en': 'Suppliers management',
      'fr': 'Gestion des fournisseurs'
    },
    'proveedores.newTitle': {
      'es': 'Nuevo Proveedor',
      'en': 'New Supplier',
      'fr': 'Nouveau Fournisseur'
    },
    'proveedores.editTitle': {
      'es': 'Editar Proveedor',
      'en': 'Edit Supplier',
      'fr': 'Modifier Fournisseur'
    },
    'proveedores.detailTitle': {
      'es': 'Detalle del Proveedor',
      'en': 'Supplier Details',
      'fr': 'Détails du Fournisseur'
    },
    'proveedores.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'proveedores.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
    },
    'proveedores.rfc': {
      'es': 'RFC/CIF',
      'en': 'Tax ID',
      'fr': 'Numéro de TVA'
    },
    'proveedores.telefono': {
      'es': 'Teléfono',
      'en': 'Phone',
      'fr': 'Téléphone'
    },
    'proveedores.email': {
      'es': 'Email',
      'en': 'Email',
      'fr': 'Email'
    },
    'proveedores.direccion': {
      'es': 'Dirección',
      'en': 'Address',
      'fr': 'Adresse'
    },
    'proveedores.iva': {
      'es': 'IVA',
      'en': 'VAT',
      'fr': 'TVA'
    },
    'proveedores.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'proveedores.codigoPlaceholder': {
      'es': 'Ingrese código (5 dígitos)',
      'en': 'Enter code (5 chars)',
      'fr': 'Entrez le code (5 caractères)'
    },
    'proveedores.descripcionPlaceholder': {
      'es': 'Ej: Nombre del proveedor',
      'en': 'Ex: Supplier name',
      'fr': 'Ex: Nom du fournisseur'
    },
    'proveedores.direccionPlaceholder': {
      'es': 'Ej: Calle y número',
      'en': 'Ex: Street and number',
      'fr': 'Ex: Rue et numéro'
    },
    'proveedores.telefonoPlaceholder': {
      'es': 'Ej: +34 912 345 678',
      'en': 'Ex: +34 912 345 678',
      'fr': 'Ex: +33 1 23 45 67 89'
    },
    'proveedores.emailPlaceholder': {
      'es': 'Ej: proveedor@empresa.com',
      'en': 'Ex: supplier@company.com',
      'fr': 'Ex: fournisseur@entreprise.com'
    },
    'proveedores.ivaPlaceholder': {
      'es': 'Ej: 21.00',
      'en': 'Ex: 21.00',
      'fr': 'Ex: 21.00'
    },

    'clientes.title': {
      'es': 'Clientes',
      'en': 'Clients',
      'fr': 'Clients'
    },
    'clientes.subtitle': {
      'es': 'Gestión de clientes',
      'en': 'Clients management',
      'fr': 'Gestion des clients'
    },
    'clientes.newTitle': {
      'es': 'Nuevo Cliente',
      'en': 'New Client',
      'fr': 'Nouveau Client'
    },
    'clientes.editTitle': {
      'es': 'Editar Cliente',
      'en': 'Edit Client',
      'fr': 'Modifier Client'
    },
    'clientes.detailTitle': {
      'es': 'Detalle del Cliente',
      'en': 'Client Details',
      'fr': 'Détails du Client'
    },
    'clientes.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'fr': 'Informations Principales'
    },
    'clientes.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'clientes.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
    },
    'clientes.telefono': {
      'es': 'Teléfono',
      'en': 'Phone',
      'fr': 'Téléphone'
    },
    'clientes.email': {
      'es': 'Email',
      'en': 'Email',
      'fr': 'Email'
    },
    'clientes.direccion': {
      'es': 'Dirección',
      'en': 'Address',
      'fr': 'Adresse'
    },
    'clientes.iva': {
      'es': 'IVA',
      'en': 'VAT',
      'fr': 'TVA'
    },
    'clientes.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'clientes.nif': {
      'es': 'NIF',
      'en': 'NIF',
      'fr': 'NIF'
    },
    'clientes.codigoPlaceholder': {
      'es': 'Ingrese código (8 dígitos)',
      'en': 'Enter code (8 chars)',
      'fr': 'Entrez le code (8 caractères)'
    },
    'clientes.descripcionPlaceholder': {
      'es': 'Ej: Nombre del cliente o razón social',
      'en': 'Ex: Customer name or company',
      'fr': 'Ex: Nom du client ou raison sociale'
    },
    'clientes.direccionPlaceholder': {
      'es': 'Ej: Calle y número',
      'en': 'Ex: Street and number',
      'fr': 'Ex: Rue et numéro'
    },
    'clientes.telefonoPlaceholder': {
      'es': 'Ej: +34 912 345 678',
      'en': 'Ex: +34 912 345 678',
      'fr': 'Ex: +33 1 23 45 67 89'
    },
    'clientes.emailPlaceholder': {
      'es': 'Ej: cliente@empresa.com',
      'en': 'Ex: customer@company.com',
      'fr': 'Ex: client@entreprise.com'
    },
    'clientes.ivaPlaceholder': {
      'es': 'Ej: 21.00',
      'en': 'Ex: 21.00',
      'fr': 'Ex: 21.00'
    },

    // ═══════════════════════════════════════════════════════════
    // 🏢 ORGANIZACIÓN
    // ═══════════════════════════════════════════════════════════
    'establecimiento.title': {
      'es': 'Establecimientos',
      'en': 'Establishments',
      'fr': 'Établissements'
    },
    'establecimiento.subtitle': {
      'es': 'Gestión de establecimientos',
      'en': 'Establishments management',
      'fr': 'Gestion des établissements'
    },
    'establecimiento.newTitle': {
      'es': 'Nuevo Establecimiento',
      'en': 'New Establishment',
      'fr': 'Nouvel Établissement'
    },
    'establecimiento.editTitle': {
      'es': 'Editar Establecimiento',
      'en': 'Edit Establishment',
      'fr': 'Modifier Établissement'
    },
    'establecimiento.detailTitle': {
      'es': 'Detalle del Establecimiento',
      'en': 'Establishment Details',
      'fr': 'Détails de l\'Établissement'
    },
    'establecimiento.detailSubtitle': {
      'es': 'Visualización completa de la información del establecimiento',
      'en': 'Complete view of establishment information',
      'fr': 'Vue complète des informations de l\'établissement'
    },
    'establecimiento.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'fr': 'Informations Principales'
    },
    'establecimiento.infoContacto': {
      'es': 'Información de Contacto',
      'en': 'Contact Information',
      'fr': 'Informations de Contact'
    },
    'establecimiento.infoUbicacion': {
      'es': 'Información de Ubicación',
      'en': 'Location Information',
      'fr': 'Informations de Localisation'
    },
    'establecimiento.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'establecimiento.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
    },
    'establecimiento.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'establecimiento.telefono': {
      'es': 'Teléfono',
      'en': 'Phone',
      'fr': 'Téléphone'
    },
    'establecimiento.email': {
      'es': 'Email',
      'en': 'Email',
      'fr': 'Email'
    },
    'establecimiento.direccion': {
      'es': 'Dirección',
      'en': 'Address',
      'fr': 'Adresse'
    },
    'establecimiento.localidad': {
      'es': 'Localidad',
      'en': 'Locality',
      'fr': 'Localité'
    },
    'establecimiento.codigoPlaceholder': {
      'es': 'Ej: EST001',
      'en': 'Ex: EST001',
      'fr': 'Ex: EST001'
    },
    'establecimiento.nombrePlaceholder': {
      'es': 'Ej: Sucursal Centro',
      'en': 'Ex: Downtown Branch',
      'fr': 'Ex: Succursale Centre-Ville'
    },
    'establecimiento.descripcionPlaceholder': {
      'es': 'Ej: Sucursal principal en el centro de la ciudad',
      'en': 'Ex: Main branch in downtown',
      'fr': 'Ex: Succursale principale au centre-ville'
    },
    'establecimiento.direccionPlaceholder': {
      'es': 'Ej: Calle Principal 123',
      'en': 'Ex: Main Street 123',
      'fr': 'Ex: Rue Principale 123'
    },
    'establecimiento.telefonoPlaceholder': {
      'es': 'Ej: +52 55 1234 5678',
      'en': 'Ex: +52 55 1234 5678',
      'fr': 'Ex: +52 55 1234 5678'
    },
    'establecimiento.emailPlaceholder': {
      'es': 'Ej: establecimiento@empresa.com',
      'en': 'Ex: establishment@company.com',
      'fr': 'Ex: etablissement@entreprise.com'
    },
    'establecimiento.negocio': {
      'es': 'Negocio',
      'en': 'Business',
      'fr': 'Entreprise'
    },
    'establecimiento.pais': {
      'es': 'País',
      'en': 'Country',
      'fr': 'Pays'
    },
    'establecimiento.provincia': {
      'es': 'Provincia',
      'en': 'Province',
      'fr': 'Province'
    },
    'establecimiento.municipio': {
      'es': 'Municipio',
      'en': 'Municipality',
      'fr': 'Municipalité'
    },
    'establecimiento.codigoPostal': {
      'es': 'Código Postal',
      'en': 'Postal Code',
      'fr': 'Code Postal'
    },
    'establecimiento.activo': {
      'es': 'Activo',
      'en': 'Active',
      'fr': 'Actif'
    },
    'establecimiento.activoHelp': {
      'es': 'Indica si el establecimiento está activo.',
      'en': 'Indicates if the establishment is active.',
      'fr': 'Indique si l’établissement est actif.'
    },
    'localidad.title': {
      'es': 'Localidades',
      'en': 'Localities',
      'fr': 'Localités'
    },
    'localidad.subtitle': {
      'es': 'Gestión de localidades',
      'en': 'Localities management',
      'fr': 'Gestion des localités'
    },
    'localidad.newTitle': {
      'es': 'Nueva Localidad',
      'en': 'New Locality',
      'fr': 'Nouvelle Localité'
    },
    'localidad.editTitle': {
      'es': 'Editar Localidad',
      'en': 'Edit Locality',
      'fr': 'Modifier Localité'
    },
    'localidad.detailTitle': {
      'es': 'Detalle de Localidad',
      'en': 'Locality Details',
      'fr': 'Détails de la Localité'
    },
    'localidad.detailSubtitle': {
      'es': 'Visualización completa de la información de la localidad',
      'en': 'Complete view of locality information',
      'fr': 'Vue complète des informations de la localité'
    },
    'localidad.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'fr': 'Informations Principales'
    },
    'localidad.establecimiento': {
      'es': 'Establecimiento',
      'en': 'Establishment',
      'fr': 'Établissement'
    },
    'localidad.almacen': {
      'es': 'Almacén',
      'en': 'Warehouse',
      'fr': 'Entrepôt'
    },
    'localidad.cuentasContables': {
      'es': 'Cuentas Contables',
      'en': 'Accounting Accounts',
      'fr': 'Comptes Comptables'
    },
    'localidad.cuentasContablesSubtitle': {
      'es': 'Configuración de cuentas para inventario, costos, ventas y devoluciones.',
      'en': 'Account settings for inventory, costs, sales, and returns.',
      'fr': 'Configuration des comptes pour les stocks, coûts, ventes et retours.'
    },
    'localidad.cuentasInventarioCosto': {
      'es': 'Inventario y Costo',
      'en': 'Inventory and Cost',
      'fr': 'Stock et Coût'
    },
    'localidad.cuentasVentaDevolucion': {
      'es': 'Venta y Devolución',
      'en': 'Sales and Returns',
      'fr': 'Vente et Retour'
    },
    'localidad.cuentaInventario': {
      'es': 'Cuenta de Inventario',
      'en': 'Inventory Account',
      'fr': 'Compte de Stock'
    },
    'localidad.cuentaCosto': {
      'es': 'Cuenta de Costo',
      'en': 'Cost Account',
      'fr': 'Compte de Coût'
    },
    'localidad.cuentaVenta': {
      'es': 'Cuenta de Venta',
      'en': 'Sales Account',
      'fr': 'Compte de Vente'
    },
    'localidad.cuentaDevolucion': {
      'es': 'Cuenta de Devolución',
      'en': 'Returns Account',
      'fr': 'Compte de Retour'
    },
    'localidad.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'localidad.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
    },
    'localidad.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'localidad.municipio': {
      'es': 'Municipio',
      'en': 'Municipality',
      'fr': 'Municipalité'
    },
    'localidad.codigoPostal': {
      'es': 'Código Postal',
      'en': 'Postal Code',
      'fr': 'Code Postal'
    },
    'localidad.codigoPlaceholder': {
      'es': 'Ej: LOC001',
      'en': 'Ex: LOC001',
      'fr': 'Ex: LOC001'
    },
    'localidad.nombrePlaceholder': {
      'es': 'Ej: Centro Histórico',
      'en': 'Ex: Historic Center',
      'fr': 'Ex: Centre Historique'
    },

    'moneda.title': {
      'es': 'Monedas',
      'en': 'Currencies',
      'fr': 'Devises'
    },
    'moneda.subtitle': {
      'es': 'Gestión de monedas',
      'en': 'Currencies management',
      'fr': 'Gestion des devises'
    },
    'moneda.newTitle': {
      'es': 'Nueva Moneda',
      'en': 'New Currency',
      'fr': 'Nouvelle Devise'
    },
    'moneda.editTitle': {
      'es': 'Editar Moneda',
      'en': 'Edit Currency',
      'fr': 'Modifier Devise'
    },
    'moneda.detailTitle': {
      'es': 'Detalle de Moneda',
      'en': 'Currency Details',
      'fr': 'Détails de la Devise'
    },
    'moneda.detailSubtitle': {
      'es': 'Visualización completa de la información de la moneda',
      'en': 'Complete view of currency information',
      'fr': 'Vue complète des informations de la devise'
    },
    'moneda.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'fr': 'Informations Principales'
    },
    'moneda.infoTasaCambio': {
      'es': 'Información de Tasa de Cambio',
      'en': 'Exchange Rate Information',
      'fr': 'Informations de Taux de Change'
    },
    'moneda.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'moneda.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
    },
    'moneda.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'moneda.simbolo': {
      'es': 'Símbolo',
      'en': 'Symbol',
      'fr': 'Symbole'
    },
    'moneda.cambio': {
      'es': 'Tasa de Cambio',
      'en': 'Exchange Rate',
      'fr': 'Taux de Change'
    },
    'moneda.codigoPlaceholder': {
      'es': 'Ej: USD, EUR, MXN',
      'en': 'Ex: USD, EUR, MXN',
      'fr': 'Ex: USD, EUR, MXN'
    },
    'moneda.nombrePlaceholder': {
      'es': 'Ej: Dólar Estadounidense',
      'en': 'Ex: US Dollar',
      'fr': 'Ex: Dollar Américain'
    },
    'moneda.descripcionPlaceholder': {
      'es': 'Ej: Moneda oficial de Estados Unidos',
      'en': 'Ex: Official currency of United States',
      'fr': 'Ex: Monnaie officielle des États-Unis'
    },
    'moneda.simboloPlaceholder': {
      'es': 'Ej: $, €, £',
      'en': 'Ex: $, €, £',
      'fr': 'Ex: $, €, £'
    },
    'moneda.cambioPlaceholder': {
      'es': 'Ej: 1.00, 20.50',
      'en': 'Ex: 1.00, 20.50',
      'fr': 'Ex: 1.00, 20.50'
    },

    'pais.title': {
      'es': 'Países',
      'en': 'Countries',
      'fr': 'Pays'
    },
    'pais.subtitle': {
      'es': 'Gestión de países',
      'en': 'Countries management',
      'fr': 'Gestion des pays'
    },
    'pais.newTitle': {
      'es': 'Nuevo País',
      'en': 'New Country',
      'fr': 'Nouveau Pays'
    },
    'pais.editTitle': {
      'es': 'Editar País',
      'en': 'Edit Country',
      'fr': 'Modifier Pays'
    },
    'pais.detailTitle': {
      'es': 'Detalle de País',
      'en': 'Country Details',
      'fr': 'Détails du Pays'
    },
    'pais.detailSubtitle': {
      'es': 'Visualización completa de la información del país',
      'en': 'Complete view of country information',
      'fr': 'Vue complète des informations du pays'
    },
    'pais.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'fr': 'Informations Principales'
    },
    'pais.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'pais.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
    },
    'pais.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'pais.nacionalidad': {
      'es': 'Nacionalidad',
      'en': 'Nationality',
      'fr': 'Nationalité'
    },
    'pais.codigoPlaceholder': {
      'es': 'Ej: MX, US, ES',
      'en': 'Ex: MX, US, ES',
      'fr': 'Ex: MX, US, ES'
    },
    'pais.nombrePlaceholder': {
      'es': 'Ej: México',
      'en': 'Ex: Mexico',
      'fr': 'Ex: Mexique'
    },
    'pais.descripcionPlaceholder': {
      'es': 'Ej: País de América del Norte',
      'en': 'Ex: Country in North America',
      'fr': 'Ex: Pays en Amérique du Nord'
    },
    'pais.nacionalidadPlaceholder': {
      'es': 'Ej: Mexicana',
      'en': 'Ex: Mexican',
      'fr': 'Ex: Mexicaine'
    },

    'provincia.title': {
      'es': 'Provincias',
      'en': 'Provinces',
      'fr': 'Provinces'
    },
    'provincia.subtitle': {
      'es': 'Gestión de provincias',
      'en': 'Provinces management',
      'fr': 'Gestion des provinces'
    },
    'provincia.newTitle': {
      'es': 'Nueva Provincia',
      'en': 'New Province',
      'fr': 'Nouvelle Province'
    },
    'provincia.editTitle': {
      'es': 'Editar Provincia',
      'en': 'Edit Province',
      'fr': 'Modifier Province'
    },
    'provincia.detailTitle': {
      'es': 'Detalle de Provincia',
      'en': 'Province Details',
      'fr': 'Détails de la Province'
    },
    'provincia.detailSubtitle': {
      'es': 'Visualización completa de la información de la provincia',
      'en': 'Complete view of province information',
      'fr': 'Vue complète des informations de la province'
    },
    'provincia.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'fr': 'Informations Principales'
    },
    'provincia.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'provincia.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
    },
    'provincia.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'provincia.pais': {
      'es': 'País',
      'en': 'Country',
      'fr': 'Pays'
    },
    'provincia.codigoPlaceholder': {
      'es': 'Ej: PROV01',
      'en': 'Ex: PROV01',
      'fr': 'Ex: PROV01'
    },
    'provincia.nombrePlaceholder': {
      'es': 'Ej: Ciudad de México',
      'en': 'Ex: Mexico City',
      'fr': 'Ex: Mexico'
    },
    'provincia.descripcionPlaceholder': {
      'es': 'Ej: Capital del país',
      'en': 'Ex: Capital of the country',
      'fr': 'Ex: Capitale du pays'
    },

    'municipio.title': {
      'es': 'Municipios',
      'en': 'Municipalities',
      'fr': 'Municipalités'
    },
    'municipio.subtitle': {
      'es': 'Gestión de municipios',
      'en': 'Municipalities management',
      'fr': 'Gestion des municipalités'
    },
    'municipio.newTitle': {
      'es': 'Nuevo Municipio',
      'en': 'New Municipality',
      'fr': 'Nouvelle Municipalité'
    },
    'municipio.editTitle': {
      'es': 'Editar Municipio',
      'en': 'Edit Municipality',
      'fr': 'Modifier Municipalité'
    },
    'municipio.detailTitle': {
      'es': 'Detalle de Municipio',
      'en': 'Municipality Details',
      'fr': 'Détails de la Municipalité'
    },
    'municipio.detailSubtitle': {
      'es': 'Visualización completa de la información del municipio',
      'en': 'Complete view of municipality information',
      'fr': 'Vue complète des informations de la municipalité'
    },
    'municipio.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'fr': 'Informations Principales'
    },
    'municipio.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'municipio.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
    },
    'municipio.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'municipio.provincia': {
      'es': 'Provincia',
      'en': 'Province',
      'fr': 'Province'
    },
    'municipio.codigoPlaceholder': {
      'es': 'Ej: MUN001',
      'en': 'Ex: MUN001',
      'fr': 'Ex: MUN001'
    },
    'municipio.nombrePlaceholder': {
      'es': 'Ej: Benito Juárez',
      'en': 'Ex: Benito Juarez',
      'fr': 'Ex: Benito Juarez'
    },
    'municipio.descripcionPlaceholder': {
      'es': 'Ej: Municipio central de la provincia',
      'en': 'Ex: Central municipality of the province',
      'fr': 'Ex: Municipalité centrale de la province'
    },

    'codigoPostal.title': {
      'es': 'Códigos Postales',
      'en': 'Postal Codes',
      'fr': 'Codes Postaux'
    },
    'codigoPostal.subtitle': {
      'es': 'Gestión de códigos postales',
      'en': 'Postal codes management',
      'fr': 'Gestion des codes postaux'
    },
    'codigoPostal.newTitle': {
      'es': 'Nuevo Código Postal',
      'en': 'New Postal Code',
      'fr': 'Nouveau Code Postal'
    },
    'codigoPostal.editTitle': {
      'es': 'Editar Código Postal',
      'en': 'Edit Postal Code',
      'fr': 'Modifier Code Postal'
    },
    'codigoPostal.detailTitle': {
      'es': 'Detalle de Código Postal',
      'en': 'Postal Code Details',
      'fr': 'Détails du Code Postal'
    },
    'codigoPostal.detailSubtitle': {
      'es': 'Visualización completa de la información del código postal',
      'en': 'Complete view of postal code information',
      'fr': 'Vue complète des informations du code postal'
    },
    'codigoPostal.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'fr': 'Informations Principales'
    },
    'codigoPostal.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'codigoPostal.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'codigoPostal.pais': {
      'es': 'País',
      'en': 'Country',
      'fr': 'Pays'
    },
    'codigoPostal.provincia': {
      'es': 'Provincia',
      'en': 'Province',
      'fr': 'Province'
    },
    'codigoPostal.municipio': {
      'es': 'Municipio',
      'en': 'Municipality',
      'fr': 'Municipalité'
    },
    'codigoPostal.selectPaisFirst': {
      'es': 'Seleccione un país primero',
      'en': 'Select a country first',
      'fr': 'Sélectionnez d\'abord un pays'
    },
    'codigoPostal.selectProvinciaFirst': {
      'es': 'Seleccione una provincia primero',
      'en': 'Select a province first',
      'fr': 'Sélectionnez d\'abord une province'
    },
    'codigoPostal.paisDescripcion': {
      'es': 'País',
      'en': 'Country',
      'fr': 'Pays'
    },
    'codigoPostal.provinciaDescripcion': {
      'es': 'Provincia',
      'en': 'Province',
      'fr': 'Province'
    },
    'codigoPostal.municipioDescripcion': {
      'es': 'Municipio',
      'en': 'Municipality',
      'fr': 'Municipalité'
    },
    'codigoPostal.codigoPlaceholder': {
      'es': 'Ej: 03100',
      'en': 'Ex: 03100',
      'fr': 'Ex: 03100'
    },
    'codigoPostal.descripcionPlaceholder': {
      'es': 'Ej: Colonia Del Valle Centro',
      'en': 'Ex: Del Valle Centro Neighborhood',
      'fr': 'Ex: Quartier Del Valle Centro'
    },

    // ═══════════════════════════════════════════════════════════
    // 📋 CLASIFICADOR
    // ═══════════════════════════════════════════════════════════
    'linea.title': {
      'es': 'Líneas',
      'en': 'Lines',
      'fr': 'Lignes'
    },
    'linea.subtitle': {
      'es': 'Gestión de líneas',
      'en': 'Lines management',
      'fr': 'Gestion des lignes'
    },
    'linea.newTitle': {
      'es': 'Nueva Línea',
      'en': 'New Line',
      'fr': 'Nouvelle Ligne'
    },
    'linea.editTitle': {
      'es': 'Editar Línea',
      'en': 'Edit Line',
      'fr': 'Modifier Ligne'
    },
    'linea.detailTitle': {
      'es': 'Detalle de Línea',
      'en': 'Line Details',
      'fr': 'Détails de la Ligne'
    },
    'linea.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'linea.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
    },
    'linea.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'linea.descripcionPlaceholder': {
      'es': 'Ej: Alimentos, Bebidas',
      'en': 'Ex: Food, Beverages',
      'fr': 'Ex: Aliments, Boissons'
    },

    'subLinea.title': {
      'es': 'Sublíneas',
      'en': 'Sublines',
      'fr': 'Sous-lignes'
    },
    'subLinea.subtitle': {
      'es': 'Gestión de sublíneas',
      'en': 'Sublines management',
      'fr': 'Gestion des sous-lignes'
    },
    'subLinea.newTitle': {
      'es': 'Nueva Sublínea',
      'en': 'New Subline',
      'fr': 'Nouvelle Sous-ligne'
    },
    'subLinea.editTitle': {
      'es': 'Editar Sublínea',
      'en': 'Edit Subline',
      'fr': 'Modifier Sous-ligne'
    },
    'subLinea.detailTitle': {
      'es': 'Detalle de Sublínea',
      'en': 'Subline Details',
      'fr': 'Détails de la Sous-ligne'
    },
    'subLinea.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'subLinea.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
    },
    'subLinea.linea': {
      'es': 'Línea',
      'en': 'Line',
      'fr': 'Ligne'
    },
    'subLinea.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'sub-linea.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'sub-linea.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'sub-linea.lineaId': {
      'es': 'Línea',
      'en': 'Line',
      'fr': 'Ligne'
    },
    'sub-linea.editTitle': {
      'es': 'Editar Sublínea',
      'en': 'Edit Subline',
      'fr': 'Modifier Sous-ligne'
    },
    'sub-linea.newTitle': {
      'es': 'Nueva Sublínea',
      'en': 'New Subline',
      'fr': 'Nouvelle Sous-ligne'
    },
    'sub-linea.descripcionPlaceholder': {
      'es': 'Ej: Confecciones Exteriores',
      'en': 'Ex: Outdoor Clothing',
      'fr': 'Ex: Vêtements d\'Extérieur'
    },

    'unidadMedida.title': {
      'es': 'Unidades de Medida',
      'en': 'Units of Measure',
      'fr': 'Unités de Mesure'
    },
    'unidadMedida.subtitle': {
      'es': 'Gestión de unidades de medida',
      'en': 'Units of measure management',
      'fr': 'Gestion des unités de mesure'
    },
    'unidadMedida.newTitle': {
      'es': 'Nueva Unidad de Medida',
      'en': 'New Unit of Measure',
      'fr': 'Nouvelle Unité de Mesure'
    },
    'unidadMedida.editTitle': {
      'es': 'Editar Unidad de Medida',
      'en': 'Edit Unit of Measure',
      'fr': 'Modifier Unité de Mesure'
    },
    'unidadMedida.detailTitle': {
      'es': 'Detalle de Unidad de Medida',
      'en': 'Unit of Measure Details',
      'fr': 'Détails de l\'Unité de Mesure'
    },
    'unidadMedida.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'unidadMedida.abreviatura': {
      'es': 'Abreviatura',
      'en': 'Abbreviation',
      'fr': 'Abréviation'
    },
    'unidadMedida.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'unidadMedida.descripcionPlaceholder': {
      'es': 'Ej: Unidad, Kilogramo, Litro',
      'en': 'Ex: Unit, Kilogram, Liter',
      'fr': 'Ex: Unité, Kilogramme, Litre'
    },
    'unidadMedida.codigoPlaceholder': {
      'es': 'Ej: UNO',
      'en': 'Ex: UNE',
      'fr': 'Ex: UNÉ'
    },

    // ═══════════════════════════════════════════════════════════
    // 🔄 OPERACIONES
    // ═══════════════════════════════════════════════════════════
    'transaccion.title': {
      'es': 'Transacciones',
      'en': 'Transactions',
      'fr': 'Transactions'
    },
    'transaccion.subtitle': {
      'es': 'Gestión de transacciones',
      'en': 'Transactions management',
      'fr': 'Gestion des transactions'
    },
    'transaccion.newTitle': {
      'es': 'Nueva Transacción',
      'en': 'New Transaction',
      'fr': 'Nouvelle Transaction'
    },
    'transaccion.editTitle': {
      'es': 'Editar Transacción',
      'en': 'Edit Transaction',
      'fr': 'Modifier Transaction'
    },
    'transaccion.detailTitle': {
      'es': 'Detalle de Transacción',
      'en': 'Transaction Details',
      'fr': 'Détails de la Transaction'
    },
    'transaccion.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'transaccion.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
    },
    'transaccion.tipo': {
      'es': 'Tipo',
      'en': 'Type',
      'fr': 'Type'
    },
    'transaccion.ingreso': {
      'es': 'Ingreso',
      'en': 'Income',
      'fr': 'Revenu'
    },
    'transaccion.egreso': {
      'es': 'Egreso',
      'en': 'Expense',
      'fr': 'Dépense'
    },

    'conceptoAjuste.title': {
      'es': 'Conceptos de Ajuste',
      'en': 'Adjustment Concepts',
      'fr': 'Concepts d\'Ajustement'
    },
    'conceptoAjuste.subtitle': {
      'es': 'Gestión de conceptos de ajuste',
      'en': 'Adjustment concepts management',
      'fr': 'Gestion des concepts d\'ajustement'
    },
    'conceptoAjuste.newTitle': {
      'es': 'Nuevo Concepto de Ajuste',
      'en': 'New Adjustment Concept',
      'fr': 'Nouveau Concept d\'Ajustement'
    },
    'conceptoAjuste.editTitle': {
      'es': 'Editar Concepto de Ajuste',
      'en': 'Edit Adjustment Concept',
      'fr': 'Modifier Concept d\'Ajustement'
    },
    'conceptoAjuste.detailTitle': {
      'es': 'Detalle de Concepto de Ajuste',
      'en': 'Adjustment Concept Details',
      'fr': 'Détails du Concept d\'Ajustement'
    },
    'conceptoAjuste.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'conceptoAjuste.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
    },
    'conceptoAjuste.tipo': {
      'es': 'Tipo de Ajuste',
      'en': 'Adjustment Type',
      'fr': 'Type d\'Ajustement'
    },
    'conceptoAjuste.aumento': {
      'es': 'Aumento',
      'en': 'Increase',
      'fr': 'Augmentation'
    },
    'conceptoAjuste.disminucion': {
      'es': 'Disminución',
      'en': 'Decrease',
      'fr': 'Diminution'
    },

    // ═══════════════════════════════════════════════════════════
    // 📦 PRODUCTO
    // ═══════════════════════════════════════════════════════════
    'producto.title': {
      'es': 'Productos',
      'en': 'Products',
      'fr': 'Produits'
    },
    'producto.subtitle': {
      'es': 'Gestión de productos',
      'en': 'Products management',
      'fr': 'Gestion des produits'
    },
    'producto.newTitle': {
      'es': 'Nuevo Producto',
      'en': 'New Product',
      'fr': 'Nouveau Produit'
    },
    'producto.editTitle': {
      'es': 'Editar Producto',
      'en': 'Edit Product',
      'fr': 'Modifier Produit'
    },
    'producto.detailTitle': {
      'es': 'Detalle de Producto',
      'en': 'Product Details',
      'fr': 'Détails du Produit'
    },
    'producto.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'producto.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
    },
    'producto.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'producto.precio': {
      'es': 'Precio',
      'en': 'Price',
      'fr': 'Prix'
    },
    'producto.costo': {
      'es': 'Costo',
      'en': 'Cost',
      'fr': 'Coût'
    },
    'producto.stock': {
      'es': 'Stock',
      'en': 'Stock',
      'fr': 'Stock'
    },
    'producto.unidadMedida': {
      'es': 'Unidad de Medida',
      'en': 'Unit of Measure',
      'fr': 'Unité de Mesure'
    },
    'producto.linea': {
      'es': 'Línea',
      'en': 'Line',
      'fr': 'Ligne'
    },
    'producto.sublinea': {
      'es': 'Sublínea',
      'en': 'Subline',
      'fr': 'Sous-ligne'
    },

    // ═══════════════════════════════════════════════════════════
    // 📊 DASHBOARD
    // ═══════════════════════════════════════════════════════════
    'dashboard.title': {
      'es': 'Panel de Control',
      'en': 'Dashboard',
      'fr': 'Tableau de Bord'
    },
    'dashboard.subtitle': {
      'es': 'Bienvenido a GoldBusiness ERP',
      'en': 'Welcome to GoldBusiness ERP',
      'fr': 'Bienvenue à GoldBusiness ERP'
    },
    'dashboard.viewReports': {
      'es': 'Ver Reportes',
      'en': 'View Reports',
      'fr': 'Voir Rapports'
    },
    'dashboard.totalAccounts': {
      'es': 'Total de Cuentas',
      'en': 'Total Accounts',
      'fr': 'Total des Comptes'
    },
    'dashboard.activeUsers': {
      'es': 'Usuarios Activos',
      'en': 'Active Users',
      'fr': 'Utilisateurs Actifs'
    },
    'dashboard.accountGroups': {
      'es': 'Grupos de Cuenta',
      'en': 'Account Groups',
      'fr': 'Groupes de Comptes'
    },
    'dashboard.pendingTasks': {
      'es': 'Tareas Pendientes',
      'en': 'Pending Tasks',
      'fr': 'Tâches en Attente'
    },
    'dashboard.recentActivities': {
      'es': 'Actividades Recientes',
      'en': 'Recent Activities',
      'fr': 'Activités Récentes'
    },
    'dashboard.quickAccess': {
      'es': 'Acceso Rápido',
      'en': 'Quick Access',
      'fr': 'Accès Rapide'
    },
    'dashboard.monthlyTrends': {
      'es': 'Tendencias Mensuales',
      'en': 'Monthly Trends',
      'fr': 'Tendances Mensuelles'
    },
    'dashboard.accountsDistribution': {
      'es': 'Distribución de Cuentas',
      'en': 'Accounts Distribution',
      'fr': 'Distribution des Comptes'
    },
    'dashboard.chartComingSoon': {
      'es': 'Gráfico próximamente...',
      'en': 'Chart coming soon...',
      'fr': 'Graphique à venir...'
    },
    'dashboard.timeAgo': {
      'es': 'hace {0}',
      'en': '{0} ago',
      'fr': 'il y a {0}'
    },
    'dashboard.activity.accountCreated': {
      'es': 'Cuenta creada:',
      'en': 'Account created:',
      'fr': 'Compte créé:'
    },
    'dashboard.activity.accountModified': {
      'es': 'Cuenta modificada:',
      'en': 'Account modified:',
      'fr': 'Compte modifié:'
    },
    'dashboard.activity.configUpdated': {
      'es': 'Configuración actualizada:',
      'en': 'Configuration updated:',
      'fr': 'Configuration mise à jour:'
    },
    'dashboard.quickLinks.newAccount': {
      'es': 'Nueva Cuenta',
      'en': 'New Account',
      'fr': 'Nouveau Compte'
    },
    'dashboard.quickLinks.viewAccounts': {
      'es': 'Ver Cuentas',
      'en': 'View Accounts',
      'fr': 'Voir Comptes'
    },
    'dashboard.quickLinks.configuration': {
      'es': 'Configuración',
      'en': 'Configuration',
      'fr': 'Configuration'
    },
    'dashboard.quickLinks.reports': {
      'es': 'Reportes',
      'en': 'Reports',
      'fr': 'Rapports'
    },
    'dashboard.time.minutes': {
      'es': 'hace {0} minutos',
      'en': '{0} minutes ago',
      'fr': 'il y a {0} minutes'
    },
    'dashboard.time.oneHour': {
      'es': 'hace 1 hora',
      'en': '1 hour ago',
      'fr': 'il y a 1 heure'
    },
    'dashboard.time.hours': {
      'es': 'hace {0} horas',
      'en': '{0} hours ago',
      'fr': 'il y a {0} heures'
    },
    'dashboard.time.oneDay': {
      'es': 'hace 1 día',
      'en': '1 day ago',
      'fr': 'il y a 1 jour'
    },
    'dashboard.time.days': {
      'es': 'hace {0} días',
      'en': '{0} days ago',
      'fr': 'il y a {0} jours'
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
    'common.noData': {
      'es': 'No hay datos disponibles',
      'en': 'No data available',
      'fr': 'Aucune donnée disponible'
    },
    'common.ifNotChecked': {
      'es': '(si no está marcado)',
      'en': '(if not checked)',
      'fr': '(si non coché)'
    },
    'common.information': {
      'es': 'Información',
      'en': 'Information',
      'fr': 'Information'
    },
    'common.createdAt': {
      'es': 'Fecha de Creación',
      'en': 'Created At',
      'fr': 'Date de Création'
    },
    'common.updatedAt': {
      'es': 'Última Actualización',
      'en': 'Updated At',
      'fr': 'Dernière Mise à Jour'
    },
    'common.id': {
      'es': 'ID',
      'en': 'ID',
      'fr': 'ID'
    },
    'common.status': {
      'es': 'Estado',
      'en': 'Status',
      'fr': 'Statut'
    },
    'common.active': {
      'es': 'Activo',
      'en': 'Active',
      'fr': 'Actif'
    },
    'common.yes': {
      'es': 'Sí',
      'en': 'Yes',
      'fr': 'Oui'
    },
    'common.no': {
      'es': 'No',
      'en': 'No',
      'fr': 'Non'
    },
    'common.inactive': {
      'es': 'Inactivo',
      'en': 'Inactive',
      'fr': 'Inactif'
    },
    'common.show': {
      'es': 'Mostrar',
      'en': 'Show',
      'fr': 'Afficher'
    },
    'common.showing': {
      'es': 'Mostrando',
      'en': 'Showing',
      'fr': 'Affichage'
    },
    'common.of': {
      'es': 'de',
      'en': 'of',
      'fr': 'sur'
    },
    'common.previous': {
      'es': 'Anterior',
      'en': 'Previous',
      'fr': 'Précédent'
    },
    'common.next': {
      'es': 'Siguiente',
      'en': 'Next',
      'fr': 'Suivant'
    },
    'common.noResults': {
      'es': 'No se encontraron resultados',
      'en': 'No results found',
      'fr': 'Aucun résultat trouvé'
    },
    'common.retry': {
      'es': 'Reintentar',
      'en': 'Retry',
      'fr': 'Réessayer'
    },
    'common.saving': {
      'es': 'Guardando...',
      'en': 'Saving...',
      'fr': 'Enregistrement...'
    },
    'common.deleting': {
      'es': 'Eliminando...',
      'en': 'Deleting...',
      'fr': 'Suppression...'
    },
    'common.processing': {
      'es': 'Procesando...',
      'en': 'Processing...',
      'fr': 'Traitement...'
    },
    'common.select': {
      'es': 'Seleccione',
      'en': 'Select',
      'fr': 'Sélectionner'
    },
    'common.loading': {
      'es': 'Cargando',
      'en': 'Loading',
      'fr': 'Chargement'
    },
    'common.emailPlaceholder': {
      'es': 'Ej: ejemplo@empresa.com',
      'en': 'Ex: example@company.com',
      'fr': 'Ex: exemple@entreprise.com'
    },
    'common.telefonoPlaceholder': {
      'es': 'Ej: +52 55 1234 5678',
      'en': 'Ex: +52 55 1234 5678',
      'fr': 'Ex: +52 55 1234 5678'
    },
    'common.webPlaceholder': {
      'es': 'Ej: www.empresa.com',
      'en': 'Ex: www.company.com',
      'fr': 'Ex: www.entreprise.com'
    },
    'common.fillRequired': {
      'es': 'Rellena los campos obligatorios (*)',
      'en': 'Please complete the required fields (*)',
      'fr': 'Veuillez remplir les champs obligatoires (*)'
    },
    'common.fillRequiredHint': {
      'es': 'Rellena los campos obligatorios (*) y revisa los textos de ayuda antes de guardar.',
      'en': 'Complete the required fields (*) and review help texts before saving.',
      'fr': 'Remplissez les champs obligatoires (*) et vérifiez les textes d’aide avant d’enregistrer.'
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
    'grupoCuenta.detailSubtitle': {
      'es': 'Visualización completa de la información del grupo de cuenta',
      'en': 'Complete view of account group information',
      'fr': 'Vue complète des informations du groupe de comptes'
    },
    'grupoCuenta.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'fr': 'Informations Principales'
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
    'grupoCuenta.codigoPlaceholder': {
      'es': 'Ej: 01',
      'en': 'Ex: 01',
      'fr': 'Ex: 01'
    },
    'grupoCuenta.descripcionPlaceholder': {
      'es': 'Ej: Activo, Pasivo, Patrimonio',
      'en': 'Ex: Asset, Liability, Equity',
      'fr': 'Ex: Actif, Passif, Capitaux Propres'
    },
    'grupoCuenta.infoBasica': {
      'es': 'Información General',
      'en': 'General Information',
      'fr': 'Informations Générales'
    },

    // ═══════════════════════════════════════════════════════════
    // 📂 SUBGRUPO CUENTA
    // ═══════════════════════════════════════════════════════════
    'subGrupoCuenta.title': {
      'es': 'SubGrupos de Cuenta',
      'en': 'Account Subgroups',
      'fr': 'Sous-groupes de Comptes'
    },
    'subGrupoCuenta.subtitle': {
      'es': 'Segundo nivel del plan de cuentas',
      'en': 'Second level of chart of accounts',
      'fr': 'Deuxième niveau du plan comptable'
    },
    'subGrupoCuenta.newTitle': {
      'es': 'Nuevo SubGrupo de Cuenta',
      'en': 'New Account Subgroup',
      'fr': 'Nouveau Sous-groupe de Comptes'
    },
    'subGrupoCuenta.editTitle': {
      'es': 'Editar SubGrupo de Cuenta',
      'en': 'Edit Account Subgroup',
      'fr': 'Modifier Sous-groupe de Comptes'
    },
    'subGrupoCuenta.detailTitle': {
      'es': 'Detalle del SubGrupo de Cuenta',
      'en': 'Account Subgroup Details',
      'fr': 'Détails du Sous-groupe de Comptes'
    },
    'subGrupoCuenta.detailSubtitle': {
      'es': 'Visualización completa de la información del subgrupo de cuenta',
      'en': 'Complete view of account subgroup information',
      'fr': 'Vue complète des informations du sous-groupe de comptes'
    },
    'subGrupoCuenta.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'fr': 'Informations Principales'
    },
    'subGrupoCuenta.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'subGrupoCuenta.grupoCuenta': {
      'es': 'Grupo de Cuenta',
      'en': 'Account Group',
      'fr': 'Groupe de Comptes'
    },
    'subGrupoCuenta.tipo': {
      'es': 'Tipo',
      'en': 'Type',
      'fr': 'Type'
    },
    'subGrupoCuenta.deudora': {
      'es': 'Deudora',
      'en': 'Debit',
      'fr': 'Débiteur'
    },
    'subGrupoCuenta.acreedora': {
      'es': 'Acreedora',
      'en': 'Credit',
      'fr': 'Créditeur'
    },
    'subGrupoCuenta.estado': {
      'es': 'Estado',
      'en': 'Status',
      'fr': 'Statut'
    },
    'subGrupoCuenta.codigoPlaceholder': {
      'es': 'Ej: 01001',
      'en': 'Ex: 01001',
      'fr': 'Ex: 01001'
    },
    'subGrupoCuenta.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'subGrupoCuenta.descripcionPlaceholder': {
      'es': 'Ej: Activo Circulante, Efectivo y Equivalentes',
      'en': 'Ex: Current Assets, Cash and Equivalents',
      'fr': 'Ex: Actif Circulant, Trésorerie et Équivalents'
    },
    'subGrupoCuenta.codigoUsuario': {
      'es': 'Código (3 dígitos)',
      'en': 'Code (3 digits)',
      'fr': 'Code (3 chiffres)'
    },
    'subGrupoCuenta.codigoUsuarioPlaceholder': {
      'es': 'Ej: 001',
      'en': 'Ex: 001',
      'fr': 'Ex: 001'
    },
    'subGrupoCuenta.codigoUsuarioHelp': {
      'es': 'Ingrese los últimos 3 dígitos del código',
      'en': 'Enter the last 3 digits of the code',
      'fr': 'Entrez les 3 derniers chiffres du code'
    },
    'subGrupoCuenta.codigoCompleto': {
      'es': 'Código Completo',
      'en': 'Full Code',
      'fr': 'Code Complet'
    },
    'subGrupoCuenta.codigoCompletoPlaceholder': {
      'es': 'Se generará automáticamente',
      'en': 'Will be generated automatically',
      'fr': 'Sera généré automatiquement'
    },
    'subGrupoCuenta.codigoCompletoHelp': {
      'es': 'Código del grupo + sus 3 dígitos = Código completo',
      'en': 'Group code + your 3 digits = Full code',
      'fr': 'Code du groupe + vos 3 chiffres = Code complet'
    },
    'subGrupoCuenta.prefijoSeleccionado': {
      'es': 'Prefijo seleccionado',
      'en': 'Selected prefix',
      'fr': 'Préfixe sélectionné'
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
    'cuenta.detailSubtitle': {
      'es': 'Visualización completa de la información de la cuenta',
      'en': 'Complete view of account information',
      'fr': 'Vue complète des informations du compte'
    },
    'cuenta.mainInfo': {
      'es': 'Información Principal',
      'en': 'Main Information',
      'fr': 'Informations Principales'
    },
    'cuenta.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'cuenta.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'cuenta.subGrupoCuenta': {
      'es': 'SubGrupo de Cuenta',
      'en': 'Account Subgroup',
      'fr': 'Sous-groupe de Comptes'
    },
    'cuenta.grupoCuenta': {
      'es': 'Grupo de Cuenta',
      'en': 'Account Group',
      'fr': 'Groupe de Comptes'
    },
    'cuenta.selectGrupoToSeeSubgrupos': {
      'es': 'Seleccione un grupo para ver subgrupos.',
      'en': 'Select a group to see subgroups.',
      'fr': 'Sélectionnez un groupe pour voir les sous-groupes.'
    },

      // Ayudas para selects padre y placeholders
      'subLinea.noLineasDisponibles': {
        'es': 'No hay líneas disponibles.',
        'en': 'No lines available.',
        'fr': 'Aucune ligne disponible.'
      },
      'subGrupoCuenta.noGruposDisponibles': {
        'es': 'No hay grupos disponibles.',
        'en': 'No groups available.',
        'fr': 'Aucun groupe disponible.'
      },
      'conceptoAjuste.noCuentasDisponibles': {
        'es': 'No hay cuentas disponibles.',
        'en': 'No accounts available.',
        'fr': 'Aucun compte disponible.'
      },
      'proveedores.placeholderNif': {
        'es': 'Ej: 12345678A',
        'en': 'Ex: 12345678A',
        'fr': 'Ex: 12345678A'
      },
      'proveedores.placeholderIva': {
        'es': 'Ej: 21.00',
        'en': 'Ex: 21.00',
        'fr': 'Ex: 21.00'
      },
      'clientes.placeholderIva': {
        'es': 'Ej: 21.00',
        'en': 'Ex: 21.00',
        'fr': 'Ex: 21.00'
      },

      // Ayudas para selects dependientes y país
      'cliente.selectPaisToSeeProvincias': {
        'es': 'Seleccione un país para ver provincias.',
        'en': 'Select a country to see provinces.',
        'fr': 'Sélectionnez un pays pour voir les provinces.'
      },
      'cliente.selectProvinciaToSeeMunicipios': {
        'es': 'Seleccione una provincia para ver municipios.',
        'en': 'Select a province to see municipalities.',
        'fr': 'Sélectionnez une province pour voir les municipalités.'
      },
      'cliente.selectMunicipioToSeeCodigosPostales': {
        'es': 'Seleccione un municipio para ver códigos postales.',
        'en': 'Select a municipality to see postal codes.',
        'fr': 'Sélectionnez une municipalité pour voir les codes postaux.'
      },
      'cliente.noPaisesDisponibles': {
        'es': 'No hay países disponibles.',
        'en': 'No countries available.',
        'fr': 'Aucun pays disponible.'
      },
      'proveedor.selectPaisToSeeProvincias': {
        'es': 'Seleccione un país para ver provincias.',
        'en': 'Select a country to see provinces.',
        'fr': 'Sélectionnez un pays pour voir les provinces.'
      },
      'proveedor.selectProvinciaToSeeMunicipios': {
        'es': 'Seleccione una provincia para ver municipios.',
        'en': 'Select a province to see municipalities.',
        'fr': 'Sélectionnez une province pour voir les municipalités.'
      },
      'proveedor.selectMunicipioToSeeCodigosPostales': {
        'es': 'Seleccione un municipio para ver códigos postales.',
        'en': 'Select a municipality to see postal codes.',
        'fr': 'Sélectionnez une municipalité pour voir les codes postaux.'
      },
      'proveedor.selectCodigoPostalHelp': {
        'es': 'Seleccione un código postal disponible.',
        'en': 'Select an available postal code.',
        'fr': 'Sélectionnez un code postal disponible.'
      },
      'proveedor.noPaisesDisponibles': {
        'es': 'No hay países disponibles.',
        'en': 'No countries available.',
        'fr': 'Aucun pays disponible.'
      },
      'establecimiento.selectPaisToSeeProvincias': {
        'es': 'Seleccione un país para ver provincias.',
        'en': 'Select a country to see provinces.',
        'fr': 'Sélectionnez un pays pour voir les provinces.'
      },
      'establecimiento.selectProvinciaToSeeMunicipios': {
        'es': 'Seleccione una provincia para ver municipios.',
        'en': 'Select a province to see municipalities.',
        'fr': 'Sélectionnez une province pour voir les municipalités.'
      },
      'establecimiento.selectMunicipioToSeeCodigosPostales': {
        'es': 'Seleccione un municipio para ver códigos postales.',
        'en': 'Select a municipality to see postal codes.',
        'fr': 'Sélectionnez une municipalité pour voir les codes postaux.'
      },
      'establecimiento.selectCodigoPostalHelp': {
        'es': 'Seleccione un código postal disponible.',
        'en': 'Select an available postal code.',
        'fr': 'Sélectionnez un code postal disponible.'
      },
      'establecimiento.noPaisesDisponibles': {
        'es': 'No hay países disponibles.',
        'en': 'No countries available.',
        'fr': 'Aucun pays disponible.'
      },
      'systemConfig.selectPaisToSeeProvincias': {
        'es': 'Seleccione un país para ver provincias.',
        'en': 'Select a country to see provinces.',
        'fr': 'Sélectionnez un pays pour voir les provinces.'
      },
      'systemConfig.selectProvinciaToSeeMunicipios': {
        'es': 'Seleccione una provincia para ver municipios.',
        'en': 'Select a province to see municipalities.',
        'fr': 'Sélectionnez une province pour voir les municipalités.'
      },
      'systemConfig.selectMunicipioToSeeCodigosPostales': {
        'es': 'Seleccione un municipio para ver códigos postales.',
        'en': 'Select a municipality to see postal codes.',
        'fr': 'Sélectionnez une municipalité pour voir les codes postaux.'
      },
      'systemConfig.noPaisesDisponibles': {
        'es': 'No hay países disponibles.',
        'en': 'No countries available.',
        'fr': 'Aucun pays disponible.'
      },
      'provincia.selectPaisToSeeProvincias': {
        'es': 'Seleccione un país para ver provincias.',
        'en': 'Select a country to see provinces.',
        'fr': 'Sélectionnez un pays pour voir les provinces.'
      },
      'provincia.noPaisesDisponibles': {
        'es': 'No hay países disponibles.',
        'en': 'No countries available.',
        'fr': 'Aucun pays disponible.'
      },
      'municipio.selectProvinciaToSeeMunicipios': {
        'es': 'Seleccione una provincia para ver municipios.',
        'en': 'Select a province to see municipalities.',
        'fr': 'Sélectionnez une province pour voir les municipalités.'
      },
      'codigoPostal.selectPaisToSeeProvincias': {
        'es': 'Seleccione un país para ver provincias.',
        'en': 'Select a country to see provinces.',
        'fr': 'Sélectionnez un pays pour voir les provinces.'
      },
      'codigoPostal.selectProvinciaToSeeMunicipios': {
        'es': 'Seleccione una provincia para ver municipios.',
        'en': 'Select a province to see municipalities.',
        'fr': 'Sélectionnez une province pour voir les municipalités.'
      },
      'codigoPostal.selectMunicipioToSeeCodigosPostales': {
        'es': 'Seleccione un municipio para ver códigos postales.',
        'en': 'Select a municipality to see postal codes.',
        'fr': 'Sélectionnez une municipalité pour voir les codes postaux.'
      },
      'codigoPostal.noPaisesDisponibles': {
        'es': 'No hay países disponibles.',
        'en': 'No countries available.',
        'fr': 'Aucun pays disponible.'
      },
    'cuenta.descripcionPlaceholder': {
      'es': 'Ej: Efectivo en Caja, Bancos Moneda Nacional',
      'en': 'Ex: Cash on Hand, National Currency Banks',
      'fr': 'Ex: Espèces en Caisse, Banques en Monnaie Nationale'
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
    'systemConfig.detailSubtitle': {
      'es': 'Visualización completa de la configuración del sistema',
      'en': 'Complete view of system configuration',
      'fr': 'Vue complète de la configuration du système'
    },
    'systemConfig.infoSistema': {
      'es': 'Información del Sistema',
      'en': 'System Information',
      'fr': 'Informations du Système'
    },
    'systemConfig.infoNegocio': {
      'es': 'Información del Negocio',
      'en': 'Business Information',
      'fr': 'Informations de l\'Entreprise'
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
    'systemConfig.pais': {
      'es': 'País',
      'en': 'Country',
      'fr': 'Pays'
    },
    'systemConfig.codigoPostal': {
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
    'systemConfig.estado': {
      'es': 'Estado',
      'en': 'Status',
      'fr': 'Statut'
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
    // 🖼️ LOGO
    // ═══════════════════════════════════════════════════════════
    // ═══════════════════════════════════════════════════════════
    // 🏢 ESTABLECIMIENTO
    // ═══════════════════════════════════════════════════════════
    'establecimiento.negocioHelp': {
      'es': 'Seleccione el negocio al que pertenece el establecimiento.',
      'en': 'Select the business this establishment belongs to.',
      'fr': 'Sélectionnez l’entreprise à laquelle appartient l’établissement.'
    },
    'establecimiento.codigoHelp': {
      'es': 'Ingrese el código único del establecimiento. Ejemplo: EST001',
      'en': 'Enter the unique code for the establishment. Example: EST001',
      'fr': 'Entrez le code unique de l’établissement. Exemple : EST001'
    },
    'establecimiento.descripcionHelp': {
      'es': 'Ingrese una breve descripción del establecimiento.',
      'en': 'Enter a brief description of the establishment.',
      'fr': 'Entrez une brève description de l’établissement.'
    },
    'establecimiento.direccionHelp': {
      'es': 'Ingrese la dirección completa del establecimiento.',
      'en': 'Enter the full address of the establishment.',
      'fr': 'Entrez l’adresse complète de l’établissement.'
    },
    'establecimiento.telefonoHelp': {
      'es': 'Ingrese el número de teléfono de contacto.',
      'en': 'Enter the contact phone number.',
      'fr': 'Entrez le numéro de téléphone de contact.'
    },
    'establecimiento.paisHelp': {
      'es': 'Seleccione un país para ver provincias.',
      'en': 'Select a country to see provinces.',
      'fr': 'Sélectionnez un pays pour voir les provinces.'
    },
    'establecimiento.provinciaHelp': {
      'es': 'Seleccione una provincia para ver municipios.',
      'en': 'Select a province to see municipalities.',
      'fr': 'Sélectionnez une province pour voir les municipalités.'
    },
    'establecimiento.municipioHelp': {
      'es': 'Seleccione un municipio para ver códigos postales.',
      'en': 'Select a municipality to see postal codes.',
      'fr': 'Sélectionnez une municipalité pour voir les codes postaux.'
    },
    'establecimiento.codigoPostalHelp': {
      'es': 'Seleccione un código postal disponible.',
      'en': 'Select an available postal code.',
      'fr': 'Sélectionnez un code postal disponible.'
    },
    'establecimiento.placeholderCodigo': {
      'es': 'Ej: EST001',
      'en': 'Ex: EST001',
      'fr': 'Ex: EST001'
    },
    'establecimiento.placeholderDescripcion': {
      'es': 'Ej: Sucursal Central',
      'en': 'Ex: Main Branch',
      'fr': 'Ex: Succursale Principale'
    },
    'establecimiento.placeholderDireccion': {
      'es': 'Ej: Calle 123, Ciudad',
      'en': 'Ex: 123 Street, City',
      'fr': 'Ex: 123 Rue, Ville'
    },
    'establecimiento.placeholderTelefono': {
      'es': 'Ej: +34 912 345 678',
      'en': 'Ex: +34 912 345 678',
      'fr': 'Ex: +33 1 23 45 67 89'
    },

    // ═══════════════════════════════════════════════════════════
    // 🏢 LOCALIDAD
    // ═══════════════════════════════════════════════════════════
    'localidad.establecimientoHelp': {
      'es': 'Seleccione un establecimiento para ver localidades.',
      'en': 'Select an establishment to see localities.',
      'fr': 'Sélectionnez un établissement pour voir les localités.'
    },
    'localidad.descripcionPlaceholder': {
      'es': 'Ej: Zona Industrial',
      'en': 'Ex: Industrial Area',
      'fr': 'Ex: Zone Industrielle'
    },

    // ═══════════════════════════════════════════════════════════
    // 👤 USUARIO
    // ═══════════════════════════════════════════════════════════
    'usuario.placeholderUserName': {
      'es': 'Ej: jdoe',
      'en': 'Ex: jdoe',
      'fr': 'Ex: jdoe'
    },
    'usuario.placeholderFullName': {
      'es': 'Ej: Juan Pérez',
      'en': 'Ex: John Smith',
      'fr': 'Ex: Jean Dupont'
    },
    'usuario.placeholderEmail': {
      'es': 'Ej: usuario@empresa.com',
      'en': 'Ex: user@company.com',
      'fr': 'Ex: utilisateur@entreprise.com'
    },
    'usuario.authProviderHelp': {
      'es': 'Seleccione el método de autenticación.',
      'en': 'Select the authentication method.',
      'fr': 'Sélectionnez la méthode d’authentification.'
    },
    'usuario.isActiveHelp': {
      'es': 'Indica si el usuario está activo o inactivo.',
      'en': 'Indicates if the user is active or inactive.',
      'fr': 'Indique si l’utilisateur est actif ou inactif.'
    },

    // ═══════════════════════════════════════════════════════════
    // ⚙️ SYSTEM CONFIGURATION
    // ═══════════════════════════════════════════════════════════
    'systemConfig.placeholderCodigoSistema': {
      'es': 'Ej: GOL-ERP-001',
      'en': 'Ex: GOL-ERP-001',
      'fr': 'Ex: GOL-ERP-001'
    },
    'systemConfig.placeholderLicencia': {
      'es': 'Código de licencia',
      'en': 'License code',
      'fr': 'Code de licence'
    },
    'systemConfig.placeholderNombreNegocio': {
      'es': 'Nombre completo del negocio',
      'en': 'Full business name',
      'fr': 'Nom complet de l’entreprise'
    },
    'systemConfig.placeholderDireccion': {
      'es': 'Dirección completa del negocio',
      'en': 'Full business address',
      'fr': 'Adresse complète de l’entreprise'
    },
    'systemConfig.logoUpload': {
      'es': 'Subir Logo',
      'en': 'Upload Logo',
      'fr': 'Télécharger Logo'
    },
    'systemConfig.logoCurrentFile': {
      'es': 'Archivo actual',
      'en': 'Current file',
      'fr': 'Fichier actuel'
    },
    'systemConfig.logoSelectFile': {
      'es': 'Seleccionar archivo de logo',
      'en': 'Select logo file',
      'fr': 'Sélectionner fichier logo'
    },
    'systemConfig.logoChooseFile': {
      'es': 'Elegir Archivo',
      'en': 'Choose File',
      'fr': 'Choisir Fichier'
    },
    'systemConfig.logoRemove': {
      'es': 'Quitar',
      'en': 'Remove',
      'fr': 'Retirer'
    },
    'systemConfig.logoFormatHint': {
      'es': 'PNG, JPG, GIF o WEBP (máx. 2MB)',
      'en': 'PNG, JPG, GIF or WEBP (max 2MB)',
      'fr': 'PNG, JPG, GIF ou WEBP (max 2Mo)'
    },
    'systemConfig.logoInvalidFormat': {
      'es': 'Formato no válido. Use PNG, JPG, GIF o WEBP.',
      'en': 'Invalid format. Use PNG, JPG, GIF or WEBP.',
      'fr': 'Format invalide. Utilisez PNG, JPG, GIF ou WEBP.'
    },
    'systemConfig.logoSizeExceeded': {
      'es': 'El archivo no puede superar 2MB.',
      'en': 'File cannot exceed 2MB.',
      'fr': 'Le fichier ne peut pas dépasser 2Mo.'
    },
    'systemConfig.logoUploadError': {
      'es': 'Error al subir el logo',
      'en': 'Error uploading logo',
      'fr': 'Erreur lors du téléchargement du logo'
    },
    'systemConfig.noLogo': {
      'es': 'Sin logo configurado',
      'en': 'No logo configured',
      'fr': 'Aucun logo configuré'
    },
    'systemConfig.logoPreview': {
      'es': 'Vista previa del logo',
      'en': 'Logo preview',
      'fr': 'Aperçu du logo'
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
    'validation.codigo5Digitos': {
      'es': 'El código debe ser un número de 5 dígitos',
      'en': 'Code must be a 5-digit number',
      'fr': 'Le code doit être un nombre à 5 chiffres'
    },
    'validation.codigo3Digitos': {
      'es': 'El código debe ser un número de 3 dígitos',
      'en': 'Code must be a 3-digit number',
      'fr': 'Le code doit être un nombre à 3 chiffres'
    },
    'validation.codigo2Digitos': {
      'es': 'El código debe ser un número de 2 dígitos',
      'en': 'Code must be a 2-digit number',
      'fr': 'Le code doit être un nombre à 2 chiffres'
    },
    'validation.numeric': {
      'es': 'El código debe ser numérico',
      'en': 'Code must be numeric',
      'fr': 'Le code doit être numérique'
    },
    'validation.alphanumeric': {
      'es': 'El código debe ser alfanumérico',
      'en': 'Code must be alphanumeric',
      'fr': 'Le code doit être alphanumérique'
    },
    'validation.length': {
      'es': 'Longitud inválida',
      'en': 'Invalid length',
      'fr': 'Longueur invalide'
    },
    'validation.invalid': {
      'es': 'Valor inválido',
      'en': 'Invalid value',
      'fr': 'Valeur invalide'
    },
    'validation.ivaRequired': {
      'es': 'El IVA es obligatorio',
      'en': 'VAT is required',
      'fr': 'La TVA est obligatoire'
    },
    'validation.ivaInteger': {
      'es': 'El IVA debe ser un número entero',
      'en': 'VAT must be an integer',
      'fr': 'La TVA doit être un entier'
    },
    'validation.ivaRange': {
      'es': 'El IVA debe estar entre 0 y 100',
      'en': 'VAT must be between 0 and 100',
      'fr': 'La TVA doit être comprise entre 0 et 100'
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

    // If requesting login.footer and no params provided, inject current year automatically
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
