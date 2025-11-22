using FluentValidation;
using MyWebApi.Models;

namespace MyWebApi.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Повне ім'я не може бути порожнім.");
            RuleFor(x => x.Email).NotEmpty().Matches(@"^[\w\.-]+@[\w\.-]+\.\w+$").WithMessage("Неправильний формат електронної пошти.");
            RuleFor(x => x.Role).IsInEnum().WithMessage("Роль користувача має бути або Driver, або Admin.");
        }
    }
}