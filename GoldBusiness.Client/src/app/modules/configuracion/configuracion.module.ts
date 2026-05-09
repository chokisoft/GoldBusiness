import { NgModule } from '@angular/core';
import { ConfiguracionRoutingModule } from './configuracion-routing.module';
import { SharedModule } from '../../shared/shared.module';

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

@NgModule({
  declarations: [
    // Establecimiento
    EstablecimientoListComponent,
    EstablecimientoFormComponent,
    EstablecimientoDetailComponent,
    
    // Localidad
    LocalidadListComponent,
    LocalidadFormComponent,
    LocalidadDetailComponent,
    
    // System Configuration
    SystemConfigurationListComponent,
    SystemConfigurationFormComponent,
    SystemConfigurationDetailComponent,
    
    // Usuarios
    UsuarioListComponent,
    UsuarioFormComponent,
    UsuarioDetailComponent,

    // Test conexión
    TestConnectionComponent,
  ],
  imports: [
    SharedModule,
    ConfiguracionRoutingModule
  ]
})
export class ConfiguracionModule { }
