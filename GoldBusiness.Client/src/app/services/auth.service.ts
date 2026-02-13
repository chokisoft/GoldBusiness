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
   */
  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.apiService.post<LoginResponse>('auth/login', credentials)
      .pipe(
        tap(response => {
          if (response.succeeded && response.data) {
            this.storeAuthData(response.data);
            console.log('✅ Login exitoso:', response.data.user);
          }
        })
      );
  }

  /**
   * Logout del usuario
   */
  logout(): void {
    localStorage.removeItem('authToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
    console.log('👋 Sesión cerrada');
  }

  /**
   * Verificar si el usuario está autenticado
   */
  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) return false;

    // Verificar si el token ha expirado (opcional)
    return !this.isTokenExpired();
  }

  /**
   * Obtener el token de autenticación
   */
  getToken(): string | null {
    return localStorage.getItem('authToken');
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
   * Almacenar datos de autenticación en localStorage
   */
  private storeAuthData(data: any): void {
    localStorage.setItem('authToken', data.token);
    localStorage.setItem('refreshToken', data.refreshToken);
    localStorage.setItem('expiresAt', data.expiresAt);
    localStorage.setItem('currentUser', JSON.stringify(data.user));
    this.currentUserSubject.next(data.user);
  }

  /**
   * Cargar usuario desde localStorage al iniciar
   */
  private loadUserFromStorage(): void {
    try {
      const userStr = localStorage.getItem('currentUser');
      if (userStr) {
        const user = JSON.parse(userStr);
        this.currentUserSubject.next(user);
      }
    } catch (error) {
      console.error('❌ Error al cargar usuario desde localStorage:', error);
      this.logout();
    }
  }

  /**
   * Verificar si el token ha expirado
   */
  private isTokenExpired(): boolean {
    const expiresAt = localStorage.getItem('expiresAt');
    if (!expiresAt) return true;

    const expirationDate = new Date(expiresAt);
    return expirationDate <= new Date();
  }
}
