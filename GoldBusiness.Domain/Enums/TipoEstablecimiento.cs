namespace GoldBusiness.Domain.Enums
{
    /// <summary>
    /// Tipo de establecimiento según clasificación organizacional y legal.
    /// Basado en SAP Plant Types y Oracle EBS Organization Types.
    /// Define la NATURALEZA JURÍDICA/ORGANIZACIONAL del establecimiento, NO su función operativa.
    /// </summary>
    public enum TipoEstablecimiento
    {
        /// <summary>
        /// Sede Central / Head Office - Centro corporativo principal de la empresa.
        /// SAP: Plant Type 'HQ' / Oracle: Organization Type 'CORPORATE'
        /// </summary>
        SedeCentral = 1,

        /// <summary>
        /// Sucursal / Branch Office - Oficina regional con cierta autonomía administrativa.
        /// SAP: Plant Type 'BR' / Oracle: Organization Type 'BRANCH'
        /// </summary>
        Sucursal = 2,

        /// <summary>
        /// Centro de Distribución / Distribution Center - Instalación logística de gran escala.
        /// SAP: Plant Type 'DC' / Oracle: Organization Type 'WAREHOUSE'
        /// Enfocado en almacenamiento y distribución masiva.
        /// </summary>
        CentroDistribucion = 3,

        /// <summary>
        /// Planta de Producción / Manufacturing Plant - Instalación de fabricación/manufactura.
        /// SAP: Plant Type 'MP' / Oracle: Organization Type 'MANUFACTURING'
        /// Incluye fábricas, plantas industriales, talleres de producción.
        /// </summary>
        PlantaProduccion = 4,

        /// <summary>
        /// Centro de Servicios / Service Center - Instalación de prestación de servicios.
        /// SAP: Plant Type 'SC' / Oracle: Organization Type 'SERVICE'
        /// Incluye centros de soporte técnico, mantenimiento, servicio al cliente.
        /// </summary>
        CentroServicios = 5,

        /// <summary>
        /// Oficina Comercial / Sales Office - Oficina dedicada a actividades comerciales/ventas.
        /// SAP: Plant Type 'SO' / Oracle: Organization Type 'SALES'
        /// Sin inventario propio, enfocada en gestión comercial.
        /// </summary>
        OficinaComercial = 6,

        /// <summary>
        /// Centro Logístico / Logistics Hub - Instalación de operaciones logísticas complejas.
        /// SAP: Plant Type 'LH' / Oracle: Organization Type 'LOGISTICS'
        /// Cross-docking, consolidación, tránsito internacional.
        /// </summary>
        CentroLogistico = 7,

        /// <summary>
        /// Punto de Atención / Customer Service Point - Instalación de atención directa al cliente.
        /// SAP: Plant Type 'CS' / Oracle: Organization Type 'CUSTOMER_SERVICE'
        /// Showrooms, oficinas de atención, puntos de información.
        /// </summary>
        PuntoAtencion = 8,

        /// <summary>
        /// Franquicia / Franchise - Establecimiento operado bajo modelo de franquicia.
        /// SAP: Plant Type 'FR' / Oracle: Organization Type 'FRANCHISE'
        /// Relación contractual especial con el negocio matriz.
        /// </summary>
        Franquicia = 9,

        /// <summary>
        /// Oficina Administrativa / Administrative Office - Instalación puramente administrativa.
        /// SAP: Plant Type 'AO' / Oracle: Organization Type 'ADMIN'
        /// Sin operaciones de inventario ni producción.
        /// </summary>
        OficinaAdministrativa = 10
    }
}
