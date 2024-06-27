using FluentValidation;
using GuessTheNumber.Application.Models.Requests;

namespace GuessTheNumber.Application.Models.Validators;

public class InitializeGameRequestValidator : AbstractValidator<InitializeGameRequest>
{
    public InitializeGameRequestValidator()
    {
        RuleFor(x => x.LeftBorder).NotEmpty()
            .LessThan(x => x.RightBorder)
            .WithMessage("Левая граница должна быть меньше правой границы.");

        RuleFor(x => x.RightBorder).NotEmpty();

        RuleFor(x => x.AttemptCount).NotEmpty()
            .GreaterThan(0)
            .WithMessage("Кажется вы попытались ввести некоректное число");
    }
}