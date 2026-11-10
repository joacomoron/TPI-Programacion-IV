using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Domain.Interfaces;

namespace ClinicaTurnos.Application.Services
{
    public class TurnoService : ITurnoService
    {
        private readonly ITurnoRepository _turnoRepository;

        public TurnoService(ITurnoRepository turnoRepository)
        {
            _turnoRepository = turnoRepository;
        }
    }
}
