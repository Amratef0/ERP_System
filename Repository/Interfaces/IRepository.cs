using ERP_System_Project.Specification.Interfaces;
using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.Inventory;
using System.Linq.Expressions;

namespace ERP_System_Project.Repository.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region Retriving
        Task<TEntity?> GetByIdAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetAllAsNoTrackedAsync();
        IQueryable<TEntity> GetAllAsIQueryable();


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
            bool expandable = false,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] Includes)
            where TResult : class;

        Task<PageSourcePagination<TResult>> GetAllPaginatedEnhancedAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            int pageNumber = 1,
            int pageSize = 10,
            Expression<Func<TEntity, bool>>? filter = null,
            bool expandable = false,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
            where TResult : class;

        Task<List<object>> GetBySpecificationAsync(ISpecification<TEntity> specification);

        #endregion

        #region Add_Update_Delete
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
        Task BulkDeleteAsync(Expression<Func<TEntity, bool>> filter);
        void SoftDelete(int id);
        #endregion

        #region Count
        Task<int> Count();
        Task<int> Count(Expression<Func<TEntity, bool>> filter);
        #endregion

        #region Existance
        Task<bool> IsExistAsync(int id);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<bool> AllAsync(Expression<Func<TEntity, bool>> filter);
        #endregion

        Task<int> SaveAsync();

    }
}
