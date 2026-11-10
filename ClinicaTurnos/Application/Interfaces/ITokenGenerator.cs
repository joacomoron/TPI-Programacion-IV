using ClinicaTurnos.Domain.Entities;

namespace ClinicaTurnos.Application.Interfaces
{
    // Application solo sabe que "alguien" genera tokens; cómo (JWT, clave, algoritmo) lo resuelve Web/Security
    public interface ITokenGenerator
    {
        string GenerarToken(Paciente paciente);
    }
}
