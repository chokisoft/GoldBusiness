import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface GrupoCuentaDTO {
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
export class GrupoCuentaService {
  constructor(private api: ApiService) {}

  getAll(): Observable<GrupoCuentaDTO[]> {
    return this.api.get<GrupoCuentaDTO[]>('grupocuenta');
  }

  getById(id: number): Observable<GrupoCuentaDTO> {
    return this.api.get<GrupoCuentaDTO>(`grupocuenta/${id}`);
  }

  create(data: GrupoCuentaDTO): Observable<GrupoCuentaDTO> {
    return this.api.post<GrupoCuentaDTO>('grupocuenta', data);
  }

  update(id: number, data: GrupoCuentaDTO): Observable<GrupoCuentaDTO> {
    return this.api.put<GrupoCuentaDTO>(`grupocuenta/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`grupocuenta/${id}`);
  }
}
