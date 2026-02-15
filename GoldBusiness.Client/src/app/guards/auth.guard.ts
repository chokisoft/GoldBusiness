import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuthenticated()) {
    console.log('✅ Usuario autenticado, acceso permitido');
    return true;
  }

  console.warn('⚠️ Usuario no autenticado, redirigiendo al login');
  router.navigate(['/login']);
  return false;
};
