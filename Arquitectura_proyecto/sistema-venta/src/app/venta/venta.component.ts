import { Component, OnInit } from '@angular/core';
import { ProductoService , Producto } from '../services/producto.service'; 

interface ItemCarrito {
  nombre: string;
  precio: number;
  cantidad: number;
}

@Component({
  selector: 'app-venta',
  templateUrl: './venta.component.html',
  styleUrls: ['./venta.component.css']
})
export class VentaComponent implements OnInit {
  productos: Producto[] = [];
  carrito: ItemCarrito[] = [];
  productoSeleccionado: Producto | null = null; // Variable para el producto seleccionado

  displayedColumns: string[] = ['nombre', 'descripcionCategoria', 'stock', 'precio', 'seleccionar'];
  carritoColumns: string[] = ['nombre', 'cantidad', 'precio', 'remover'];

  constructor(private productoService: ProductoService) { }

  ngOnInit(): void {
    this.cargarProductos();
  }

  cargarProductos(): void {
    this.productoService.obtenerProductos().subscribe(
      (productos: Producto[]) => {
        this.productos = productos;
      },
      (error) => {
        console.error('Error al cargar los productos', error);
        // Maneja el error aquí (por ejemplo, mostrar un mensaje al usuario)
      }
    );
  }

  seleccionarProducto(producto: Producto): void {
    this.productoSeleccionado = producto; // Guardamos el producto seleccionado
  }

  agregarAlCarrito(producto: Producto): void {
    const item = this.carrito.find(item => item.nombre === producto.nombre);
    if (item) {
      item.cantidad++;
    } else {
      this.carrito.push({ nombre: producto.nombre, precio: producto.precio, cantidad: 1 });
    }
  }

  removerDelCarrito(item: ItemCarrito): void {
    this.carrito = this.carrito.filter(i => i.nombre !== item.nombre);
  }

  calcularTotal(): number {
    return this.carrito.reduce((total, item) => total + item.precio * item.cantidad, 0);
  }

  finalizarCompra(): void {
    alert('Compra finalizada con éxito!');
    this.carrito = []; // Limpiar el carrito después de finalizar la compra
  }
}
