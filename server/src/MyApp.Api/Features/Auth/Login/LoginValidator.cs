using FluentValidation;

namespace MyApp.Features.Auth.Login;

public sealed class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("AUTH_USERNAME_REQUIRED")
            .MinimumLength(3).WithMessage("AUTH_USERNAME_REQUIRED")
            .MaximumLength(128).WithMessage("AUTH_USERNAME_REQUIRED");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("AUTH_PASSWORD_REQUIRED")
            .MaximumLength(256).WithMessage("AUTH_PASSWORD_REQUIRED");
    }
}
