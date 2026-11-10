using ClinicaTurnos.Domain.Interfaces;
using ClinicaTurnos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaTurnos.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly ApplicationContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositoryBase(ApplicationContext context)
        {
            _context = context;
            _dbSet = context.Set<T>(); // la tabla que corresponde a T (Pacientes, Medicos, etc.)
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity; // después del SaveChanges ya tiene el Id generado por la base
        }

        public async Task UpdateAsync(T entity)
        {
            // Si la entidad la trajimos con este mismo contexto, EF ya la está siguiendo (tracking)
            // y detecta solo qué cambió. Update() solo hace falta si viene "desconectada".
            if (_context.Entry(entity).State == EntityState.Detached)
                _dbSet.Update(entity);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
