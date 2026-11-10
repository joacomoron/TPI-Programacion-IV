using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Domain.Interfaces;

namespace ClinicaTurnos.Application.Services
{
    public class PagoService : IPagoService
    {
        private readonly IPagoRepository _pagoRepository;

        public PagoService(IPagoRepository pagoRepository)
        {
            _pagoRepository = pagoRepository;
        }
    }
}
