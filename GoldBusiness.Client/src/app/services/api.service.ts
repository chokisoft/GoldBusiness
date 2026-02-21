import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../environments/environment.development';
import { LanguageService } from './language.service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = environment.apiUrl;
  private apiVersion = environment.apiVersion;

  constructor(
    private http: HttpClient,
    private languageService: LanguageService
  ) { }

  /**
   * Obtener headers con configuración de idioma
   * El token JWT se agrega automáticamente por el AuthInterceptor
   */
  private getHeaders(): HttpHeaders {
    const currentLanguage = this.languageService.getCurrentLanguage();

    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept-Language': currentLanguage
    });
  }

  /**
   * GET request genérico
   */
  get<T>(endpoint: string): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;
    console.log('📡 GET:', url);
    
    return this.http.get<T>(url, {
      headers: this.getHeaders()
    }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * POST request genérico
   */
  post<T>(endpoint: string, data: any): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;
    console.log('📡 POST:', url);
    
    return this.http.post<T>(url, data, {
      headers: this.getHeaders()
    }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * PUT request genérico
   */
  put<T>(endpoint: string, data: any): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;
    console.log('📡 PUT:', url);
    
    return this.http.put<T>(url, data, {
      headers: this.getHeaders()
    }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * DELETE request genérico
   */
  delete<T>(endpoint: string): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;
    console.log('📡 DELETE:', url);
    
    return this.http.delete<T>(url, {
      headers: this.getHeaders()
    }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * POST multipart/form-data — NO establecer Content-Type manualmente,
   * el navegador lo agrega automáticamente con el boundary correcto.
   */
  postFormData<T>(endpoint: string, formData: FormData): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;
    const currentLanguage = this.languageService.getCurrentLanguage();
    console.log('📡 POST (FormData):', url);
    return this.http.post<T>(url, formData, {
      headers: new HttpHeaders({ 'Accept-Language': currentLanguage })
    }).pipe(catchError(this.handleError));
  }

  /** Construye la URL completa para un endpoint (útil para src de imágenes) */
  buildUrl(endpoint: string): string {
    return `${this.apiUrl}/${endpoint}`;
  }

  /**
   * Manejo centralizado de errores HTTP
   */
  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Ocurrió un error desconocido';

    if (error.error instanceof ErrorEvent) {
      // Error del lado del cliente
      errorMessage = `Error del cliente: ${error.error.message}`;
    } else {
      // Error del lado del servidor
      errorMessage = `Código ${error.status}: ${error.message}`;

      // Mostrar errores de validación de la API
      if (error.error?.errors) {
        console.error('Errores de validación:', error.error.errors);
      }
    }

    console.error('❌ Error HTTP:', errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
