import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface CodigoPostalDTO {
  id?: number;
  codigo: string;
  descripcion?: string;
  municipioId: number;
  municipioDescripcion?: string;
  provinciaDescripcion?: string;
  paisDescripcion?: string;
  activo?: boolean;
  fechaHoraCreado?: string;
  fechaHoraModificado?: string;
  creadoPor?: string;
  modificadoPor?: string;
  municipio?: {
    id: number;
    provinciaId: number;
    descripcion: string;
    provincia?: {
      id: number;
      paisId: number;
      descripcion: string;
    };
  };
}

export interface PagedResult<T> {
  items: T[];
  total: number;
}

@Injectable({
  providedIn: 'root'
})
export class CodigoPostalService {
  constructor(private api: ApiService) {}

  // Método paginado que usa el endpoint del servidor
  getPaged(page: number = 1, pageSize: number = 50, term?: string, municipioId?: number): Observable<PagedResult<CodigoPostalDTO>> {
    let url = `CodigoPostal/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;
    if (municipioId) url += `&municipioId=${municipioId}`;
    return this.api.get<PagedResult<CodigoPostalDTO>>(url);
  }

  // Mantener getAll() pero con warning - solo para casos específicos
  getAll(): Observable<CodigoPostalDTO[]> {
    console.warn('⚠️ CodigoPostalService.getAll() puede causar timeout. Considera usar getPaged()');
    return this.api.get<CodigoPostalDTO[]>('CodigoPostal');
  }

  getById(id: number): Observable<CodigoPostalDTO> {
    return this.api.get<CodigoPostalDTO>(`CodigoPostal/${id}`);
  }

  getByMunicipioId(municipioId: number): Observable<CodigoPostalDTO[]> {
    return this.api.get<CodigoPostalDTO[]>(`CodigoPostal/municipio/${municipioId}`);
  }

  create(data: CodigoPostalDTO): Observable<CodigoPostalDTO> {
    return this.api.post<CodigoPostalDTO>('CodigoPostal', data);
  }

  update(id: number, data: CodigoPostalDTO): Observable<CodigoPostalDTO> {
    return this.api.put<CodigoPostalDTO>(`CodigoPostal/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`CodigoPostal/${id}`);
  }

  buscar(termino: string, municipioId?: number): Observable<CodigoPostalDTO[]> {
    let url = `CodigoPostal/buscar?termino=${encodeURIComponent(termino)}`;
    if (municipioId) url += `&municipioId=${municipioId}`;
    return this.api.get<CodigoPostalDTO[]>(url);
  }
}
