namespace ERP_System_Project.Services.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id); 
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();

    }
}
