using ClinicaTurnos.Application.Models.Pacientes;

namespace ClinicaTurnos.Application.Interfaces
{
    public interface IPacienteService
    {
        Task<List<PacienteDto>> GetAllAsync();
        Task<PacienteDto> GetByIdAsync(int id);
        Task<PacienteDto> CreateAsync(PacienteRequest request);
        Task<PacienteDto> UpdateAsync(int id, PacienteRequest request);
        Task DeleteAsync(int id);
    }
}
