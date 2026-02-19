import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface CuentaDTO {
  id?: number;
  codigo: string;
  descripcion: string;
  subGrupoCuentaId: number;
  systemConfigurationId: number;
  
  // ✅ AGREGAR: Propiedades calculadas del backend
  subGrupoCuentaCodigo?: string;
  subGrupoCuentaDescripcion?: string;
  grupoCuentaCodigo?: string;
  grupoCuentaDescripcion?: string;
  
  cancelado?: boolean;
  creadoPor?: string;
  fechaHoraCreado?: Date;
  modificadoPor?: string;
  fechaHoraModificado?: Date;
}

@Injectable({
  providedIn: 'root'
})
export class CuentaService {
  private endpoint = 'cuenta';

  constructor(private apiService: ApiService) {}

  getAll(): Observable<CuentaDTO[]> {
    return this.apiService.get<CuentaDTO[]>(this.endpoint);
  }

  getById(id: number): Observable<CuentaDTO> {
    return this.apiService.get<CuentaDTO>(`${this.endpoint}/${id}`);
  }

  create(dto: CuentaDTO): Observable<CuentaDTO> {
    return this.apiService.post<CuentaDTO>(this.endpoint, dto);
  }

  update(id: number, dto: CuentaDTO): Observable<void> {
    return this.apiService.put<void>(`${this.endpoint}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }
}
