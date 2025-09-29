using ERP_System_Project.Models;
using ERP_System_Project.Models.Interfaces;
using ERP_System_Project.Repository.Interfaces;
using ERP_System_Project.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ERP_System_Project.Repository.Implementation
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly Erpdbcontext _db;
        private readonly DbSet<TEntity> _dbSet;
        public Repository(Erpdbcontext db)
        {
            _db = db;
            _dbSet = db.Set<TEntity>();
        }

        #region Retriving

        public async Task<TEntity?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<List<TEntity>> GetAllAsync()
            => await _dbSet.ToListAsync();
        public async Task<List<TEntity>> GetAllAsNoTrackedAsync()
            => await _dbSet.AsNoTracking().ToListAsync();

        public IQueryable<TEntity> GetAllAsIQueryable() => _dbSet;


        public Task<TResult?> GetAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] Includes) where TResult : class
        {
            IQueryable<TEntity> query = _dbSet;

            query = query.Where(filter);

            foreach (var include in Includes)
                query = query.Include(include);

            return query.Select(selector).FirstOrDefaultAsync();
        }

        public async Task<List<TResult>> GetAllAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] Includes) where TResult : class
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (Includes != null)
                foreach (var include in Includes)
                    query = query.Include(include);

            if (orderBy != null)
                query = orderBy(query);

            return await query.Select(selector).ToListAsync();
        }


        public async Task<PageSourcePagination<TResult>> GetAllPaginatedAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            int pageNumber = 1, int pageSize = 10,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] Includes) where TResult : class
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 5) pageSize = 5;

            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (Includes != null)
                foreach (var include in Includes)
                    query = query.Include(include);

            if (orderBy != null)
                query = orderBy(query);

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var result = query.Select(selector);


            //var totalRecords = await query.CountAsync();
            //var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            return new PageSourcePagination<TResult>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                Data = await result.ToListAsync()
            };
        }
        #endregion

        #region Add_Update_Delete
        public async Task AddAsync(TEntity entity)
            => await _dbSet.AddAsync(entity);
        public void Update(TEntity entity)
            => _dbSet.Update(entity);

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }
        public async Task BulkDeleteAsync(Expression<Func<TEntity, bool>> filter)
            =>  await _dbSet.Where(filter).ExecuteDeleteAsync();

        public void SoftDelete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null && entity is ISoftDeletable deletableEntity)
            {
                deletableEntity.IsDeleted = true;
                deletableEntity.DeletedAt = DateOnly.FromDateTime(DateTime.Now);
                _dbSet.Update(entity);
            }
        }
        #endregion

        #region Count
        public async Task<int> Count()
            => await _dbSet.CountAsync();

        public async Task<int> Count(Expression<Func<TEntity, bool>> filter)
            => await _dbSet.Where(filter).CountAsync();
        #endregion

        #region Existance
        public async Task<bool> IsExistAsync(int id)
            => await _dbSet.FindAsync(id) == null ? false : true;

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            if (filter != null)
                return await _dbSet.AnyAsync(filter);
            return await _dbSet.AnyAsync();
        }

        public async Task<bool> AllAsync(Expression<Func<TEntity, bool>> filter)
            => await _dbSet.AllAsync(filter);




        #endregion


        public async Task<int> SaveAsync() => await _db.SaveChangesAsync();


    }
}
