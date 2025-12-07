using FluentResults;
using Mapster;
using SmartSchool.Application.Students.Dtos;
using SmartSchool.Application.Students.Interfaces;
using SmartSchool.Application.Users.Interfaces;
using SmartSchool.Domain.Entities;
using SmartSchool.Shared;

namespace SmartSchool.Infrastructure.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IUserRepository _userRepo;

        public StudentService(IStudentRepository studentRepo, IUserRepository userRepo)
        {
            _studentRepo = studentRepo;
            _userRepo = userRepo;
        }

        public async Task<Result<StudentDto>> CreateAsync(CreateStudentDto dto, CancellationToken ct)
        {
            var user = await _userRepo.GetByIdAsync(dto.UserId, ct);
            if (user == null)
                return Result.Fail<StudentDto>("User not found for student creation");

            var student = dto.Adapt<Student>();
            await _studentRepo.AddAsync(student, ct);

            var loaded = await _studentRepo.GetFullByIdAsync(student.Id, ct);

            return Result.Ok(loaded!.Adapt<StudentDto>());
        }

        public async Task<Result<StudentDto>> UpdateAsync(Guid id, UpdateStudentDto dto, CancellationToken ct)
        {
            var student = await _studentRepo.GetByIdAsync(id, ct);
            if (student == null)
                return Result.Fail<StudentDto>("Student not found");

            student.FullName = dto.FullName;
            student.Grade = dto.Grade;
            student.DOB = dto.DOB;
            student.NationalId = dto.NationalId;
            student.Nationality = dto.Nationality;
            student.UpdatedAt = DateTime.UtcNow;

            await _studentRepo.UpdateAsync(student, ct);
            return Result.Ok(student.Adapt<StudentDto>());
        }

        public async Task<Result<StudentDto>> SoftDeleteAsync(Guid id, CancellationToken ct)
        {
            var student = await _studentRepo.GetByIdAsync(id, ct);
            if (student == null)
                return Result.Fail<StudentDto>("Student not found");

            if (student.IsDeleted)
                return Result.Fail<StudentDto>("Student already soft deleted");

            student.IsDeleted = true;
            await _studentRepo.UpdateAsync(student, ct);

            return Result.Ok(student.Adapt<StudentDto>());
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken ct)
        {
            var student = await _studentRepo.GetFullByIdAsync(id, ct);
            if (student == null)
                return Result.Fail("Student not found");

            if (!student.IsDeleted)
                return Result.Fail("Student must be soft deleted before permanent delete");

            await _studentRepo.DeleteAsync(student, ct);
            return Result.Ok();
        }

        public async Task<Result<StudentDto>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var student = await _studentRepo.GetFullByIdAsync(id, ct);
            if (student == null)
                return Result.Fail<StudentDto>("Student not found");

            return Result.Ok(student.Adapt<StudentDto>());
        }

        public async Task<Result<StudentDto>> GetByUserIdAsync(Guid userId, CancellationToken ct)
        {
            var student = await _studentRepo.GetByUserIdAsync(userId, ct);
            if (student == null)
                return Result.Fail<StudentDto>("Student not found");

            return Result.Ok(student.Adapt<StudentDto>());
        }

        public async Task<Result<PagedResult<StudentDto>>> GetPagedAsync(int page, int pageSize, CancellationToken ct)
        {
            var paged = await _studentRepo.GetPagedAsync(page, pageSize, ct);

            var dtoItems = paged.Items.Select(s => s.Adapt<StudentDto>());
            return Result.Ok(new PagedResult<StudentDto>(dtoItems, paged.TotalCount, page, pageSize));
        }
    }
}
