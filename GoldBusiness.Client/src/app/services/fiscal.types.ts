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

export interface FiscalOption<T extends number> {
  value: T;
  label: string;
}

export function getTipoIdentificacionFiscalOptions(
  translate: (key: string) => string
): FiscalOption<TipoIdentificacionFiscal>[] {
  return [
    { value: TipoIdentificacionFiscal.NIF, label: translate('fiscal.option.tipoIdentificadorFiscal.nif') },
    { value: TipoIdentificacionFiscal.CIF, label: translate('fiscal.option.tipoIdentificadorFiscal.cif') },
    { value: TipoIdentificacionFiscal.DNI, label: translate('fiscal.option.tipoIdentificadorFiscal.dni') },
    { value: TipoIdentificacionFiscal.NIE, label: translate('fiscal.option.tipoIdentificadorFiscal.nie') },
    { value: TipoIdentificacionFiscal.VAT, label: translate('fiscal.option.tipoIdentificadorFiscal.vat') },
    { value: TipoIdentificacionFiscal.RFC, label: translate('fiscal.option.tipoIdentificadorFiscal.rfc') },
    { value: TipoIdentificacionFiscal.RUC, label: translate('fiscal.option.tipoIdentificadorFiscal.ruc') },
    { value: TipoIdentificacionFiscal.RUT, label: translate('fiscal.option.tipoIdentificadorFiscal.rut') },
    { value: TipoIdentificacionFiscal.CUIT, label: translate('fiscal.option.tipoIdentificadorFiscal.cuit') },
    { value: TipoIdentificacionFiscal.SSN, label: translate('fiscal.option.tipoIdentificadorFiscal.ssn') },
    { value: TipoIdentificacionFiscal.EIN, label: translate('fiscal.option.tipoIdentificadorFiscal.ein') },
    { value: TipoIdentificacionFiscal.Pasaporte, label: translate('fiscal.option.tipoIdentificadorFiscal.pasaporte') },
    { value: TipoIdentificacionFiscal.Otro, label: translate('fiscal.option.tipoIdentificadorFiscal.otro') }
  ];
}

export function getRegimenFiscalOptions(
  translate: (key: string) => string
): FiscalOption<RegimenFiscal>[] {
  return [
    { value: RegimenFiscal.General, label: translate('fiscal.option.regimenFiscal.general') },
    { value: RegimenFiscal.Simplificado, label: translate('fiscal.option.regimenFiscal.simplificado') },
    { value: RegimenFiscal.Autonomo, label: translate('fiscal.option.regimenFiscal.autonomo') },
    { value: RegimenFiscal.Exento, label: translate('fiscal.option.regimenFiscal.exento') },
    { value: RegimenFiscal.Exportador, label: translate('fiscal.option.regimenFiscal.exportador') },
    { value: RegimenFiscal.Intracomunitario, label: translate('fiscal.option.regimenFiscal.intracomunitario') },
    { value: RegimenFiscal.PequenoContribuyente, label: translate('fiscal.option.regimenFiscal.pequenoContribuyente') },
    { value: RegimenFiscal.GranContribuyente, label: translate('fiscal.option.regimenFiscal.granContribuyente') },
    { value: RegimenFiscal.PersonaFisica, label: translate('fiscal.option.regimenFiscal.personaFisica') },
    { value: RegimenFiscal.PersonaJuridica, label: translate('fiscal.option.regimenFiscal.personaJuridica') },
    { value: RegimenFiscal.Gobierno, label: translate('fiscal.option.regimenFiscal.gobierno') },
    { value: RegimenFiscal.ONG, label: translate('fiscal.option.regimenFiscal.ong') }
  ];
}
