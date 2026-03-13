import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ApiService } from './api.service';

export interface ProveedorDTO {
  id?: number;
  codigo: string;
  nombre: string;
  rfc?: string;
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
export class ProveedorService {
  private readonly endpoint = 'Proveedor';
  private readonly storageKey = 'gb_proveedores';

  constructor(private api: ApiService) {}

  getPaged(page: number = 1, pageSize: number = 50, term?: string): Observable<PagedResult<ProveedorDTO>> {
    let url = `${this.endpoint}/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;

    return this.api.get<PagedResult<ProveedorDTO>>(url).pipe(
      catchError(() => of(this.getLocalPaged(page, pageSize, term)))
    );
  }

  getAll(): Observable<ProveedorDTO[]> {
    return this.api.get<ProveedorDTO[]>(this.endpoint).pipe(
      catchError(() => of(this.getLocalItems()))
    );
  }

  getById(id: number): Observable<ProveedorDTO> {
    return this.api.get<ProveedorDTO>(`${this.endpoint}/${id}`).pipe(
      catchError(() => {
        const item = this.getLocalItems().find(x => x.id === id);
        return item ? of(item) : throwError(() => new Error('Proveedor no encontrado'));
      })
    );
  }

  create(dto: ProveedorDTO): Observable<ProveedorDTO> {
    return this.api.post<ProveedorDTO>(this.endpoint, dto).pipe(
      catchError(() => {
        const items = this.getLocalItems();
        const id = this.getNextId(items);
        const item: ProveedorDTO = { ...dto, id };
        items.push(item);
        this.setLocalItems(items);
        return of(item);
      })
    );
  }

  update(id: number, dto: ProveedorDTO): Observable<ProveedorDTO> {
    return this.api.put<ProveedorDTO>(`${this.endpoint}/${id}`, dto).pipe(
      catchError(() => {
        const items = this.getLocalItems();
        const index = items.findIndex(x => x.id === id);
        if (index === -1) {
          return throwError(() => new Error('Proveedor no encontrado'));
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

  private getLocalPaged(page: number, pageSize: number, term?: string): PagedResult<ProveedorDTO> {
    const filtered = this.filterByTerm(this.getLocalItems(), term);
    const start = (page - 1) * pageSize;
    return {
      items: filtered.slice(start, start + pageSize),
      total: filtered.length
    };
  }

  private filterByTerm(items: ProveedorDTO[], term?: string): ProveedorDTO[] {
    const text = (term || '').trim().toLowerCase();
    if (!text) return items;

    return items.filter(item =>
      [item.codigo, item.nombre, item.rfc, item.telefono, item.email, item.direccion]
        .some(value => (value ?? '').toString().toLowerCase().includes(text))
    );
  }

  private getLocalItems(): ProveedorDTO[] {
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

  private setLocalItems(items: ProveedorDTO[]): void {
    try {
      if (typeof localStorage === 'undefined') return;
      localStorage.setItem(this.storageKey, JSON.stringify(items));
    } catch {
      // ignore
    }
  }

  private getNextId(items: ProveedorDTO[]): number {
    return items.length > 0 ? Math.max(...items.map(x => x.id || 0)) + 1 : 1;
  }
}
