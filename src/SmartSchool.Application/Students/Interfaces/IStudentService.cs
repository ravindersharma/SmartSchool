using FluentResults;
using SmartSchool.Application.Students.Dtos;
using SmartSchool.Shared;

namespace SmartSchool.Application.Students.Interfaces
{
    public interface IStudentService
    {
        Task<Result<StudentDto>> CreateAsync(CreateStudentDto dto, CancellationToken ct);
        Task<Result<StudentDto>> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Result<StudentDto>> UpdateAsync(Guid id, UpdateStudentDto dto, CancellationToken ct);
        Task<Result<StudentDto>> SoftDeleteAsync(Guid id, CancellationToken ct);
        Task<Result> DeleteAsync(Guid id, CancellationToken ct);
        Task<Result<PagedResult<StudentDto>>> GetPagedAsync(int page, int pageSize, CancellationToken ct);
        Task<Result<StudentDto>> GetByUserIdAsync(Guid userId, CancellationToken ct);
    }
}
