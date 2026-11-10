using ClinicaTurnos.Domain.Entities;
using ClinicaTurnos.Domain.Interfaces;
using ClinicaTurnos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaTurnos.Infrastructure.Repositories
{
    public class PagoRepository : RepositoryBase<Pago>, IPagoRepository
    {
        public PagoRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<Pago?> GetByTurnoIdAsync(int turnoId)
        {
            return await _context.Pagos.FirstOrDefaultAsync(p => p.TurnoId == turnoId);
        }
    }
}
