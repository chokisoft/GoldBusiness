import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { TipoIdentificacionFiscal, RegimenFiscal } from './fiscal.types';

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
export class ClienteService {
  constructor(private api: ApiService) { }

  getPaged(page: number = 1, pageSize: number = 50, term?: string): Observable<PagedResult<ClienteDTO>> {
    let url = `Cliente/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;
    return this.api.get<PagedResult<ClienteDTO>>(url);
  }

  getById(id: number): Observable<ClienteDTO> {
    return this.api.get<ClienteDTO>(`Cliente/${id}`);
  }

  create(cliente: Partial<ClienteDTO>): Observable<ClienteDTO> {
    return this.api.post<ClienteDTO>('Cliente', cliente);
  }

  update(id: number, cliente: Partial<ClienteDTO>): Observable<ClienteDTO> {
    return this.api.put<ClienteDTO>(`Cliente/${id}`, cliente);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`Cliente/${id}`);
  }

  reactivate(id: number): Observable<ClienteDTO> {
    return this.api.post<ClienteDTO>(`Cliente/${id}/reactivate`, {});
  }
}
