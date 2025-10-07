using ERP_System_Project.Models.Logs;
using ERP_System_Project.Services.Interfaces.ECommerce;
using ERP_System_Project.ViewModels.ECommerce;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System_Project.Controllers.ECommerce
{
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        public async Task<IActionResult> Set(int productId)
        {
            var offer = await _offerService.GetOfferAsync(productId);
            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Set(OfferVM model)
        {
            await _offerService.SetOfferAsync(model);
            return RedirectToAction("Index","Product");
        }
    }
}
