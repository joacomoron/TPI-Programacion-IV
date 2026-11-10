using ClinicaTurnos.Domain.Interfaces;
using ClinicaTurnos.Infrastructure.Data;

namespace ClinicaTurnos.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly ApplicationContext _context;

        public RepositoryBase(ApplicationContext context)
        {
            _context = context;
        }
    }
}
