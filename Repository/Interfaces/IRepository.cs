
using ERP_System_Project.DTOs;
using System.Linq.Expressions;

namespace ERP_System_Project.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        #region Retriving
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> GetAllToIQueryable();
        Task<List<T>> GetAllAsync();



        Task<T?> GetAsync(
            Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] Includes
            );

        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            params Expression<Func<T, object>>[] Includes
            );
        Task<PageSourcePagination<T>> GetAllPaginatedAsync(
            int pageNumber = 1, int pageSize = 10,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] Includes
            );
        #endregion

        #region Add_Update_Delete
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(int id);
        #endregion

        #region Others
        Task<bool> IsExistAsync(int id);
        Task<int> Count();
        #endregion

    }
}
