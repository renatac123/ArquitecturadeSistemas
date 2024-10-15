
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../enviroments/enviroments';
import { ResponseApi } from '../../interfaces/response-api';
import { Usuario } from '../../interfaces/usuario';
import { Login } from '../../interfaces/login';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {
  // private urlApi: string = environment.endpoint + "Usuario/"
  constructor(private http: HttpClient) { }

  iniciarSesion(request: Login): Observable<ResponseApi> {
    return this.http.post<ResponseApi>('${this.urlApi}IniciarSesion', request)
  }
  lista(): Observable<ResponseApi> {
    return this.http.get<ResponseApi>('${this.urlApi}Lista')
  }
  guardar(request: Usuario): Observable<ResponseApi> {
    return this.http.post<ResponseApi>('${this.urlApi}Guardar', request)
  }
  editar(request: Usuario): Observable<ResponseApi> {
    return this.http.post<ResponseApi>('${this.urlApi}Editar', request)
  }
  eliminar(id: number): Observable<ResponseApi> {
    return this.http.delete<ResponseApi>('${this.urlApi}Eliminar/${id}')
  }
}
