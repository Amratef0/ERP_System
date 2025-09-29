using ERP_System_Project.Extensions;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.Inventory;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP_System_Project.Services.Implementation.Inventory
{
    public class ProductService : GenericService<Product>,IProductService
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
            // 1️⃣ Create and save product FIRST
            var product = new Product
            {
                Name = model.Name,
                BrandId = model.BrandId,
                CategoryId = model.CategoryId,
                Description = model.Description,
                UnitCost = model.UnitCost,
                StandardPrice = model.StandardPrice,
            };

            await _uow.Products.AddAsync(product);
            await _uow.CompleteAsync(); // ✅ Ensures Product.Id is generated

            // 2️⃣ Now add product variants
            foreach (var variant in model.ProductVariants)
            {
                var imageUrl = await variant.Image.SaveImageAsync(_env);

                var newVariant = new ProductVariant
                {
                    ProductId = product.Id,      // ✅ Link to product
                    AdditionalPrice = variant.AdditionalPrice,
                    ImageURL = imageUrl,
                    Quantity = variant.Quantity,
                    IsDefault = variant.IsDefault,
                };

                await _uow.ProductVariants.AddAsync(newVariant);
                await _uow.CompleteAsync(); // ✅ Ensures Variant.Id is generated

                var newAttributeValue = new VariantAttributeValue
                {
                    VariantId = newVariant.Id,       // ✅ Now valid
                    AtrributeId = variant.AtrributeId,
                    Value = variant.AtrributeValue
                };

                await _uow.VariantAttributeValues.AddAsync(newAttributeValue);
            }

            await _uow.CompleteAsync();
        }


        public async Task<List<ProductAttributeVM>> GetAllProductAttributes()
        {
            return await _uow.ProductAttributes.GetAllAsync(
                     selector: pa => new ProductAttributeVM { Id =  pa.Id, Name = pa.Name }
                     );
        }
        public Task<PageSourcePagination<ProductVM>> GetProductsPaginated(int pageNumber, int pageSize, string? searchByName = null)
        {
            if (!string.IsNullOrEmpty(searchByName))
            {
                return _uow.Products.GetAllPaginatedAsync(
                    selector: b => new ProductVM
                    {
                        Description = b.Description,
                        Name = b.Name,
                        Id = b.Id,
                    },
                    filter: b => b.Name.Contains(searchByName),
                    pageNumber: pageNumber,
                    pageSize: pageSize
                );
            }

            return _uow.Products.GetAllPaginatedAsync(
                selector: b => new ProductVM
                {
                    Description = b.Description,
                    Name = b.Name,
                    Id = b.Id,
                },
                pageNumber: pageNumber,
                pageSize: pageSize
            );
        }
    }
}
