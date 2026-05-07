import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { TranslationService } from './translation.service';

// ═══════════════════════════════════════════════════════════════
// ENUM TipoLocalidad (sincronizado con backend)
// Basado en SAP Storage Location Types y Oracle Subinventory Types
// ═══════════════════════════════════════════════════════════════
export enum TipoLocalidad {
  Almacen = 1,
  PuntoVenta = 2,
  AreaRecepcion = 3,
  AreaDespacho = 4,
  Produccion = 5,
  Transito = 6,
  Cuarentena = 7,
  Consignacion = 8,
  Devoluciones = 9,
  Obsoletos = 10,
  Gerencia = 11,
  Administrativa = 12
}

export interface LocalidadDTO {
  id?: number;
  establecimientoId: number;
  establecimientoCodigo?: string;
  establecimientoDescripcion?: string;
  codigo: string;
  descripcion: string;
  
  // ══ Tipo y Capacidades Operativas (Estándares ERP) ══
  tipo: TipoLocalidad;
  tipoDescripcion?: string;
  permiteVentas: boolean;
  permiteCompras: boolean;
  permiteTransferencias: boolean;
  permiteAjustes: boolean;
  requiereControlLotes: boolean;
  requiereNumerosSerie: boolean;
  
  // ══ Cuentas Contables (GL Accounts) ══
  cuentaInventarioId?: number;
  cuentaInventarioCodigo?: string;
  cuentaInventarioDescripcion?: string;
  cuentaCostoId?: number;
  cuentaCostoCodigo?: string;
  cuentaCostoDescripcion?: string;
  cuentaVentaId?: number;
  cuentaVentaCodigo?: string;
  cuentaVentaDescripcion?: string;
  cuentaDevolucionId?: number;
  cuentaDevolucionCodigo?: string;
  cuentaDevolucionDescripcion?: string;
  
  // ══ Metadatos ══
  activo?: boolean;
  cancelado?: boolean;
  creadoPor?: string;
  fechaHoraCreado?: string;
  modificadoPor?: string;
  fechaHoraModificado?: string;
}

export interface PagedResult<T> {
  items: T[];
  total: number;
}

@Injectable({
  providedIn: 'root'
})
export class LocalidadService {
  constructor(
    private api: ApiService,
    private translationService: TranslationService
  ) {}

  // ═══════════════════════════════════════════════════════════════
  // Métodos Helper para TipoLocalidad
  // ═══════════════════════════════════════════════════════════════

  /**
   * Obtiene todas las opciones de tipo de localidad
   */
  getTiposLocalidad(): { value: TipoLocalidad; label: string }[] {
    return [
      { value: TipoLocalidad.Almacen, label: this.translationService.translate('localidad.tipo.almacen') },
      { value: TipoLocalidad.PuntoVenta, label: this.translationService.translate('localidad.tipo.puntoVenta') },
      { value: TipoLocalidad.AreaRecepcion, label: this.translationService.translate('localidad.tipo.areaRecepcion') },
      { value: TipoLocalidad.AreaDespacho, label: this.translationService.translate('localidad.tipo.areaDespacho') },
      { value: TipoLocalidad.Produccion, label: this.translationService.translate('localidad.tipo.produccion') },
      { value: TipoLocalidad.Transito, label: this.translationService.translate('localidad.tipo.transito') },
      { value: TipoLocalidad.Cuarentena, label: this.translationService.translate('localidad.tipo.cuarentena') },
      { value: TipoLocalidad.Consignacion, label: this.translationService.translate('localidad.tipo.consignacion') },
      { value: TipoLocalidad.Devoluciones, label: this.translationService.translate('localidad.tipo.devoluciones') },
      { value: TipoLocalidad.Obsoletos, label: this.translationService.translate('localidad.tipo.obsoletos') },
      { value: TipoLocalidad.Gerencia, label: this.translationService.translate('localidad.tipo.gerencia') },
      { value: TipoLocalidad.Administrativa, label: this.translationService.translate('localidad.tipo.administrativa') }
    ];
  }

  /**
   * Obtiene la etiqueta de un tipo de localidad
   */
  getTipoLocalidadLabel(tipo: TipoLocalidad): string {
    const tipos = this.getTiposLocalidad();
    return tipos.find(t => t.value === tipo)?.label || 'Desconocido';
  }

  // ═══════════════════════════════════════════════════════════════
  // Métodos API
  // ═══════════════════════════════════════════════════════════════

  getPaged(page: number = 1, pageSize: number = 50, term?: string, establecimientoId?: number): Observable<PagedResult<LocalidadDTO>> {
    let url = `Localidad/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;
    if (establecimientoId) url += `&establecimientoId=${establecimientoId}`;
    return this.api.get<PagedResult<LocalidadDTO>>(url);
  }

  getAll(): Observable<LocalidadDTO[]> {
    console.warn('⚠️ LocalidadService.getAll() puede ser lento. Considera usar getPaged()');
    return this.api.get<LocalidadDTO[]>('Localidad');
  }

  getByEstablecimientoId(establecimientoId: number): Observable<LocalidadDTO[]> {
    return this.api.get<LocalidadDTO[]>(`Localidad/establecimiento/${establecimientoId}`);
  }

  getById(id: number): Observable<LocalidadDTO> {
    return this.api.get<LocalidadDTO>(`Localidad/${id}`);
  }

  create(data: LocalidadDTO): Observable<LocalidadDTO> {
    return this.api.post<LocalidadDTO>('Localidad', data);
  }

  update(id: number, data: LocalidadDTO): Observable<LocalidadDTO> {
    return this.api.put<LocalidadDTO>(`Localidad/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`Localidad/${id}`);
  }
}
