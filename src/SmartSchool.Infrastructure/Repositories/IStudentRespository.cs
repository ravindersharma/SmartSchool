using SmartSchool.Domain.Entities;

namespace SmartSchool.Infrastructure.Repositories;

public interface IStudentRespository
{
    Task<Student> AddAsync(Student student,CancellationToken ct);
    Task<Student?> GetByIdAsync(Guid id,CancellationToken ct);

}
