using ClinicaTurnos.Domain.Entities;
using ClinicaTurnos.Domain.Interfaces;
using ClinicaTurnos.Infrastructure.Data;

namespace ClinicaTurnos.Infrastructure.Repositories
{
    public class TurnoRepository : RepositoryBase<Turno>, ITurnoRepository
    {
        public TurnoRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
