import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface CuentaDTO {
  id?: number;
  codigo: string;
  descripcion: string;
  subGrupoCuentaId: number;
  subGrupoCuentaCodigo?: string;
  subGrupoCuentaDescripcion?: string;
  grupoCuentaCodigo?: string;
  grupoCuentaDescripcion?: string;
  systemConfigurationId?: number;
  cancelado?: boolean;
  creadoPor?: string;
  fechaHoraCreado?: string;
  modificadoPor?: string;
  fechaHoraModificado?: string;
}

// ✅ AGREGADO: Interface PagedResult
export interface PagedResult<T> {
  items: T[];
  total: number;
}

@Injectable({
  providedIn: 'root'
})
export class CuentaService {
  constructor(private api: ApiService) {}

  // ✅ AGREGADO: Paginación del servidor
  getPaged(page: number = 1, pageSize: number = 50, term?: string, subGrupoCuentaId?: number): Observable<PagedResult<CuentaDTO>> {
    let url = `Cuenta/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;
    if (subGrupoCuentaId) url += `&subGrupoCuentaId=${subGrupoCuentaId}`;
    return this.api.get<PagedResult<CuentaDTO>>(url);
  }

  getAll(): Observable<CuentaDTO[]> {
    console.warn('⚠️ CuentaService.getAll() puede ser lento. Considera usar getPaged()');
    return this.api.get<CuentaDTO[]>('Cuenta');
  }

  getById(id: number): Observable<CuentaDTO> {
    return this.api.get<CuentaDTO>(`Cuenta/${id}`);
  }

  getBySubGrupoCuentaId(subGrupoCuentaId: number): Observable<CuentaDTO[]> {
    return this.api.get<CuentaDTO[]>(`Cuenta/subgrupo/${subGrupoCuentaId}`);
  }

  create(data: CuentaDTO): Observable<CuentaDTO> {
    return this.api.post<CuentaDTO>('Cuenta', data);
  }

  update(id: number, data: CuentaDTO): Observable<CuentaDTO> {
    return this.api.put<CuentaDTO>(`Cuenta/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`Cuenta/${id}`);
  }
}
