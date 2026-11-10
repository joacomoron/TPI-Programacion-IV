using ClinicaTurnos.Domain.Entities;
using ClinicaTurnos.Domain.Interfaces;
using ClinicaTurnos.Infrastructure.Data;

namespace ClinicaTurnos.Infrastructure.Repositories
{
    public class PagoRepository : RepositoryBase<Pago>, IPagoRepository
    {
        public PagoRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
