import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TestConnectionComponent } from './components/test-connection/test-connection.component';
import { LanguageSelectorComponent } from './components/language-selector/language-selector.component';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

// Layout
import { NavbarComponent } from './components/navbar/navbar.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { MainLayoutComponent } from './components/layout/main-layout.component';

// GrupoCuenta
import { GrupoCuentaListComponent } from './pages/grupoCuenta/grupo-cuenta-list/grupo-cuenta-list.component';
import { GrupoCuentaFormComponent } from './pages/grupoCuenta/grupo-cuenta-form/grupo-cuenta-form.component';
import { GrupoCuentaDetailComponent } from './pages/grupoCuenta/grupo-cuenta-detail/grupo-cuenta-detail.component';

// SubGrupoCuenta
import { SubGrupoCuentaListComponent } from './pages/subGrupoCuenta/subgrupo-cuenta-list/subgrupo-cuenta-list.component';
import { SubGrupoCuentaFormComponent } from './pages/subGrupoCuenta/subgrupo-cuenta-form/subgrupo-cuenta-form.component';
import { SubGrupoCuentaDetailComponent } from './pages/subGrupoCuenta/subgrupo-cuenta-detail/subgrupo-cuenta-detail.component';

// Cuenta
import { CuentaListComponent } from './pages/cuenta/cuenta-list/cuenta-list.component';
import { CuentaFormComponent } from './pages/cuenta/cuenta-form/cuenta-form.component';
import { CuentaDetailComponent } from './pages/cuenta/cuenta-detail/cuenta-detail.component';

// SystemConfiguration
import { SystemConfigurationListComponent } from './pages/systemConfiguration/system-configuration-list/system-configuration-list.component';
import { SystemConfigurationFormComponent } from './pages/systemConfiguration/system-configuration-form/system-configuration-form.component';
import { SystemConfigurationDetailComponent } from './pages/systemConfiguration/system-configuration-detail/system-configuration-detail.component';

// Pipes
import { TranslatePipe } from './pipes/translate.pipe';
import { LocalizedDatePipe } from './pipes/localized-date.pipe';
import { LocalizedPhonePipe } from './pipes/localized-phone.pipe';

// Servicios
import { ApiService } from './services/api.service';
import { AuthService } from './services/auth.service';
import { LanguageService } from './services/language.service';
import { TranslationService } from './services/translation.service';
import { SidebarService } from './services/sidebar.service';
import { GrupoCuentaService } from './services/grupo-cuenta.service';
import { SubGrupoCuentaService } from './services/subgrupo-cuenta.service';
import { CuentaService } from './services/cuenta.service';
import { SystemConfigurationService } from './services/system-configuration.service';

// Interceptors
import { LanguageInterceptor } from './interceptors/language.interceptor';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { MonedaListComponent } from './pages/moneda/moneda-list/moneda-list.component';
import { MonedaDetailComponent } from './pages/moneda/moneda-detail/moneda-detail.component';
import { MonedaFormComponent } from './pages/moneda/moneda-form/moneda-form.component';
import { EstablecimientoFormComponent } from './pages/establecimiento/establecimiento-form/establecimiento-form.component';
import { EstablecimientoListComponent } from './pages/establecimiento/establecimiento-list/establecimiento-list.component';
import { EstablecimientoDetailComponent } from './pages/establecimiento/establecimiento-detail/establecimiento-detail.component';
import { ClienteDetailComponent } from './pages/cliente/cliente-detail/cliente-detail.component';
import { ClienteFormComponent } from './pages/cliente/cliente-form/cliente-form.component';
import { ClienteListComponent } from './pages/cliente/cliente-list/cliente-list.component';
import { ProveedorListComponent } from './pages/proveedor/proveedor-list/proveedor-list.component';
import { ProveedorDetailComponent } from './pages/proveedor/proveedor-detail/proveedor-detail.component';
import { ProveedorFormComponent } from './pages/proveedor/proveedor-form/proveedor-form.component';
import { LocalidadFormComponent } from './pages/localidad/localidad-form/localidad-form.component';
import { LocalidadDetailComponent } from './pages/localidad/localidad-detail/localidad-detail.component';
import { LocalidadListComponent } from './pages/localidad/localidad-list/localidad-list.component';
import { LineaListComponent } from './pages/linea/linea-list/linea-list.component';
import { LineaFormComponent } from './pages/linea/linea-form/linea-form.component';
import { LineaDetailComponent } from './pages/linea/linea-detail/linea-detail.component';
import { SubLineaDetailComponent } from './pages/subLinea/sub-linea-detail/sub-linea-detail.component';
import { SubLineaFormComponent } from './pages/subLinea/sub-linea-form/sub-linea-form.component';
import { SubLineaListComponent } from './pages/subLinea/sub-linea-list/sub-linea-list.component';
import { UnidadMedidaListComponent } from './pages/unidadMedida/unidad-medida-list/unidad-medida-list.component';
import { UnidadMedidaFormComponent } from './pages/unidadMedida/unidad-medida-form/unidad-medida-form.component';
import { UnidadMedidaDetailComponent } from './pages/unidadMedida/unidad-medida-detail/unidad-medida-detail.component';
import { PaisDetailComponent } from './pages/pais/pais-detail/pais-detail.component';
import { PaisListComponent } from './pages/pais/pais-list/pais-list.component';
import { PaisFormComponent } from './pages/pais/pais-form/pais-form.component';
import { ProvinciaFormComponent } from './pages/provincia/provincia-form/provincia-form.component';
import { ProvinciaListComponent } from './pages/provincia/provincia-list/provincia-list.component';
import { ProvinciaDetailComponent } from './pages/provincia/provincia-detail/provincia-detail.component';
import { MunicipioDetailComponent } from './pages/municipio/municipio-detail/municipio-detail.component';
import { MunicipioListComponent } from './pages/municipio/municipio-list/municipio-list.component';
import { MunicipioFormComponent } from './pages/municipio/municipio-form/municipio-form.component';
import { CodigoPostalFormComponent } from './pages/codigoPostal/codigo-postal-form/codigo-postal-form.component';
import { CodigoPostalDetailComponent } from './pages/codigoPostal/codigo-postal-detail/codigo-postal-detail.component';
import { CodigoPostalListComponent } from './pages/codigoPostal/codigo-postal-list/codigo-postal-list.component';
import { ProductoListComponent } from './pages/producto/producto-list/producto-list.component';
import { ProductoFormComponent } from './pages/producto/producto-form/producto-form.component';
import { ProductoDetailComponent } from './pages/producto/producto-detail/producto-detail.component';
import { TransaccionDetailComponent } from './pages/transaccion/transaccion-detail/transaccion-detail.component';
import { TransaccionFormComponent } from './pages/transaccion/transaccion-form/transaccion-form.component';
import { TransaccionListComponent } from './pages/transaccion/transaccion-list/transaccion-list.component';
import { ConceptoAjusteListComponent } from './pages/conceptoAjuste/concepto-ajuste-list/concepto-ajuste-list.component';
import { ConceptoAjusteFormComponent } from './pages/conceptoAjuste/concepto-ajuste-form/concepto-ajuste-form.component';
import { ConceptoAjusteDetailComponent } from './pages/conceptoAjuste/concepto-ajuste-detail/concepto-ajuste-detail.component';
import { LoaderComponent } from './components/loader/loader.component';

