import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface MonedaDTO {
  id?: number;
  codigo: string;
  descripcion: string;  // ✅ CAMBIO: Solo 'descripcion', eliminado 'nombre'
  activo?: boolean;
  fechaHoraCreado?: string;
  fechaHoraModificado?: string;
  creadoPor?: string;
  modificadoPor?: string;
}

@Injectable({
  providedIn: 'root'
})
export class MonedaService {
  constructor(private api: ApiService) {}

  getAll(): Observable<MonedaDTO[]> {
    return this.api.get<MonedaDTO[]>('Moneda');
  }

  getById(id: number): Observable<MonedaDTO> {
    return this.api.get<MonedaDTO>(`Moneda/${id}`);
  }

  create(data: MonedaDTO): Observable<MonedaDTO> {
    return this.api.post<MonedaDTO>('Moneda', data);
  }

  update(id: number, data: MonedaDTO): Observable<MonedaDTO> {
    return this.api.put<MonedaDTO>(`Moneda/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`Moneda/${id}`);
  }
}
