using ERP_System_Project.Models.Inventory;

namespace ERP_System_Project.Specification.Implementation
{
    public class ProductWithCategoryAndBrandSpec : BaseSpecification<Product>
    {
        public ProductWithCategoryAndBrandSpec(int quantity)
        {
            Criteria = p => p.Quantity > quantity;

            ApplyOrderBy(p => p.Quantity);

            ApplyInclude(p => p.Category);
            ApplyInclude(p => p.Brand);

            ApplySelector(p => new test
            {
                ProductName = p.Name,
                CategoryName = p.Category.Name,
                BrandName = p.Brand.Name,
                Quantity = p.Quantity
            });
        }

        // HOW TO USE THIS 
        // GO TO YOUR METHOD IN SERVICE AND DO THAT: 
        /*
            var spec = new ProductWithCategoryAndBrandSpec(5);

            var result = await _uow.Products.GetBySpecification(spec);
        */
    }
    public class test
    {
        public string ProductName { get; set; }
        public string CategoryName {get; set;}
        public string BrandName {get; set;}  
        public int Quantity  {get; set;}
    }
}
