using FluentValidation;

namespace SmartSchool.Application.Students.Commands.UpdateStudent
{
    public class UpdateStudentValidator : AbstractValidator<UpdateStudentCommand>
    {
        public UpdateStudentValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Grade).NotEqual(0).InclusiveBetween(1, 12);
            RuleFor(x => x.DOB).LessThan(DateTime.UtcNow);

        }
    }
}
