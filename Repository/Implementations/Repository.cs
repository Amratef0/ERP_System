using ERP_System_Project.DTOs;
using ERP_System_Project.Models;
using ERP_System_Project.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ERP_System_Project.Repository.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly Erpdbcontext _db;
        private readonly DbSet<T> _dbSet;
        public Repository(Erpdbcontext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }

        #region Retriving
        public async Task<List<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public IQueryable<T> GetAllToIQueryable() => _dbSet;


        public Task<TResult?> GetAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] Includes) where TResult : class
        {
            IQueryable<T> query = _dbSet;

            query = query.Where(filter);

            foreach (var include in Includes)
                query = query.Include(include);

            return query.Select(selector).FirstOrDefaultAsync();
        }

        public async Task<List<TResult>> GetAllAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] Includes) where TResult : class
        {
            IQueryable<T> query = _dbSet;

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
            Expression<Func<T, TResult>> selector,
            int pageNumber = 1, int pageSize = 10,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] Includes) where TResult : class
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 10) pageSize = 10;

            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (Includes != null)
                foreach (var include in Includes)
                    query = query.Include(include);

            if (orderBy != null)
                query = orderBy(query);


            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var result = query.Select(selector);


            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

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
        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);
        public void Update(T entity)
            => _dbSet.Update(entity);

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }
        #endregion

        #region Count
        public async Task<int> Count()
            => await _dbSet.CountAsync();

        public async Task<int> Count(Expression<Func<T, bool>> filter)
            => await _dbSet.Where(filter).CountAsync();
        #endregion

        #region Existance
        public async Task<bool> IsExistAsync(int id)
            => await _dbSet.FindAsync(id) == null ? false : true;

        public async Task<bool> AnyAsync(Expression<Func<T, bool>>? filter = null)
        {
            if(filter != null)
                return await _dbSet.AnyAsync(filter);
            return await _dbSet.AnyAsync();
        }

        public async Task<bool> AllAsync(Expression<Func<T, bool>> filter)
            => await _dbSet.AllAsync(filter);
        #endregion


    }
}
