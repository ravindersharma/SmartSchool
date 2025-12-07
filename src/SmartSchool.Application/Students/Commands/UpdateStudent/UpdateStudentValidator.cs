using FluentValidation;

namespace SmartSchool.Application.Students.Commands.UpdateStudent
{
    public class UpdateStudentValidator : AbstractValidator<UpdateStudentCommand>
    {
        public UpdateStudentValidator()
        {
            RuleFor(x => x.Dto.FullName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Dto.Grade).NotEqual(0).InclusiveBetween(1, 12);
            RuleFor(x => x.Dto.DOB).LessThan(DateTime.UtcNow);

        }
    }
}
