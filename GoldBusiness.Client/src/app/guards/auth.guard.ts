import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

/**
 * Guard de autenticación con validación estricta
 * Verifica:
 * 1. Token existe
 * 2. Token no ha expirado
 * 3. Sesión de navegador activa
 * 4. Usuario cargado
 */
export const authGuard = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Verificación completa
  const isAuthenticated = authService.isAuthenticated();
  const currentUser = authService.getCurrentUser();

  if (isAuthenticated && currentUser) {
    console.log('✅ Acceso permitido:', currentUser.userName);
    return true;
  }

  console.warn('⚠️ Acceso denegado, redirigiendo a login');
  console.log('   - Token válido:', !!authService.getToken());
  console.log('   - Usuario cargado:', !!currentUser);
  console.log('   - Autenticado:', isAuthenticated);

  // Limpiar datos por seguridad
  authService.logout();
  
  return false;
};
