import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service'; // Asegúrate de que esta ruta sea correcta
import { LoginDTO } from '../../models/models'; // Asegúrate de definir este modelo
import { Router } from '@angular/router'; // Importa Router

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) { // Agrega Router al constructor
    this.loginForm = this.fb.group({
      correo: ['', [Validators.required, Validators.email]],
      clave: ['', [Validators.required]],
    });
  }

  ngOnInit(): void {}

  onSubmit(): void {
    if (this.loginForm.valid) {
      const loginData: LoginDTO = {
        correo: this.loginForm.get('correo')?.value,
        clave: this.loginForm.get('clave')?.value,
      };

      this.authService.iniciarSesion(loginData).subscribe(
        (response: any) => {
          console.log('Inicio de sesión exitoso', response);
          
                 // Guardar el token en sessionStorage
                 if (response.isSuccess && response.token) {
                  sessionStorage.setItem('authToken', response.token);
                  // Redirigir a la pantalla de venta
                  this.router.navigate(['/venta']);
                }


        },
        (error: any) => {
          console.error('Error al iniciar sesión', error);
          // Maneja el error aquí (por ejemplo, mostrar un mensaje al usuario)
        }
      );
    }
  }
}
