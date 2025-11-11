using ERP_System_Project.Services.Interfaces.ECommerce;
using ERP_System_Project.ViewModels.Inventory;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERP_System_Project.Controllers.ECommerce
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<IActionResult> MakeOrder()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _orderService.MakeOrderAsync(userId);
            TempData["SuccessMessage"] = "Your order is on its way and will reach you soon";
            return RedirectToAction("Index","Market");
        }
    }
}
