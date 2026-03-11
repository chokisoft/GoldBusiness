import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface GrupoCuentaDTO {
  id?: number;
  codigo: string;
  descripcion: string;
  cancelado?: boolean;  // ✅ CORREGIDO: 'cancelado' en lugar de 'activo'
  creadoPor?: string;
  fechaHoraCreado?: string;
  modificadoPor?: string;
  fechaHoraModificado?: string;
}

// ✅ AGREGADO: Interfaz PagedResult que faltaba
export interface PagedResult<T> {
  items: T[];
  total: number;
}

@Injectable({
  providedIn: 'root'
})
export class GrupoCuentaService {
  constructor(private api: ApiService) {}

  // ✅ Método paginado del servidor
  getPaged(page: number = 1, pageSize: number = 50, term?: string): Observable<PagedResult<GrupoCuentaDTO>> {
    let url = `GrupoCuenta/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;
    return this.api.get<PagedResult<GrupoCuentaDTO>>(url);
  }

  getAll(): Observable<GrupoCuentaDTO[]> {
    console.warn('⚠️ GrupoCuentaService.getAll() puede ser lento. Considera usar getPaged()');
    return this.api.get<GrupoCuentaDTO[]>('GrupoCuenta');
  }

  getById(id: number): Observable<GrupoCuentaDTO> {
    return this.api.get<GrupoCuentaDTO>(`GrupoCuenta/${id}`);
  }

  create(data: GrupoCuentaDTO): Observable<GrupoCuentaDTO> {
    return this.api.post<GrupoCuentaDTO>('GrupoCuenta', data);
  }

  update(id: number, data: GrupoCuentaDTO): Observable<GrupoCuentaDTO> {
    return this.api.put<GrupoCuentaDTO>(`GrupoCuenta/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`GrupoCuenta/${id}`);
  }

  buscar(termino: string): Observable<GrupoCuentaDTO[]> {
    return this.api.get<GrupoCuentaDTO[]>(`GrupoCuenta/buscar?termino=${encodeURIComponent(termino)}`);
  }
}
