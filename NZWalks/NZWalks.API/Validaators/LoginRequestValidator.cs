using FluentValidation;

namespace NZWalks.API.Validaators
{
    public class LoginRequestValidator : AbstractValidator<Models.DTO.LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
