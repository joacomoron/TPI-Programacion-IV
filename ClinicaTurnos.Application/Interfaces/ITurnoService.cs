using ClinicaTurnos.Application.Models.Turnos;

namespace ClinicaTurnos.Application.Interfaces
{
    public interface ITurnoService
    {
        Task<List<TurnoDto>> GetAllAsync();
        Task<TurnoDto> GetByIdAsync(int id);
        Task<List<TurnoDto>> GetByPacienteAsync(int pacienteId);
        Task<List<TimeOnly>> GetHorariosDisponiblesAsync(int medicoId, DateOnly fecha);
        Task<TurnoDto> CreateAsync(TurnoRequest request);
        Task<TurnoDto> ConfirmarAsync(int id);
        Task<TurnoDto> CancelarAsync(int id);
        Task DeleteAsync(int id);
    }
}
