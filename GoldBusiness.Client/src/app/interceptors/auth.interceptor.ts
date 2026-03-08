import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, filter, take, switchMap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // ✅ NO agregar token a endpoints públicos
    if (this.isPublicEndpoint(request.url)) {
      return next.handle(request);
    }

    // ✅ Agregar token a la petición
    const token = this.authService.getToken();
    if (token) {
      request = this.addToken(request, token);
    }

    // ✅ Manejar la respuesta y errores
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        
        // ✅ Si es 401, intentar renovar token
        if (error.status === 401 && !this.isRefreshing) {
          return this.handle401Error(request, next);
        }

        // ✅ Si es 403, no tiene permisos
        if (error.status === 403) {
          console.warn('⚠️ Acceso denegado. Sin permisos suficientes.');
        }

        return throwError(() => error);
      })
    );
  }

  /**
   * Agregar token JWT a la petición
   */
  private addToken(request: HttpRequest<any>, token: string): HttpRequest<any> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  /**
   * Verificar si el endpoint es público (no requiere autenticación)
   */
  private isPublicEndpoint(url: string): boolean {
    const publicEndpoints = ['/auth/login', '/auth/refresh', '/auth/register'];
    return publicEndpoints.some(endpoint => url.includes(endpoint));
  }

  /**
   * ✅ NUEVO: Manejar error 401 con renovación automática
   */
  private handle401Error(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      console.log('🔄 Token expirado, renovando automáticamente...');

      return this.authService.refreshToken().pipe(
        switchMap((response: any) => {
          this.isRefreshing = false;

          if (response.succeeded && response.data) {
            this.refreshTokenSubject.next(response.data.token);
            console.log('✅ Token renovado, reintentando petición');
            
            // ✅ Reintentar la petición original con el nuevo token
            return next.handle(this.addToken(request, response.data.token));
          }

          // Si falla el refresh, hacer logout
          console.error('❌ No se pudo renovar el token');
          this.authService.logout();
          return throwError(() => new Error('Token refresh failed'));
        }),
        catchError((err) => {
          this.isRefreshing = false;
          console.error('❌ Error al renovar token:', err);
          this.authService.logout();
          return throwError(() => err);
        })
      );
    } else {
      // ✅ Si ya se está renovando, esperar al nuevo token
      return this.refreshTokenSubject.pipe(
        filter(token => token != null),
        take(1),
        switchMap(token => {
          return next.handle(this.addToken(request, token));
        })
      );
    }
  }
}
