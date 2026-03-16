import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Establecimiento {
  id: number;
  codigo: string;
  descripcion: string;
  negocioId: number;
  negocioDescripcion?: string;
  direccion?: string;
  telefono?: string;
  paisId?: number;
  paisDescripcion?: string;
  provinciaId?: number;
  provinciaDescripcion?: string;
  municipioId?: number;
  municipioDescripcion?: string;
  codigoPostalId?: number;
  codigoPostalCodigo?: string;
  activo: boolean;
  cancelado: boolean;
  creadoPor: string;
  fechaHoraCreado: Date;
  modificadoPor?: string;
  fechaHoraModificado?: Date;
}

export interface PagedResult<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class EstablecimientoService {
  private apiUrl = `${environment.apiUrl}/establecimiento`;

  constructor(private http: HttpClient) { }

  getAll(lang: string = 'es'): Observable<Establecimiento[]> {
    return this.http.get<Establecimiento[]>(this.apiUrl, {
      params: { lang }
    });
  }

  getPaged(page: number, pageSize: number, termino?: string, negocioId?: number, lang: string = 'es'): Observable<PagedResult<Establecimiento>> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('lang', lang);

    if (termino) {
      // Controller expects 'term'
      params = params.set('term', termino);
    }
    if (negocioId) {
      params = params.set('negocioId', negocioId.toString());
    }

    return this.http.get<PagedResult<Establecimiento>>(`${this.apiUrl}/paged`, { params });
  }

  getByNegocioId(negocioId: number, lang: string = 'es'): Observable<Establecimiento[]> {
    return this.http.get<Establecimiento[]>(`${this.apiUrl}/negocio/${negocioId}`, {
      params: { lang }
    });
  }

  getById(id: number, lang: string = 'es'): Observable<Establecimiento> {
    return this.http.get<Establecimiento>(`${this.apiUrl}/${id}`, {
      params: { lang }
    });
  }

  create(establecimiento: Establecimiento, lang: string = 'es'): Observable<Establecimiento> {
    return this.http.post<Establecimiento>(this.apiUrl, establecimiento, {
      params: { lang }
    });
  }

  update(id: number, establecimiento: Establecimiento, lang: string = 'es'): Observable<Establecimiento> {
    return this.http.put<Establecimiento>(`${this.apiUrl}/${id}`, establecimiento, {
      params: { lang }
    });
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  addOrUpdateTranslation(id: number, lang: string, descripcion: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/translation`, null, {
      params: { lang, descripcion }
    });
  }
}
