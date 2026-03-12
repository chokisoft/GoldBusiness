import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface EstablecimientoDTO {
  id?: number;
  codigo: string;
  descripcion: string;
  negocioId: number;
  negocioCodigo?: string;
  negocioDescripcion?: string;
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
export class EstablecimientoService {
  constructor(private api: ApiService) {}

  getPaged(page: number = 1, pageSize: number = 50, term?: string, negocioId?: number): Observable<PagedResult<EstablecimientoDTO>> {
    let url = `Establecimiento/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;
    if (negocioId) url += `&negocioId=${negocioId}`;
    return this.api.get<PagedResult<EstablecimientoDTO>>(url);
  }

  getAll(): Observable<EstablecimientoDTO[]> {
    console.warn('⚠️ EstablecimientoService.getAll() puede ser lento. Considera usar getPaged()');
    return this.api.get<EstablecimientoDTO[]>('Establecimiento');
  }

  getByNegocioId(negocioId: number): Observable<EstablecimientoDTO[]> {
    return this.api.get<EstablecimientoDTO[]>(`Establecimiento/negocio/${negocioId}`);
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
