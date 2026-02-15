import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface CuentaDTO {
  id?: number;
  codigo: string;
  descripcion: string;
  subGrupoCuentaId: number;
  systemConfigurationId?: number;
  cancelado?: boolean;
  creadoPor?: string;
  fechaHoraCreado?: string;
  modificadoPor?: string;
  fechaHoraModificado?: string;
  // Propiedades adicionales para visualización
  subGrupoCuentaCodigo?: string;
  subGrupoCuentaDescripcion?: string;
}

@Injectable({
  providedIn: 'root'
})
export class CuentaService {
  private readonly endpoint = 'Cuenta';

  constructor(private apiService: ApiService) {}

  getAll(): Observable<CuentaDTO[]> {
    console.log('📊 Obteniendo todas las cuentas...');
    return this.apiService.get<CuentaDTO[]>(this.endpoint);
  }

  getById(id: number): Observable<CuentaDTO> {
    console.log('📊 Obteniendo cuenta:', id);
    return this.apiService.get<CuentaDTO>(`${this.endpoint}/${id}`);
  }

  create(dto: CuentaDTO): Observable<CuentaDTO> {
    console.log('📊 Creando cuenta:', dto);
    return this.apiService.post<CuentaDTO>(this.endpoint, dto);
  }

  update(id: number, dto: CuentaDTO): Observable<CuentaDTO> {
    console.log('📊 Actualizando cuenta:', id, dto);
    return this.apiService.put<CuentaDTO>(`${this.endpoint}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    console.log('📊 Eliminando cuenta:', id);
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }
}
