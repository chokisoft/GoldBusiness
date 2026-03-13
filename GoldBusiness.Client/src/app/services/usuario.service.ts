import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { ApiService } from './api.service';

export interface UsuarioDTO {
  id?: string;
  userName: string;
  email: string;
  fullName: string;
  roles: string[];
  permissions: string[];
  accessLevels: string[];
  authProvider: 'Local' | 'Google';
  isActive: boolean;
  password?: string;
}

export interface PagedResult<T> {
  items: T[];
  total: number;
}

interface ApiResponse<T> {
  succeeded: boolean;
  message?: string;
  data: T;
}

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {
  private readonly storageKey = 'gb_usuarios';

  constructor(private apiService: ApiService) {}

  getRoles(): Observable<string[]> {
    return this.apiService.get<string[]>('auth/users/roles').pipe(
      catchError(() => of(['ADMINISTRADOR', 'DESARROLLADOR', 'ECONOMICO', 'CONTADOR']))
    );
  }

  getPermissions(): Observable<string[]> {
    return this.apiService.get<string[]>('auth/users/permissions').pipe(
      catchError(() => of(['ERP:FullAccess', 'ERP:AdminAccess', 'ERP:FinanceAccess', 'ERP:AccountingAccess']))
    );
  }

  getPaged(page: number, pageSize: number, term: string = ''): Observable<PagedResult<UsuarioDTO>> {
    const safePage = Math.max(1, page);
    const safeSize = Math.max(1, pageSize);
    const endpoint = `auth/users/paged?page=${safePage}&pageSize=${safeSize}&term=${encodeURIComponent(term)}`;

    return this.apiService.get<ApiResponse<PagedResult<UsuarioDTO>> | PagedResult<UsuarioDTO>>(endpoint).pipe(
      map((response) => {
        if ((response as ApiResponse<PagedResult<UsuarioDTO>>).data) {
          return (response as ApiResponse<PagedResult<UsuarioDTO>>).data;
        }
        return response as PagedResult<UsuarioDTO>;
      }),
      catchError(() => of(this.getPagedFromLocal(safePage, safeSize, term)))
    );
  }

  getById(id: string): Observable<UsuarioDTO> {
    return this.apiService.get<ApiResponse<UsuarioDTO> | UsuarioDTO>(`auth/users/${id}`).pipe(
      map((response) => {
        if ((response as ApiResponse<UsuarioDTO>).data) {
          return (response as ApiResponse<UsuarioDTO>).data;
        }
        return response as UsuarioDTO;
      }),
      catchError(() => {
        const item = this.getLocalItems().find(u => u.id === id);
        return of(item ?? this.emptyUser());
      })
    );
  }

  create(data: UsuarioDTO): Observable<UsuarioDTO> {
    const payload = {
      userName: data.userName,
      email: data.email,
      fullName: data.fullName,
      password: data.password,
      roles: data.roles,
      permissions: data.permissions,
      accessLevels: data.accessLevels,
      authProvider: data.authProvider,
      isActive: data.isActive
    };

    // Alta administrativa: NO usar registro público
    return this.apiService.post<ApiResponse<UsuarioDTO> | UsuarioDTO>('auth/users', payload).pipe(
      map((response) => {
        if ((response as ApiResponse<UsuarioDTO>).data) {
          return (response as ApiResponse<UsuarioDTO>).data;
        }
        return response as UsuarioDTO;
      }),
      catchError(() => {
        const items = this.getLocalItems();
        const newId = this.generateLocalId(items);
        const created: UsuarioDTO = { ...data, id: newId, password: undefined };
        this.saveLocalItems([...items, created]);
        return of(created);
      })
    );
  }

  update(id: string, data: UsuarioDTO): Observable<UsuarioDTO> {
    const payload = {
      userName: data.userName,
      email: data.email,
      fullName: data.fullName,
      roles: data.roles,
      permissions: data.permissions,
      accessLevels: data.accessLevels,
      authProvider: data.authProvider,
      isActive: data.isActive,
      password: data.password
    };

    return this.apiService.put<ApiResponse<UsuarioDTO> | UsuarioDTO>(`auth/users/${id}`, payload).pipe(
      map((response) => {
        if ((response as ApiResponse<UsuarioDTO>).data) {
          return (response as ApiResponse<UsuarioDTO>).data;
        }
        return response as UsuarioDTO;
      }),
      catchError(() => {
        const items = this.getLocalItems();
        const updated = items.map(u => u.id === id ? { ...u, ...data, id, password: undefined } : u);
        this.saveLocalItems(updated);
        const item = updated.find(u => u.id === id) ?? this.emptyUser();
        return of(item);
      })
    );
  }

  delete(id: string): Observable<void> {
    return this.apiService.delete<void>(`auth/users/${id}`).pipe(
      catchError(() => {
        const filtered = this.getLocalItems().filter(u => u.id !== id);
        this.saveLocalItems(filtered);
        return of(void 0);
      })
    );
  }

  private getPagedFromLocal(page: number, pageSize: number, term: string): PagedResult<UsuarioDTO> {
    const normalized = term.trim().toLowerCase();
    const filtered = this.getLocalItems().filter(item => {
      if (!normalized) return true;
      return item.userName.toLowerCase().includes(normalized)
        || item.email.toLowerCase().includes(normalized)
        || item.fullName.toLowerCase().includes(normalized);
    });

    const start = (page - 1) * pageSize;
    return {
      items: filtered.slice(start, start + pageSize),
      total: filtered.length
    };
  }

  private getLocalItems(): UsuarioDTO[] {
    const raw = localStorage.getItem(this.storageKey);
    if (!raw) return [];

    try {
      return JSON.parse(raw) as UsuarioDTO[];
    } catch {
      return [];
    }
  }

  private saveLocalItems(items: UsuarioDTO[]): void {
    localStorage.setItem(this.storageKey, JSON.stringify(items));
  }

  private generateLocalId(items: UsuarioDTO[]): string {
    return items.length
      ? (Math.max(...items.map(i => Number(i.id ?? '0'))) + 1).toString()
      : '1';
  }

  private emptyUser(): UsuarioDTO {
    return {
      userName: '',
      email: '',
      fullName: '',
      roles: [],
      permissions: [],
      accessLevels: ['*'],
      authProvider: 'Local',
      isActive: true
    };
  }
}
