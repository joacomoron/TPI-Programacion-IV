using ClinicaTurnos.Domain.Entities;
using ClinicaTurnos.Domain.Enums;
using ClinicaTurnos.Domain.Interfaces;
using ClinicaTurnos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaTurnos.Infrastructure.Repositories
{
    public class TurnoRepository : RepositoryBase<Turno>, ITurnoRepository
    {
        public TurnoRepository(ApplicationContext context) : base(context)
        {
        }

        // Trae el turno junto con sus entidades relacionadas (JOINs)
        private IQueryable<Turno> ConDetalles()
        {
            return _context.Turnos
                .Include(t => t.Paciente)
                .Include(t => t.Medico)
                .Include(t => t.Consultorio)
                .Include(t => t.Pago);
        }

        public async Task<List<Turno>> GetAllWithDetailsAsync()
        {
            return await ConDetalles()
                .OrderBy(t => t.Fecha)
                .ThenBy(t => t.Hora)
                .ToListAsync();
        }

        public async Task<Turno?> GetByIdWithDetailsAsync(int id)
        {
            return await ConDetalles().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Turno>> GetByPacienteAsync(int pacienteId)
        {
            return await ConDetalles()
                .Where(t => t.PacienteId == pacienteId)
                .OrderBy(t => t.Fecha)
                .ThenBy(t => t.Hora)
                .ToListAsync();
        }

        public async Task<List<Turno>> GetActivosByMedicoAndFechaAsync(int medicoId, DateOnly fecha)
        {
            return await _context.Turnos
                .Where(t => t.MedicoId == medicoId
                         && t.Fecha == fecha
                         && t.Estado != EstadoTurno.Cancelado)
                .ToListAsync();
        }

        // Hay superposición si a esa fecha y hora ya está ocupado el médico o el consultorio
        public async Task<bool> ExisteSuperposicionAsync(int medicoId, int consultorioId, DateOnly fecha, TimeOnly hora)
        {
            return await _context.Turnos.AnyAsync(t =>
                t.Fecha == fecha
                && t.Hora == hora
                && t.Estado != EstadoTurno.Cancelado
                && (t.MedicoId == medicoId || t.ConsultorioId == consultorioId));
        }
    }
}
