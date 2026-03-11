import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface UnidadMedidaDTO {
  id?: number;
  codigo: string;
  descripcion: string;
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
export class UnidadMedidaService {
  constructor(private api: ApiService) {}

  getPaged(page: number = 1, pageSize: number = 50, term?: string): Observable<PagedResult<UnidadMedidaDTO>> {
    let url = `UnidadMedida/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;
    return this.api.get<PagedResult<UnidadMedidaDTO>>(url);
  }

  getAll(): Observable<UnidadMedidaDTO[]> {
    console.warn('⚠️ UnidadMedidaService.getAll() puede ser lento. Considera usar getPaged()');
    return this.api.get<UnidadMedidaDTO[]>('UnidadMedida');
  }

  getById(id: number): Observable<UnidadMedidaDTO> {
    return this.api.get<UnidadMedidaDTO>(`UnidadMedida/${id}`);
  }

  create(dto: UnidadMedidaDTO): Observable<UnidadMedidaDTO> {
    return this.api.post<UnidadMedidaDTO>('UnidadMedida', dto);
  }

  update(id: number, dto: UnidadMedidaDTO): Observable<UnidadMedidaDTO> {
    return this.api.put<UnidadMedidaDTO>(`UnidadMedida/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`UnidadMedida/${id}`);
  }
}
