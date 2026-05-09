import { NgModule } from '@angular/core';
import { InventarioRoutingModule } from './inventario-routing.module';
import { SharedModule } from '../../shared/shared.module';

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

@NgModule({
  declarations: [
    // Productos
    ProductoListComponent,
    ProductoFormComponent,
    ProductoDetailComponent,
    
    // Línea
    LineaListComponent,
    LineaFormComponent,
    LineaDetailComponent,
    
    // SubLínea
    SubLineaListComponent,
    SubLineaFormComponent,
    SubLineaDetailComponent,
    
    // Unidad de Medida
    UnidadMedidaListComponent,
    UnidadMedidaFormComponent,
    UnidadMedidaDetailComponent,
    
    // Transacciones
    TransaccionListComponent,
    TransaccionFormComponent,
    TransaccionDetailComponent,
    
    // Concepto Ajuste
    ConceptoAjusteListComponent,
    ConceptoAjusteFormComponent,
    ConceptoAjusteDetailComponent,
  ],
  imports: [
    SharedModule,
    InventarioRoutingModule
  ]
})
export class InventarioModule { }
