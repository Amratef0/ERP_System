using ERP_System_Project.Services.Interfaces.ECommerce;
using ERP_System_Project.UOW;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ERP_System_Project.Services.Implementation.ECommerce
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string SessionKey = "MyCart";

        public OrderService(IUnitOfWork unitOfWork, ICartService cartService,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task MakeOrderAsync()
        {
            var productDictionary = GetDictionaryFromSession(SessionKey);
            var productIDs = productDictionary.Keys.ToList();
            var productQuantities = productDictionary.Values.ToList();
            var numberOfItems = productIDs.Count;
            

            for(int i = 0; i < numberOfItems; i++)
            {
                await _unitOfWork.Products.GetAllAsIQueryable()
                   .Where(p => p.Id == productIDs[i])
                   .ExecuteUpdateAsync(p => p.SetProperty(
                       p => p.Quantity,
                       p => p.Quantity - productQuantities[i]));
            }

            ClearCartSession(SessionKey);
        }


        private void ClearCartSession(string sessionKey)
           => _httpContextAccessor.HttpContext.Session.Remove(SessionKey);


        private Dictionary<int, int> GetDictionaryFromSession(string sessionKey)
        {
            var json = _httpContextAccessor
                .HttpContext.Session.GetString(SessionKey);

            if (string.IsNullOrEmpty(json))
                return new Dictionary<int, int>();

            return JsonConvert.DeserializeObject<Dictionary<int, int>>(json)
                ?? new Dictionary<int, int>(); ;
        }
    }
}
