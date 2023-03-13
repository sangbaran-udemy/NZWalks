using FluentValidation;

namespace NZWalks.API.Validaators
{
    public class UpdateWalkDifficultyRequestValidator : AbstractValidator<Models.DTO.UpdateWalkDifficultyRequest>
    {
        public UpdateWalkDifficultyRequestValidator() 
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
