using ClinicaTurnos.Domain.Entities;
using ClinicaTurnos.Domain.Interfaces;
using ClinicaTurnos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaTurnos.Infrastructure.Repositories
{
    public class ConsultorioRepository : RepositoryBase<Consultorio>, IConsultorioRepository
    {
        public ConsultorioRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByNumeroAsync(int nroConsultorio)
        {
            return await _context.Consultorios.AnyAsync(c => c.NroConsultorio == nroConsultorio);
        }
    }
}
