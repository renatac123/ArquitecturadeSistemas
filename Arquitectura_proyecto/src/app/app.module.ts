import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { LayoutComponent } from './Components/layout/layout.component';
import { LoginComponent } from './Components/login/login.component';
import { AppRoutingModule } from './app-routing.module';
import { SharedModule } from './Reutilizable/shared/shared.module';
@NgModule({
  declarations: [
    AppComponent,
    LayoutComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    SharedModule,
    BrowserModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
