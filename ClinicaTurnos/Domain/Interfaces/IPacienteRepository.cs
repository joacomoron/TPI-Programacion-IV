using ClinicaTurnos.Domain.Entities;

namespace ClinicaTurnos.Domain.Interfaces
{
    public interface IPacienteRepository : IRepositoryBase<Paciente>
    {
        Task<Paciente?> GetByEmailAsync(string email);
        Task<bool> ExistsByDniAsync(string dni);
    }
}
