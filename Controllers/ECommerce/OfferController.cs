using ERP_System_Project.Models.Logs;
using ERP_System_Project.Services.Interfaces.ECommerce;
using ERP_System_Project.ViewModels.ECommerce;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System_Project.Controllers.ECommerce
{
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly IValidator<OfferVM> _validator;

        public OfferController(IOfferService offerService, IValidator<OfferVM> validator)
        {
            _offerService = offerService;
            _validator = validator;
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
            var result = _validator.Validate(model);
            if (result.IsValid)
            {
                await _offerService.SetOfferAsync(model);
                return RedirectToAction("Index", "Product");
            }

            foreach(var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(model);

        }
    }
}
