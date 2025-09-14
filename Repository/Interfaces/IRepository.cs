
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



        Task<TResult?> GetAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] Includes
            ) where TResult : class;

        Task<List<TResult>> GetAllAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] Includes
            ) where TResult : class;
        Task<PageSourcePagination<TResult>> GetAllPaginatedAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            int pageNumber = 1, int pageSize = 10,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] Includes
            ) where TResult : class;
        #endregion

        #region Add_Update_Delete
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(int id);
        #endregion

        #region Count
        Task<int> Count();
        Task<int> Count(Expression<Func<T,bool>> filter);
        #endregion

        #region Existance
        Task<bool> IsExistAsync(int id);
        Task<bool> AnyAsync(Expression<Func<T, bool>>? filter = null);
        Task<bool> AllAsync(Expression<Func<T, bool>> filter);
        #endregion

    }
}
