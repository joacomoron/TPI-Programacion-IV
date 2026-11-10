using ClinicaTurnos.Domain.Entities;
using ClinicaTurnos.Domain.Interfaces;
using ClinicaTurnos.Infrastructure.Data;

namespace ClinicaTurnos.Infrastructure.Repositories
{
    public class ConsultorioRepository : RepositoryBase<Consultorio>, IConsultorioRepository
    {
        public ConsultorioRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
