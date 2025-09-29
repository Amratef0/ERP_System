using ERP_System_Project.ViewModels.Inventory;
using FluentValidation;

namespace ERP_System_Project.Validators.Inventory
{
    public class ProductVMValidator : AbstractValidator<ProductVM>
    {
        public ProductVMValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product Name Is Required")
                .MaximumLength(50).WithMessage("Product Name Must Be Less Than 50 Characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Product Description Is Required");

            RuleFor(x => x.UnitCost)
                .NotEmpty().WithMessage("Unit Cost Is Required")
                .GreaterThan(0).WithMessage("Unit Cost must be Graeter than 0");

            RuleFor(x => x.StandardPrice)
                .NotEmpty().WithMessage("Standard Price Is Required")
                .GreaterThan(0).WithMessage("Standard Price must be Greater Than 0");

            RuleFor(x => x.BrandId)
                .NotEmpty().WithMessage("Brand Is Required")
                .NotEqual(0).WithMessage("Please select a brand.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category Is Required")
                .NotEqual(0).WithMessage("Please select a category.");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity Is Required")
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be >= 0");

      

            RuleFor(x => x.AttributesVM)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("At least one attribute is required.")
                .Must(list => list.Count <= 5).WithMessage("You can only add up to 5 attributes.")
                .Must(attrs => attrs.Select(a => a.AtrributeId).Distinct().Count() == attrs.Count)
                .WithMessage("Duplicate attributes are not allowed.");

            RuleForEach(x => x.AttributesVM).Cascade(CascadeMode.Stop).ChildRules(attr =>
            {
                attr.RuleFor(a => a.AtrributeId)
                    .NotEmpty().WithMessage("Attribute is required.")
                    .NotEqual(0).WithMessage("Please select an attribute.");

                attr.RuleFor(a => a.Value)
                    .NotEmpty().WithMessage("Attribute value is required.")
                    .MaximumLength(255).WithMessage("Attribute Value must be less than 255 characters");
            });


            RuleFor(x => x.Image)
                .NotNull().WithMessage("Image is required.");
        }
    }

}
