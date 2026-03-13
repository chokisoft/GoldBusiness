import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ApiService } from './api.service';

export interface ProductoDTO {
  id?: number;
  codigo: string;
  nombre: string;
  descripcion?: string;
  precio?: number;
  costo?: number;
  stock?: number;
  unidadMedida?: string;
  linea?: string;
  sublinea?: string;
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
export class ProductoService {
  private readonly endpoint = 'Producto';
  private readonly storageKey = 'gb_productos';

  constructor(private api: ApiService) {}

  getPaged(page: number = 1, pageSize: number = 50, term?: string): Observable<PagedResult<ProductoDTO>> {
    let url = `${this.endpoint}/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;

    return this.api.get<PagedResult<ProductoDTO>>(url).pipe(
      catchError(() => of(this.getLocalPaged(page, pageSize, term)))
    );
  }

  getAll(): Observable<ProductoDTO[]> {
    return this.api.get<ProductoDTO[]>(this.endpoint).pipe(
      catchError(() => of(this.getLocalItems()))
    );
  }

  getById(id: number): Observable<ProductoDTO> {
    return this.api.get<ProductoDTO>(`${this.endpoint}/${id}`).pipe(
      catchError(() => {
        const item = this.getLocalItems().find(x => x.id === id);
        return item ? of(item) : throwError(() => new Error('Producto no encontrado'));
      })
    );
  }

  create(dto: ProductoDTO): Observable<ProductoDTO> {
    return this.api.post<ProductoDTO>(this.endpoint, dto).pipe(
      catchError(() => {
        const items = this.getLocalItems();
        const id = this.getNextId(items);
        const item: ProductoDTO = { ...dto, id };
        items.push(item);
        this.setLocalItems(items);
        return of(item);
      })
    );
  }

  update(id: number, dto: ProductoDTO): Observable<ProductoDTO> {
    return this.api.put<ProductoDTO>(`${this.endpoint}/${id}`, dto).pipe(
      catchError(() => {
        const items = this.getLocalItems();
        const index = items.findIndex(x => x.id === id);
        if (index === -1) {
          return throwError(() => new Error('Producto no encontrado'));
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

  private getLocalPaged(page: number, pageSize: number, term?: string): PagedResult<ProductoDTO> {
    const filtered = this.filterByTerm(this.getLocalItems(), term);
    const start = (page - 1) * pageSize;
    return {
      items: filtered.slice(start, start + pageSize),
      total: filtered.length
    };
  }

  private filterByTerm(items: ProductoDTO[], term?: string): ProductoDTO[] {
    const text = (term || '').trim().toLowerCase();
    if (!text) return items;

    return items.filter(item =>
      [
        item.codigo,
        item.nombre,
        item.descripcion,
        item.unidadMedida,
        item.linea,
        item.sublinea,
        item.precio,
        item.costo,
        item.stock
      ].some(value => (value ?? '').toString().toLowerCase().includes(text))
    );
  }

  private getLocalItems(): ProductoDTO[] {
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

  private setLocalItems(items: ProductoDTO[]): void {
    try {
      if (typeof localStorage === 'undefined') return;
      localStorage.setItem(this.storageKey, JSON.stringify(items));
    } catch {
      // ignore
    }
  }

  private getNextId(items: ProductoDTO[]): number {
    return items.length > 0 ? Math.max(...items.map(x => x.id || 0)) + 1 : 1;
  }
}
