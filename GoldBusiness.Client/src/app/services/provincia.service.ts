import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface ProvinciaDTO {
  id?: number;
  codigo: string;
  descripcion: string;  // ✅ CAMBIO: Solo 'descripcion', eliminado 'nombre'
  paisId?: number; // ✅ Vinculo al país
  paisDescripcion?: string; // Opcional: texto para mostrar el país en listas
  activo?: boolean;
  fechaHoraCreado?: string;
  fechaHoraModificado?: string;
  creadoPor?: string;
  modificadoPor?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ProvinciaService {
  constructor(private api: ApiService) { }

  getAll(): Observable<ProvinciaDTO[]> {
    return this.api.get<ProvinciaDTO[]>('Provincia');
  }

  getById(id: number): Observable<ProvinciaDTO> {
    return this.api.get<ProvinciaDTO>(`Provincia/${id}`);
  }

  create(data: ProvinciaDTO): Observable<ProvinciaDTO> {
    return this.api.post<ProvinciaDTO>('Provincia', data);
  }

  update(id: number, data: ProvinciaDTO): Observable<ProvinciaDTO> {
    return this.api.put<ProvinciaDTO>(`Provincia/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`Provincia/${id}`);
  }
}
