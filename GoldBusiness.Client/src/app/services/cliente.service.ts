import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ApiService } from './api.service';

export interface ClienteDTO {
  id?: number;
  codigo: string;
  nombre: string;
  telefono?: string;
  email?: string;
  direccion?: string;
  cancelado?: boolean;
  creadoPor?: string;
  fechaHoraCreado?: string;
  modificadoPor?: string;
  fechaHoraModificado?: string;
}

export interface PagedResult<T> {
  items: T[];
  total: number;
}

@Injectable({
  providedIn: 'root'
})
export class ClienteService {
  private readonly endpoint = 'Cliente';
  private readonly storageKey = 'gb_clientes';

  constructor(private api: ApiService) {}

  getPaged(page: number = 1, pageSize: number = 50, term?: string): Observable<PagedResult<ClienteDTO>> {
    let url = `${this.endpoint}/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;

    return this.api.get<PagedResult<ClienteDTO>>(url).pipe(
      catchError(() => of(this.getLocalPaged(page, pageSize, term)))
    );
  }

  getAll(): Observable<ClienteDTO[]> {
    return this.api.get<ClienteDTO[]>(this.endpoint).pipe(
      catchError(() => of(this.getLocalItems()))
    );
  }

  getById(id: number): Observable<ClienteDTO> {
    return this.api.get<ClienteDTO>(`${this.endpoint}/${id}`).pipe(
      catchError(() => {
        const item = this.getLocalItems().find(x => x.id === id);
        return item ? of(item) : throwError(() => new Error('Cliente no encontrado'));
      })
    );
  }

  create(dto: ClienteDTO): Observable<ClienteDTO> {
    return this.api.post<ClienteDTO>(this.endpoint, dto).pipe(
      catchError(() => {
        const items = this.getLocalItems();
        const id = this.getNextId(items);
        const item: ClienteDTO = { ...dto, id };
        items.push(item);
        this.setLocalItems(items);
        return of(item);
      })
    );
  }

  update(id: number, dto: ClienteDTO): Observable<ClienteDTO> {
    return this.api.put<ClienteDTO>(`${this.endpoint}/${id}`, dto).pipe(
      catchError(() => {
        const items = this.getLocalItems();
        const index = items.findIndex(x => x.id === id);
        if (index === -1) {
          return throwError(() => new Error('Cliente no encontrado'));
        }
        items[index] = { ...items[index], ...dto, id };
        this.setLocalItems(items);
        return of(items[index]);
      })
    );
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`${this.endpoint}/${id}`).pipe(
      catchError(() => {
        const items = this.getLocalItems().filter(x => x.id !== id);
        this.setLocalItems(items);
        return of(void 0);
      })
    );
  }

  private getLocalPaged(page: number, pageSize: number, term?: string): PagedResult<ClienteDTO> {
    const filtered = this.filterByTerm(this.getLocalItems(), term);
    const start = (page - 1) * pageSize;
    return {
      items: filtered.slice(start, start + pageSize),
      total: filtered.length
    };
  }

  private filterByTerm(items: ClienteDTO[], term?: string): ClienteDTO[] {
    const text = (term || '').trim().toLowerCase();
    if (!text) return items;

    return items.filter(item =>
      [item.codigo, item.nombre, item.telefono, item.email, item.direccion]
        .some(value => (value ?? '').toString().toLowerCase().includes(text))
    );
  }

  private getLocalItems(): ClienteDTO[] {
    try {
      if (typeof localStorage === 'undefined') return [];
      const raw = localStorage.getItem(this.storageKey);
      if (!raw) return [];
      const parsed = JSON.parse(raw);
      return Array.isArray(parsed) ? parsed : [];
    } catch {
      return [];
    }
  }

  private setLocalItems(items: ClienteDTO[]): void {
    try {
      if (typeof localStorage === 'undefined') return;
      localStorage.setItem(this.storageKey, JSON.stringify(items));
    } catch {
      // ignore
    }
  }

  private getNextId(items: ClienteDTO[]): number {
    return items.length > 0 ? Math.max(...items.map(x => x.id || 0)) + 1 : 1;
  }
}
