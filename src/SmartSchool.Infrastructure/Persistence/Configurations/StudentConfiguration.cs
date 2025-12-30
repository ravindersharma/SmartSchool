using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Domain.Entities;

namespace SmartSchool.Infrastructure.Persistence.Configurations
{
    public class StudentConfiguration : BaseEntityConfiguration<Student>
    {
        public override void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            base.Configure(builder);
            builder.HasKey(s => s.Id);
            builder.Property(s => s.FullName)
                   .IsRequired()
                   .HasMaxLength(150);
            builder.Property(s => s.DOB).IsRequired();
            builder.Property(s => s.Grade).IsRequired();
            builder.Property(s => s.CreatedAt)
                   .HasDefaultValueSql("GETDATE()")
                   .ValueGeneratedOnAdd();
            //builder.Property(x => x.IsDeleted).HasDefaultValue(false);

            //builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
