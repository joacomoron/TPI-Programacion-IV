using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Domain.Interfaces;

namespace ClinicaTurnos.Application.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _pacienteRepository;

        public PacienteService(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }
    }
}
