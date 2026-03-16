import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Cliente {
  id: number;
  codigo: string;
  descripcion: string;
  nif?: string;
  iban?: string;
  bicoSwift?: string;
  iva: number;
  direccion?: string;
  paisId?: number;
  paisDescripcion?: string;
  provinciaId?: number;
  provinciaDescripcion?: string;
  municipioId?: number;
  municipioDescripcion?: string;
  codigoPostalId?: number;
  codigoPostalCodigo?: string;
  web?: string;
  email1?: string;
  email2?: string;
  telefono1?: string;
  telefono2?: string;
  fax1?: string;
  fax2?: string;
  cancelado: boolean;
  creadoPor: string;
  fechaHoraCreado: Date;
  modificadoPor?: string;
  fechaHoraModificado?: Date;
}

@Injectable({
  providedIn: 'root'
})
export class ClienteService {
  private apiUrl = `${environment.apiUrl}/cliente`;

  constructor(private http: HttpClient) { }

  getAll(lang: string = 'es'): Observable<Cliente[]> {
    return this.http.get<Cliente[]>(this.apiUrl, {
      params: { lang }
    });
  }

  getById(id: number, lang: string = 'es'): Observable<Cliente> {
    return this.http.get<Cliente>(`${this.apiUrl}/${id}`, {
      params: { lang }
    });
  }

  create(cliente: Cliente, lang: string = 'es'): Observable<Cliente> {
    return this.http.post<Cliente>(this.apiUrl, cliente, {
      params: { lang }
    });
  }

  update(id: number, cliente: Cliente, lang: string = 'es'): Observable<Cliente> {
    return this.http.put<Cliente>(`${this.apiUrl}/${id}`, cliente, {
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

  getPaged(page: number, pageSize: number, search?: string, lang: string = 'es'): Observable<{ items: Cliente[]; total: number }> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('lang', lang);
    if (search) params = params.set('search', search);

    return this.http.get<{ items: Cliente[]; total: number }>(`${this.apiUrl}/paged`, { params });
  }
}
