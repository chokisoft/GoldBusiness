import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestConnectionComponent } from './components/test-connection/test-connection.component';
import { LoginComponent } from './components/login/login.component';
import { authGuard } from './guards/auth.guard';

const routes: Routes = [
  // Ruta pública de login
  {
    path: 'login',
    component: LoginComponent
  },

  // Rutas protegidas
  {
    path: 'test',
    component: TestConnectionComponent,
    canActivate: [authGuard] // ← Protegida
  },

  // Ruta por defecto
  {
    path: '',
    redirectTo: '/test',
    pathMatch: 'full'
  },

  // Ruta 404 (opcional)
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
