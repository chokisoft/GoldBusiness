import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface LocalidadDTO {
  id?: number;
  establecimientoId: number;
  establecimientoCodigo?: string;
  establecimientoDescripcion?: string;
  codigo: string;
  descripcion: string;
  almacen?: boolean;
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
  constructor(private api: ApiService) {}

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
