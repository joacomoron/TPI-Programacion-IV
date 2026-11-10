using ClinicaTurnos.Domain.Entities;

namespace ClinicaTurnos.Domain.Interfaces
{
    public interface ITurnoRepository : IRepositoryBase<Turno>
    {
        Task<List<Turno>> GetAllWithDetailsAsync();
        Task<Turno?> GetByIdWithDetailsAsync(int id);
        Task<List<Turno>> GetByPacienteAsync(int pacienteId);
        Task<List<Turno>> GetActivosByMedicoAndFechaAsync(int medicoId, DateOnly fecha);
        Task<bool> ExisteSuperposicionAsync(int medicoId, int consultorioId, DateOnly fecha, TimeOnly hora);
    }
}
