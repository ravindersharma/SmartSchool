using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Domain.Entities;


namespace SmartSchool.Infrastructure.Persistence.Configurations;


public class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T>
    where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.IsDeleted)
               .HasDefaultValue(false);

        builder.Property(x => x.CreatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP"); // SQLite-safe

        builder.Property(x => x.UpdatedAt)
               .IsRequired(false);

        // Global soft-delete filter
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}

