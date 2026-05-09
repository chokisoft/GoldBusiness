import { NgModule } from '@angular/core';
import { RouterModule, Routes, PreloadAllModules } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { MainLayoutComponent } from './components/layout/main-layout.component';
import { authGuard } from './guards/auth.guard';

// ============================================
// 🚀 RUTAS CON LAZY LOADING
// ============================================
const routes: Routes = [
  // ============================================
  // 🔓 PÚBLICAS (Sin autenticación)
  // ============================================
  { 
    path: 'login', 
    component: LoginComponent 
  },

  // ============================================
  // 🔒 PROTEGIDAS (Con autenticación)
  // ============================================
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      // 📊 DASHBOARD
      { 
        path: '', 
        redirectTo: 'inicio', 
        pathMatch: 'full' 
      },
      { 
        path: 'inicio', 
        component: DashboardComponent 
      },
      {
        path: 'dashboard',
        redirectTo: 'inicio',
        pathMatch: 'full'
      },

      // ============================================
      // 📁 PLAN DE CUENTAS (LAZY LOADING)
      // ============================================
      {
        path: 'plan-cuentas',
        loadChildren: () => import('./modules/plan-cuentas/plan-cuentas.module').then(m => m.PlanCuentasModule)
      },

      // ============================================
      // 👥 NOMENCLADORES - CLIENTES, PROVEEDORES, ETC (LAZY LOADING)
      // ============================================
      {
        path: 'nomencladores',
        loadChildren: () => import('./modules/nomencladores/nomencladores.module').then(m => m.NomencladoresModule)
      },

      // ============================================
      // 📦 INVENTARIO - PRODUCTOS, LÍNEAS, ETC (LAZY LOADING)
      // ============================================
      {
        path: 'inventario',
        loadChildren: () => import('./modules/inventario/inventario.module').then(m => m.InventarioModule)
      },

      // ============================================
      // 🏢 CONFIGURACIÓN - SISTEMA, ESTABLECIMIENTO, USUARIOS (LAZY LOADING)
      // ============================================
      {
        path: 'configuracion',
        loadChildren: () => import('./modules/configuracion/configuracion.module').then(m => m.ConfiguracionModule)
      },
    ]
  },

  // ============================================
  // 🔄 REDIRECCIONES Y 404
  // ============================================
  { 
    path: '**', 
    redirectTo: 'inicio' 
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    // ✅ Preload all modules after initial load for better UX
    preloadingStrategy: PreloadAllModules,
    // Enable tracing for debugging (set to false in production)
    enableTracing: false
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
