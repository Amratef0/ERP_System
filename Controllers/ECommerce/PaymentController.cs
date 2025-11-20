using ERP_System_Project.Services.Interfaces.ECommerce;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.ViewModels.ECommerce;
using System.Linq;

namespace ERP_System_Project.Controllers.ECommerce
{
    public class PaymentController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string _apiKey = "ZXlKaGJHY2lPaUpJVXpVeE1pSXNJblI1Y0NJNklrcFhWQ0o5LmV5SmpiR0Z6Y3lJNklrMWxjbU5vWVc1MElpd2ljSEp2Wm1sc1pWOXdheUk2TVRBM09UZ3hOaXdpYm1GdFpTSTZJakUzTlRnNU5qZzJOamd1TWpFNE5qUXhJbjAuRVk2TlE1dTB5NE03UXVFTGYzbFZGYnJ3bFJCeTdTODVaaUN6WS1KbVZYLW03WkgzX2tQZ19OUzYtVEI0MVlic25HSWZuR29uUVhIeFlIbHZtc2EwVHc=";
        private readonly int IntegrationId = 5310230;
        private readonly int IframeId = 963970;
        private readonly string HppEndpoint = "https://accept.paymob.com/api/acceptance/payment_keys";

        public PaymentController(IOrderService orderService, IHttpClientFactory httpClientFactory)
        {
            _orderService = orderService;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> ProcessPayment()
        {
            var cartVM = await _orderService.GetCartVMAsync(User.Identity.Name);
            if (cartVM.productsCart.Count == 0)
                return RedirectToAction("Index", "Market");

            decimal totalAmount = cartVM.TotalPrice;
            int amountCents = (int)(totalAmount * 100);

            // تحويل CartVM → CartViewModel
            var cartModel = new CartViewModel
            {
                productsCart = cartVM.productsCart
                    .Select(p => new CartItemViewModel
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        Price = p.Price,
                        Quantity = p.Quantity
                    }).ToList()
            };

            // حفظ الكارت مؤقتًا قبل الدفع
            await _orderService.SaveCartForPayment(User.Identity.Name, cartModel);

            var paymentToken = await GeneratePaymentKey(amountCents);

            var model = new PaymentViewModel
            {
                IframeId = IframeId,
                PaymentToken = paymentToken
            };

            return View(model);
        }

        private async Task<string> GetAuthTokenAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var body = new { api_key = _apiKey };
            var content = new StringContent(JObject.FromObject(body).ToString(), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://accept.paymob.com/api/auth/tokens", content);
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            return json["token"]?.ToString() ?? "";
        }

        private async Task<string> GeneratePaymentKey(int amountCents)
        {
            string authToken = await GetAuthTokenAsync();
            var client = _httpClientFactory.CreateClient();

            var payload = new
            {
                auth_token = authToken,
                amount_cents = amountCents,
                currency = "EGP",
                expiration = 3600,
                integration_id = IntegrationId,
                billing_data = new
                {
                    first_name = User.Identity.Name.Split(" ")[0],
                    last_name = User.Identity.Name.Split(" ").Last(),
                    email = User.Identity.Name + "@example.com",
                    phone_number = "01200000000",
                    apartment = "NA",
                    floor = "NA",
                    street = "NA",
                    building = "NA",
                    shipping_method = "NA",
                    postal_code = "NA",
                    city = "NA",
                    country = "NA",
                    state = "NA"
                }
            };

            var content = new StringContent(JObject.FromObject(payload).ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(HppEndpoint, content);
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            return json["token"]?.ToString() ?? "";
        }

        [HttpGet]
        public async Task<IActionResult> PaymentCallback(string token, string success)
        {
            if (success == "true")
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "User not found or not logged in.";
                    return RedirectToAction("Index", "Market");
                }

                var cart = await _orderService.GetCartFromPayment(userId);
                if (cart == null || cart.productsCart.Count == 0)
                {
                    TempData["ErrorMessage"] = "Cart not found after payment.";
                    return RedirectToAction("Index", "Market");
                }

                // إنشاء الأوردر باستخدام بيانات الكارت
                await _orderService.MakeOrderAsync(userId, cart);

                TempData["SuccessMessage"] = "Your payment was successful and your order is on its way!";
                return RedirectToAction("Index", "Market");
            }
            else
            {
                TempData["ErrorMessage"] = "Payment failed or canceled.";
                return RedirectToAction("Index", "Market");
            }
        }


    }

    public class PaymentViewModel
    {
        public int IframeId { get; set; }
        public string PaymentToken { get; set; }
    }
}
