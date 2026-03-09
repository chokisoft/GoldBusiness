import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface CodigoPostalDTO {
  id?: number;
  codigo: string;
  descripcion?: string;
  municipioId: number;
  municipioDescripcion?: string;
  provinciaDescripcion?: string;
  paisDescripcion?: string;
  activo?: boolean;
  fechaHoraCreado?: string;
  fechaHoraModificado?: string;
  creadoPor?: string;
  modificadoPor?: string;
  municipio?: {
    id: number;
    provinciaId: number;
    descripcion: string;
    provincia?: {
      id: number;
      paisId: number;
      descripcion: string;
    };
  };
}

@Injectable({
  providedIn: 'root'
})
export class CodigoPostalService {
  constructor(private api: ApiService) {}

  getAll(): Observable<CodigoPostalDTO[]> {
    return this.api.get<CodigoPostalDTO[]>('CodigoPostal');
  }

  getById(id: number): Observable<CodigoPostalDTO> {
    return this.api.get<CodigoPostalDTO>(`CodigoPostal/${id}`);
  }

  create(data: CodigoPostalDTO): Observable<CodigoPostalDTO> {
    return this.api.post<CodigoPostalDTO>('CodigoPostal', data);
  }

  update(id: number, data: CodigoPostalDTO): Observable<CodigoPostalDTO> {
    return this.api.put<CodigoPostalDTO>(`CodigoPostal/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`CodigoPostal/${id}`);
  }
}
