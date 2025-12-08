using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Students.Interfaces;
using SmartSchool.Domain.Entities;
using SmartSchool.Infrastructure.Persistence;
using SmartSchool.Shared;

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
        return await _db.Students.FirstOrDefaultAsync(s => s.Id == id  && !s.IsDeleted , ct);
    }
    public async Task<Student?> GetFullByIdAsync(Guid id, CancellationToken ct)
    {
        return await _db.Students.Include(s=>s.User).FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted, ct);
    }
    public async Task<Student?> GetByUserIdAsync(Guid userId, CancellationToken ct)
    {
        return await _db.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.UserId == userId && !s.IsDeleted, ct);
    }

    public async Task<PagedResult<Student>> GetPagedAsync(int page, int pageSize, CancellationToken ct)
    {
        var query = _db.Students.AsNoTracking().Where(s => !s.IsDeleted).OrderBy(s => s.CreatedAt);
        var totalItems = await query.CountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return new PagedResult<Student>(items, totalItems, page, pageSize);
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
