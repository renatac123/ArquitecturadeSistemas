using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;
using SistemaVenta.Utility;
using Microsoft.AspNetCore.Authorization;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioServicio;
        private readonly Token _token;

        public UsuarioController(IUsuarioService usuarioServicio, Token token)
        {
            _usuarioServicio = usuarioServicio;
            _token = token;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var rsp = new Response<List<UsuarioDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await _usuarioServicio.Lista();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO login)
        {
            var rsp = new Response<SesionDTO>();


            try
            {
                /*
                rsp.status = true;
                rsp.value = await _usuarioServicio.ValidarCredenciales(login.Correo, login.Clave);*/
                // Validar las credenciales del usuario
                var usuarioValido = await _usuarioServicio.ValidarCredenciales(login.Correo, _token.encriptarSHA256(login.Clave));

                if (usuarioValido == null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
                }
                else
                {
                    // Si el usuario es válido, generar el JWT y devolverlo
                    string token = _token.generarJWT(usuarioValido);

                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = token });
                }
            }
            catch (Exception ex)
            {
                
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }


        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] UsuarioDTO usuario)
        {
            var rsp = new Response<UsuarioDTO>();

            try
            {
                // Encriptar la clave del usuario
                if (!string.IsNullOrEmpty(usuario.Clave))
                {
                    usuario.Clave = _token.encriptarSHA256(usuario.Clave);
                }

                rsp.status = true;
                rsp.value = await _usuarioServicio.Crear(usuario);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] UsuarioDTO usuario)
        {
            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _usuarioServicio.Editar(usuario);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _usuarioServicio.Eliminar(id);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }

    }
}
