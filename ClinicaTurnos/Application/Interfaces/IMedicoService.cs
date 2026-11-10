using ClinicaTurnos.Application.Models.Medicos;

namespace ClinicaTurnos.Application.Interfaces
{
    public interface IMedicoService
    {
        Task<List<MedicoDto>> GetAllAsync(string? especialidad);
        Task<MedicoDto> GetByIdAsync(int id);
        Task<MedicoDto> CreateAsync(MedicoRequest request);
        Task<MedicoDto> UpdateAsync(int id, MedicoRequest request);
        Task DeleteAsync(int id);
    }
}
