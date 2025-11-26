using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Interfaces.ECommerce;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.ECommerce;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using System.Drawing.Printing;
using System.Linq.Expressions;

namespace ERP_System_Project.Services.Implementation.ECommerce
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _uow;
        private readonly string SessionKey = "MyCart"; 

        public CartService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _uow = unitOfWork;
        }
        public async Task<CartVM> GetAllFromCart()
        {
            //var productIds = GetProductsCartIds();
            //var productsSessionDictionary = GetDictionaryFromSession(SessionKey);

            //var productsCart = await _uow.Products.GetAllAsync(
            //    selector: p => new ProductCartVM
            //    {
            //        Id = p.Id,
            //        Name = p.Name,
            //        ImageURL = p.ImageURL,

            //        Price = p.StandardPrice,
            //        HaveOffer = p.Offer != null,
            //        DiscountPercentage = p.Offer != null ? p.Offer.DiscountPercentage : 0,
            //        NetPrice =
            //            p.Offer != null ?
            //            p.StandardPrice - ((p.Offer.DiscountPercentage / 100m) * p.StandardPrice)
            //            : p.StandardPrice,

            //        Quantity =
            //            productsSessionDictionary[p.Id] > p.Quantity 
            //            ?
            //            p.Quantity : productsSessionDictionary[p.Id]
            //    },
            //    filter: p => p.Quantity > 0 && productIds.Contains(p.Id),
            //    Includes: new Expression<Func<Product, object>>[]
            //    {
            //        p => p.Offer,
            //    }

            //);

            //decimal totalPrice = 0;
            //foreach ( var product in productsCart ) 
            //    totalPrice += product.NetPrice;

            //return new CartVM
            //{
            //    productsCart = productsCart,
            //    TotalPrice = totalPrice
            //};

            var productsSessionDictionary = GetDictionaryFromSession(SessionKey);
            var productIds = productsSessionDictionary.Keys.ToList();

            var products = await _uow.Products.GetAllAsync(
                selector: p => new
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageURL = p.ImageURL,
                    StandardPrice = p.StandardPrice,
                    Quantity = p.Quantity,
                    Offer = p.Offer
                },
                filter: p => p.Quantity > 0 && productIds.Contains(p.Id),
                Includes: new Expression<Func<Product, object>>[]
                {
                    p => p.Offer
                }
            );

            var productsCart = products.Select(p =>
            {
                var cartQuantity = productsSessionDictionary[p.Id];
                var discount = p.Offer?.DiscountPercentage ?? 0;
                var netPrice = p.StandardPrice - ((discount / 100m) * p.StandardPrice);
                var finalQuantity = cartQuantity > p.Quantity ? p.Quantity : cartQuantity;

                return new ProductCartVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageURL = p.ImageURL,
                    Price = p.StandardPrice,
                    HaveOffer = p.Offer != null,
                    DiscountPercentage = discount,
                    NetPrice = netPrice,
                    Quantity = finalQuantity
                };
            }).ToList();

            var totalPrice = productsCart.Sum(p => p.NetPrice * p.Quantity);

            return new CartVM
            {
                productsCart = productsCart,
                TotalPrice = totalPrice
            };

        }
        public void AddToCart(int productId, int quantity)
        {
            var dictionary = GetDictionaryFromSession(SessionKey);
            dictionary[productId] = quantity;
            SaveDictionarySession(dictionary);
        }

        public void RemoveFromCart(int productId)
        {
            var dictionary = GetDictionaryFromSession(SessionKey);
            dictionary.Remove(productId);
            SaveDictionarySession(dictionary);
        }

        private Dictionary<int, int> GetDictionaryFromSession(string sessionKey)
        {
            var json = _httpContextAccessor
                .HttpContext.Session.GetString(SessionKey);

            if (string.IsNullOrEmpty(json))
                return new Dictionary<int, int>();

            return  JsonConvert.DeserializeObject<Dictionary<int, int>>(json)
                ?? new Dictionary<int, int>(); ;
        }

        private void SaveDictionarySession(Dictionary<int, int> dictionary)
        {
            var json = JsonConvert.SerializeObject(dictionary);
            _httpContextAccessor.HttpContext.Session
                .SetString(SessionKey, json);
        }
        public async Task ClearCartAsync()
        {
            _httpContextAccessor.HttpContext.Session.Remove(SessionKey);
        }

    }
}
