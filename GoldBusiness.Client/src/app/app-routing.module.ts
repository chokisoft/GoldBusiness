import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestConnectionComponent } from './components/test-connection/test-connection.component';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { MainLayoutComponent } from './components/layout/main-layout.component';
import { authGuard } from './guards/auth.guard';

// ============================================
// 📁 NOMENCLADORES - GRUPO CUENTA
// ============================================
import { GrupoCuentaListComponent } from './pages/grupo-cuenta/grupo-cuenta-list/grupo-cuenta-list.component';
import { GrupoCuentaFormComponent } from './pages/grupo-cuenta/grupo-cuenta-form/grupo-cuenta-form.component';
import { GrupoCuentaDetailComponent } from './pages/grupo-cuenta/grupo-cuenta-detail/grupo-cuenta-detail.component';

// ============================================
// 📁 NOMENCLADORES - SUBGRUPO CUENTA
// ============================================
import { SubGrupoCuentaListComponent } from './pages/subgrupo-cuenta/subgrupo-cuenta-list/subgrupo-cuenta-list.component';
import { SubGrupoCuentaFormComponent } from './pages/subgrupo-cuenta/subgrupo-cuenta-form/subgrupo-cuenta-form.component';
import { SubGrupoCuentaDetailComponent } from './pages/subgrupo-cuenta/subgrupo-cuenta-detail/subgrupo-cuenta-detail.component';

// ============================================
// 📁 NOMENCLADORES - CUENTA
// ============================================
import { CuentaListComponent } from './pages/cuenta/cuenta-list/cuenta-list.component';
import { CuentaFormComponent } from './pages/cuenta/cuenta-form/cuenta-form.component';
import { CuentaDetailComponent } from './pages/cuenta/cuenta-detail/cuenta-detail.component';

// ============================================
// 📁 NOMENCLADORES - CLIENTES
// ============================================
import { ClienteListComponent } from './pages/cliente/cliente-list/cliente-list.component';
import { ClienteFormComponent } from './pages/cliente/cliente-form/cliente-form.component';
import { ClienteDetailComponent } from './pages/cliente/cliente-detail/cliente-detail.component';

// ============================================
// 📁 NOMENCLADORES - PROVEEDORES
// ============================================
import { ProveedorListComponent } from './pages/proveedor/proveedor-list/proveedor-list.component';
import { ProveedorFormComponent } from './pages/proveedor/proveedor-form/proveedor-form.component';
import { ProveedorDetailComponent } from './pages/proveedor/proveedor-detail/proveedor-detail.component';

// ============================================
// 📁 NOMENCLADORES - ESTABLECIMIENTO
// ============================================
import { EstablecimientoListComponent } from './pages/establecimiento/establecimiento-list/establecimiento-list.component';
import { EstablecimientoFormComponent } from './pages/establecimiento/establecimiento-form/establecimiento-form.component';
import { EstablecimientoDetailComponent } from './pages/establecimiento/establecimiento-detail/establecimiento-detail.component';

// ============================================
// 📁 NOMENCLADORES - LOCALIDAD
// ============================================
import { LocalidadListComponent } from './pages/localidad/localidad-list/localidad-list.component';
import { LocalidadFormComponent } from './pages/localidad/localidad-form/localidad-form.component';
import { LocalidadDetailComponent } from './pages/localidad/localidad-detail/localidad-detail.component';

// ============================================
// 📁 NOMENCLADORES - MONEDA
// ============================================
import { MonedaListComponent } from './pages/moneda/moneda-list/moneda-list.component';
import { MonedaFormComponent } from './pages/moneda/moneda-form/moneda-form.component';
import { MonedaDetailComponent } from './pages/moneda/moneda-detail/moneda-detail.component';

// ============================================
// 📁 NOMENCLADORES - LÍNEA
// ============================================
import { LineaListComponent } from './pages/linea/linea-list/linea-list.component';
import { LineaFormComponent } from './pages/linea/linea-form/linea-form.component';
import { LineaDetailComponent } from './pages/linea/linea-detail/linea-detail.component';

// ============================================
// 📁 NOMENCLADORES - SUBLÍNEA
// ============================================
import { SubLineaListComponent } from './pages/subLinea/sub-linea-list/sub-linea-list.component';
import { SubLineaFormComponent } from './pages/subLinea/sub-linea-form/sub-linea-form.component';
import { SubLineaDetailComponent } from './pages/subLinea/sub-linea-detail/sub-linea-detail.component';

// ============================================
// 📁 NOMENCLADORES - UNIDAD DE MEDIDA
// ============================================
import { UnidadMedidaListComponent } from './pages/unidadMedida/unidad-medida-list/unidad-medida-list.component';
import { UnidadMedidaFormComponent } from './pages/unidadMedida/unidad-medida-form/unidad-medida-form.component';
import { UnidadMedidaDetailComponent } from './pages/unidadMedida/unidad-medida-detail/unidad-medida-detail.component';

