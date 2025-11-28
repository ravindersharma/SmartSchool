using Microsoft.EntityFrameworkCore;
using SmartSchool.Domain.Entities;
using SmartSchool.Infrastructure.Persistence;

namespace SmartSchool.Infrastructure.Repositories;

public class StudentRepository : IStudentRespository
{
    private readonly SchoolDbContext _db;

    public StudentRepository(SchoolDbContext db)
    {
        _db = db;
    }
    public async Task<Student> AddAsync(Student student, CancellationToken ct)
    {
        _db.Students.Add(student);
        await _db.SaveChangesAsync(ct);
        return student;
    }

    public async Task<Student?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _db.Students.FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
