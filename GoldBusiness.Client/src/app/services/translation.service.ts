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
    'localidad.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
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
    'pais.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'pais.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'pais.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
    },
    'pais.nacionalidad': {
      'es': 'Nacionalidad',
      'en': 'Nationality',
      'fr': 'Nationalité'
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
    'provincia.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'provincia.descripcion': {
      'es': 'Descripción',
      'en': 'Description',
      'fr': 'Description'
    },
    'provincia.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
    },
    'provincia.pais': {
      'es': 'País',
      'en': 'Country',
      'fr': 'Pays'
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
    'municipio.provincia': {
      'es': 'Provincia',
      'en': 'Province',
      'fr': 'Province'
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
    'codigoPostal.codigo': {
      'es': 'Código',
      'en': 'Code',
      'fr': 'Code'
    },
    'codigoPostal.municipio': {
      'es': 'Municipio',
      'en': 'Municipality',
      'fr': 'Municipalité'
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
    'unidadMedida.nombre': {
      'es': 'Nombre',
      'en': 'Name',
      'fr': 'Nom'
    },
    'unidadMedida.abreviatura': {
      'es': 'Abreviatura',
      'en': 'Abbreviation',
      'fr': 'Abréviation'
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
      'es': 'Ej: Descripción del Grupo Cuenta',
      'en': 'Ex: Account Group Description',
      'fr': 'Ex: Description du Groupe de Compte'
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
      'es': 'Ej: Activo Circulante',
      'en': 'Ex: Current Assets',
      'fr': 'Ex: Actif Circulant'
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
