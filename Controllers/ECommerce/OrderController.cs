using ERP_System_Project.Services.Interfaces.ECommerce;
using ERP_System_Project.ViewModels.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using ERP_System_Project.Services;
using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.Enums;
using ERP_System_Project.ViewModels.ECommerce;
using ERP_System_Project.ViewModels;
using Microsoft.AspNetCore.Identity;
using ERP_System_Project.Models.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using ERP_System_Project.Services.Implementation.ECommerce;
using ERP_System_Project.Services.Interfaces;

namespace ERP_System_Project.Controllers.ECommerce
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService,
                               UserManager<ApplicationUser> userManager,
                               IEmailService emailService)
        {
            _orderService = orderService;
            _userManager = userManager;
            _emailService = emailService;
        }


        // ????? ??????? ?????? ?? ?????? ??????
        // Controller
        public async Task<IActionResult> MakeOrder()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartVM = await _orderService.GetCartVMAsync(userId);

            if (cartVM == null || cartVM.productsCart.Count == 0)
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("Index", "Market");
            }

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

            await _orderService.MakeOrderAsync(userId, cartModel, PaymentMethod.Cash);

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
        [AllowAnonymous]
        public async Task<IActionResult> AdminOrders(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync(pageNumber, pageSize);
                ViewBag.StatusList = await _orderService.GetOrderStatusListAsync();
                return View(orders);
            }
            catch (Exception ex)
            {
                // هنا ممكن تعمل Log للخطأ أو تخزنه في قاعدة البيانات
                TempData["ErrorMessage"] = "An error occurred while loading orders: " + ex.Message;
                return View(new PageSourcePagination<MyOrdersVM>()); // عرض View فارغ بدل الكراش
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatusAdmin(int orderId, int statusId)
        {
            try
            {
                await _orderService.UpdateOrderStatusAsync(orderId, statusId);
                TempData["SuccessMessage"] = "Order status updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to update order status: " + ex.Message;
            }

            return RedirectToAction("AdminOrders");
        }




    }
}
