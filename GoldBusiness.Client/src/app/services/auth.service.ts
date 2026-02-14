import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { Router } from '@angular/router';

/**
 * Interface para la petición de login
 */
export interface LoginRequest {
  username: string;
  password: string;
}

/**
 * Interface para la respuesta de login
 */
export interface LoginResponse {
  succeeded: boolean;
  message: string;
  data: {
    token: string;
    refreshToken: string;
    expiresAt: string;
    user: {
      userName: string;
      email: string;
      fullName: string;
      roles: string[];
    };
  };
}

/**
 * Interface para el usuario actual
 */
export interface CurrentUser {
  userName: string;
  email: string;
  fullName: string;
  roles: string[];
}

/**
 * Servicio de autenticación con JWT
 * Usa sessionStorage para que la sesión expire al cerrar el navegador
 */
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<CurrentUser | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private apiService: ApiService,
    private router: Router
  ) {
    this.loadUserFromStorage();
  }

  /**
   * Login de usuario
   * Los datos se guardan en sessionStorage (expira al cerrar navegador)
   */
  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.apiService.post<LoginResponse>('auth/login', credentials)
      .pipe(
        tap(response => {
          if (response.succeeded && response.data) {
            this.storeAuthData(response.data);
            console.log('✅ Login exitoso:', response.data.user);
            console.log('💾 Datos guardados en sessionStorage (expira al cerrar navegador)');
          }
        })
      );
  }

  /**
   * Logout del usuario
   * Limpia sessionStorage y localStorage por seguridad
   */
  logout(): void {
    // Limpiar sessionStorage
    sessionStorage.removeItem('authToken');
    sessionStorage.removeItem('refreshToken');
    sessionStorage.removeItem('currentUser');
    sessionStorage.removeItem('expiresAt');

    // Limpiar localStorage también por seguridad (por si había datos antiguos)
    localStorage.removeItem('authToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('currentUser');
    localStorage.removeItem('expiresAt');

    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
    console.log('👋 Sesión cerrada completamente');
  }

  /**
   * Verificar si el usuario está autenticado
   */
  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) {
      console.log('❌ No hay token disponible');
      return false;
    }

    // Verificar si el token ha expirado
    const expired = this.isTokenExpired();
    if (expired) {
      console.log('⏰ Token expirado');
      this.logout();
      return false;
    }

    return true;
  }

  /**
   * Obtener el token de autenticación desde sessionStorage
   */
  getToken(): string | null {
    return sessionStorage.getItem('authToken');
  }

  /**
   * Obtener el usuario actual
   */
  getCurrentUser(): CurrentUser | null {
    return this.currentUserSubject.value;
  }

  /**
   * Verificar si el usuario tiene un rol específico
   */
  hasRole(role: string): boolean {
    const user = this.getCurrentUser();
    return user?.roles.includes(role) || false;
  }

  /**
   * Verificar si el usuario tiene alguno de los roles especificados
   */
  hasAnyRole(roles: string[]): boolean {
    const user = this.getCurrentUser();
    if (!user) return false;
    return roles.some(role => user.roles.includes(role));
  }

  /**
   * Verificar si el usuario tiene todos los roles especificados
   */
  hasAllRoles(roles: string[]): boolean {
    const user = this.getCurrentUser();
    if (!user) return false;
    return roles.every(role => user.roles.includes(role));
  }

  /**
   * Almacenar datos de autenticación en sessionStorage
   */
  private storeAuthData(data: any): void {
    sessionStorage.setItem('authToken', data.token);
    sessionStorage.setItem('refreshToken', data.refreshToken);
    sessionStorage.setItem('expiresAt', data.expiresAt);
    sessionStorage.setItem('currentUser', JSON.stringify(data.user));
    this.currentUserSubject.next(data.user);

    console.log('💾 Datos de autenticación guardados en sessionStorage');
    console.log('⏰ Token expira en:', data.expiresAt);
  }

  /**
   * Cargar usuario desde sessionStorage al iniciar
   * Si no hay datos, el usuario no está autenticado
   */
  private loadUserFromStorage(): void {
    try {
      const userStr = sessionStorage.getItem('currentUser');
      if (userStr) {
        const user = JSON.parse(userStr);

        // Verificar que el token no haya expirado
        if (!this.isTokenExpired()) {
          this.currentUserSubject.next(user);
          console.log('📦 Usuario restaurado desde sessionStorage:', user.userName);
        } else {
          console.log('⏰ Sesión expirada al cargar');
          this.logout();
        }
      } else {
        console.log('ℹ️ No hay sesión previa');
      }
    } catch (error) {
      console.error('❌ Error al cargar usuario desde sessionStorage:', error);
      this.logout();
    }
  }

  /**
   * Verificar si el token ha expirado
   */
  private isTokenExpired(): boolean {
    const expiresAt = sessionStorage.getItem('expiresAt');
    if (!expiresAt) return true;

    try {
      const expirationDate = new Date(expiresAt);
      const now = new Date();
      const isExpired = expirationDate <= now;

      if (isExpired) {
        console.log('⏰ Token expirado:', {
          expiresAt: expirationDate.toISOString(),
          now: now.toISOString()
        });
      }

      return isExpired;
    } catch (error) {
      console.error('❌ Error al verificar expiración del token:', error);
      return true;
    }
  }

  /**
   * Obtener tiempo restante del token en minutos
   */
  getTokenRemainingTime(): number {
    const expiresAt = sessionStorage.getItem('expiresAt');
    if (!expiresAt) return 0;

    try {
      const expirationDate = new Date(expiresAt);
      const now = new Date();
      const diffMs = expirationDate.getTime() - now.getTime();
      const diffMinutes = Math.floor(diffMs / 60000);

      return diffMinutes > 0 ? diffMinutes : 0;
    } catch {
      return 0;
    }
  }
}
