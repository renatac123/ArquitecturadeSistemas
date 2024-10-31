import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { VentaComponent } from './venta/venta.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'venta', component: VentaComponent }, 

  { path: '', redirectTo: '/login', pathMatch: 'full' },
  // otras rutas aquí
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}

