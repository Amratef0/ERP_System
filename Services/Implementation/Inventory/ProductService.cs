using ERP_System_Project.Extensions;
using ERP_System_Project.Helpers;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Repository.Interfaces;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.ECommerce;
using ERP_System_Project.ViewModels.Inventory;
using LinqKit;
using System.Linq.Expressions;

namespace ERP_System_Project.Services.Implementation.Inventory
{
    public class ProductService : GenericService<Product>, IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _env;
        private readonly IProductRepository _productRepository;
        public ProductService(IUnitOfWork uow,  IWebHostEnvironment env, IProductRepository productRepositroy) : base(uow)
        {
            _uow = uow;
            _env = env;
            _productRepository = productRepositroy;
        }

        public async Task AddNewProduct(ProductVM model)
        {
            var imageUrl = await model.Image.SaveImageAsync(_env,"Uploads/Images/Products");

            var product = new Product
            {
                Name = model.Name,
                BrandId = model.BrandId,
                CategoryId = model.CategoryId,
                Description = model.Description,
                UnitCost = model.UnitCost,
                StandardPrice = model.StandardPrice,
                ImageURL = imageUrl,
                Quantity = model.Quantity,
                Attributes = model.AttributesVM.Select(m => new ProductAttributeValue
                {
                    AtrributeId = m.AtrributeId,
                    Value = m.Value
                }).ToList()
            };

            await _uow.Products.AddAsync(product);
            await _uow.CompleteAsync(); // Save so we can get product.Id

        }


        public async Task UpdateCustomProduct(EditProductVM model)
        {
            var product = await _uow.Products.GetByIdAsync(model.Id);
            product.Name = model.Name;
            product.Description = model.Description;
            product.UnitCost = model.UnitCost;
            product.StandardPrice = model.StandardPrice;
            product.Quantity = model.Quantity;
            product.BrandId = model.BrandId;
            product.CategoryId = model.CategoryId;
            product.ModifiedDate = DateTime.UtcNow;

            // Handle image removal
            if (model.RemoveImage && !string.IsNullOrEmpty(product.ImageURL))
            {
                await FileHelper.DeleteImageFileAsync(product.ImageURL);
                product.ImageURL = null;
            }

            // Handle new image upload
            if (model.NewImage != null)
            {
                if (!string.IsNullOrEmpty(product.ImageURL))
                {
                    await FileHelper.DeleteImageFileAsync(product.ImageURL);
                }
                product.ImageURL = await model.NewImage.SaveImageAsync(_env, "Uploads/Images/Products");
            }

            // Update attributes
            await _uow.ProductAttributeValues.BulkDeleteAsync(pav => pav.ProductId == model.Id);

            foreach (var attrVM in model.AttributesVM)
            {
                if (!string.IsNullOrEmpty(attrVM.Value) && attrVM.AtrributeId > 0)
                {
                    var productAttributeValue = new ProductAttributeValue
                    {
                        ProductId = model.Id,
                        AtrributeId = attrVM.AtrributeId,
                        Value = attrVM.Value
                    };
                    await _uow.ProductAttributeValues.AddAsync(productAttributeValue);
                }
            }

            await _uow.CompleteAsync();
        }



        public async Task<List<ProductAttributeVM>> GetAllProductAttributes()
        {
            return await _uow.ProductAttributes.GetAllAsync(
                     selector: pa => new ProductAttributeVM { Id =  pa.Id, Name = pa.Name }
                     );
        }

        public async Task<EditProductVM> GetCustomProduct(int productId)
        {
            var product = await _uow.Products.GetAsync(
                filter: p => p.Id == productId,
                selector: p => new EditProductVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    BrandId = p.BrandId ?? 0,
                    CategoryId = p.CategoryId ?? 0,
                    StandardPrice = p.StandardPrice,
                    UnitCost = p.UnitCost,
                    ImageURL = p.ImageURL,
                    Quantity = p.Quantity,
                    AttributesVM = p.Attributes.Select(pa => new ProductAttributeValueVM
                    {
                        AtrributeId = pa.AtrributeId,
                        Value = pa.Value,
                    }).ToList()
                }
            );

