import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { TipoIdentificacionFiscal, RegimenFiscal } from './fiscal.types';

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
  
  // ═══════════════════════════════════════════════════════════════
  // 🧾 DATOS FISCALES EXTENDIDOS
  // ═══════════════════════════════════════════════════════════════
  tipoIdentificadorFiscal?: TipoIdentificacionFiscal;
  regimenFiscal?: RegimenFiscal;
  exentoIva?: boolean;
  extranjero?: boolean;
  codigoPaisIso?: string;
  validarIdentificadorFiscal?: boolean;
  inversionSujetoPasivo?: boolean;
  
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
export class ProveedorService {
  constructor(private api: ApiService) { }

  getPaged(page: number = 1, pageSize: number = 50, term?: string): Observable<PagedResult<ProveedorDTO>> {
    let url = `Proveedor/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;
    return this.api.get<PagedResult<ProveedorDTO>>(url);
  }

  getById(id: number): Observable<ProveedorDTO> {
    return this.api.get<ProveedorDTO>(`Proveedor/${id}`);
  }

  create(proveedor: Partial<ProveedorDTO>): Observable<ProveedorDTO> {
    return this.api.post<ProveedorDTO>('Proveedor', proveedor);
  }

  update(id: number, proveedor: Partial<ProveedorDTO>): Observable<ProveedorDTO> {
    return this.api.put<ProveedorDTO>(`Proveedor/${id}`, proveedor);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`Proveedor/${id}`);
  }

  reactivate(id: number): Observable<ProveedorDTO> {
    return this.api.post<ProveedorDTO>(`Proveedor/${id}/reactivate`, {});
  }
}
