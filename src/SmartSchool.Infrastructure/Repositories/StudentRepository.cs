using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Students.Interfaces;
using SmartSchool.Domain.Entities;
using SmartSchool.Infrastructure.Persistence;

namespace SmartSchool.Infrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly SchoolDbContext _db;

    public StudentRepository(SchoolDbContext db) => _db = db;
    public async Task<Student> AddAsync(Student student, CancellationToken ct)
    {
        _db.Students.Add(student);
        await _db.SaveChangesAsync(ct);
        return student;
    }

    public async Task<Student?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _db.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IEnumerable<Student>> GetPagedAsync(int page, int pageSize, CancellationToken ct)
    {
        return await _db.Students.AsNoTracking().OrderBy(s => s.FullName).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task<Student> UpdateAsync(Student student, CancellationToken ct)
    {
        _db.Students.Update(student);
        await _db.SaveChangesAsync(ct);
        return student;
    }


    public async Task DeleteAsync(Student student, CancellationToken ct)
    {
        _db.Students.Remove(student);
        await _db.SaveChangesAsync(ct);
    }
}
