import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface FormaJuridicaDTO {
  id: number;
  descripcion: string;
}

@Injectable({
  providedIn: 'root'
})
export class FormaJuridicaService {
  constructor(private api: ApiService) {}

  getAll(): Observable<FormaJuridicaDTO[]> {
    return this.api.get<FormaJuridicaDTO[]>('FormaJuridica');
  }

  getById(id: number): Observable<FormaJuridicaDTO> {
    return this.api.get<FormaJuridicaDTO>(`FormaJuridica/${id}`);
  }
}
