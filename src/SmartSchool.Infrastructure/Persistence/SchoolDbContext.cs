using Microsoft.EntityFrameworkCore;
using SmartSchool.Domain.Entities;

namespace SmartSchool.Infrastructure.Persistence;

public class SchoolDbContext : DbContext
{
    public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
    {
    }
    public DbSet<Student> Students => Set<Student>();
}