// ============================================
// 📁 NOMENCLADORES - PAÍS
// ============================================
import { PaisListComponent } from './pages/pais/pais-list/pais-list.component';
import { PaisFormComponent } from './pages/pais/pais-form/pais-form.component';
import { PaisDetailComponent } from './pages/pais/pais-detail/pais-detail.component';

// ============================================
// 📁 NOMENCLADORES - PROVINCIA
// ============================================
import { ProvinciaListComponent } from './pages/provincia/provincia-list/provincia-list.component';
import { ProvinciaFormComponent } from './pages/provincia/provincia-form/provincia-form.component';
import { ProvinciaDetailComponent } from './pages/provincia/provincia-detail/provincia-detail.component';

// ============================================
// 📁 NOMENCLADORES - MUNICIPIO
// ============================================
import { MunicipioListComponent } from './pages/municipio/municipio-list/municipio-list.component';
import { MunicipioFormComponent } from './pages/municipio/municipio-form/municipio-form.component';
import { MunicipioDetailComponent } from './pages/municipio/municipio-detail/municipio-detail.component';

// ============================================
// 📁 NOMENCLADORES - CÓDIGO POSTAL
// ============================================
import { CodigoPostalListComponent } from './pages/codigoPostal/codigo-postal-list/codigo-postal-list.component';
import { CodigoPostalFormComponent } from './pages/codigoPostal/codigo-postal-form/codigo-postal-form.component';
import { CodigoPostalDetailComponent } from './pages/codigoPostal/codigo-postal-detail/codigo-postal-detail.component';

// ============================================
// 📁 NOMENCLADORES - TRANSACCIÓN
// ============================================
import { TransaccionListComponent } from './pages/transaccion/transaccion-list/transaccion-list.component';
import { TransaccionFormComponent } from './pages/transaccion/transaccion-form/transaccion-form.component';
import { TransaccionDetailComponent } from './pages/transaccion/transaccion-detail/transaccion-detail.component';

// ============================================
// 📁 NOMENCLADORES - CONCEPTO AJUSTE
// ============================================
import { ConceptoAjusteListComponent } from './pages/conceptoAjuste/concepto-ajuste-list/concepto-ajuste-list.component';
import { ConceptoAjusteFormComponent } from './pages/conceptoAjuste/concepto-ajuste-form/concepto-ajuste-form.component';
import { ConceptoAjusteDetailComponent } from './pages/conceptoAjuste/concepto-ajuste-detail/concepto-ajuste-detail.component';

// ============================================
// 📁 NOMENCLADORES - PRODUCTO
// ============================================
import { ProductoListComponent } from './pages/producto/producto-list/producto-list.component';
import { ProductoFormComponent } from './pages/producto/producto-form/producto-form.component';
import { ProductoDetailComponent } from './pages/producto/producto-detail/producto-detail.component';

