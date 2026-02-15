import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Obtener el token del servicio de autenticación
    const token = this.authService.getToken();

    // Clonar la petición y agregar el token si existe
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    console.log('🔐 Petición interceptada:', request.url);
    console.log('🎫 Token enviado:', token ? 'Sí' : 'No');

    // Continuar con la petición y manejar errores
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        console.error('❌ Error en petición:', error);

        // Si el error es 401 (No autorizado), redirigir al login
        if (error.status === 401) {
          console.warn('⚠️ Token inválido o expirado. Redirigiendo al login...');
          this.authService.logout();
          this.router.navigate(['/login']);
        }

        // Si el error es 403 (Prohibido)
        if (error.status === 403) {
          console.warn('⚠️ Acceso denegado. Sin permisos suficientes.');
        }

        return throwError(() => error);
      })
    );
  }
}
