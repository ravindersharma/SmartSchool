using FluentValidation;

namespace SmartSchool.Application.Students.Commands.CreateStudent;

public class CreateStudentValidator : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(x => x.DOB)
            .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.Grade)
            .InclusiveBetween(1, 12).WithMessage("Grade must be between 1 and 12.");

    }
}