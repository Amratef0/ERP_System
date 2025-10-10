using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Services.Interfaces.ECommerce;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.ECommerce;

namespace ERP_System_Project.Services.Implementation.ECommerce
{
    public class OfferService : IOfferService
    {
        private readonly IUnitOfWork _uow;
        

        public OfferService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<OfferVM> GetOfferAsync(int productId)
        {
            var DefaultOffer = new OfferVM()
            {
                StartDate = DateTime.Now,
                DiscountPercentage = 0,
                OfferDays = 0,
                Name = "New Offer",
                ProductId = productId,
            };


            var offer = _uow.Offers
            .GetAllAsIQueryable()
            .FirstOrDefault(o => o.ProductId == productId);

            if (offer is not null)
            {
                var offerVM = new OfferVM
                {
                    ProductId = offer.ProductId,
                    Name = offer.Name,
                    DiscountPercentage = offer.DiscountPercentage,
                    OfferDays = offer.EndDate.Subtract(offer.StartDate).Days,
                    StartDate = offer.StartDate
                };
                return offerVM;
            }
            return DefaultOffer;
        }
        
        public async Task SetOfferAsync(OfferVM model)
        {
            var offer = _uow.Offers
                .GetAllAsIQueryable()
                .FirstOrDefault(o => o.ProductId == model.ProductId);

            if(offer is null)
            {
                await _uow.Offers.AddAsync(new Offer
                {
                    Name = model.Name,
                    DiscountPercentage = model.DiscountPercentage,
                    EndDate = DateTime.Now.AddDays(model.OfferDays),
                    ProductId = model.ProductId,
                });
            }
            else
            {
                offer.EndDate = offer.StartDate.AddDays(model.OfferDays);
                offer.DiscountPercentage = model.DiscountPercentage;
                offer.Name = model.Name;
            }

            await _uow.CompleteAsync();
        }
    }
}
