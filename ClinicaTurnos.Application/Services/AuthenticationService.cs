using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Domain.Interfaces;

namespace ClinicaTurnos.Application.Services
{
    // Para promoción: valida las credenciales contra Paciente y genera el JWT
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPacienteRepository _pacienteRepository;

        public AuthenticationService(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }
    }
}
