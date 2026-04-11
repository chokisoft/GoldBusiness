/**
 * Enum de tipos de identificación fiscal según normativas internacionales
 */
export enum TipoIdentificacionFiscal {
  NIF = 1,
  CIF = 2,
  DNI = 3,
  NIE = 4,
  VAT = 5,
  RFC = 6,
  RUC = 7,
  RUT = 8,
  CUIT = 9,
  SSN = 10,
  EIN = 11,
  Pasaporte = 12,
  Otro = 99
}

/**
 * Enum de régimen fiscal
 */
export enum RegimenFiscal {
  General = 1,
  Simplificado = 2,
  Autonomo = 3,
  Exento = 4,
  Exportador = 5,
  Intracomunitario = 6,
  PequenoContribuyente = 7,
  GranContribuyente = 8,
  PersonaFisica = 9,
  PersonaJuridica = 10,
  Gobierno = 11,
  ONG = 12
}

/**
 * Opciones para dropdowns de tipo de identificación fiscal
 */
export const TIPO_IDENTIFICACION_FISCAL_OPTIONS = [
  { value: TipoIdentificacionFiscal.NIF, label: 'NIF - Número de Identificación Fiscal' },
  { value: TipoIdentificacionFiscal.CIF, label: 'CIF - Código de Identificación Fiscal' },
  { value: TipoIdentificacionFiscal.DNI, label: 'DNI - Documento Nacional de Identidad' },
  { value: TipoIdentificacionFiscal.NIE, label: 'NIE - Número de Identidad de Extranjero' },
  { value: TipoIdentificacionFiscal.VAT, label: 'VAT - Value Added Tax ID' },
  { value: TipoIdentificacionFiscal.RFC, label: 'RFC - Registro Federal de Contribuyentes' },
  { value: TipoIdentificacionFiscal.RUC, label: 'RUC - Registro Único de Contribuyentes' },
  { value: TipoIdentificacionFiscal.RUT, label: 'RUT - Rol Único Tributario' },
  { value: TipoIdentificacionFiscal.CUIT, label: 'CUIT - Clave Única de Identificación Tributaria' },
  { value: TipoIdentificacionFiscal.SSN, label: 'SSN - Social Security Number' },
  { value: TipoIdentificacionFiscal.EIN, label: 'EIN - Employer Identification Number' },
  { value: TipoIdentificacionFiscal.Pasaporte, label: 'Pasaporte' },
  { value: TipoIdentificacionFiscal.Otro, label: 'Otro' }
];

/**
 * Opciones para dropdowns de régimen fiscal
 */
export const REGIMEN_FISCAL_OPTIONS = [
  { value: RegimenFiscal.General, label: 'Régimen General' },
  { value: RegimenFiscal.Simplificado, label: 'Régimen Simplificado' },
  { value: RegimenFiscal.Autonomo, label: 'Autónomo / Profesional' },
  { value: RegimenFiscal.Exento, label: 'Exento de IVA' },
  { value: RegimenFiscal.Exportador, label: 'Exportador' },
  { value: RegimenFiscal.Intracomunitario, label: 'Intracomunitario (UE)' },
  { value: RegimenFiscal.PequenoContribuyente, label: 'Pequeño Contribuyente (PYME)' },
  { value: RegimenFiscal.GranContribuyente, label: 'Gran Contribuyente' },
  { value: RegimenFiscal.PersonaFisica, label: 'Persona Física' },
  { value: RegimenFiscal.PersonaJuridica, label: 'Persona Jurídica' },
  { value: RegimenFiscal.Gobierno, label: 'Gobierno' },
  { value: RegimenFiscal.ONG, label: 'ONG / Sin Fines de Lucro' }
];
