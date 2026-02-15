import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface GrupoCuentaDTO {
  id?: number;
  codigo: string;
  nombre: string;
  descripcion?: string;
  activo?: boolean;
  fechaHoraCreado?: string;
  fechaHoraModificado?: string;
  creadoPor?: string;
  modificadoPor?: string;
}

@Injectable({
  providedIn: 'root'
})
export class GrupoCuentaService {
  private readonly endpoint = 'GrupoCuenta';

  constructor(private apiService: ApiService) {}

  getAll(): Observable<GrupoCuentaDTO[]> {
    console.log('📊 Obteniendo todos los grupos de cuenta...');
    return this.apiService.get<GrupoCuentaDTO[]>(this.endpoint);
  }

  getById(id: number): Observable<GrupoCuentaDTO> {
    console.log('📊 Obteniendo grupo de cuenta:', id);
    return this.apiService.get<GrupoCuentaDTO>(`${this.endpoint}/${id}`);
  }

  create(dto: GrupoCuentaDTO): Observable<GrupoCuentaDTO> {
    console.log('📊 Creando grupo de cuenta:', dto);
    return this.apiService.post<GrupoCuentaDTO>(this.endpoint, dto);
  }

  update(id: number, dto: GrupoCuentaDTO): Observable<GrupoCuentaDTO> {
    console.log('📊 Actualizando grupo de cuenta:', id, dto);
    return this.apiService.put<GrupoCuentaDTO>(`${this.endpoint}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    console.log('📊 Eliminando grupo de cuenta:', id);
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }
}
