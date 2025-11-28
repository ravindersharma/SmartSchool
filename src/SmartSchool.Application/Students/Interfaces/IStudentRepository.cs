using SmartSchool.Domain.Entities;

namespace SmartSchool.Application.Students.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student> AddAsync(Student student, CancellationToken ct);
        Task<Student?> GetByIdAsync(Guid Id, CancellationToken ct);

    }
}
