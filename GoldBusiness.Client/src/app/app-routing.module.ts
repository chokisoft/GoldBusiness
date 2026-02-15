import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestConnectionComponent } from './components/test-connection/test-connection.component';
import { LoginComponent } from './components/login/login.component';
import { MainLayoutComponent } from './components/layout/main-layout.component';
import { authGuard } from './guards/auth.guard';

// GrupoCuenta
import { GrupoCuentaListComponent } from './pages/grupo-cuenta/grupo-cuenta-list/grupo-cuenta-list.component';
import { GrupoCuentaFormComponent } from './pages/grupo-cuenta/grupo-cuenta-form/grupo-cuenta-form.component';

// SubGrupoCuenta
import { SubGrupoCuentaListComponent } from './pages/subgrupo-cuenta/subgrupo-cuenta-list/subgrupo-cuenta-list.component';
import { SubGrupoCuentaFormComponent } from './pages/subgrupo-cuenta/subgrupo-cuenta-form/subgrupo-cuenta-form.component';

// Cuenta
import { CuentaListComponent } from './pages/cuenta/cuenta-list/cuenta-list.component';
import { CuentaFormComponent } from './pages/cuenta/cuenta-form/cuenta-form.component';

const routes: Routes = [
  // Ruta pública de login
  {
    path: 'login',
    component: LoginComponent
  },

  // Rutas protegidas con el nuevo layout
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      { 
        path: '', 
        redirectTo: 'inicio', 
        pathMatch: 'full' 
      },
      {
        path: 'inicio',
        component: TestConnectionComponent
      },
      {
        path: 'acerca',
        component: TestConnectionComponent
      },
      // GrupoCuenta
      {
        path: 'nomencladores/grupo-cuenta',
        children: [
          { path: '', component: GrupoCuentaListComponent },
          { path: 'nuevo', component: GrupoCuentaFormComponent },
          { path: 'editar/:id', component: GrupoCuentaFormComponent }
        ]
      },
      // SubGrupoCuenta
      {
        path: 'nomencladores/subgrupo-cuenta',
        children: [
          { path: '', component: SubGrupoCuentaListComponent },
          { path: 'nuevo', component: SubGrupoCuentaFormComponent },
          { path: 'editar/:id', component: SubGrupoCuentaFormComponent }
        ]
      },
      // Cuenta
      {
        path: 'nomencladores/cuenta',
        children: [
          { path: '', component: CuentaListComponent },
          { path: 'nuevo', component: CuentaFormComponent },
          { path: 'editar/:id', component: CuentaFormComponent }
        ]
      },
      {
        path: 'test',
        component: TestConnectionComponent
      }
    ]
  },

  // Ruta 404
  {
    path: '**',
    redirectTo: '/login'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
