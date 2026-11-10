namespace ClinicaTurnos.Domain.Interfaces
{
    // Repositorio genérico: los métodos comunes a todas las entidades se escriben una sola vez
    public interface IRepositoryBase<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
