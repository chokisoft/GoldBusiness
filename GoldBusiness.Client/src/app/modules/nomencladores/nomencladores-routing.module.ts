import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

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

const routes: Routes = [
  // CLIENTES
  { path: 'clientes', component: ClienteListComponent },
  { path: 'clientes/nuevo', component: ClienteFormComponent },
  { path: 'clientes/editar/:id', component: ClienteFormComponent },
  { path: 'clientes/detalle/:id', component: ClienteDetailComponent },

  // PROVEEDORES
  { path: 'proveedor', component: ProveedorListComponent },
  { path: 'proveedor/nuevo', component: ProveedorFormComponent },
  { path: 'proveedor/editar/:id', component: ProveedorFormComponent },
  { path: 'proveedor/detalle/:id', component: ProveedorDetailComponent },

  // PAÍS
  { path: 'pais', component: PaisListComponent },
  { path: 'pais/nuevo', component: PaisFormComponent },
  { path: 'pais/editar/:id', component: PaisFormComponent },
  { path: 'pais/detalle/:id', component: PaisDetailComponent },

  // PROVINCIA
  { path: 'provincia', component: ProvinciaListComponent },
  { path: 'provincia/nuevo', component: ProvinciaFormComponent },
  { path: 'provincia/editar/:id', component: ProvinciaFormComponent },
  { path: 'provincia/detalle/:id', component: ProvinciaDetailComponent },

  // MUNICIPIO
  { path: 'municipio', component: MunicipioListComponent },
  { path: 'municipio/nuevo', component: MunicipioFormComponent },
  { path: 'municipio/editar/:id', component: MunicipioFormComponent },
  { path: 'municipio/detalle/:id', component: MunicipioDetailComponent },

  // CÓDIGO POSTAL
  { path: 'codigo-postal', component: CodigoPostalListComponent },
  { path: 'codigo-postal/nuevo', component: CodigoPostalFormComponent },
  { path: 'codigo-postal/editar/:id', component: CodigoPostalFormComponent },
  { path: 'codigo-postal/detalle/:id', component: CodigoPostalDetailComponent },

  // MONEDA
  { path: 'moneda', component: MonedaListComponent },
  { path: 'moneda/nuevo', component: MonedaFormComponent },
  { path: 'moneda/editar/:id', component: MonedaFormComponent },
  { path: 'moneda/detalle/:id', component: MonedaDetailComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NomencladoresRoutingModule { }
