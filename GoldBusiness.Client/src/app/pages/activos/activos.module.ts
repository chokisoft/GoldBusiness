import { NgModule, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({ selector: 'app-activos-home', template: `<div class="page-placeholder"><h2>Activos Fijos</h2><p>Módulo en desarrollo.</p></div>` })
export class ActivosHomeComponent { }

@Component({ selector: 'app-catalogo-activos', template: `<div class="page-placeholder"><h2>Catálogo de Activos</h2></div>` })
export class CatalogoActivosComponent { }

@Component({ selector: 'app-alta-activo', template: `<div class="page-placeholder"><h2>Alta de Activo</h2></div>` })
export class AltaActivoComponent { }

@Component({ selector: 'app-baja-activo', template: `<div class="page-placeholder"><h2>Baja de Activo</h2></div>` })
export class BajaActivoComponent { }

@Component({ selector: 'app-depreciacion', template: `<div class="page-placeholder"><h2>Depreciación</h2></div>` })
export class DepreciacionComponent { }

@NgModule({
  declarations: [
    ActivosHomeComponent,
    CatalogoActivosComponent,
    AltaActivoComponent,
    BajaActivoComponent,
    DepreciacionComponent
  ],
  imports: [CommonModule, RouterModule.forChild([
    { path: '', component: ActivosHomeComponent },
    { path: 'catalogo', component: CatalogoActivosComponent },
    { path: 'alta', component: AltaActivoComponent },
    { path: 'baja', component: BajaActivoComponent },
    { path: 'depreciacion', component: DepreciacionComponent }
  ])]
})
export class ActivosModule { }
