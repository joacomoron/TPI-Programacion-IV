using ClinicaTurnos.Domain.Entities;
using ClinicaTurnos.Domain.Interfaces;
using ClinicaTurnos.Infrastructure.Data;

namespace ClinicaTurnos.Infrastructure.Repositories
{
    public class MedicoRepository : RepositoryBase<Medico>, IMedicoRepository
    {
        public MedicoRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
