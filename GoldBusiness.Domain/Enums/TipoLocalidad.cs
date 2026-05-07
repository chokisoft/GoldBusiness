namespace GoldBusiness.Domain.Enums
{
    /// <summary>
    /// Tipo de localidad seg�n funci�n operativa de control de inventario.
    /// Basado en SAP Storage Location Types y Oracle EBS Subinventory Types.
    /// Define el PROP�SITO OPERATIVO de la ubicaci�n de inventario, NO su naturaleza organizacional.
    /// </summary>
    public enum TipoLocalidad
    {
        /// <summary>
        /// Almac�n / Warehouse - Almacenamiento general de inventario.
        /// SAP: Storage Location Type '0001' / Oracle: Subinventory Type 'STORES'
        /// Ubicaci�n principal de almacenamiento de productos.
        /// </summary>
        Almacen = 1,

        /// <summary>
        /// Punto de Venta / Retail Point - Inventario destinado a venta al p�blico.
        /// SAP: Storage Location Type '0002' / Oracle: Subinventory Type 'RETAIL'
        /// Inventario expuesto para venta directa al consumidor final.
        /// </summary>
        PuntoVenta = 2,

        /// <summary>
        /// �rea de Recepci�n / Receiving Area - Zona de recepci�n de mercanc�a entrante.
        /// SAP: Storage Location Type '0101' / Oracle: Subinventory Type 'RECEIVING'
        /// Ubicaci�n temporal para inspecci�n y procesamiento de entradas.
        /// </summary>
        AreaRecepcion = 3,

        /// <summary>
        /// �rea de Despacho / Shipping Area - Zona de preparaci�n de mercanc�a saliente.
        /// SAP: Storage Location Type '0102' / Oracle: Subinventory Type 'SHIPPING'
        /// Ubicaci�n temporal para preparaci�n de pedidos y despachos.
        /// </summary>
        AreaDespacho = 4,

        /// <summary>
        /// Producci�n / Manufacturing Floor - Inventario en proceso de manufactura (WIP).
        /// SAP: Storage Location Type '0201' / Oracle: Subinventory Type 'WIP'
        /// Materiales en l�nea de producci�n, trabajo en proceso.
        /// </summary>
        Produccion = 5,

        /// <summary>
        /// Tr�nsito / In-Transit - Inventario en movimiento entre ubicaciones.
        /// SAP: Storage Location Type '0301' / Oracle: Subinventory Type 'INTRANSIT'
        /// Mercanc�a que est� siendo transportada, no disponible f�sicamente.
        /// </summary>
        Transito = 6,

        /// <summary>
        /// Cuarentena / Quarantine - Inventario bloqueado para control de calidad.
        /// SAP: Storage Location Type '0401' / Oracle: Subinventory Type 'QUARANTINE'
        /// Productos en evaluaci�n, inspecci�n o que no cumplen est�ndares.
        /// </summary>
        Cuarentena = 7,

        /// <summary>
        /// Consignaci�n / Consignment - Inventario de propiedad de terceros.
        /// SAP: Storage Location Type '0501' / Oracle: Subinventory Type 'CONSIGNMENT'
        /// Productos de proveedores almacenados en nuestras instalaciones.
        /// </summary>
        Consignacion = 8,

        /// <summary>
        /// Devoluciones / Returns - Inventario devuelto por clientes.
        /// SAP: Storage Location Type '0601' / Oracle: Subinventory Type 'RETURNS'
        /// Productos retornados para procesamiento (reingreso, reparaci�n, desecho).
        /// </summary>
        Devoluciones = 9,

        /// <summary>
        /// Obsoletos / Obsolete Stock - Inventario obsoleto, da�ado o de baja rotaci�n.
        /// SAP: Storage Location Type '0701' / Oracle: Subinventory Type 'OBSOLETE'
        /// Productos que ser�n desechados, donados o vendidos como scrap.
        /// </summary>
        Obsoletos = 10,

        /// <summary>
        /// Gerencia / Management - �rea destinada a funciones gerenciales y de direcci�n.
        /// Ubicaci�n reservada para uso de gerencia y toma de decisiones.
        /// </summary>
        Gerencia = 11,

        /// <summary>
        /// Administrativa / Administrative - �rea destinada a funciones administrativas.
        /// Ubicaci�n reservada para operaciones administrativas y de gesti�n interna.
        /// </summary>
        Administrativa = 12
    }
}
