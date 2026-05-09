import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// ============================================
// 📁 ESTABLECIMIENTO
// ============================================
import { EstablecimientoListComponent } from '../../pages/establecimiento/establecimiento-list/establecimiento-list.component';
import { EstablecimientoFormComponent } from '../../pages/establecimiento/establecimiento-form/establecimiento-form.component';
import { EstablecimientoDetailComponent } from '../../pages/establecimiento/establecimiento-detail/establecimiento-detail.component';

// ============================================
// 📁 LOCALIDAD
// ============================================
import { LocalidadListComponent } from '../../pages/localidad/localidad-list/localidad-list.component';
import { LocalidadFormComponent } from '../../pages/localidad/localidad-form/localidad-form.component';
import { LocalidadDetailComponent } from '../../pages/localidad/localidad-detail/localidad-detail.component';

// ============================================
// 📁 SYSTEM CONFIGURATION
// ============================================
import { SystemConfigurationListComponent } from '../../pages/systemConfiguration/system-configuration-list/system-configuration-list.component';
import { SystemConfigurationFormComponent } from '../../pages/systemConfiguration/system-configuration-form/system-configuration-form.component';
import { SystemConfigurationDetailComponent } from '../../pages/systemConfiguration/system-configuration-detail/system-configuration-detail.component';

// ============================================
// 📁 USUARIOS
// ============================================
import { UsuarioListComponent } from '../../pages/usuario/usuario-list/usuario-list.component';
import { UsuarioFormComponent } from '../../pages/usuario/usuario-form/usuario-form.component';
import { UsuarioDetailComponent } from '../../pages/usuario/usuario-detail/usuario-detail.component';

// ============================================
// 📁 TEST CONEXIÓN
// ============================================
import { TestConnectionComponent } from '../../components/test-connection/test-connection.component';

const routes: Routes = [
  // ESTABLECIMIENTO
  { path: 'establecimiento', component: EstablecimientoListComponent },
  { path: 'establecimiento/nuevo', component: EstablecimientoFormComponent },
  { path: 'establecimiento/editar/:id', component: EstablecimientoFormComponent },
  { path: 'establecimiento/detalle/:id', component: EstablecimientoDetailComponent },

  // LOCALIDAD
  { path: 'localidad', component: LocalidadListComponent },
  { path: 'localidad/nuevo', component: LocalidadFormComponent },
  { path: 'localidad/editar/:id', component: LocalidadFormComponent },
  { path: 'localidad/detalle/:id', component: LocalidadDetailComponent },

  // SYSTEM CONFIGURATION (Negocio = SystemConfiguration)
  { path: 'negocio', component: SystemConfigurationListComponent },
  { path: 'negocio/editar/:id', component: SystemConfigurationFormComponent },
  { path: 'negocio/detalle/:id', component: SystemConfigurationDetailComponent },

  // Alias de compatibilidad histórica
  { path: 'sistema', redirectTo: 'negocio', pathMatch: 'full' },
  { path: 'sistema/editar/:id', redirectTo: 'negocio/editar/:id', pathMatch: 'full' },
  { path: 'sistema/detalle/:id', redirectTo: 'negocio/detalle/:id', pathMatch: 'full' },

  // USUARIOS
  { path: 'usuarios', component: UsuarioListComponent },
  { path: 'usuarios/nuevo', component: UsuarioFormComponent },
  { path: 'usuarios/editar/:id', component: UsuarioFormComponent },
  { path: 'usuarios/detalle/:id', component: UsuarioDetailComponent },

  // TEST CONEXIÓN
  { path: 'test-conexion', component: TestConnectionComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConfiguracionRoutingModule { }
