import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface CodigoPostalDTO {
  id?: number;
  codigo: string;
  // No hay descripción en CodigoPostal; mantenemos codigo y relaciones
  municipioId?: number;
  municipioCodigo?: string;
  municipioDescripcion?: string;

  // Provincia (agregado)
  provinciaId?: number;
  provinciaCodigo?: string;
  provinciaDescripcion?: string;

  activo?: boolean;
  fechaHoraCreado?: string;
  fechaHoraModificado?: string;
  creadoPor?: string;
  modificadoPor?: string;
}

@Injectable({
  providedIn: 'root'
})
export class CodigoPostalService {
  constructor(private api: ApiService) { }

  getAll(): Observable<CodigoPostalDTO[]> {
    return this.api.get<CodigoPostalDTO[]>('CodigoPostal');
  }

  getPaged(page: number, pageSize: number, term?: string, municipioId?: number) {
    const params = new URLSearchParams();
    params.set('page', String(page));
    params.set('pageSize', String(pageSize));
    if (term) params.set('term', term);
    if (municipioId) params.set('municipioId', String(municipioId));
    const url = `CodigoPostal/paged?${params.toString()}`;
    return this.api.get<{ items: CodigoPostalDTO[]; total: number }>(url);
  }

  getById(id: number): Observable<CodigoPostalDTO> {
    return this.api.get<CodigoPostalDTO>(`CodigoPostal/${id}`);
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
}
