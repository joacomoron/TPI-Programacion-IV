using ClinicaTurnos.Domain.Entities;

namespace ClinicaTurnos.Domain.Interfaces
{
    public interface IConsultorioRepository : IRepositoryBase<Consultorio>
    {
        Task<bool> ExistsByNumeroAsync(int nroConsultorio);
    }
}