// ============================================
// ⚙️ CONFIGURACIÓN
// ============================================
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
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },

      // ============================================
      // 📊 DASHBOARD
      // ============================================
      {
        path: 'dashboard',
        component: DashboardComponent
      },

      // ============================================
      // 📁 NOMENCLADORES
      // ============================================
      {
        path: 'nomencladores',
        children: [
          // GrupoCuenta
          {
            path: 'grupo-cuenta',
            children: [
              { path: '', component: GrupoCuentaListComponent },
              { path: 'nuevo', component: GrupoCuentaFormComponent },
              { path: 'editar/:id', component: GrupoCuentaFormComponent },
              { path: ':id', component: GrupoCuentaDetailComponent }
            ]
          },
          // SubGrupoCuenta
          {
            path: 'subgrupo-cuenta',
            children: [
              { path: '', component: SubGrupoCuentaListComponent },
              { path: 'nuevo', component: SubGrupoCuentaFormComponent },
              { path: 'editar/:id', component: SubGrupoCuentaFormComponent },
              { path: ':id', component: SubGrupoCuentaDetailComponent }
            ]
          },
          // Cuenta
          {
            path: 'cuenta',
            children: [
              { path: '', component: CuentaListComponent },
              { path: 'nuevo', component: CuentaFormComponent },
              { path: 'editar/:id', component: CuentaFormComponent },
              { path: ':id', component: CuentaDetailComponent }
            ]
          },
          // Clientes
          {
            path: 'clientes',
            children: [
              { path: '', component: ClienteListComponent },
              { path: 'nuevo', component: ClienteFormComponent },
              { path: 'editar/:id', component: ClienteFormComponent },
              { path: ':id', component: ClienteDetailComponent }
            ]
          },
          // Proveedores
          {
            path: 'proveedores',
            children: [
              { path: '', component: ProveedorListComponent },
              { path: 'nuevo', component: ProveedorFormComponent },
              { path: 'editar/:id', component: ProveedorFormComponent },
              { path: ':id', component: ProveedorDetailComponent }
            ]
          },
          // Establecimiento
          {
            path: 'establecimiento',
            children: [
              { path: '', component: EstablecimientoListComponent },
              { path: 'nuevo', component: EstablecimientoFormComponent },
              { path: 'editar/:id', component: EstablecimientoFormComponent },
              { path: ':id', component: EstablecimientoDetailComponent }
            ]
          },
          // Localidad
          {
            path: 'localidad',
            children: [
              { path: '', component: LocalidadListComponent },
              { path: 'nuevo', component: LocalidadFormComponent },
              { path: 'editar/:id', component: LocalidadFormComponent },
              { path: ':id', component: LocalidadDetailComponent }
            ]
          },
          // Moneda
          {
            path: 'moneda',
            children: [
              { path: '', component: MonedaListComponent },
              { path: 'nuevo', component: MonedaFormComponent },
              { path: 'editar/:id', component: MonedaFormComponent },
              { path: ':id', component: MonedaDetailComponent }
            ]
          },
          // Línea
          {
            path: 'linea',
            children: [
              { path: '', component: LineaListComponent },
              { path: 'nuevo', component: LineaFormComponent },
              { path: 'editar/:id', component: LineaFormComponent },
              { path: ':id', component: LineaDetailComponent }
            ]
          },
          // Sublínea
          {
            path: 'sublinea',
            children: [
              { path: '', component: SubLineaListComponent },
              { path: 'nuevo', component: SubLineaFormComponent },
              { path: 'editar/:id', component: SubLineaFormComponent },
              { path: ':id', component: SubLineaDetailComponent }
            ]
          },
          // Unidad de Medida
          {
            path: 'unidad-medida',
            children: [
              { path: '', component: UnidadMedidaListComponent },
              { path: 'nuevo', component: UnidadMedidaFormComponent },
              { path: 'editar/:id', component: UnidadMedidaFormComponent },
              { path: ':id', component: UnidadMedidaDetailComponent }
            ]
          },
          // País
          {
            path: 'pais',
            children: [
              { path: '', component: PaisListComponent },
              { path: 'nuevo', component: PaisFormComponent },
              { path: 'editar/:id', component: PaisFormComponent },
              { path: ':id', component: PaisDetailComponent }
            ]
          },
          // Provincia
          {
            path: 'provincia',
            children: [
              { path: '', component: ProvinciaListComponent },
              { path: 'nuevo', component: ProvinciaFormComponent },
              { path: 'editar/:id', component: ProvinciaFormComponent },
              { path: ':id', component: ProvinciaDetailComponent }
            ]
          },
          // Municipio
          {
            path: 'municipio',
            children: [
              { path: '', component: MunicipioListComponent },
              { path: 'nuevo', component: MunicipioFormComponent },
              { path: 'editar/:id', component: MunicipioFormComponent },
              { path: ':id', component: MunicipioDetailComponent }
            ]
          },
          // Código Postal
          {
            path: 'codigo-postal',
            children: [
              { path: '', component: CodigoPostalListComponent },
              { path: 'nuevo', component: CodigoPostalFormComponent },
              { path: 'editar/:id', component: CodigoPostalFormComponent },
              { path: ':id', component: CodigoPostalDetailComponent }
            ]
          },
          // Transacción
          {
            path: 'transaccion',
            children: [
              { path: '', component: TransaccionListComponent },
              { path: 'nuevo', component: TransaccionFormComponent },
              { path: 'editar/:id', component: TransaccionFormComponent },
              { path: ':id', component: TransaccionDetailComponent }
            ]
          },
          // Concepto Ajuste
          {
            path: 'concepto-ajuste',
            children: [
              { path: '', component: ConceptoAjusteListComponent },
              { path: 'nuevo', component: ConceptoAjusteFormComponent },
              { path: 'editar/:id', component: ConceptoAjusteFormComponent },
              { path: ':id', component: ConceptoAjusteDetailComponent }
            ]
          },
          // Producto
          {
            path: 'producto',
            children: [
              { path: '', component: ProductoListComponent },
              { path: 'nuevo', component: ProductoFormComponent },
              { path: 'editar/:id', component: ProductoFormComponent },
              { path: ':id', component: ProductoDetailComponent }
            ]
          }
        ]
      },

      // ============================================
      // ⚙️ CONFIGURACIÓN
      // ============================================
      {
        path: 'configuracion',
        children: [
          // Negocio
          {
            path: 'negocio',
            children: [
              { path: '', component: SystemConfigurationListComponent },
              { path: 'nuevo', component: SystemConfigurationFormComponent },
              { path: 'editar/:id', component: SystemConfigurationFormComponent },
              { path: ':id', component: SystemConfigurationDetailComponent }
            ]
          },
          // Usuarios (placeholder)
          {
            path: 'usuarios',
            component: TestConnectionComponent
          },
          // Prueba de Conexión
          {
            path: 'test-conexion',
            component: TestConnectionComponent
          }
        ]
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
