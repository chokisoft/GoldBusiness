namespace GoldBusiness.Domain.Enums
{
    /// <summary>
    /// Tipo de identificación fiscal según normativas internacionales.
    /// </summary>
    public enum TipoIdentificacionFiscal
    {
        /// <summary>NIF - Número de Identificación Fiscal (España - Persona física)</summary>
        NIF = 1,
        
        /// <summary>CIF - Código de Identificación Fiscal (España - Persona jurídica)</summary>
        CIF = 2,
        
        /// <summary>DNI - Documento Nacional de Identidad</summary>
        DNI = 3,
        
        /// <summary>NIE - Número de Identidad de Extranjero (España)</summary>
        NIE = 4,
        
        /// <summary>VAT - Value Added Tax ID (Unión Europea)</summary>
        VAT = 5,
        
        /// <summary>RFC - Registro Federal de Contribuyentes (México)</summary>
        RFC = 6,
        
        /// <summary>RUC - Registro Único de Contribuyentes (Perú, Ecuador, etc.)</summary>
        RUC = 7,
        
        /// <summary>RUT - Rol Único Tributario (Chile, Uruguay)</summary>
        RUT = 8,
        
        /// <summary>CUIT - Clave Única de Identificación Tributaria (Argentina)</summary>
        CUIT = 9,
        
        /// <summary>SSN - Social Security Number (USA)</summary>
        SSN = 10,
        
        /// <summary>EIN - Employer Identification Number (USA)</summary>
        EIN = 11,
        
        /// <summary>Pasaporte</summary>
        Pasaporte = 12,
        
        /// <summary>Otro tipo de identificación</summary>
        Otro = 99
    }
}