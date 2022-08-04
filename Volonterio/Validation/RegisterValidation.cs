using FluentValidation;
using Volonterio.Models;

namespace Volonterio.Validation
{
    public class RegisterValidation : AbstractValidator<RegisterUserModel>
    {
        public RegisterValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Поле не може бути пустим!")
                .NotNull().WithMessage("Поле не може бути пустим!")
                .EmailAddress().WithMessage("Поле не відповідає вимогам емейлу");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Поле не може бути пустим!")
                .NotNull().WithMessage("Поле не може бути пустим!");

            RuleFor(x => x.SecondName)
                .NotEmpty().WithMessage("Поле не може бути пустим!")
                .NotNull().WithMessage("Поле не може бути пустим!");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Поле не може бути пустим!")
                .NotNull().WithMessage("Поле не може бути пустим!")
                .MinimumLength(5).WithMessage("Поле має містити більше 5 символів");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Поле не може бути пустим!")
                .NotNull().WithMessage("Поле не може бути пустим!")
                .MinimumLength(5).WithMessage("Поле має містити більше 5 символів");
        }
    }
}
