import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface LocalidadDTO {
  id?: number;
  codigo: string;
  descripcion: string;
  municipioId: number;
  municipioDescripcion?: string;
  activo?: boolean;
  fechaHoraCreado?: string;
  fechaHoraModificado?: string;
  creadoPor?: string;
  modificadoPor?: string;
}

@Injectable({
  providedIn: 'root'
})
export class LocalidadService {
  constructor(private api: ApiService) {}

  getAll(): Observable<LocalidadDTO[]> {
    return this.api.get<LocalidadDTO[]>('Localidad');
  }

  getById(id: number): Observable<LocalidadDTO> {
    return this.api.get<LocalidadDTO>(`Localidad/${id}`);
  }

  // MÉTODO AGREGADO para cascada
  getByMunicipioId(municipioId: number): Observable<LocalidadDTO[]> {
    return this.api.get<LocalidadDTO[]>(`Localidad/municipio/${municipioId}`);
  }

  create(data: LocalidadDTO): Observable<LocalidadDTO> {
    return this.api.post<LocalidadDTO>('Localidad', data);
  }

  update(id: number, data: LocalidadDTO): Observable<LocalidadDTO> {
    return this.api.put<LocalidadDTO>(`Localidad/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`Localidad/${id}`);
  }
}
