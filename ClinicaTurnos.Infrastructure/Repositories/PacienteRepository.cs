using ClinicaTurnos.Domain.Entities;
using ClinicaTurnos.Domain.Interfaces;
using ClinicaTurnos.Infrastructure.Data;

namespace ClinicaTurnos.Infrastructure.Repositories
{
    public class PacienteRepository : RepositoryBase<Paciente>, IPacienteRepository
    {
        public PacienteRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
