using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.ECommerece;
using ERP_System_Project.Models.Enums;
using ERP_System_Project.Services.Interfaces.ECommerce;
using ERP_System_Project.UOW;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ERP_System_Project.Services.Implementation.ECommerce
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string SessionKey = "MyCart";

        public OrderService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task MakeOrderAsync(string userId)
        {
            var productDictionary = GetDictionaryFromSession(SessionKey);

            var productIDs = productDictionary.Keys.ToList();
            var productQuantities = productDictionary.Values.ToList();
            var numberOfItems = productIDs.Count;

            var customer = await _unitOfWork.Customers.GetAllAsIQueryable()
                .Include(c => c.CustomerAddresses)
                .FirstOrDefaultAsync(c => c.ApplicationUserId ==  userId);
            int customerId = customer!.Id;

            var orderItems = new List<OrderItem>();

            for(int i = 0; i < numberOfItems; i++)
            {
                await _unitOfWork.Products.GetAllAsIQueryable()
                   .Where(p => p.Id == productIDs[i])
                   .ExecuteUpdateAsync(p => p.SetProperty(
                       p => p.Quantity,
                       p => p.Quantity - productQuantities[i]));

                var product = _unitOfWork.Products
                    .GetAllAsIQueryable()
                    .Include(p => p.Offer)
                    .FirstOrDefault(p => p.Id == productIDs[i]);

                OrderItem orderItem = new OrderItem()
                {
                    ProductId = productIDs[i],
                    Quantity = productQuantities[i],
                    CreatedDate = DateTime.Now,
                    UnitPrice = product!.StandardPrice,
                    DiscountPercentage = product.Offer != null ? product.Offer.DiscountPercentage : 0,
                    DiscountAmount = product.Offer != null ?
                                    (product.StandardPrice * productQuantities[i])
                                    -
                                    (product.StandardPrice - ((product.Offer.DiscountPercentage / 100m) * product.StandardPrice)) * productQuantities[i]
                                    :0,
                    LineTotal =
                                   product.Offer != null ?
                                   (product.StandardPrice - ((product.Offer.DiscountPercentage / 100m) * product.StandardPrice)) * productQuantities[i]
                                   : product.StandardPrice * productQuantities[i],
                };
                orderItems.Add(orderItem);
            }

            Order order = new Order()
            {
                CustomerId = customerId,
                BillingAddressId = customer.CustomerAddresses.FirstOrDefault()!.Id,
                CurrencyId = 1,
                StatusId = (int)OrderStatus.Pending,
                PaymentMethodTypeId = (int)PaymentMethod.Cash,
                OrderDate = DateTime.Now,
                ShippingAmount = 0,
                TaxAmount = 0,
                EstimatedDeliveryDate = DateTime.Now.AddDays(1),
                TotalAmount = orderItems.Sum(oi => oi.LineTotal),
                Items = orderItems
            };
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();

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
