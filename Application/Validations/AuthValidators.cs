using Domain.Models;
using FluentValidation;

namespace Application.Validations;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email wajib diisi")
            .EmailAddress().WithMessage("Format email tidak valid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password wajib diisi");
    }
}

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email wajib diisi")
            .EmailAddress().WithMessage("Format email tidak valid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password wajib diisi")
            .MinimumLength(8).WithMessage("Password minimal 8 karakter");

        RuleFor(x => x.Role)
            .Must(role => string.IsNullOrEmpty(role) || role.Equals("USER", StringComparison.OrdinalIgnoreCase) || role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Role hanya boleh bernilai USER atau ADMIN");
    }
}
