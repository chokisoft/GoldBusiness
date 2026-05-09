import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// ============================================
// 📁 PRODUCTOS
// ============================================
import { ProductoListComponent } from '../../pages/producto/producto-list/producto-list.component';
import { ProductoFormComponent } from '../../pages/producto/producto-form/producto-form.component';
import { ProductoDetailComponent } from '../../pages/producto/producto-detail/producto-detail.component';

// ============================================
// 📁 LÍNEA
// ============================================
import { LineaListComponent } from '../../pages/linea/linea-list/linea-list.component';
import { LineaFormComponent } from '../../pages/linea/linea-form/linea-form.component';
import { LineaDetailComponent } from '../../pages/linea/linea-detail/linea-detail.component';

// ============================================
// 📁 SUBLÍNEA
// ============================================
import { SubLineaListComponent } from '../../pages/subLinea/sub-linea-list/sub-linea-list.component';
import { SubLineaFormComponent } from '../../pages/subLinea/sub-linea-form/sub-linea-form.component';
import { SubLineaDetailComponent } from '../../pages/subLinea/sub-linea-detail/sub-linea-detail.component';

// ============================================
// 📁 UNIDAD DE MEDIDA
// ============================================
import { UnidadMedidaListComponent } from '../../pages/unidadMedida/unidad-medida-list/unidad-medida-list.component';
import { UnidadMedidaFormComponent } from '../../pages/unidadMedida/unidad-medida-form/unidad-medida-form.component';
import { UnidadMedidaDetailComponent } from '../../pages/unidadMedida/unidad-medida-detail/unidad-medida-detail.component';

// ============================================
// 📁 TRANSACCIONES
// ============================================
import { TransaccionListComponent } from '../../pages/transaccion/transaccion-list/transaccion-list.component';
import { TransaccionFormComponent } from '../../pages/transaccion/transaccion-form/transaccion-form.component';
import { TransaccionDetailComponent } from '../../pages/transaccion/transaccion-detail/transaccion-detail.component';

// ============================================
// 📁 CONCEPTO AJUSTE
// ============================================
import { ConceptoAjusteListComponent } from '../../pages/conceptoAjuste/concepto-ajuste-list/concepto-ajuste-list.component';
import { ConceptoAjusteFormComponent } from '../../pages/conceptoAjuste/concepto-ajuste-form/concepto-ajuste-form.component';
import { ConceptoAjusteDetailComponent } from '../../pages/conceptoAjuste/concepto-ajuste-detail/concepto-ajuste-detail.component';

const routes: Routes = [
  // PRODUCTOS
  { path: 'productos', component: ProductoListComponent },
  { path: 'productos/nuevo', component: ProductoFormComponent },
  { path: 'productos/editar/:id', component: ProductoFormComponent },
  { path: 'productos/detalle/:id', component: ProductoDetailComponent },

  // LÍNEA
  { path: 'linea', component: LineaListComponent },
  { path: 'linea/nuevo', component: LineaFormComponent },
  { path: 'linea/editar/:id', component: LineaFormComponent },
  { path: 'linea/detalle/:id', component: LineaDetailComponent },

  // SUBLÍNEA
  { path: 'sublinea', component: SubLineaListComponent },
  { path: 'sublinea/nuevo', component: SubLineaFormComponent },
  { path: 'sublinea/editar/:id', component: SubLineaFormComponent },
  { path: 'sublinea/detalle/:id', component: SubLineaDetailComponent },

  // UNIDAD DE MEDIDA
  { path: 'unidad-medida', component: UnidadMedidaListComponent },
  { path: 'unidad-medida/nuevo', component: UnidadMedidaFormComponent },
  { path: 'unidad-medida/editar/:id', component: UnidadMedidaFormComponent },
  { path: 'unidad-medida/detalle/:id', component: UnidadMedidaDetailComponent },

  // TRANSACCIONES
  { path: 'transacciones', component: TransaccionListComponent },
  { path: 'transacciones/nuevo', component: TransaccionFormComponent },
  { path: 'transacciones/editar/:id', component: TransaccionFormComponent },
  { path: 'transacciones/detalle/:id', component: TransaccionDetailComponent },

  // CONCEPTO AJUSTE
  { path: 'concepto-ajuste', component: ConceptoAjusteListComponent },
  { path: 'concepto-ajuste/nuevo', component: ConceptoAjusteFormComponent },
  { path: 'concepto-ajuste/editar/:id', component: ConceptoAjusteFormComponent },
  { path: 'concepto-ajuste/detalle/:id', component: ConceptoAjusteDetailComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InventarioRoutingModule { }
