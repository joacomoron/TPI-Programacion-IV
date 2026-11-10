using ClinicaTurnos.Domain.Entities;
using ClinicaTurnos.Domain.Interfaces;
using ClinicaTurnos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaTurnos.Infrastructure.Repositories
{
    public class MedicoRepository : RepositoryBase<Medico>, IMedicoRepository
    {
        public MedicoRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<List<Medico>> GetByEspecialidadAsync(string especialidad)
        {
            return await _context.Medicos
                .Where(m => m.Especialidad == especialidad)
                .ToListAsync();
        }

        public async Task<bool> ExistsByMatriculaAsync(string matricula)
        {
            return await _context.Medicos.AnyAsync(m => m.Matricula == matricula);
        }
    }
}
