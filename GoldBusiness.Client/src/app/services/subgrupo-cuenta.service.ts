import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface SubGrupoCuentaDTO {
  id?: number;
  codigo: string;
  descripcion: string;
  grupoCuentaId: number;
  deudora: boolean;
  
  // ✅ AGREGAR: Propiedades calculadas del backend
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
export class SubGrupoCuentaService {
  private endpoint = 'subgrupocuenta';

  constructor(private apiService: ApiService) {}

  getAll(): Observable<SubGrupoCuentaDTO[]> {
    return this.apiService.get<SubGrupoCuentaDTO[]>(this.endpoint);
  }

  getById(id: number): Observable<SubGrupoCuentaDTO> {
    return this.apiService.get<SubGrupoCuentaDTO>(`${this.endpoint}/${id}`);
  }

  create(dto: SubGrupoCuentaDTO): Observable<SubGrupoCuentaDTO> {
    return this.apiService.post<SubGrupoCuentaDTO>(this.endpoint, dto);
  }

  update(id: number, dto: SubGrupoCuentaDTO): Observable<void> {
    return this.apiService.put<void>(`${this.endpoint}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }
}
