using ExpenseTrackerApp.Models;
using FluentValidation;

namespace ExpenseTrackerApp.Validations
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(person => person.Name).NotNull().NotEmpty();
            RuleFor(person => person.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(person => person.Password)
                .NotNull().NotEmpty()
                .MinimumLength(6)
                .Must(password => password.FirstOrDefault(character => character >= 'a' && character <= 'z') != 0)
                    .WithMessage("Password should contain at least one lowercase letter")
                .Must(password => password.FirstOrDefault(character => character >= 'A' && character <= 'Z') != 0)
                    .WithMessage("Password should contain at least one uppercase letter")
                .Must(password => password.FirstOrDefault(character => character >= '0' && character <= '9') != 0)
                    .WithMessage("Password should contain at least one digit");
        }
    }
}
