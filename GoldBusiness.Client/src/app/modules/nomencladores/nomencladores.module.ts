import { NgModule } from '@angular/core';
import { NomencladoresRoutingModule } from './nomencladores-routing.module';
import { SharedModule } from '../../shared/shared.module';

// ============================================
// 📁 CLIENTES
// ============================================
import { ClienteListComponent } from '../../pages/cliente/cliente-list/cliente-list.component';
import { ClienteFormComponent } from '../../pages/cliente/cliente-form/cliente-form.component';
import { ClienteDetailComponent } from '../../pages/cliente/cliente-detail/cliente-detail.component';

// ============================================
// 📁 PROVEEDORES
// ============================================
import { ProveedorListComponent } from '../../pages/proveedor/proveedor-list/proveedor-list.component';
import { ProveedorFormComponent } from '../../pages/proveedor/proveedor-form/proveedor-form.component';
import { ProveedorDetailComponent } from '../../pages/proveedor/proveedor-detail/proveedor-detail.component';

// ============================================
// 📁 PAÍS
// ============================================
import { PaisListComponent } from '../../pages/pais/pais-list/pais-list.component';
import { PaisFormComponent } from '../../pages/pais/pais-form/pais-form.component';
import { PaisDetailComponent } from '../../pages/pais/pais-detail/pais-detail.component';

// ============================================
// 📁 PROVINCIA
// ============================================
import { ProvinciaListComponent } from '../../pages/provincia/provincia-list/provincia-list.component';
import { ProvinciaFormComponent } from '../../pages/provincia/provincia-form/provincia-form.component';
import { ProvinciaDetailComponent } from '../../pages/provincia/provincia-detail/provincia-detail.component';

// ============================================
// 📁 MUNICIPIO
// ============================================
import { MunicipioListComponent } from '../../pages/municipio/municipio-list/municipio-list.component';
import { MunicipioFormComponent } from '../../pages/municipio/municipio-form/municipio-form.component';
import { MunicipioDetailComponent } from '../../pages/municipio/municipio-detail/municipio-detail.component';

// ============================================
// 📁 CÓDIGO POSTAL
// ============================================
import { CodigoPostalListComponent } from '../../pages/codigoPostal/codigo-postal-list/codigo-postal-list.component';
import { CodigoPostalFormComponent } from '../../pages/codigoPostal/codigo-postal-form/codigo-postal-form.component';
import { CodigoPostalDetailComponent } from '../../pages/codigoPostal/codigo-postal-detail/codigo-postal-detail.component';

// ============================================
// 📁 MONEDA
// ============================================
import { MonedaListComponent } from '../../pages/moneda/moneda-list/moneda-list.component';
import { MonedaFormComponent } from '../../pages/moneda/moneda-form/moneda-form.component';
import { MonedaDetailComponent } from '../../pages/moneda/moneda-detail/moneda-detail.component';

@NgModule({
  declarations: [
    // Clientes
    ClienteListComponent,
    ClienteFormComponent,
    ClienteDetailComponent,
    
    // Proveedores
    ProveedorListComponent,
    ProveedorFormComponent,
    ProveedorDetailComponent,
    
    // País
    PaisListComponent,
    PaisFormComponent,
    PaisDetailComponent,
    
    // Provincia
    ProvinciaListComponent,
    ProvinciaFormComponent,
    ProvinciaDetailComponent,
    
    // Municipio
    MunicipioListComponent,
    MunicipioFormComponent,
    MunicipioDetailComponent,
    
    // Código Postal
    CodigoPostalListComponent,
    CodigoPostalFormComponent,
    CodigoPostalDetailComponent,
    
    // Moneda
    MonedaListComponent,
    MonedaFormComponent,
    MonedaDetailComponent,
  ],
  imports: [
    SharedModule,
    NomencladoresRoutingModule
  ]
})
export class NomencladoresModule { }
