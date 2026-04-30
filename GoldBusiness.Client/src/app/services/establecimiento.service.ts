import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

// ═══════════════════════════════════════════════════════════════
// 🏢 ENUM: TIPO DE ESTABLECIMIENTO (sincronizado con backend)
// Basado en SAP Plant Types y Oracle Organization Types
// ═══════════════════════════════════════════════════════════════
export enum TipoEstablecimiento {
  SedeCentral = 1,
  Sucursal = 2,
  CentroDistribucion = 3,
  PlantaProduccion = 4,
  CentroServicios = 5,
  OficinaComercial = 6,
  CentroLogistico = 7,
  PuntoAtencion = 8,
  Franquicia = 9,
  OficinaAdministrativa = 10
}

// ═══════════════════════════════════════════════════════════════
// 🏢 INTERFACE: ESTABLECIMIENTO DTO (OPTIMIZADO)
// ═══════════════════════════════════════════════════════════════
export interface EstablecimientoDTO {
  id: number;
  codigo: string;
  descripcion: string;
  negocioId: number;
  negocioDescripcion?: string;
  direccion?: string;
  telefono?: string;
  
  // ═══════════════════════════════════════════════════════════════
  // 🏢 INFORMACIÓN ORGANIZACIONAL
  // ═══════════════════════════════════════════════════════════════
  tipo: TipoEstablecimiento;
  
  // ═══════════════════════════════════════════════════════════════
  // 📍 INFORMACIÓN GEOGRÁFICA
  // ═══════════════════════════════════════════════════════════════
  paisId?: number;
  paisDescripcion?: string;
  provinciaId?: number;
  provinciaDescripcion?: string;
  municipioId?: number;
  municipioDescripcion?: string;
  codigoPostalId?: number;
  codigoPostalCodigo?: string;
  
  // ═══════════════════════════════════════════════════════════════
  // 📊 CONTROL DE ESTADO
  // ═══════════════════════════════════════════════════════════════
  operativoActualmente: boolean;
  activo: boolean;
  cancelado: boolean;
  
  // ═══════════════════════════════════════════════════════════════
  // 🔧 AUDITORÍA
  // ═══════════════════════════════════════════════════════════════
  creadoPor: string;
  fechaHoraCreado: Date;
  modificadoPor?: string;
  fechaHoraModificado?: Date;
}

export interface PagedResult<T> {
  items: T[];
  total: number;
}

@Injectable({
  providedIn: 'root'
})
export class EstablecimientoService {
  constructor(private api: ApiService) { }

  getPaged(page: number = 1, pageSize: number = 50, term?: string): Observable<PagedResult<EstablecimientoDTO>> {
    let url = `Establecimiento/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;
    return this.api.get<PagedResult<EstablecimientoDTO>>(url);
  }

  getAll(): Observable<EstablecimientoDTO[]> {
    console.warn('⚠️ EstablecimientoService.getAll() puede ser lento. Considera usar getPaged()');
    return this.api.get<EstablecimientoDTO[]>('Establecimiento');
  }

  getById(id: number): Observable<EstablecimientoDTO> {
    return this.api.get<EstablecimientoDTO>(`Establecimiento/${id}`);
  }

  create(data: EstablecimientoDTO): Observable<EstablecimientoDTO> {
    return this.api.post<EstablecimientoDTO>('Establecimiento', data);
  }

  update(id: number, data: EstablecimientoDTO): Observable<EstablecimientoDTO> {
    return this.api.put<EstablecimientoDTO>(`Establecimiento/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`Establecimiento/${id}`);
  }
}
