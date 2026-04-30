namespace GoldBusiness.Domain.Enums
{
    /// <summary>
    /// Tipo de localidad según función operativa de control de inventario.
    /// Basado en SAP Storage Location Types y Oracle EBS Subinventory Types.
    /// Define el PROPÓSITO OPERATIVO de la ubicación de inventario, NO su naturaleza organizacional.
    /// </summary>
    public enum TipoLocalidad
    {
        /// <summary>
        /// Almacén / Warehouse - Almacenamiento general de inventario.
        /// SAP: Storage Location Type '0001' / Oracle: Subinventory Type 'STORES'
        /// Ubicación principal de almacenamiento de productos.
        /// </summary>
        Almacen = 1,

        /// <summary>
        /// Punto de Venta / Retail Point - Inventario destinado a venta al público.
        /// SAP: Storage Location Type '0002' / Oracle: Subinventory Type 'RETAIL'
        /// Inventario expuesto para venta directa al consumidor final.
        /// </summary>
        PuntoVenta = 2,

        /// <summary>
        /// Área de Recepción / Receiving Area - Zona de recepción de mercancía entrante.
        /// SAP: Storage Location Type '0101' / Oracle: Subinventory Type 'RECEIVING'
        /// Ubicación temporal para inspección y procesamiento de entradas.
        /// </summary>
        AreaRecepcion = 3,

        /// <summary>
        /// Área de Despacho / Shipping Area - Zona de preparación de mercancía saliente.
        /// SAP: Storage Location Type '0102' / Oracle: Subinventory Type 'SHIPPING'
        /// Ubicación temporal para preparación de pedidos y despachos.
        /// </summary>
        AreaDespacho = 4,

        /// <summary>
        /// Producción / Manufacturing Floor - Inventario en proceso de manufactura (WIP).
        /// SAP: Storage Location Type '0201' / Oracle: Subinventory Type 'WIP'
        /// Materiales en línea de producción, trabajo en proceso.
        /// </summary>
        Produccion = 5,

        /// <summary>
        /// Tránsito / In-Transit - Inventario en movimiento entre ubicaciones.
        /// SAP: Storage Location Type '0301' / Oracle: Subinventory Type 'INTRANSIT'
        /// Mercancía que está siendo transportada, no disponible físicamente.
        /// </summary>
        Transito = 6,

        /// <summary>
        /// Cuarentena / Quarantine - Inventario bloqueado para control de calidad.
        /// SAP: Storage Location Type '0401' / Oracle: Subinventory Type 'QUARANTINE'
        /// Productos en evaluación, inspección o que no cumplen estándares.
        /// </summary>
        Cuarentena = 7,

        /// <summary>
        /// Consignación / Consignment - Inventario de propiedad de terceros.
        /// SAP: Storage Location Type '0501' / Oracle: Subinventory Type 'CONSIGNMENT'
        /// Productos de proveedores almacenados en nuestras instalaciones.
        /// </summary>
        Consignacion = 8,

        /// <summary>
        /// Devoluciones / Returns - Inventario devuelto por clientes.
        /// SAP: Storage Location Type '0601' / Oracle: Subinventory Type 'RETURNS'
        /// Productos retornados para procesamiento (reingreso, reparación, desecho).
        /// </summary>
        Devoluciones = 9,

        /// <summary>
        /// Obsoletos / Obsolete Stock - Inventario obsoleto, dañado o de baja rotación.
        /// SAP: Storage Location Type '0701' / Oracle: Subinventory Type 'OBSOLETE'
        /// Productos que serán desechados, donados o vendidos como scrap.
        /// </summary>
        Obsoletos = 10
    }
}
