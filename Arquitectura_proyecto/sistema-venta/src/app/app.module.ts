import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router'; // Importar RouterModule y Routes
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';

// Configuración de las rutas
const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' }, // Redirige a login por defecto
  { path: 'login', component: LoginComponent },
  // Agrega otras rutas aquí si es necesario
];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    // Otros componentes pueden ir aquí
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule.forRoot(routes) // Configura el enrutamiento
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
