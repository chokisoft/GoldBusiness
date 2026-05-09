import { NgModule, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({ selector: 'app-nomina-home', template: `<div class="page-placeholder"><h2>Nómina</h2><p>Módulo en desarrollo.</p></div>` })
export class NominaHomeComponent { }

@Component({ selector: 'app-entrada-registros', template: `<div class="page-placeholder"><h2>Entrada de registros</h2></div>` })
export class EntradaRegistrosComponent { }

@Component({ selector: 'app-calculo-nomina', template: `<div class="page-placeholder"><h2>Cálculo de Nómina</h2></div>` })
export class CalculoNominaComponent { }

@NgModule({
  declarations: [NominaHomeComponent, EntradaRegistrosComponent, CalculoNominaComponent],
  imports: [CommonModule, RouterModule.forChild([
    { path: '', component: NominaHomeComponent },
    { path: 'entrada-registros', component: EntradaRegistrosComponent },
    { path: 'calculo', component: CalculoNominaComponent }
  ])]
})
export class NominaModule { }
