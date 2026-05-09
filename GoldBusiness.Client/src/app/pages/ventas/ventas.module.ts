import { NgModule, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { VentasListComponent } from './ventas-list.component';
import { VentasFormComponent } from './ventas-form.component';
import { VentasDetailComponent } from './ventas-detail.component';

@Component({ selector: 'app-ventas-home', template: `<div class="page-placeholder"><h2>Ventas</h2><p>Módulo en desarrollo.</p></div>` })
export class VentasHomeComponent { }

@Component({ selector: 'app-factura', template: `<div class="page-placeholder"><h2>Factura</h2></div>` })
export class FacturaComponent { }

@Component({ selector: 'app-nota-credito-ventas', template: `<div class="page-placeholder"><h2>Nota de Crédito (Ventas)</h2></div>` })
export class NotaCreditoVentasComponent { }

@Component({ selector: 'app-nota-debito-ventas', template: `<div class="page-placeholder"><h2>Nota de Débito (Ventas)</h2></div>` })
export class NotaDebitoVentasComponent { }

@NgModule({
  declarations: [VentasHomeComponent, VentasListComponent, VentasFormComponent, VentasDetailComponent, FacturaComponent, NotaCreditoVentasComponent, NotaDebitoVentasComponent],
  imports: [CommonModule, RouterModule.forChild([
    { path: '', component: VentasListComponent },
    { path: 'nuevo', component: VentasFormComponent },
    { path: 'editar/:id', component: VentasFormComponent },
    { path: ':id', component: VentasDetailComponent },
    { path: 'factura', component: FacturaComponent },
    { path: 'nota-credito', component: NotaCreditoVentasComponent },
    { path: 'nota-debito', component: NotaDebitoVentasComponent }
  ])]
})
export class VentasModule { }
