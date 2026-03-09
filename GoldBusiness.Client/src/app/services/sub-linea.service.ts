import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface SubLineaDTO {
  codigo?: string;
  descripcion?: string;
  lineaId: number;
  lineaDescripcion?: string;
  linea: any;
  id?: number;

  cancelado?: boolean;
  creadoPor?: string;
  fechaHoraCreado?: Date;
  modificadoPor?: string;
  fechaHoraModificado?: Date;
}

@Injectable({
  providedIn: 'root'
})
export class SubLineaService {
  private endpoint = 'subLinea';

  constructor(private apiService: ApiService) {}

  getAll(): Observable<SubLineaDTO[]> {
    return this.apiService.get<SubLineaDTO[]>(this.endpoint);
  }

  getById(id: number): Observable<SubLineaDTO> {
    return this.apiService.get<SubLineaDTO>(`${this.endpoint}/${id}`);
  }

  create(dto: SubLineaDTO): Observable<SubLineaDTO> {
    return this.apiService.post<SubLineaDTO>(this.endpoint, dto);
  }

  update(id: number, dto: SubLineaDTO): Observable<void> {
    return this.apiService.put<void>(`${this.endpoint}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }
}
