import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { LanguageService } from '../services/language.service';

@Injectable()
export class LanguageInterceptor implements HttpInterceptor {
  constructor(private languageService: LanguageService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // Agregar el header de idioma a todas las peticiones
    const currentLanguage = this.languageService.getCurrentLanguage();

    const modifiedRequest = request.clone({
      setHeaders: {
        'Accept-Language': currentLanguage
      }
    });

    console.log('📡 Petición HTTP con idioma:', currentLanguage);

    return next.handle(modifiedRequest);
  }
}
