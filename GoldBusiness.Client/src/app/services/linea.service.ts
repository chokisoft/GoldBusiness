import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface LineaDTO {
  codigo?: string;
  descripcion?: string;
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
export class LineaService {
  private endpoint = 'linea';

  constructor(private apiService: ApiService) {}

  getAll(): Observable<LineaDTO[]> {
    return this.apiService.get<LineaDTO[]>(this.endpoint);
  }

  getById(id: number): Observable<LineaDTO> {
    return this.apiService.get<LineaDTO>(`${this.endpoint}/${id}`);
  }

  create(dto: LineaDTO): Observable<LineaDTO> {
    return this.apiService.post<LineaDTO>(this.endpoint, dto);
  }

  update(id: number, dto: LineaDTO): Observable<void> {
    return this.apiService.put<void>(`${this.endpoint}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }
}
