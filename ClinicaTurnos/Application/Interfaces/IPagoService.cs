using ClinicaTurnos.Application.Models.Pagos;

namespace ClinicaTurnos.Application.Interfaces
{
    public interface IPagoService
    {
        Task<List<PagoDto>> GetAllAsync();
        Task<PagoDto> GetByIdAsync(int id);
        Task<PagoDto> CreateAsync(PagoRequest request);
        Task DeleteAsync(int id);
    }
}
