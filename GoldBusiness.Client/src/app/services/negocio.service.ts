import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface NegocioDTO {
  id: number;
  descripcion: string;
}

@Injectable({
  providedIn: 'root'
})
export class NegocioService {
  constructor(private api: ApiService) {}

  getAll(): Observable<NegocioDTO[]> {
    return this.api.get<NegocioDTO[]>('Negocio');
  }
}
