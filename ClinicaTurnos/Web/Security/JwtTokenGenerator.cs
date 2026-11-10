using System.Security.Claims;
using System.Text;
using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Domain.Entities;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace ClinicaTurnos.Web.Security
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerarToken(Paciente paciente)
        {
            // El secret NO está en el código: sale de la configuración
            // (appsettings.Development.json en local, variable de entorno Jwt__Secret en Azure)
            var secret = _configuration["Jwt:Secret"]!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, paciente.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, paciente.Email),
                    new Claim("nombre", $"{paciente.Nombre} {paciente.Apellido}")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            return new JsonWebTokenHandler().CreateToken(descriptor);
        }
    }
}
