using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Domain.Interfaces;

namespace ClinicaTurnos.Application.Services
{
    public class ConsultorioService : IConsultorioService
    {
        private readonly IConsultorioRepository _consultorioRepository;

        public ConsultorioService(IConsultorioRepository consultorioRepository)
        {
            _consultorioRepository = consultorioRepository;
        }
    }
}
