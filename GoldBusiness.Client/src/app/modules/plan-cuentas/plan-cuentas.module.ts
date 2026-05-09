import { NgModule } from '@angular/core';
import { PlanCuentasRoutingModule } from './plan-cuentas-routing.module';
import { SharedModule } from '../../shared/shared.module';

// ============================================
// 📁 GRUPO CUENTA
// ============================================
import { GrupoCuentaListComponent } from '../../pages/grupoCuenta/grupo-cuenta-list/grupo-cuenta-list.component';
import { GrupoCuentaFormComponent } from '../../pages/grupoCuenta/grupo-cuenta-form/grupo-cuenta-form.component';
import { GrupoCuentaDetailComponent } from '../../pages/grupoCuenta/grupo-cuenta-detail/grupo-cuenta-detail.component';

// ============================================
// 📁 SUBGRUPO CUENTA
// ============================================
import { SubGrupoCuentaListComponent } from '../../pages/subGrupoCuenta/subgrupo-cuenta-list/subgrupo-cuenta-list.component';
import { SubGrupoCuentaFormComponent } from '../../pages/subGrupoCuenta/subgrupo-cuenta-form/subgrupo-cuenta-form.component';
import { SubGrupoCuentaDetailComponent } from '../../pages/subGrupoCuenta/subgrupo-cuenta-detail/subgrupo-cuenta-detail.component';

// ============================================
// 📁 CUENTA
// ============================================
import { CuentaListComponent } from '../../pages/cuenta/cuenta-list/cuenta-list.component';
import { CuentaFormComponent } from '../../pages/cuenta/cuenta-form/cuenta-form.component';
import { CuentaDetailComponent } from '../../pages/cuenta/cuenta-detail/cuenta-detail.component';

@NgModule({
  declarations: [
    // Grupo Cuenta
    GrupoCuentaListComponent,
    GrupoCuentaFormComponent,
    GrupoCuentaDetailComponent,
    
    // SubGrupo Cuenta
    SubGrupoCuentaListComponent,
    SubGrupoCuentaFormComponent,
    SubGrupoCuentaDetailComponent,
    
    // Cuenta
    CuentaListComponent,
    CuentaFormComponent,
    CuentaDetailComponent,
  ],
  imports: [
    SharedModule,
    PlanCuentasRoutingModule
  ]
})
export class PlanCuentasModule { }