            return product;

        }


        public async Task<PageSourcePagination<ProductVM>> GetProductsPaginated(int pageNumber, int pageSize,
                                                                        string? searchByName = null, string? lowStock = null)
        {
            Expression<Func<Product, bool>>? searchFilter = null;

            if (!string.IsNullOrEmpty(searchByName))
                searchFilter = p => p.Name.Contains(searchByName);

            if (lowStock == "lowStock")
            {
                searchFilter = searchFilter == null
                    ? p => p.Quantity <= 10
                    : p => p.Quantity <= 10 && p.Name.Contains(searchByName);
            }
            else if (lowStock == "outOfStock")
            {
                searchFilter = searchFilter == null
                    ? p => p.Quantity == 0
                    : p => p.Quantity == 0 && p.Name.Contains(searchByName);
            }
            else if (lowStock == "inStock")
            {
                searchFilter = searchFilter == null
                    ? p => p.Quantity > 10
                    : p => p.Quantity > 10 && p.Name.Contains(searchByName);
            }

            return await _uow.Products.GetAllPaginatedAsync(
                selector: p => new ProductVM
                {
                    Description = p.Description,
                    Name = p.Name,
                    Id = p.Id,
                    ImageURL = p.ImageURL,
                    CategoryId = p.CategoryId ?? 0,
                    BrandId = p.BrandId ?? 0,
                    Quantity = p.Quantity,
                    UnitCost = p.UnitCost,
                    StandardPrice = p.StandardPrice,
                    AttributesVM = p.Attributes.Select(a => new ProductAttributeValueVM
                    {
                        AtrributeId = a.AtrributeId,
                        Value = a.Value
                    }).ToList(),
                    OfferPercentege = p.Offer != null ? p.Offer.DiscountPercentage : 0
                },
                filter: searchFilter,
                pageNumber: pageNumber,
                pageSize: pageSize,
                Includes: new Expression<Func<Product, object>>[]
                {
                    p => p.Attributes,
                    p => p.Offer
                }
            );
        }

        public async Task<PageSourcePagination<ProductCardVM>> GetProductsPaginated(int pageNumber, int pageSize,
                                                                        string? searchByName = null,
                                                                        string? brandName = null,
                                                                        string? categoryName = null,
                                                                        int? minPrice = null, int? maxPrice = null)
        {
            Expression<Func<Product, bool>> searchFilter = p => true;

            if (!string.IsNullOrEmpty(searchByName))
                searchFilter = searchFilter.And(p => p.Name.Contains(searchByName));

            if (!string.IsNullOrEmpty(brandName))
                searchFilter = searchFilter.And(p => p.Brand.Name.Contains(brandName));

            if (!string.IsNullOrEmpty(categoryName))
                searchFilter = searchFilter.And(p => p.Category.Name.Contains(categoryName));

            if(minPrice.HasValue && maxPrice.HasValue)
            {
                minPrice = Math.Min(minPrice.Value, maxPrice.Value);
                maxPrice = Math.Max(minPrice.Value, maxPrice.Value);
            }

            if (minPrice.HasValue)
                searchFilter = searchFilter.And(p => p.StandardPrice >= minPrice);

            if (maxPrice.HasValue)
                searchFilter = searchFilter.And(p => p.StandardPrice <= maxPrice);


            return await _uow.Products.GetAllPaginatedAsync(
                selector: p => new ProductCardVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageURL = p.ImageURL,

                    CategoryName = p.Category.Name,
                    BrandName = p.Brand.Name,
                    BrandImageURL = p.Brand.LogoURL,

                    Price = p.StandardPrice,
                    HaveOffer = p.Offer != null,
                    DiscountPercentage = p.Offer != null ? p.Offer.DiscountPercentage : 0,
                    NetPrice =
                        p.Offer != null ?
                        p.StandardPrice - ((p.Offer.DiscountPercentage / 100m) * p.StandardPrice) 
                        : p.StandardPrice,

                    NumberOfReviews = p.CustomerReviews != null ? p.CustomerReviews.Count : 0,
                    TotalRate = p.CustomerReviews != null && p.CustomerReviews.Any()
                        ? (int)Math.Round(p.CustomerReviews.Average(cr => cr.Rating))
                        : 0,

                },
                expandable: true,
                filter: searchFilter,
                pageNumber: pageNumber,
                pageSize: pageSize,
                Includes: new Expression<Func<Product, object>>[]
                {
                    p => p.Attributes,
                    p => p.Offer,
                    p => p.Brand,
                    p => p.Category,
                    p => p.CustomerReviews
                }
            );
        }

        public async Task<ProductDetailsVM> GetProductDetails(int productId)
        {
            var product =  await _productRepository.GetAsync(
                filter: p => p.Id == productId,
                selector: p => new ProductDetailsVM
                {
                    Id = productId,
                    Name = p.Name,
                    Description = p.Description,
                    ImageURL = p.ImageURL,

                    BrandName = p.Brand.Name,
                    BrandImageURL = p.Brand.LogoURL,
                    CategoryName = p.Category.Name,

                    Price = p.StandardPrice,
                    HasOffer = p.Offer != null,
                    DiscountPercentage = p.Offer != null ? p.Offer.DiscountPercentage : 0,
                    NetPrice = p.Offer != null ?
                        p.StandardPrice - ((p.Offer.DiscountPercentage / 100m) * p.StandardPrice)
                        : p.StandardPrice,

                    NumberOfReviews = p.CustomerReviews != null ? p.CustomerReviews.Count : 0,
                    TotalRate = p.CustomerReviews != null && p.CustomerReviews.Any()
                        ? (int)Math.Round(p.CustomerReviews.Average(cr => cr.Rating))
                        : 0,

                    QuantityInStock = p.Quantity,
                    Reviews = p.CustomerReviews.Select(cr => new CustomerReviewVM
                    {
                        CustomerId = cr.CustomerId,
                        Comment = cr.Comment,
                        CustomerName = cr.Customer.FullName,
                        DateCreated = cr.CreatedAt,
                        Rate = (int)cr.Rating
                    }).ToList(),

                    Attributes = p.Attributes.Select(a => new AttributeVM
                    {
                        Id = a.AtrributeId,
                        Name = a.ProductAttribute.Name,
                        Type = a.Value
                    }).ToList()
                },

                Includes: new Expression<Func<Product, object>>[]
                {
                    p => p.Attributes,
                    p => p.Offer,
                    p => p.Brand,
                    p => p.Category,
                    p => p.CustomerReviews,
                }

                );

            return product!;
        }
    }
}
