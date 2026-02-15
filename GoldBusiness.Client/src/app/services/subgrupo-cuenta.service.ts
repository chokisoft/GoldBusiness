import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface SubGrupoCuentaDTO {
  id?: number;
  codigo: string;
  grupoCuentaId: number;
  descripcion: string;
  deudora: boolean;
  cancelado?: boolean;
  creadoPor?: string;
  fechaHoraCreado?: string;
  modificadoPor?: string;
  fechaHoraModificado?: string;
  // Propiedades adicionales para visualización
  grupoCuentaCodigo?: string;
  grupoCuentaDescripcion?: string;
}

@Injectable({
  providedIn: 'root'
})
export class SubGrupoCuentaService {
  private readonly endpoint = 'SubGrupoCuenta';

  constructor(private apiService: ApiService) {}

  getAll(): Observable<SubGrupoCuentaDTO[]> {
    console.log('📊 Obteniendo todos los subgrupos de cuenta...');
    return this.apiService.get<SubGrupoCuentaDTO[]>(this.endpoint);
  }

  getById(id: number): Observable<SubGrupoCuentaDTO> {
    console.log('📊 Obteniendo subgrupo de cuenta:', id);
    return this.apiService.get<SubGrupoCuentaDTO>(`${this.endpoint}/${id}`);
  }

  create(dto: SubGrupoCuentaDTO): Observable<SubGrupoCuentaDTO> {
    console.log('📊 Creando subgrupo de cuenta:', dto);
    return this.apiService.post<SubGrupoCuentaDTO>(this.endpoint, dto);
  }

  update(id: number, dto: SubGrupoCuentaDTO): Observable<SubGrupoCuentaDTO> {
    console.log('📊 Actualizando subgrupo de cuenta:', id, dto);
    return this.apiService.put<SubGrupoCuentaDTO>(`${this.endpoint}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    console.log('📊 Eliminando subgrupo de cuenta:', id);
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }
}