@NgModule({
  declarations: [
    AppComponent,
    TestConnectionComponent,
    LanguageSelectorComponent,
    LoginComponent,
    DashboardComponent,
    LoaderComponent,
    NavbarComponent,
    SidebarComponent,
    MainLayoutComponent,
    // GrupoCuenta
    GrupoCuentaListComponent,
    GrupoCuentaFormComponent,
    GrupoCuentaDetailComponent,
    // SubGrupoCuenta
    SubGrupoCuentaListComponent,
    SubGrupoCuentaFormComponent,
    SubGrupoCuentaDetailComponent,
    // Cuenta
    CuentaListComponent,
    CuentaFormComponent,
    CuentaDetailComponent,
    // Clientes
    ClienteListComponent,
    ClienteFormComponent,
    ClienteDetailComponent,
    // Proveedores
    ProveedorListComponent,
    ProveedorFormComponent,
    ProveedorDetailComponent,
    // SystemConfiguration
    SystemConfigurationListComponent,
    SystemConfigurationFormComponent,
    SystemConfigurationDetailComponent,
    // Pipes
    TranslatePipe,
    LocalizedDatePipe,
    LocalizedPhonePipe,
    MonedaListComponent,
    MonedaDetailComponent,
    MonedaFormComponent,
    EstablecimientoFormComponent,
    EstablecimientoListComponent,
    EstablecimientoDetailComponent,
    ClienteDetailComponent,
    ClienteFormComponent,
    ClienteListComponent,
    ProveedorListComponent,
    ProveedorDetailComponent,
    ProveedorFormComponent,
    LocalidadFormComponent,
    LocalidadDetailComponent,
    LocalidadListComponent,
    LineaListComponent,
    LineaFormComponent,
    LineaDetailComponent,
    SubLineaDetailComponent,
    SubLineaFormComponent,
    SubLineaListComponent,
    UnidadMedidaListComponent,
    UnidadMedidaFormComponent,
    UnidadMedidaDetailComponent,
    PaisDetailComponent,
    PaisListComponent,
    PaisFormComponent,
    ProvinciaFormComponent,
    ProvinciaListComponent,
    ProvinciaDetailComponent,
    MunicipioDetailComponent,
    MunicipioListComponent,
    MunicipioFormComponent,
    CodigoPostalFormComponent,
    CodigoPostalDetailComponent,
    CodigoPostalListComponent,
    ProductoListComponent,
    ProductoFormComponent,
    ProductoDetailComponent,
    TransaccionDetailComponent,
    TransaccionFormComponent,
    TransaccionListComponent,
    ConceptoAjusteListComponent,
    ConceptoAjusteFormComponent,
    ConceptoAjusteDetailComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,
    RouterModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LanguageInterceptor,
      multi: true
    },
    ApiService,
    AuthService,
    LanguageService,
    TranslationService,
    SidebarService,
    GrupoCuentaService,
    SubGrupoCuentaService,
    CuentaService,
    SystemConfigurationService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
