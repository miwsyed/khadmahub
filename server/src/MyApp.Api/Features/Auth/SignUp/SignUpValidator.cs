using FluentValidation;

namespace MyApp.Features.Auth.SignUp;

public sealed class SignUpValidator : AbstractValidator<SignUpRequest>
{
    public SignUpValidator()
    {
        RuleFor(request => request.FullName)
            .NotEmpty().WithMessage("AUTH_FULL_NAME_REQUIRED")
            .MinimumLength(2).WithMessage("AUTH_FULL_NAME_REQUIRED")
            .MaximumLength(256).WithMessage("AUTH_FULL_NAME_REQUIRED");

        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("AUTH_USERNAME_REQUIRED")
            .MinimumLength(3).WithMessage("AUTH_USERNAME_REQUIRED")
            .MaximumLength(128).WithMessage("AUTH_USERNAME_REQUIRED")
            .Matches("^[a-zA-Z0-9._]+$").WithMessage("AUTH_USERNAME_PATTERN");

        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("AUTH_EMAIL_REQUIRED")
            .EmailAddress().WithMessage("AUTH_EMAIL_REQUIRED")
            .MaximumLength(256).WithMessage("AUTH_EMAIL_REQUIRED");

        RuleFor(request => request.NationalId)
            .Matches("^\\d{8,12}$").When(request => !string.IsNullOrWhiteSpace(request.NationalId)).WithMessage("AUTH_NATIONAL_ID_FORMAT");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("AUTH_PASSWORD_REQUIRED")
            .MinimumLength(8).WithMessage("AUTH_PASSWORD_MIN_LENGTH")
            .MaximumLength(256).WithMessage("AUTH_PASSWORD_MIN_LENGTH")
            .Matches("[A-Z]").WithMessage("AUTH_PASSWORD_UPPERCASE")
            .Matches("[0-9]").WithMessage("AUTH_PASSWORD_NUMBER");
    }
}
