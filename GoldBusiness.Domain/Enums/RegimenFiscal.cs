namespace GoldBusiness.Domain.Enums
{
    /// <summary>
    /// Régimen fiscal aplicable a la empresa, clientes y proveedores.
    /// </summary>
    public enum RegimenFiscal
    {
        /// <summary>Régimen General</summary>
        General = 1,
        
        /// <summary>Régimen Simplificado</summary>
        Simplificado = 2,
        
        /// <summary>Autónomo / Profesional</summary>
        Autonomo = 3,
        
        /// <summary>Exento de IVA</summary>
        Exento = 4,
        
        /// <summary>Exportador</summary>
        Exportador = 5,
        
        /// <summary>Intracomunitario (UE)</summary>
        Intracomunitario = 6,
        
        /// <summary>Pequeño Contribuyente (PYME)</summary>
        PequenoContribuyente = 7,
        
        /// <summary>Gran Contribuyente</summary>
        GranContribuyente = 8,
        
        /// <summary>Persona Física</summary>
        PersonaFisica = 9,
        
        /// <summary>Persona Jurídica</summary>
        PersonaJuridica = 10,
        
        /// <summary>Gobierno</summary>
        Gobierno = 11,
        
        /// <summary>ONG / Sin Fines de Lucro</summary>
        ONG = 12
    }
}