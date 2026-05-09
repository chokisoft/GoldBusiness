import { NgModule, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MovimientosListComponent } from './movimientos-list.component';
import { MovimientosFormComponent } from './movimientos-form.component';
import { MovimientosDetailComponent } from './movimientos-detail.component';

@Component({ selector: 'app-movimientos-home', template: `<div class="page-placeholder"><h2>Movimientos</h2><p>Módulo en desarrollo.</p></div>` })
export class MovimientosHomeComponent { }

@Component({ selector: 'app-toma-fisica', template: `<div class="page-placeholder"><h2>Toma Física</h2></div>` })
export class TomaFisicaComponent { }

@Component({ selector: 'app-transferencias-internas', template: `<div class="page-placeholder"><h2>Transferencias Internas</h2></div>` })
export class TransferenciasInternasComponent { }

@Component({ selector: 'app-ajuste-positivo', template: `<div class="page-placeholder"><h2>Ajuste Positivo</h2></div>` })
export class AjustePositivoComponent { }

@Component({ selector: 'app-ajuste-negativo', template: `<div class="page-placeholder"><h2>Ajuste Negativo</h2></div>` })
export class AjusteNegativoComponent { }

@Component({ selector: 'app-ajuste-valor', template: `<div class="page-placeholder"><h2>Ajuste en Valor</h2></div>` })
export class AjusteValorComponent { }

@NgModule({
  declarations: [
    MovimientosHomeComponent,
    MovimientosListComponent,
    MovimientosFormComponent,
    MovimientosDetailComponent,
    TomaFisicaComponent,
    TransferenciasInternasComponent,
    AjustePositivoComponent,
    AjusteNegativoComponent,
    AjusteValorComponent
  ],
  imports: [CommonModule, RouterModule.forChild([
    // Feature standard routes
    { path: '', component: MovimientosListComponent },
    { path: 'nuevo', component: MovimientosFormComponent },
    { path: 'editar/:id', component: MovimientosFormComponent },
    { path: ':id', component: MovimientosDetailComponent },

    // Additional subfeatures
    { path: 'toma-fisica', component: TomaFisicaComponent },
    { path: 'transferencias-internas', component: TransferenciasInternasComponent },
    { path: 'ajuste-positivo', component: AjustePositivoComponent },
    { path: 'ajuste-negativo', component: AjusteNegativoComponent },
    { path: 'ajuste-valor', component: AjusteValorComponent }
  ])]
})
export class MovimientosModule { }
