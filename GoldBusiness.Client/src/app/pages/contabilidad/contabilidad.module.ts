import { NgModule, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ContabilidadListComponent } from './contabilidad-list.component';
import { ContabilidadFormComponent } from './contabilidad-form.component';
import { ContabilidadDetailComponent } from './contabilidad-detail.component';

@Component({ selector: 'app-contabilidad-home', template: `<div class="page-placeholder"><h2>Contabilidad</h2><p>Módulo en desarrollo.</p></div>` })
export class ContabilidadHomeComponent { }

@Component({ selector: 'app-generar-comprobante', template: `<div class="page-placeholder"><h2>Generar Comprobante</h2><p>Placeholder.</p></div>` })
export class GenerarComprobanteComponent { }

@Component({ selector: 'app-comprobante', template: `<div class="page-placeholder"><h2>Comprobante</h2><p>Placeholder.</p></div>` })
export class ComprobanteComponent { }

@Component({ selector: 'app-cuentas-por-pagar', template: `<div class="page-placeholder"><h2>Cuentas por Pagar</h2><p>Placeholder.</p></div>` })
export class CuentasPorPagarComponent { }

@Component({ selector: 'app-cuentas-por-cobrar', template: `<div class="page-placeholder"><h2>Cuentas por Cobrar</h2><p>Placeholder.</p></div>` })
export class CuentasPorCobrarComponent { }

@Component({ selector: 'app-balance-general', template: `<div class="page-placeholder"><h2>Balance General</h2><p>Placeholder.</p></div>` })
export class BalanceGeneralComponent { }

@Component({ selector: 'app-estado-resultados', template: `<div class="page-placeholder"><h2>Estado de Resultados</h2><p>Placeholder.</p></div>` })
export class EstadoResultadosComponent { }

@NgModule({
  declarations: [
    ContabilidadHomeComponent,
    ContabilidadListComponent,
    ContabilidadFormComponent,
    ContabilidadDetailComponent,
    GenerarComprobanteComponent,
    ComprobanteComponent,
    CuentasPorPagarComponent,
    CuentasPorCobrarComponent,
    BalanceGeneralComponent,
    EstadoResultadosComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: '', component: ContabilidadListComponent },
      { path: 'nuevo', component: ContabilidadFormComponent },
      { path: 'editar/:id', component: ContabilidadFormComponent },
      { path: ':id', component: ContabilidadDetailComponent },
      { path: 'generar-comprobante', component: GenerarComprobanteComponent },
      { path: 'comprobante', component: ComprobanteComponent },
      { path: 'cuentas-por-pagar', component: CuentasPorPagarComponent },
      { path: 'cuentas-por-cobrar', component: CuentasPorCobrarComponent },
      { path: 'balance-general', component: BalanceGeneralComponent },
      { path: 'estado-resultados', component: EstadoResultadosComponent }
    ])
  ]
})
export class ContabilidadModule { }
