using ClinicaTurnos.Domain.Entities;

namespace ClinicaTurnos.Domain.Interfaces
{
    public interface IMedicoRepository : IRepositoryBase<Medico>
    {
        Task<List<Medico>> GetByEspecialidadAsync(string especialidad);
        Task<bool> ExistsByMatriculaAsync(string matricula);
    }
}
