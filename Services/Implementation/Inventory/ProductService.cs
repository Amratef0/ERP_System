using ERP_System_Project.Extensions;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.Inventory;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var imageUrl = await model.Image.SaveImageAsync(_env);

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
                await DeleteImageFileAsync(product.ImageURL);
                product.ImageURL = null;
            }

            // Handle new image upload
            if (model.NewImage != null)
            {
                if (!string.IsNullOrEmpty(product.ImageURL))
                {
                    await DeleteImageFileAsync(product.ImageURL);
                }
                product.ImageURL = await model.NewImage.SaveImageAsync(_env);
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


        private async Task DeleteImageFileAsync(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var filePath = Path.Combine("wwwroot", imageUrl.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    await Task.Run(() => File.Delete(filePath));
                }
            }
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


        public Task<PageSourcePagination<ProductVM>> GetProductsPaginated(int pageNumber, int pageSize,
                                                                        string? searchByName = null, string? lowStock = null)
        {
            Expression<Func<Product, bool>>? serachFilter = null;
            if (!string.IsNullOrEmpty(lowStock))
            {
                if (lowStock == "lowStock")
                {
                    serachFilter = p => p.Quantity <= 10;
                    if (!string.IsNullOrEmpty(searchByName))
                        serachFilter = p => p.Quantity <= 10 && p.Name.Contains(searchByName);
                }
                else if (lowStock == "outOfStock")
                {
                    serachFilter = p => p.Quantity == 0;
                    if (!string.IsNullOrEmpty(searchByName))
                        serachFilter = p => p.Quantity == 0 && p.Name.Contains(searchByName);
                }
                else
                {
                    serachFilter = p => p.Quantity > 10;
                    if (!string.IsNullOrEmpty(searchByName))
                        serachFilter = p => p.Quantity > 10 && p.Name.Contains(searchByName);
                }
            }
                


            if (!string.IsNullOrEmpty(searchByName) || !string.IsNullOrEmpty(lowStock))
            {
                return _uow.Products.GetAllPaginatedAsync(
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
                        }).ToList()
                    },
                    filter: serachFilter,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    Includes: p => p.Attributes
                );
            }
            //if (!string.IsNullOrEmpty(stockLevel))
            //{
            //    return _uow.Products.GetAllPaginatedAsync(
            //        selector: p => new ProductVM
            //        {
            //            Description = p.Description,
            //            Name = p.Name,
            //            Id = p.Id,
            //            ImageURL = p.ImageURL,
            //            CategoryId = p.CategoryId,
            //            BrandId = p.BrandId,
            //            Quantity = p.Quantity,
            //            UnitCost = p.UnitCost,
            //            StandardPrice = p.StandardPrice,
            //            AttributesVM = p.Attributes.Select(a => new ProductAttributeValueVM
            //            {
            //                AtrributeId = a.AtrributeId,
            //                Value = a.Value
            //            }).ToList()
            //        },
            //        filter: serachFilter,
            //        pageNumber: pageNumber,
            //        pageSize: pageSize,
            //        Includes: p => p.Attributes
            //    );
            //}
            //if (!string.IsNullOrEmpty(searchByName) && !string.IsNullOrEmpty(stockLevel))
            //{
            //    return _uow.Products.GetAllPaginatedAsync(
            //        selector: p => new ProductVM
            //        {
            //            Description = p.Description,
            //            Name = p.Name,
            //            Id = p.Id,
            //            ImageURL = p.ImageURL,
            //            CategoryId = p.CategoryId,
            //            BrandId = p.BrandId,
            //            Quantity = p.Quantity,
            //            UnitCost = p.UnitCost,
            //            StandardPrice = p.StandardPrice,
            //            AttributesVM = p.Attributes.Select(a => new ProductAttributeValueVM
            //            {
            //                AtrributeId = a.AtrributeId,
            //                Value = a.Value
            //            }).ToList()
            //        },
            //        filter: p => (p.Name.Contains(searchByName)),
            //        pageNumber: pageNumber,
            //        pageSize: pageSize,
            //        Includes: p => p.Attributes
            //    );
            //}

            return _uow.Products.GetAllPaginatedAsync(
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
                    }).ToList()
                },
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    Includes: p => p.Attributes
            );
        }
    }
}
