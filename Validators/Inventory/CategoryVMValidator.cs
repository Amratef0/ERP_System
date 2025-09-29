using ERP_System_Project.ViewModels.Inventory;
using FluentValidation;

namespace ERP_System_Project.Validators.Inventory
{
    public class CategoryVMValidator : AbstractValidator<CategoryVM>
    {
        public CategoryVMValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name Is Required")
                .Length(1,50).WithMessage("Category Name Must Be Less Than 50 Characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description Is Required");
        }
    }
}
