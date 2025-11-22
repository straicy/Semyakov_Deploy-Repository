using FluentValidation;
using MyWebApi.Models;
using MongoDB.Bson;

namespace MyWebApi.Validators
{
    public class BookingValidator : AbstractValidator<Booking>
    {
        public BookingValidator()
        {
            RuleFor(x => x.CarId)
                .NotEmpty().WithMessage("ID автомобіля не може бути порожнім.")
                .Must(BeValidObjectId).WithMessage("ID автомобіля має бути валідним ObjectId.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("ID користувача не може бути порожнім.")
                .Must(BeValidObjectId).WithMessage("ID користувача має бути валідним ObjectId.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Дата початку бронювання не може бути порожньою.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("Кінцева дата не може бути порожньою.")
                .GreaterThan(x => x.StartDate).WithMessage("Кінцева дата має бути пізніше за початкову.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Статус бронювання має бути Active, Completed або Cancelled.");
        }

        private bool BeValidObjectId(string id)
        {
            return ObjectId.TryParse(id, out _);
        }
    }
}