using ClinicaTurnos.Domain.Entities;

namespace ClinicaTurnos.Domain.Interfaces
{
    public interface IPagoRepository : IRepositoryBase<Pago>
    {
        Task<Pago?> GetByTurnoIdAsync(int turnoId);
    }
}
