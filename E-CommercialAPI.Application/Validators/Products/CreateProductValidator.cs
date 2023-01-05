using E_CommercialAPI.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Application.Validators.Products
{
    public class CreateProductValidator: AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Please, dont skip product name empty!")
                .MaximumLength(100)
                .MinimumLength(5)
                        .WithMessage("Please, enter product name between 5 and 100 characters!");

            RuleFor(x => x.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Please, dont skip stock empty!")
                .Must(s => s >= 0)
                    .WithMessage("Stock can't be negative!");

            RuleFor(x => x.Price)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Please, dont skip price empty!")
                .Must(p => p >= 0)
                    .WithMessage("Price can't be negative!");
        }
    }
}
