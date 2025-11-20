using ERP_System_Project.Services.Interfaces.ECommerce;
using ERP_System_Project.ViewModels.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using ERP_System_Project.Services;
using ERP_System_Project.Models.ECommerce;

namespace ERP_System_Project.Controllers.ECommerce
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // ????? ??????? ?????? ?? ?????? ??????
        public async Task<IActionResult> MakeOrder()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // ??? ?????? ??????
            var cartVM = await _orderService.GetCartVMAsync(userId);

            if (cartVM == null || cartVM.productsCart.Count == 0)
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("Index", "Market");
            }

            // ????? CartVM ? CartViewModel
            var cartModel = new CartViewModel
            {
                productsCart = cartVM.productsCart.Select(p => new CartItemViewModel
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity
                }).ToList()
            };

            // ????? ??????? ???????? ?????? ??????
            await _orderService.MakeOrderAsync(userId, cartModel);

            TempData["SuccessMessage"] = "Your order is on its way and will reach you soon";
            return RedirectToAction("Index", "Market");
        }


        // ??? ????? ?????? ?? Pagination
        public async Task<IActionResult> MyOrders(int pageNumber = 1, int pageSize = 10)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderService.GetCustomerOrdersAsync(userId, pageNumber, pageSize);
            return View(orders);
        }
    }
}
