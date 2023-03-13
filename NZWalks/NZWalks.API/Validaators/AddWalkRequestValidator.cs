using FluentValidation;

namespace NZWalks.API.Validaators
{
    public class AddWalkRequestValidator : AbstractValidator<Models.DTO.AddWalkRequest>
    {
        public AddWalkRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThanOrEqualTo(1);
        }
    }
}
