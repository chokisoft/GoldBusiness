import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface SubLineaDTO {
  id?: number;
  codigo: string;
  descripcion: string;
  lineaId: number;
  lineaCodigo?: string;
  lineaDescripcion?: string;
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
export class SubLineaService {
  constructor(private api: ApiService) {}

  getPaged(page: number = 1, pageSize: number = 50, term?: string, lineaId?: number): Observable<PagedResult<SubLineaDTO>> {
    let url = `SubLinea/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;
    if (lineaId) url += `&lineaId=${lineaId}`;
    return this.api.get<PagedResult<SubLineaDTO>>(url);
  }

  getAll(): Observable<SubLineaDTO[]> {
    console.warn('⚠️ SubLineaService.getAll() puede ser lento. Considera usar getPaged()');
    return this.api.get<SubLineaDTO[]>('SubLinea');
  }

  getById(id: number): Observable<SubLineaDTO> {
    return this.api.get<SubLineaDTO>(`SubLinea/${id}`);
  }

  create(data: SubLineaDTO): Observable<SubLineaDTO> {
    return this.api.post<SubLineaDTO>('SubLinea', data);
  }

  update(id: number, data: SubLineaDTO): Observable<SubLineaDTO> {
    return this.api.put<SubLineaDTO>(`SubLinea/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`SubLinea/${id}`);
  }
}
