import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface ConceptoAjusteDTO {
  codigo?: string;
  descripcion?: string;
  cuentaId: number;
  cuenta: any;
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
export class ConceptoAjusteService {
  private endpoint = 'conceptoAjuste';

  constructor(private apiService: ApiService) {}

  getAll(): Observable<ConceptoAjusteDTO[]> {
    return this.apiService.get<ConceptoAjusteDTO[]>(this.endpoint);
  }

  getById(id: number): Observable<ConceptoAjusteDTO> {
    return this.apiService.get<ConceptoAjusteDTO>(`${this.endpoint}/${id}`);
  }

  create(dto: ConceptoAjusteDTO): Observable<ConceptoAjusteDTO> {
    return this.apiService.post<ConceptoAjusteDTO>(this.endpoint, dto);
  }

  update(id: number, dto: ConceptoAjusteDTO): Observable<void> {
    return this.apiService.put<void>(`${this.endpoint}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }
}
