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
        public async Task<bool> CreateAsync(T entity)
        {

            try
            {
                await _repository.AddAsync(entity);
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _repository.Update(entity);
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity =  await _repository.GetByIdAsync(id);
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
