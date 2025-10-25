using ERP_System_Project.Repository.Interfaces;
using ERP_System_Project.Services.Interfaces;
using ERP_System_Project.UOW;

namespace ERP_System_Project.Services.Implementation
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        protected readonly IUnitOfWork _uow;
        protected readonly IRepository<T> _repository;
        public GenericService(IUnitOfWork uow)
        {
            _uow = uow;
            _repository = _uow.Repository<T>();
        }
        
        /// <summary>
        /// Creates a new entity. Let exceptions propagate to controller for proper error handling.
        /// </summary>
        public async Task<bool> CreateAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _uow.CompleteAsync();
            return true;
        }

        /// <summary>
        /// Updates an existing entity. Let exceptions propagate to controller for proper error handling.
        /// </summary>
        public async Task<bool> UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _uow.CompleteAsync();
            return true;
        }

        /// <summary>
        /// Deletes an entity by ID. Let exceptions propagate to controller for proper error handling.
        /// Returns false only if entity doesn't exist (not an error condition).
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            _repository.Delete(id);
            await _uow.CompleteAsync();
            return true;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities;
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
