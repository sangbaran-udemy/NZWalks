using FluentValidation;

namespace NZWalks.API.Validaators
{
    public class UpdateRegionRequestValidator : AbstractValidator<Models.DTO.UpdateRegionRequest>
    {
        public UpdateRegionRequestValidator() 
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Area).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Population).GreaterThan(0);
        }
    }
}
