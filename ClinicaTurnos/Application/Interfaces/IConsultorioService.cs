using ClinicaTurnos.Application.Models.Consultorios;

namespace ClinicaTurnos.Application.Interfaces
{
    public interface IConsultorioService
    {
        Task<List<ConsultorioDto>> GetAllAsync();
        Task<ConsultorioDto> GetByIdAsync(int id);
        Task<ConsultorioDto> CreateAsync(ConsultorioRequest request);
        Task<ConsultorioDto> UpdateAsync(int id, ConsultorioRequest request);
        Task DeleteAsync(int id);
    }
}
