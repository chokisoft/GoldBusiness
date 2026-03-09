import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface EstablecimientoDTO {
  id?: number;
  codigo: string;
  descripcion: string;
  telefono?: string;
  email?: string;
  direccion?: string;
  localidadId?: number;
  localidadDescripcion?: string;
  activo?: boolean;
  fechaHoraCreado?: string;
  fechaHoraModificado?: string;
  creadoPor?: string;
  modificadoPor?: string;
}

@Injectable({
  providedIn: 'root'
})
export class EstablecimientoService {
  constructor(private api: ApiService) {}

  getAll(): Observable<EstablecimientoDTO[]> {
    return this.api.get<EstablecimientoDTO[]>('Establecimiento');
  }

  getById(id: number): Observable<EstablecimientoDTO> {
    return this.api.get<EstablecimientoDTO>(`Establecimiento/${id}`);
  }

  create(data: EstablecimientoDTO): Observable<EstablecimientoDTO> {
    return this.api.post<EstablecimientoDTO>('Establecimiento', data);
  }

  update(id: number, data: EstablecimientoDTO): Observable<EstablecimientoDTO> {
    return this.api.put<EstablecimientoDTO>(`Establecimiento/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`Establecimiento/${id}`);
  }
}
