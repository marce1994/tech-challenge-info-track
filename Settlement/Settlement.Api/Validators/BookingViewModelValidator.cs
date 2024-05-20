using FluentValidation;
using Settlement.Api.ViewModels;

namespace Settlement.Api.Validators;

public class BookingViewModelValidator : AbstractValidator<BookingViewModel>
{
    public BookingViewModelValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(x => x.BookingTime).NotEmpty().InclusiveBetween(new TimeOnly(0, 0), new TimeOnly(23, 59));
    }
}