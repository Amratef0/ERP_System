using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Repository.Interfaces;
using ERP_System_Project.Services.Interfaces.CRM;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.CRM;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.CRM
{
    public class CustomerFavoriteService : GenericService<CustomerFavorite>, ICustomerFavoriteService
    {
        private readonly IUnitOfWork _uow;

        public CustomerFavoriteService(IUnitOfWork uow) : base(uow)
        {

            _uow = uow;
        }
        public async Task<bool> IsFavoriteAsync(int customerId, int productId)
        {
            return await _uow.CustomerFavorites.AnyAsync(cf => cf.CustomerId == customerId && cf.ProductId == productId);

        }
        public async Task<IEnumerable<CustomerFavoriteVM>> GetAllFavoritesVMsAsync(int? customerId)
        {
         var query = _uow.CustomerFavorites.GetAllAsIQueryable();
            if (customerId.HasValue)
            {
                query = query.Where(cf => cf.CustomerId == customerId.Value);
            }


            var favortiesVM = await query
                .Include(cf => cf.Product)
                .Include(cf => cf.Customer)
                .OrderByDescending(cf => cf.DateCreated)
                .Select(cf => new CustomerFavoriteVM
                {
                    Id= cf.Id,
                    ProductId = cf.ProductId,
                    CustomerId = cf.CustomerId,
                    ProductName = cf.Product.Name,
                    CustomerFullName = $"{cf.Customer.FirstName} {cf.Customer.LastName}",
                    DateCreated = cf.DateCreated,
                }).ToListAsync();
            return favortiesVM;
        }

        public async Task <bool> ToggleFavoriteAsync(int customerId, int productId)
        {
            var existing = await _uow.CustomerFavorites.GetAllAsIQueryable()
                .FirstOrDefaultAsync(cf => cf.CustomerId == customerId && cf.ProductId == productId);
            if (existing != null)
            {
                _uow.CustomerFavorites.Delete(existing.Id);
                await _uow.CompleteAsync();
                return false;
            }
            var favorite = new CustomerFavorite
            {
                CustomerId = customerId,
                ProductId = productId,
                DateCreated = DateTime.Now
            };
            await _uow.CustomerFavorites.AddAsync(favorite);
            await _uow.CompleteAsync();
            return true; // Added to favorites
        }

        public async Task<bool> RemoveFavoriteAsync(int customerId, int productId)
        {
            var favorite = await _uow.CustomerFavorites.GetAllAsIQueryable()
                .FirstOrDefaultAsync(cf => cf.CustomerId == customerId && cf.ProductId == productId);
            if (favorite == null)
                return false;
            _uow.CustomerFavorites.Delete(favorite.Id);
            await _uow.CompleteAsync();
            return true;
        }


        public async Task<bool> AddFavoriteAsync(int customerId, int productId)
        {
            var existing = await _uow.CustomerFavorites.AnyAsync(cf => cf.CustomerId == customerId && cf.ProductId == productId);
            if (existing)
                return false;
            var favorite = new CustomerFavorite
            {
                CustomerId = customerId,
                ProductId = productId,
                DateCreated = DateTime.Now
            };
            await _uow.CustomerFavorites.AddAsync(favorite);
            await _uow.CompleteAsync();
            return true;
        }
      



    }
}
