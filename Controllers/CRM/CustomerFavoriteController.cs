using AutoMapper;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.CRM;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.CRM;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Threading.Tasks;

namespace ERP_System_Project.Controllers.CRM
{
    public class CustomerFavoriteController : Controller
    {
        private readonly ICustomerFavoriteService _customerFavoriteService;
        private readonly IMapper _mapper;

        public CustomerFavoriteController(ICustomerFavoriteService customerFavoriteService, IMapper mapper)
        {
            _customerFavoriteService = customerFavoriteService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int customerId)
        {

            // we need to make sure customer is correct id it is better to get it from the logged in user
            // if he passed customer id in the query string we need to validate it
            var favoritesVM = await _customerFavoriteService.GetAllFavoritesVMsAsync(customerId);
            return View(favoritesVM);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int customerId, int productId)
        {
            try
            {
                var result = await _customerFavoriteService.ToggleFavoriteAsync(customerId, productId);

                // we will do nothing and stay at the same page but return to debugging
                return Json(new
                {
                    success = true,
                    status = result ? "added" : "removed",
                    message = result ? "Added to favorites!" : "Removed from favorites!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred while updating favorites."
                });
            }



        }

        // we can add (addFavorite and remove favorite ) instead of toggle favorite if needed (already implemented in the service)

        [HttpGet]
        public async Task<JsonResult> CheckFavoriteStatus(int customerId, int productId)
        {
            var isFavorite = await _customerFavoriteService.IsFavoriteAsync(customerId, productId);
            return Json(new { isFavorite });
        }



    }
}
