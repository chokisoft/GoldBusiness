import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface PaisDTO {
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
export class PaisService {
  constructor(private api: ApiService) {}

  getAll(): Observable<PaisDTO[]> {
    return this.api.get<PaisDTO[]>('Pais');
  }

  getById(id: number): Observable<PaisDTO> {
    return this.api.get<PaisDTO>(`Pais/${id}`);
  }

  create(data: PaisDTO): Observable<PaisDTO> {
    return this.api.post<PaisDTO>('Pais', data);
  }

  update(id: number, data: PaisDTO): Observable<PaisDTO> {
    return this.api.put<PaisDTO>(`Pais/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`Pais/${id}`);
  }
}
