using SmartSchool.Domain.Entities;

namespace SmartSchool.Application.Students.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student> AddAsync(Student student, CancellationToken ct);
        Task<Student?> GetByIdAsync(Guid Id, CancellationToken ct);
        Task<IEnumerable<Student>> GetPagedAsync(int page, int pageSize, CancellationToken ct);
        Task<Student> UpdateAsync(Student student, CancellationToken ct);

    }
}
