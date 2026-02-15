import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestConnectionComponent } from './components/test-connection/test-connection.component';
import { LoginComponent } from './components/login/login.component';
import { MainLayoutComponent } from './components/layout/main-layout.component';
import { authGuard } from './guards/auth.guard';

// GrupoCuenta
import { GrupoCuentaListComponent } from './pages/grupo-cuenta/grupo-cuenta-list/grupo-cuenta-list.component';
import { GrupoCuentaFormComponent } from './pages/grupo-cuenta/grupo-cuenta-form/grupo-cuenta-form.component';
import { GrupoCuentaDetailComponent } from './pages/grupo-cuenta/grupo-cuenta-detail/grupo-cuenta-detail.component';

// SubGrupoCuenta
import { SubGrupoCuentaListComponent } from './pages/subgrupo-cuenta/subgrupo-cuenta-list/subgrupo-cuenta-list.component';
import { SubGrupoCuentaFormComponent } from './pages/subgrupo-cuenta/subgrupo-cuenta-form/subgrupo-cuenta-form.component';
import { SubGrupoCuentaDetailComponent } from './pages/subgrupo-cuenta/subgrupo-cuenta-detail/subgrupo-cuenta-detail.component';

// Cuenta
import { CuentaListComponent } from './pages/cuenta/cuenta-list/cuenta-list.component';
import { CuentaFormComponent } from './pages/cuenta/cuenta-form/cuenta-form.component';
import { CuentaDetailComponent } from './pages/cuenta/cuenta-detail/cuenta-detail.component';

// SystemConfiguration (Negocio)
import { SystemConfigurationListComponent } from './pages/system-configuration/system-configuration-list/system-configuration-list.component';
import { SystemConfigurationFormComponent } from './pages/system-configuration/system-configuration-form/system-configuration-form.component';
import { SystemConfigurationDetailComponent } from './pages/system-configuration/system-configuration-detail/system-configuration-detail.component';

const routes: Routes = [
  // Ruta pública de login
  {
    path: 'login',
    component: LoginComponent
  },

  // Rutas protegidas con el nuevo layout
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: '',
        redirectTo: 'inicio',
        pathMatch: 'full'
      },
      {
        path: 'inicio',
        component: TestConnectionComponent
      },
      {
        path: 'acerca',
        component: TestConnectionComponent
      },
      // ═══════════════════════════════════════════════════════════
      // 📁 NOMENCLADORES
      // ═══════════════════════════════════════════════════════════
      // GrupoCuenta
      {
        path: 'nomencladores/grupo-cuenta',
        children: [
          { path: '', component: GrupoCuentaListComponent },
          { path: 'nuevo', component: GrupoCuentaFormComponent },
          { path: 'editar/:id', component: GrupoCuentaFormComponent },
          { path: ':id', component: GrupoCuentaDetailComponent }
        ]
      },
      // SubGrupoCuenta
      {
        path: 'nomencladores/subgrupo-cuenta',
        children: [
          { path: '', component: SubGrupoCuentaListComponent },
          { path: 'nuevo', component: SubGrupoCuentaFormComponent },
          { path: 'editar/:id', component: SubGrupoCuentaFormComponent },
          { path: ':id', component: SubGrupoCuentaDetailComponent }
        ]
      },
      // Cuenta
      {
        path: 'nomencladores/cuenta',
        children: [
          { path: '', component: CuentaListComponent },
          { path: 'nuevo', component: CuentaFormComponent },
          { path: 'editar/:id', component: CuentaFormComponent },
          { path: ':id', component: CuentaDetailComponent }
        ]
      },
      // ═══════════════════════════════════════════════════════════
      // ⚙️ CONFIGURACIÓN
      // ═══════════════════════════════════════════════════════════
      // Negocio (SystemConfiguration)
      {
        path: 'configuracion',
        children: [
          { path: '', component: SystemConfigurationListComponent },
          { path: 'nuevo', component: SystemConfigurationFormComponent },
          { path: 'editar/:id', component: SystemConfigurationFormComponent },
          { path: ':id', component: SystemConfigurationDetailComponent }
        ]
      },
      // Usuarios (Placeholder - próximamente)
      {
        path: 'usuarios',
        component: TestConnectionComponent
      },
      {
        path: 'test',
        component: TestConnectionComponent
      }
    ]
  },

  // Ruta 404
  {
    path: '**',
    redirectTo: '/login'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
