using ERP_System_Project.DTOs;
using ERP_System_Project.Models;
using ERP_System_Project.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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


        public Task<T?> GetAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] Includes)
        {
            IQueryable<T> query = _dbSet;

            query = query.Where(filter);

            foreach (var include in Includes)
                query = query.Include(include);

            return query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] Includes)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (Includes != null)
                foreach (var include in Includes)
                    query = query.Include(include);

            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync();
        }


        public async Task<PageSourcePagination<T>> GetAllPaginatedAsync(
            int pageNumber = 1, int pageSize = 10,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] Includes)
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


            var result = _dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var totalRecords = await _dbSet.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            return new PageSourcePagination<T>
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

        #region Otheres
        public async Task<bool> IsExistAsync(int id)
            => await _dbSet.FindAsync(id) == null ? false : true;
        public async Task<int> Count()
            => await _dbSet.CountAsync();
        #endregion


        
    }
}
