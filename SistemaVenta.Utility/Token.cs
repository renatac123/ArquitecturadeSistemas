using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.Utility
{
    public class Token
    {
        private readonly IConfiguration _configuration;

        public Token(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string encriptarSHA256(string texto)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                //Computar el hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto)); //de bytes a array de bytes

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public string generarJWT(Usuario modelo)
        {
            //crear la informacion del usuario para token
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, modelo.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, modelo.Correo!)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //crear detalle del token
            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10), //el token dura 10 minutos
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }


    }
}
