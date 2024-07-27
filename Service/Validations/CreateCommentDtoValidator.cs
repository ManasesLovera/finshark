using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using FluentValidation;

namespace api.Service.Validations
{
    public class CommentDtoValidator : AbstractValidator<ICommentDto>
    {
        public CommentDtoValidator()
        {
            RuleFor(x => x.Title)
            .NotNull().NotEmpty().WithMessage("falta el titulo")
            .MinimumLength(5).WithMessage("El titulo debe tener 5 caracteres o mas")
            .MaximumLength(280).WithMessage("El titulo no puede contener mas de 280 caracteres");
            RuleFor(x => x.Content)
            .NotNull().NotEmpty().WithMessage("falta el contenido")
            .MinimumLength(5).WithMessage("El contenido debe tener 5 caracteres o mas")
            .MaximumLength(280).WithMessage("El contenido no puede contener mas de 280 caracteres");
        }
    }
}