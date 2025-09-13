using ERP_System_Project.Models;
using ERP_System_Project.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);


        public async Task<List<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);


        public async Task<bool> IsExistAsync(int id)
            => await _dbSet.FindAsync(id) == null ? false : true;

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public async Task<List<T>> SkipAndTake(int skip, int take)
            => await _dbSet.Skip(skip).Take(take).ToListAsync();

        public async Task<int> Count()
            => await _dbSet.CountAsync();
    }
}
