import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// ============================================
// 📁 PLAN DE CUENTAS - LAZY MODULE
// ============================================
import { GrupoCuentaListComponent } from '../../pages/grupoCuenta/grupo-cuenta-list/grupo-cuenta-list.component';
import { GrupoCuentaFormComponent } from '../../pages/grupoCuenta/grupo-cuenta-form/grupo-cuenta-form.component';
import { GrupoCuentaDetailComponent } from '../../pages/grupoCuenta/grupo-cuenta-detail/grupo-cuenta-detail.component';

import { SubGrupoCuentaListComponent } from '../../pages/subGrupoCuenta/subgrupo-cuenta-list/subgrupo-cuenta-list.component';
import { SubGrupoCuentaFormComponent } from '../../pages/subGrupoCuenta/subgrupo-cuenta-form/subgrupo-cuenta-form.component';
import { SubGrupoCuentaDetailComponent } from '../../pages/subGrupoCuenta/subgrupo-cuenta-detail/subgrupo-cuenta-detail.component';

import { CuentaListComponent } from '../../pages/cuenta/cuenta-list/cuenta-list.component';
import { CuentaFormComponent } from '../../pages/cuenta/cuenta-form/cuenta-form.component';
import { CuentaDetailComponent } from '../../pages/cuenta/cuenta-detail/cuenta-detail.component';

const routes: Routes = [
  // GRUPO CUENTA
  { path: 'grupo-cuenta', component: GrupoCuentaListComponent },
  { path: 'grupo-cuenta/nuevo', component: GrupoCuentaFormComponent },
  { path: 'grupo-cuenta/editar/:id', component: GrupoCuentaFormComponent },
  { path: 'grupo-cuenta/detalle/:id', component: GrupoCuentaDetailComponent },

  // SUBGRUPO CUENTA
  { path: 'subgrupo-cuenta', component: SubGrupoCuentaListComponent },
  { path: 'subgrupo-cuenta/nuevo', component: SubGrupoCuentaFormComponent },
  { path: 'subgrupo-cuenta/editar/:id', component: SubGrupoCuentaFormComponent },
  { path: 'subgrupo-cuenta/detalle/:id', component: SubGrupoCuentaDetailComponent },

  // CUENTA
  { path: 'cuenta', component: CuentaListComponent },
  { path: 'cuenta/nuevo', component: CuentaFormComponent },
  { path: 'cuenta/editar/:id', component: CuentaFormComponent },
  { path: 'cuenta/detalle/:id', component: CuentaDetailComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlanCuentasRoutingModule { }
