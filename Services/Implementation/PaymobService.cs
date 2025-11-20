using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ERP_System_Project.Models.CRM;

namespace ERP_System_Project.Services
{
    public class PaymobService
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey = "ZXlKaGJHY2lPaUpJVXpVeE1pSXNJblI1Y0NJNklrcFhWQ0o5LmV5SmpiR0Z6Y3lJNklrMWxjbU5vWVc1MElpd2ljSEp2Wm1sc1pWOXdheUk2TVRBM09UZ3hOaXdpYm1GdFpTSTZJakUzTlRnNU5qZzJOamd1TWpFNE5qUXhJbjAuRVk2TlE1dTB5NE03UXVFTGYzbFZGYnJ3bFJCeTdTODVaaUN6WS1KbVZYLW03WkgzX2tQZ19OUzYtVEI0MVlic25HSWZuR29uUVhIeFlIbHZtc2EwVHc="; // حط هنا الـ API Key كامل
        private readonly int IntegrationId = 5310230; // الرقم الحقيقي اللي جالك من Paymob

        public PaymobService(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetAuthTokenAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var body = new { api_key = _apiKey };

            var response = await client.PostAsync(
                "https://accept.paymob.com/api/auth/tokens",
                new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to get Paymob auth token");

            var json = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(json).RootElement.GetProperty("token").GetString();
        }

        public async Task<int> CreateOrderAsync(string authToken, decimal totalAmount)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var body = new
            {
                auth_token = authToken,
                delivery_needed = "false",
                amount_cents = (int)(totalAmount * 100),
                currency = "EGP",
                items = new object[] { }
            };

            var response = await client.PostAsync(
                "https://accept.paymob.com/api/ecommerce/orders",
                new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to create Paymob order");

            var json = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(json).RootElement.GetProperty("id").GetInt32();
        }

        public async Task<string> GetPaymentKeyAsync(string authToken, int orderId, decimal total, Customer customer)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var body = new
            {
                auth_token = authToken,
                amount_cents = (int)(total * 100),
                expiration = 3600,
                order_id = orderId,
                billing_data = new
                {
                    first_name = customer.FirstName,
                    last_name = customer.LastName,
                    email = customer.Email,
                    phone_number = customer.PhoneNumber ?? "01200000000",
                    apartment = "NA",
                    floor = "NA",
                    street = "NA",
                    building = "NA",
                    shipping_method = "NA",
                    postal_code = "NA",
                    city = "NA",
                    country = "NA",
                    state = "NA"
                },
                currency = "EGP",
                integration_id = IntegrationId // استخدم الرقم الصحيح هنا بدل string
            };

            var response = await client.PostAsync(
                "https://accept.paymob.com/api/acceptance/payment_keys",
                new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to get Paymob payment key");

            var json = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(json).RootElement.GetProperty("token").GetString();
        }
    }
}
