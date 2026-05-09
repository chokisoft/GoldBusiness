import { NgModule, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({ selector: 'app-consultas-home', template: `<div class="page-placeholder"><h2>Consultas</h2><p>Módulo en desarrollo.</p></div>` })
export class ConsultasHomeComponent { }

@Component({ selector: 'app-consulta-general', template: `<div class="page-placeholder"><h2>Consulta General</h2></div>` })
export class ConsultaGeneralComponent { }

@Component({ selector: 'app-historico-producto', template: `<div class="page-placeholder"><h2>Histórico de Producto</h2></div>` })
export class HistoricoProductoComponent { }

@NgModule({
  declarations: [ConsultasHomeComponent, ConsultaGeneralComponent, HistoricoProductoComponent],
  imports: [CommonModule, RouterModule.forChild([
    { path: '', component: ConsultasHomeComponent },
    { path: 'general', component: ConsultaGeneralComponent },
    { path: 'historico-producto', component: HistoricoProductoComponent }
  ])]
})
export class ConsultasModule { }
