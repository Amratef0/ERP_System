using ERP_System_Project.Extensions;
using ERP_System_Project.Helpers;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.Inventory;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ERP_System_Project.Services.Implementation.Inventory
{
    public class ProductService : GenericService<Product>, IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _env;
        public ProductService(IUnitOfWork uow, IWebHostEnvironment env) : base(uow)
        {
            _uow = uow;
            _env = env;
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

        public Task<EditProductVM> GetCustomProduct(int productId)
        {
            var product = _uow.Products.GetAsync(
                filter: p => p.Id == productId,
                selector: p => new EditProductVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    BrandId = p.BrandId,
                    CategoryId = p.CategoryId,
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
                    CategoryId = p.CategoryId,
                    BrandId = p.BrandId,
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
    }
}
