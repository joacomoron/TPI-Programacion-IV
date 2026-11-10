using ClinicaTurnos.Application.Exceptions;
using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Auth;
using ClinicaTurnos.Domain.Interfaces;

namespace ClinicaTurnos.Application.Services
{
    // El diagrama no tiene una entidad Usuario, así que el login es del Paciente: Email + DNI
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthenticationService(IPacienteRepository pacienteRepository, ITokenGenerator tokenGenerator)
        {
            _pacienteRepository = pacienteRepository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var paciente = await _pacienteRepository.GetByEmailAsync(request.Email);

            // Mismo mensaje para "no existe" y "DNI incorrecto": no le damos pistas a un atacante
            if (paciente is null || paciente.Dni != request.Dni)
                throw new UnauthorizedException("Email o DNI incorrectos");

            return new LoginResponse
            {
                Token = _tokenGenerator.GenerarToken(paciente),
                PacienteId = paciente.Id,
                Paciente = $"{paciente.Nombre} {paciente.Apellido}"
            };
        }
    }
}
