import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable, BehaviorSubject, tap, catchError, of, throwError } from 'rxjs';
import { Router } from '@angular/router';

export interface LoginRequest {
  username: string;
  password: string;
}

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

export interface CurrentUser {
  userName: string;
  email: string;
  fullName: string;
  roles: string[];
}

/**
 * Servicio de autenticación con Session Management
 * - localStorage: Compartir entre pestañas
 * - sessionStorage: Detectar cierre de navegador
 * - Browser Open Flag: Detectar si el navegador está abierto
 */
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<CurrentUser | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  private authChannel: BroadcastChannel | null = null;
  private inactivityTimer: any = null;
  private readonly INACTIVITY_TIMEOUT = 30 * 60 * 1000; // 30 minutos
  private readonly SESSION_KEY = 'browser_session_id';
  private readonly LAST_ACTIVITY_KEY = 'last_activity';
  private readonly BROWSER_OPEN_FLAG = 'browser_open_flag'; // ✅ NUEVO

  constructor(
    private apiService: ApiService,
    private router: Router
  ) {
    this.initializeAuthChannel();
    this.initializeSessionManagement();
    this.loadUserFromStorage();
    this.setupActivityListeners();
    this.setupBrowserCloseDetection(); // ✅ NUEVO
  }

  /**
   * ✅ NUEVO: Detectar cuando se cierra el navegador
   */
  private setupBrowserCloseDetection(): void {
    // Incrementar contador de pestañas abiertas
    const openTabs = parseInt(sessionStorage.getItem('open_tabs_count') || '0');
    sessionStorage.setItem('open_tabs_count', (openTabs + 1).toString());

    // Marcar que el navegador está abierto
    localStorage.setItem(this.BROWSER_OPEN_FLAG, 'true');

    // Al cerrar la pestaña
    window.addEventListener('beforeunload', () => {
      const currentTabs = parseInt(sessionStorage.getItem('open_tabs_count') || '1');
      const newTabCount = currentTabs - 1;

      if (newTabCount <= 0) {
        // Última pestaña cerrándose, marcar navegador como cerrado
        console.log('🚪 Última pestaña cerrándose, limpiando flag de navegador');
        localStorage.removeItem(this.BROWSER_OPEN_FLAG);
      } else {
        sessionStorage.setItem('open_tabs_count', newTabCount.toString());
      }
    });
  }

  /**
   * ✅ CORREGIDO: Gestión inteligente de sesión de navegador
   */
  private initializeSessionManagement(): void {
    const sessionId = sessionStorage.getItem(this.SESSION_KEY);
    const hasTokens = localStorage.getItem('authToken');
    const browserWasOpen = localStorage.getItem(this.BROWSER_OPEN_FLAG);
    const lastActivity = localStorage.getItem(this.LAST_ACTIVITY_KEY);

    if (!sessionId) {
      // No hay SESSION_KEY en esta pestaña

      if (hasTokens) {
        // Hay tokens en localStorage

        // ✅ CLAVE: Verificar si el navegador estaba abierto
        if (browserWasOpen === 'true') {
          // El navegador ESTABA abierto, es una nueva pestaña
          console.log('📄 Nueva pestaña detectada (navegador estaba abierto)');
          const newSessionId = this.generateSessionId();
          sessionStorage.setItem(this.SESSION_KEY, newSessionId);
          sessionStorage.setItem('open_tabs_count', '1');
          this.updateLastActivity();

          // Notificar a otras pestañas
          this.authChannel?.postMessage({
            type: 'tab_opened',
            sessionId: newSessionId
          });
        } else {
          // El navegador se CERRÓ completamente
          console.log('🔒 Navegador fue cerrado, requiere nuevo login');
          this.clearAuthData();

          // Crear nuevo session ID para esta sesión limpia
          const newSessionId = this.generateSessionId();
          sessionStorage.setItem(this.SESSION_KEY, newSessionId);
        }
      } else {
        // No hay tokens, nueva sesión limpia
        console.log('🆕 Nueva sesión de navegador (sin tokens previos)');
        const newSessionId = this.generateSessionId();
        sessionStorage.setItem(this.SESSION_KEY, newSessionId);
      }
    } else {
      // Ya existe SESSION_KEY (recarga de página o navegación)
      console.log('✅ Sesión de navegador activa:', sessionId);
      this.validateSessionActivity();
    }
  }

  /**
   * Validar actividad de la sesión
   */
  private validateSessionActivity(): void {
    const lastActivity = localStorage.getItem(this.LAST_ACTIVITY_KEY);

    if (lastActivity) {
      const lastActivityTime = new Date(lastActivity).getTime();
      const now = Date.now();
      const timeDiff = now - lastActivityTime;

      // 30 minutos de inactividad → cierra sesión
      if (timeDiff > this.INACTIVITY_TIMEOUT) {
        console.log('⏰ Sesión expirada por inactividad (30 min)');
        this.clearAuthData();
      } else {
        // Actualizar última actividad
        this.updateLastActivity();
      }
    } else {
      // No hay registro de actividad, crear uno
      this.updateLastActivity();
    }
  }

  /**
   * Actualizar timestamp de última actividad
   */
  private updateLastActivity(): void {
    localStorage.setItem(this.LAST_ACTIVITY_KEY, new Date().toISOString());
  }

  /**
   * Generar ID único de sesión de navegador
   */
  private generateSessionId(): string {
    return `session_${Date.now()}_${Math.random().toString(36).substring(2, 9)}`;
  }

  /**
   * Inicializar BroadcastChannel
   */
  private initializeAuthChannel(): void {
    if (typeof BroadcastChannel !== 'undefined') {
      this.authChannel = new BroadcastChannel('auth_channel');
      this.authChannel.onmessage = (event) => {
        if (event.data.type === 'logout') {
          console.log('📡 Logout desde otra pestaña');
          this.performLogout(false);
        } else if (event.data.type === 'login') {
          console.log('📡 Login desde otra pestaña');
          this.loadUserFromStorage();
        } else if (event.data.type === 'activity') {
          // Actualizar actividad desde otras pestañas
          this.updateLastActivity();
        } else if (event.data.type === 'tab_opened') {
          // Nueva pestaña abierta, actualizar actividad
          console.log('📄 Nueva pestaña abierta:', event.data.sessionId);
          this.updateLastActivity();
        }
      };
    }
  }

  /**
   * Configurar listeners de actividad
   */
  private setupActivityListeners(): void {
    const events = ['mousedown', 'keydown', 'scroll', 'touchstart'];

    events.forEach(event => {
      document.addEventListener(event, () => {
        this.resetInactivityTimer();
        this.updateLastActivity();
        // Notificar actividad a otras pestañas
        this.authChannel?.postMessage({ type: 'activity' });
      }, true);
    });

    this.resetInactivityTimer();
  }

  /**
   * Reiniciar timer de inactividad
   */
  private resetInactivityTimer(): void {
    if (this.inactivityTimer) {
      clearTimeout(this.inactivityTimer);
    }

    if (this.isAuthenticated()) {
      this.inactivityTimer = setTimeout(() => {
        console.log('⏰ Sesión cerrada por inactividad (30 min)');
        this.logout();
      }, this.INACTIVITY_TIMEOUT);
    }
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

            // ✅ Crear SESSION_KEY y marcar navegador abierto
            const sessionId = this.generateSessionId();
            sessionStorage.setItem(this.SESSION_KEY, sessionId);
            sessionStorage.setItem('open_tabs_count', '1');
            localStorage.setItem(this.BROWSER_OPEN_FLAG, 'true'); // ✅ NUEVO

            this.updateLastActivity();
            this.resetInactivityTimer();
            this.authChannel?.postMessage({ type: 'login' });

            console.log('✅ Login exitoso:', response.data.user);
            console.log('🔑 SESSION_KEY creado:', sessionId);
          }
        })
      );
  }

  /**
   * Renovar token
   */
  refreshToken(): Observable<LoginResponse> {
    const refreshToken = localStorage.getItem('refreshToken');

    if (!refreshToken) {
      console.warn('⚠️ No hay refresh token');
      return of({
        succeeded: false,
        message: 'No hay refresh token',
        data: null as any
      });
    }

    console.log('🔄 Renovando token...');

    return this.apiService.post<LoginResponse>('auth/refresh', { refreshToken })
      .pipe(
        tap(response => {
          if (response.succeeded && response.data) {
            this.storeAuthData(response.data);
            this.updateLastActivity();
            console.log('✅ Token renovado');
          }
        }),
        catchError(error => {
          console.error('❌ Error al renovar token:', error);
          this.logout();
          return throwError(() => error);
        })
      );
  }

  /**
   * Logout del usuario
   */
  logout(): void {
    const refreshToken = localStorage.getItem('refreshToken');

    if (refreshToken) {
      this.apiService.post('auth/revoke', { refreshToken })
        .subscribe({
          next: () => console.log('✅ Token revocado'),
          error: (err) => console.warn('⚠️ Error al revocar:', err)
        });
    }

    this.performLogout(true);
  }

  /**
   * Cerrar todas las sesiones
   */
  logoutAllDevices(): Observable<any> {
    return this.apiService.post('auth/revoke-all', {}).pipe(
      tap(() => {
        console.log('✅ Todas las sesiones cerradas');
        this.performLogout(true);
      })
    );
  }

  /**
   * Realizar logout
   */
  private performLogout(broadcast: boolean = true): void {
    this.clearAuthData();

    this.currentUserSubject.next(null);

    if (this.inactivityTimer) {
      clearTimeout(this.inactivityTimer);
      this.inactivityTimer = null;
    }

    if (broadcast) {
      this.authChannel?.postMessage({ type: 'logout' });
    }

    this.router.navigate(['/login']);
    console.log('👋 Sesión cerrada');
  }

  /**
   * Limpiar todos los datos de autenticación
   */
  private clearAuthData(): void {
    // Limpiar localStorage
    localStorage.removeItem('authToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('currentUser');
    localStorage.removeItem('expiresAt');
    localStorage.removeItem(this.LAST_ACTIVITY_KEY);
    // ✅ NO limpiar BROWSER_OPEN_FLAG aquí (se limpia en beforeunload)

    // Limpiar sessionStorage
    sessionStorage.removeItem('authToken');
    sessionStorage.removeItem('refreshToken');
    sessionStorage.removeItem('currentUser');
    sessionStorage.removeItem('expiresAt');
  }

  /**
   * Validación de autenticación
   */
  isAuthenticated(): boolean {
    const token = this.getToken();
    const sessionId = sessionStorage.getItem(this.SESSION_KEY);

    if (!token || !sessionId) {
      if (!token) console.log('❌ No hay token');
      if (!sessionId) console.log('❌ No hay sesión de navegador');
      return false;
    }

    if (this.isTokenExpired()) {
      console.log('⏰ Token expirado');
      return false;
    }

    return true;
  }

  /**
   * Obtener token
   */
  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  /**
   * Obtener usuario actual
   */
  getCurrentUser(): CurrentUser | null {
    return this.currentUserSubject.value;
  }

  /**
   * Verificar roles
   */
  hasRole(role: string): boolean {
    const user = this.getCurrentUser();
    return user?.roles.includes(role) || false;
  }

  hasAnyRole(roles: string[]): boolean {
    const user = this.getCurrentUser();
    if (!user) return false;
    return roles.some(role => user.roles.includes(role));
  }

  hasAllRoles(roles: string[]): boolean {
    const user = this.getCurrentUser();
    if (!user) return false;
    return roles.every(role => user.roles.includes(role));
  }

  /**
   * Almacenar datos de autenticación
   */
  private storeAuthData(data: any): void {
    localStorage.setItem('authToken', data.token);
    localStorage.setItem('refreshToken', data.refreshToken);
    localStorage.setItem('expiresAt', data.expiresAt);
    localStorage.setItem('currentUser', JSON.stringify(data.user));
    this.currentUserSubject.next(data.user);

    console.log('💾 Datos guardados');
    console.log('⏰ Expira:', data.expiresAt);
  }

  /**
   * Cargar usuario desde storage
   */
  private loadUserFromStorage(): void {
    try {
      const userStr = localStorage.getItem('currentUser');
      const sessionId = sessionStorage.getItem(this.SESSION_KEY);

      // ✅ Solo cargar usuario si HAY sessionId
      if (!sessionId) {
        console.log('❌ No hay SESSION_KEY, sesión no válida');
        if (userStr) {
          console.log('🧹 Limpiando datos de sesión anterior');
          this.clearAuthData();
        }
        return;
      }

      if (userStr) {
        const user = JSON.parse(userStr);

        if (!this.isTokenExpired()) {
          this.currentUserSubject.next(user);
          this.resetInactivityTimer();
          this.updateLastActivity();
          console.log('📦 Usuario restaurado:', user.userName);
        } else {
          console.log('⏰ Token expirado');
          this.clearAuthData();
        }
      }
    } catch (error) {
      console.error('❌ Error al cargar usuario:', error);
      this.clearAuthData();
    }
  }

  /**
   * Verificar expiración
   */
  private isTokenExpired(): boolean {
    const expiresAt = localStorage.getItem('expiresAt');
    if (!expiresAt) return true;

    const expirationDate = new Date(expiresAt);
    const now = new Date();

    // Buffer de 5 minutos
    const buffer = 5 * 60 * 1000;
    return now.getTime() >= (expirationDate.getTime() - buffer);
  }

  /**
   * Tiempo restante del token
   */
  getTokenRemainingTime(): number {
    const expiresAt = localStorage.getItem('expiresAt');
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

  /**
   * Limpieza
   */
  ngOnDestroy(): void {
    this.authChannel?.close();
    if (this.inactivityTimer) {
      clearTimeout(this.inactivityTimer);
    }
  }
}
