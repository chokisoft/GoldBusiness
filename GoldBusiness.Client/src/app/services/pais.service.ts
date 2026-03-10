import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface PaisDTO {
  id?: number;
  codigo: string;
  descripcion: string;
  activo?: boolean;
  fechaHoraCreado?: string;
  fechaHoraModificado?: string;
  creadoPor?: string;
  modificadoPor?: string;
}

export interface PagedResult<T> {
  items: T[];
  total: number;
}

@Injectable({
  providedIn: 'root'
})
export class PaisService {
  constructor(private api: ApiService) {}

  getPaged(page: number = 1, pageSize: number = 50, term?: string): Observable<PagedResult<PaisDTO>> {
    let url = `Pais/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;
    return this.api.get<PagedResult<PaisDTO>>(url);
  }

  getAll(): Observable<PaisDTO[]> {
    console.warn('⚠️ PaisService.getAll() puede ser lento. Considera usar getPaged()');
    return this.api.get<PaisDTO[]>('Pais');
  }

  getById(id: number): Observable<PaisDTO> {
    return this.api.get<PaisDTO>(`Pais/${id}`);
  }

  create(data: PaisDTO): Observable<PaisDTO> {
    return this.api.post<PaisDTO>('Pais', data);
  }

  update(id: number, data: PaisDTO): Observable<PaisDTO> {
    return this.api.put<PaisDTO>(`Pais/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`Pais/${id}`);
  }
}
