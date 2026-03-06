import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface MunicipioDTO {
  id?: number;
  codigo: string;
  descripcion: string;  // ✅ CAMBIO: Solo 'descripcion', eliminado 'nombre'
  provinciaId?: number; // ✅ Vinculo al provincia
  provinciaDescripcion?: string; // Opcional: texto para mostrar el provincia en listas
  activo?: boolean;
  fechaHoraCreado?: string;
  fechaHoraModificado?: string;
  creadoPor?: string;
  modificadoPor?: string;
}

@Injectable({
  providedIn: 'root'
})
export class MunicipioService {
  constructor(private api: ApiService) { }

  getAll(): Observable<MunicipioDTO[]> {
    return this.api.get<MunicipioDTO[]>('Municipio');
  }

  getById(id: number): Observable<MunicipioDTO> {
    return this.api.get<MunicipioDTO>(`Municipio/${id}`);
  }

  create(data: MunicipioDTO): Observable<MunicipioDTO> {
    return this.api.post<MunicipioDTO>('Municipio', data);
  }

  update(id: number, data: MunicipioDTO): Observable<MunicipioDTO> {
    return this.api.put<MunicipioDTO>(`Municipio/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`Municipio/${id}`);
  }
}
