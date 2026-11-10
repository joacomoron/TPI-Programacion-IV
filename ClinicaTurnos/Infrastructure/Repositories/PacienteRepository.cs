using ClinicaTurnos.Domain.Entities;
using ClinicaTurnos.Domain.Interfaces;
using ClinicaTurnos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaTurnos.Infrastructure.Repositories
{
    public class PacienteRepository : RepositoryBase<Paciente>, IPacienteRepository
    {
        public PacienteRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<Paciente?> GetByEmailAsync(string email)
        {
            return await _context.Pacientes.FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<bool> ExistsByDniAsync(string dni)
        {
            return await _context.Pacientes.AnyAsync(p => p.Dni == dni);
        }
    }
}
