import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface LineaDTO {
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
export class LineaService {
  constructor(private api: ApiService) {}

  getPaged(page: number = 1, pageSize: number = 50, term?: string): Observable<PagedResult<LineaDTO>> {
    let url = `Linea/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;
    return this.api.get<PagedResult<LineaDTO>>(url);
  }

  getAll(): Observable<LineaDTO[]> {
    console.warn('⚠️ LineaService.getAll() puede ser lento. Considera usar getPaged()');
    return this.api.get<LineaDTO[]>('Linea');
  }

  getById(id: number): Observable<LineaDTO> {
    return this.api.get<LineaDTO>(`Linea/${id}`);
  }

  create(data: LineaDTO): Observable<LineaDTO> {
    return this.api.post<LineaDTO>('Linea', data);
  }

  update(id: number, data: LineaDTO): Observable<LineaDTO> {
    return this.api.put<LineaDTO>(`Linea/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`Linea/${id}`);
  }
}
