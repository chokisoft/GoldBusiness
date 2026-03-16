import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface ClienteDTO {
  id: number;
  codigo: string;
  descripcion: string;
  nif?: string;
  iban?: string;
  bicoSwift?: string;
  iva: number;
  direccion?: string;
  paisId?: number;
  paisDescripcion?: string;
  provinciaId?: number;
  provinciaDescripcion?: string;
  municipioId?: number;
  municipioDescripcion?: string;
  codigoPostalId?: number;
  codigoPostalCodigo?: string;
  web?: string;
  email1?: string;
  email2?: string;
  telefono1?: string;
  telefono2?: string;
  fax1?: string;
  fax2?: string;
  cancelado: boolean;
  creadoPor: string;
  fechaHoraCreado: Date;
  modificadoPor?: string;
  fechaHoraModificado?: Date;
}

export interface PagedResult<T> {
  items: T[];
  total: number;
}

@Injectable({
  providedIn: 'root'
})
export class ClienteService {
  constructor(private api: ApiService) { }

  getPaged(page: number = 1, pageSize: number = 50, term?: string): Observable<PagedResult<ClienteDTO>> {
    let url = `Cliente/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;
    return this.api.get<PagedResult<ClienteDTO>>(url);
  }

  getAll(): Observable<ClienteDTO[]> {
    console.warn('⚠️ ClienteService.getAll() puede ser lento. Considera usar getPaged()');
    return this.api.get<ClienteDTO[]>('Cliente');
  }

  getById(id: number): Observable<ClienteDTO> {
    return this.api.get<ClienteDTO>(`Cliente/${id}`);
  }

  create(data: ClienteDTO): Observable<ClienteDTO> {
    return this.api.post<ClienteDTO>('Cliente', data);
  }

  update(id: number, data: ClienteDTO): Observable<ClienteDTO> {
    return this.api.put<ClienteDTO>(`Cliente/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`Cliente/${id}`);
  }
}
