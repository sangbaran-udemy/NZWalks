using FluentValidation;

namespace NZWalks.API.Validaators
{
    public class AddWalkDifficultyRequestValidator : AbstractValidator<Models.DTO.AddWalkDifficultyRequest>
    {
        public AddWalkDifficultyRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
