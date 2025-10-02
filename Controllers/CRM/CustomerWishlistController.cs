using AutoMapper;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Implementation.CRM;
using ERP_System_Project.Services.Interfaces.CRM;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.CRM;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Threading.Tasks;

namespace ERP_System_Project.Controllers.CRM
{
    public class CustomerWishlistController : Controller
    {
        private readonly ICustomerWishlistService _customerWishlistService;

        public CustomerWishlistController(ICustomerWishlistService customerWishlistService)
        {
            _customerWishlistService = customerWishlistService;
        }

        public async Task<IActionResult> Index(int customerId)
        {

            // we need to make sure customer is correct id it is better to get it from the logged in user
            // if he passed customer id in the query string we need to validate it
            var wishlistVMs = await _customerWishlistService.GetAllWishlistsVMsAsync(customerId);
            return View(wishlistVMs);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleWishlist(int customerId, int productId)
        {
            try
            {
                var result = await _customerWishlistService.ToggleWishlistAsync(customerId, productId);

                // we will do nothing and stay at the same page but return to debugging
                return Json(new
                {
                    success = true,
                    status = result ? "added" : "removed",
                    message = result ? "Added to wishlists!" : "Removed from wishlists!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred while updating wishlists."
                });
            }



        }

        // we can add (add Wishlist and remove Wishlist ) instead of toggle Wishlist if needed (already implemented in the service)

        [HttpGet]
        public async Task<JsonResult> CheckWishlistStatus(int customerId, int productId)
        {
            var isWishlist = await _customerWishlistService.IsWishlistAsync(customerId, productId);
            return Json(new { isWishlist });
        }



    }
}
