import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface TransaccionDTO {
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
export class TransaccionService {
  private endpoint = 'transaccion';

  constructor(private apiService: ApiService) {}

  getAll(): Observable<TransaccionDTO[]> {
    return this.apiService.get<TransaccionDTO[]>(this.endpoint);
  }

  getById(id: number): Observable<TransaccionDTO> {
    return this.apiService.get<TransaccionDTO>(`${this.endpoint}/${id}`);
  }

  create(dto: TransaccionDTO): Observable<TransaccionDTO> {
    return this.apiService.post<TransaccionDTO>(this.endpoint, dto);
  }

  update(id: number, dto: TransaccionDTO): Observable<void> {
    return this.apiService.put<void>(`${this.endpoint}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }
}
