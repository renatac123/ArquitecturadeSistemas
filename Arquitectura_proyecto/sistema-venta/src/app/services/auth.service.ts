import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginDTO, SesionDTO } from '../models/models'; // Asegúrate de definir estos modelos

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'http://localhost:27584/api/Usuario'; // Cambia esto por la URL de tu API

  constructor(private http: HttpClient) {}

  iniciarSesion(login: LoginDTO): Observable<SesionDTO> {
    console.log("entro al servicio")
    return this.http.post<SesionDTO>(`${this.apiUrl}/IniciarSesion`, login, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    });
  }
}
