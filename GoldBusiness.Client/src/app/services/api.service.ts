import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../environments/environment.development';
import { LanguageService } from './language.service'; // ← Importar

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = environment.apiUrl;
  private apiVersion = environment.apiVersion;

  constructor(
    private http: HttpClient,
    private languageService: LanguageService // ← Inyectar
  ) { }

  /**
   * Obtener headers con JWT token y configuración de idioma
   */
  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('authToken');
    const currentLanguage = this.languageService.getCurrentLanguage(); // ← Obtener idioma actual

    let headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept-Language': currentLanguage // ← Usar idioma dinámico
    });

    if (token) {
      headers = headers.set('Authorization', `Bearer ${token}`);
    }

    return headers;
  }

  /**
   * GET request genérico
   */
  get<T>(endpoint: string): Observable<T> {
    return this.http.get<T>(`${this.apiUrl}/${endpoint}`, {
      headers: this.getHeaders(),
      withCredentials: true
    }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * POST request genérico
   */
  post<T>(endpoint: string, data: any): Observable<T> {
    return this.http.post<T>(`${this.apiUrl}/${endpoint}`, data, {
      headers: this.getHeaders(),
      withCredentials: true
    }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * PUT request genérico
   */
  put<T>(endpoint: string, data: any): Observable<T> {
    return this.http.put<T>(`${this.apiUrl}/${endpoint}`, data, {
      headers: this.getHeaders(),
      withCredentials: true
    }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * DELETE request genérico
   */
  delete<T>(endpoint: string): Observable<T> {
    return this.http.delete<T>(`${this.apiUrl}/${endpoint}`, {
      headers: this.getHeaders(),
      withCredentials: true
    }).pipe(
      catchError(this.handleError)
    );
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
      errorMessage = `Código de error: ${error.status}\nMensaje: ${error.message}`;

      // Mostrar errores de validación de la API
      if (error.error?.errors) {
        console.error('❌ Errores de validación:', error.error.errors);
      }

      // Mensaje personalizado del servidor
      if (error.error?.message) {
        console.error('❌ Mensaje del servidor:', error.error.message);
      }
    }

    console.error('🔴 Error HTTP completo:', error);
    return throwError(() => new Error(errorMessage));
  }
}
