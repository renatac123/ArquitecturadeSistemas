import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders  } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../enviroment';


@Injectable({
  providedIn: 'root'
})
export class ProductoService {
  private apiUrl = environment.apiUrl+'/Producto/Lista';

  constructor(private http: HttpClient) { }

  obtenerProductos(): Observable<Producto[]> {
    //return this.http.get<Producto[]>(this.apiUrl);
    const token = sessionStorage.getItem('authToken'); // Obtener el token de sessionStorage

    // Configurar los encabezados con el token de autorización
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`, // Usar el formato adecuado para el token
    });

    //return this.http.get<Producto[]>(this.apiUrl, { headers }); // Pasar los encabezados en la solicitud

    return this.http.get<{ status: boolean; value: Producto[] }>(this.apiUrl, { headers })
      .pipe(
        map(response => response.value) // Extraer solo el array de productos
      );
  }
  
}

export interface Producto {
    idProducto?: string;         // Identificador único del producto
    nombre: string;             // Nombre del producto
    idCategoria?: string;        // Identificador de la categoría del producto
    descripcionCategoria: string; // Descripción de la categoría
    stock: string;              // Cantidad disponible en stock
    precio: number;             // Precio del producto (se mantiene como string si viene así del backend)
    esActivo?: string;           // Estado activo del producto (1 para activo, 0 para inactivo)
  }
  