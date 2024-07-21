using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Stock;
using FluentValidation;

namespace api.Validations
{
    public class CreateStockRequestValidator : AbstractValidator<CreateStockRequest>
    {
        public CreateStockRequestValidator()
        {
            RuleFor(x => x.Symbol)
            .NotEmpty().NotNull().WithMessage("Falta el campo Symbol o esta vacio")
            .MaximumLength(10).WithMessage("Symbol no puede contener mas de 10 caracteres")
            .MinimumLength(2).WithMessage("Symbol debe contener 2 o mas caracteres");
            RuleFor(x => x.CompanyName)
            .NotEmpty().NotNull().WithMessage("Falta el campo CompanyName o esta vacio")
            .MaximumLength(20).WithMessage("CompanyName no puede contener mas de 20 caracteres");
            RuleFor(x => x.Purchase)
            .NotEmpty().NotNull().WithMessage("Falta el campo Purchase o esta vacio")
            .InclusiveBetween(1, 1000000000).WithMessage("Purchase debe estar en el rango de 1 y 1000000000");
            RuleFor(x => x.LastDiv)
            .NotEmpty().NotNull().WithMessage("Falta el campo LastDiv o esta vacio")
            .InclusiveBetween(0,100).WithMessage("LastDiv debe estar en el rango de 0.001,100");
            RuleFor(x => x.Industry)
            .NotEmpty().NotNull().WithMessage("Falta el campo Industry o esta vacio")
            .MaximumLength(30).WithMessage("Industry no puede tener mas de 30 caracteres");     
            RuleFor(x => x.MarketCap)
            .NotEmpty().NotNull().WithMessage("Falta el MarketCap o esta vacio")
            .InclusiveBetween(1,5000000000).WithMessage("MarketCap debe estar en el rango de 1 a 5000000000");
        }
    }
}