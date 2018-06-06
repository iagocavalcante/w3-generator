using <%= solutionName %>.Domain.Domain.Models;
using FluentValidation;

namespace <%= solutionName %>.Domain.Domain.Validations
{
    public class modelExampleValidations : AbstractValidator<modelExample>
    {
        public modelExampleValidations()
        {
            RuleFor(x => x.nome)
                .NotEmpty()
                .WithMessage("Nome é obrigatorio");
        }
    }
}
