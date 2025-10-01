using ERP_System_Project.ViewModels.Inventory;
using FluentValidation;

namespace ERP_System_Project.Validators.Inventory
{
    public class AttributeVMValidator : AbstractValidator<AttributeVM>
    {
        public AttributeVMValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name Is Required")
                .Length(1,25).WithMessage("Category Name Must Be Less Than 25 Characters");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Description Is Required")
                .Length(1, 25).WithMessage("Category Name Must Be Less Than 25 Characters");

        }
    }
}
