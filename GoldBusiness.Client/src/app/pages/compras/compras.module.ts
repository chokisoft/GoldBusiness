import { NgModule, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ComprasListComponent } from './compras-list.component';
import { ComprasFormComponent } from './compras-form.component';
import { ComprasDetailComponent } from './compras-detail.component';

@Component({ selector: 'app-compras-home', template: `<div class="page-placeholder"><h2>Compras</h2><p>Módulo en desarrollo.</p></div>` })
export class ComprasHomeComponent { }

@Component({ selector: 'app-informe-recepcion', template: `<div class="page-placeholder"><h2>Informe de Recepción</h2></div>` })
export class InformeRecepcionComponent { }

@Component({ selector: 'app-nota-credito', template: `<div class="page-placeholder"><h2>Nota de Crédito (Compras)</h2></div>` })
export class NotaCreditoComprasComponent { }

@Component({ selector: 'app-nota-debito', template: `<div class="page-placeholder"><h2>Nota de Débito (Compras)</h2></div>` })
export class NotaDebitoComprasComponent { }

@Component({ selector: 'app-devolucion-compra', template: `<div class="page-placeholder"><h2>Devolución de Compra</h2></div>` })
export class DevolucionCompraComponent { }

@Component({ selector: 'app-transferencias-entrada', template: `<div class="page-placeholder"><h2>Transferencias de Entrada</h2></div>` })
export class TransferenciasEntradaComponent { }

@Component({ selector: 'app-vales-entrada', template: `<div class="page-placeholder"><h2>Vales de Entrada</h2></div>` })
export class ValesEntradaComponent { }

@NgModule({
  declarations: [
    ComprasHomeComponent,
    ComprasListComponent,
    ComprasFormComponent,
    ComprasDetailComponent,
    InformeRecepcionComponent,
    NotaCreditoComprasComponent,
    NotaDebitoComprasComponent,
    DevolucionCompraComponent,
    TransferenciasEntradaComponent,
    ValesEntradaComponent
  ],
  imports: [CommonModule, RouterModule.forChild([
    { path: '', component: ComprasListComponent },
    { path: 'nuevo', component: ComprasFormComponent },
    { path: 'editar/:id', component: ComprasFormComponent },
    { path: ':id', component: ComprasDetailComponent },
    { path: 'informe-recepcion', component: InformeRecepcionComponent },
    { path: 'nota-credito', component: NotaCreditoComprasComponent },
    { path: 'nota-debito', component: NotaDebitoComprasComponent },
    { path: 'devolucion', component: DevolucionCompraComponent },
    { path: 'transferencias-entrada', component: TransferenciasEntradaComponent },
    { path: 'vales-entrada', component: ValesEntradaComponent }
  ])]
})
export class ComprasModule { }

