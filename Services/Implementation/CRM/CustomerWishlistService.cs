using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Repository.Interfaces;
using ERP_System_Project.Services.Interfaces.CRM;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.CRM;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.CRM
{
    public class CustomerWishlistService : GenericService<CustomerWishlist>, ICustomerWishlistService
    {
        private readonly IUnitOfWork _uow;

        public CustomerWishlistService(IUnitOfWork uow) : base(uow)
        {

            _uow = uow;
        }
        public async Task<bool> IsWishlistAsync(int customerId, int productId)
        {
            return await _uow.CustomerWishlists.AnyAsync(cf => cf.CustomerId == customerId && cf.ProductId == productId);

        }
        public async Task<IEnumerable<CustomerWishlistVM>> GetAllWishlistsVMsAsync(int? customerId)
        {
         var query = _uow.CustomerWishlists.GetAllAsIQueryable();
            if (customerId.HasValue)
            {
                query = query.Where(cf => cf.CustomerId == customerId.Value);
            }


            var wishlistsVM = await query
                .Include(cw => cw.Product)
                .Include(cw => cw.Customer)
                .OrderByDescending(cw => cw.DateCreated)
                .Select(cw => new CustomerWishlistVM
                {
                    Id= cw.Id,
                    ProductId = cw.ProductId,
                    CustomerId = cw.CustomerId,
                    ProductName = cw.Product.Name,
                    CustomerFullName = $"{cw.Customer.FirstName} {cw.Customer.LastName}",
                    DateCreated = cw.DateCreated,
                }).ToListAsync();
            return wishlistsVM;
        }

        public async Task <bool> ToggleWishlistAsync(int customerId, int productId)
        {
            var existing = await _uow.CustomerWishlists.GetAllAsIQueryable()
                .FirstOrDefaultAsync(cf => cf.CustomerId == customerId && cf.ProductId == productId);
            if (existing != null)
            {
                _uow.CustomerWishlists.Delete(existing.Id);
                await _uow.CompleteAsync();
                return false;
            }
            var wishlist = new CustomerWishlist
            {
                CustomerId = customerId,
                ProductId = productId,
                DateCreated = DateTime.Now
            };
            await _uow.CustomerWishlists.AddAsync(wishlist);
            await _uow.CompleteAsync();
            return true; // Added to wishlist
        }

        public async Task<bool> RemoveWishlistAsync(int customerId, int productId)
        {
            var wishlist = await _uow.CustomerWishlists.GetAllAsIQueryable()
                .FirstOrDefaultAsync(cf => cf.CustomerId == customerId && cf.ProductId == productId);
            if (wishlist == null)
                return false;
            _uow.CustomerWishlists.Delete(wishlist.Id);
            await _uow.CompleteAsync();
            return true;
        }


        public async Task<bool> AddWishlistAsync(int customerId, int productId)
        {
            var existing = await _uow.CustomerWishlists.AnyAsync(cf => cf.CustomerId == customerId && cf.ProductId == productId);
            if (existing)
                return false;
            var wishlist = new CustomerWishlist
            {
                CustomerId = customerId,
                ProductId = productId,
                DateCreated = DateTime.Now
            };
            await _uow.CustomerWishlists.AddAsync(wishlist);
            await _uow.CompleteAsync();
            return true;
        }
      



    }
}
