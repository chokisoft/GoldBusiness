import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface ProveedorDTO {
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
  cantidadProductos?: number;
}

export interface PagedResult<T> {
  items: T[];
  total: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProveedorService {
  constructor(private api: ApiService) { }

  getPaged(page: number = 1, pageSize: number = 50, term?: string): Observable<PagedResult<ProveedorDTO>> {
    let url = `Proveedor/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;
    return this.api.get<PagedResult<ProveedorDTO>>(url);
  }

  getAll(): Observable<ProveedorDTO[]> {
    console.warn('⚠️ ProveedorService.getAll() puede ser lento. Considera usar getPaged()');
    return this.api.get<ProveedorDTO[]>('Proveedor');
  }

  getById(id: number): Observable<ProveedorDTO> {
    return this.api.get<ProveedorDTO>(`Proveedor/${id}`);
  }

  create(data: ProveedorDTO): Observable<ProveedorDTO> {
    return this.api.post<ProveedorDTO>('Proveedor', data);
  }

  update(id: number, data: ProveedorDTO): Observable<ProveedorDTO> {
    return this.api.put<ProveedorDTO>(`Proveedor/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`Proveedor/${id}`);
  }
}
