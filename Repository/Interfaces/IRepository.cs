
using ERP_System_Project.DTOs;
using System.Linq.Expressions;

namespace ERP_System_Project.Repository.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region Retriving
        Task<TEntity?> GetByIdAsync(int id);
        IQueryable<TEntity> GetAllToIQueryable();
        Task<List<TEntity>> GetAllAsync();



        Task<TResult?> GetAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] Includes
            ) where TResult : class;

        Task<List<TResult>> GetAllAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] Includes
            ) where TResult : class;
        Task<PageSourcePagination<TResult>> GetAllPaginatedAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            int pageNumber = 1, int pageSize = 10,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] Includes
            ) where TResult : class;
        #endregion

        #region Add_Update_Delete
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
        #endregion

        #region Count
        Task<int> Count();
        Task<int> Count(Expression<Func<TEntity,bool>> filter);
        #endregion

        #region Existance
        Task<bool> IsExistAsync(int id);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<bool> AllAsync(Expression<Func<TEntity, bool>> filter);
        #endregion

    }
}
