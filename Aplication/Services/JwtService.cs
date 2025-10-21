using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration; 

        public JwtService(IConfiguration configuration) {
            _configuration = configuration;
        }
        public string GenerateAccessToken(string id)
        {         
            var claims = new[]
            {
                // Subject del token (puede ser el userId o email)
                new Claim(JwtRegisteredClaimNames.Sub, id),
                // JTI único para cada token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                // Issued At (en formato Unix timestamp)
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };

            // Generar la clave de firma a partir del secreto de configuración
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            // Configurar credenciales de firma con algoritmo HMAC-SHA256
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],                          // Quién emite el token
                //audience: JwtConfigHelper.GetValidAudience(_configuration, _environment),
                claims: claims,                                             
                expires: DateTime.UtcNow.AddMinutes(10),                  
                signingCredentials: signingCredentials                        
            );

            // Serializar el token a string para enviar al cliente
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
