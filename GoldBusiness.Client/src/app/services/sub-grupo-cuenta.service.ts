import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface SubGrupoCuentaDTO {
  id?: number;
  codigo: string;
  descripcion: string;
  grupoCuentaId: number;
  grupoCuentaCodigo?: string;
  grupoCuentaDescripcion?: string;
  deudora: boolean;
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
export class SubGrupoCuentaService {
  constructor(private api: ApiService) {}

  // ✅ AGREGADO: Paginación del servidor
  getPaged(page: number = 1, pageSize: number = 50, term?: string, grupoCuentaId?: number): Observable<PagedResult<SubGrupoCuentaDTO>> {
    let url = `SubGrupoCuenta/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;
    if (grupoCuentaId) url += `&grupoCuentaId=${grupoCuentaId}`;
    return this.api.get<PagedResult<SubGrupoCuentaDTO>>(url);
  }

  getAll(): Observable<SubGrupoCuentaDTO[]> {
    console.warn('⚠️ SubGrupoCuentaService.getAll() puede ser lento. Considera usar getPaged()');
    return this.api.get<SubGrupoCuentaDTO[]>('SubGrupoCuenta');
  }

  getById(id: number): Observable<SubGrupoCuentaDTO> {
    return this.api.get<SubGrupoCuentaDTO>(`SubGrupoCuenta/${id}`);
  }

  getByGrupoCuentaId(grupoCuentaId: number): Observable<SubGrupoCuentaDTO[]> {
    return this.api.get<SubGrupoCuentaDTO[]>(`SubGrupoCuenta/grupo/${grupoCuentaId}`);
  }

  create(data: SubGrupoCuentaDTO): Observable<SubGrupoCuentaDTO> {
    return this.api.post<SubGrupoCuentaDTO>('SubGrupoCuenta', data);
  }

  update(id: number, data: SubGrupoCuentaDTO): Observable<SubGrupoCuentaDTO> {
    return this.api.put<SubGrupoCuentaDTO>(`SubGrupoCuenta/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`SubGrupoCuenta/${id}`);
  }
}
