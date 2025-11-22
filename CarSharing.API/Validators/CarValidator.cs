using FluentValidation;
using MyWebApi.Models; // Corrected namespace

namespace MyWebApi.Validators
{
    public class CarValidator : AbstractValidator<Car>
    {
        public CarValidator()
        {
            RuleFor(x => x.Brand).NotEmpty().MinimumLength(2).WithMessage("Бренд автомобіля не може бути порожнім і має містити щонайменше 2 символи.");
            RuleFor(x => x.Model).NotEmpty().MinimumLength(2).WithMessage("Модель автомобіля не може бути порожнім і має містити щонайменше 2 символи.");
            RuleFor(x => x.Year).InclusiveBetween(2000, 2025).WithMessage("Рік випуску має бути між 2000 і 2025.");
        }
    }
}