using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(int id);
        Task<bool> IsExistAsync(int id);

        Task<List<T>> SkipAndTake(int skip, int take);

        Task<int> Count();
        
    }
}
