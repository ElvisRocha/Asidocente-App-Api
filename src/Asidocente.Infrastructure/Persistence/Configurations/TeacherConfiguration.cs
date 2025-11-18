using Asidocente.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asidocente.Infrastructure.Persistence.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("teachers");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("id");
        builder.Property(t => t.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
        builder.Property(t => t.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
        builder.Property(t => t.Identification).HasColumnName("identification").HasMaxLength(20).IsRequired();
        builder.Property(t => t.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
        builder.Property(t => t.Phone).HasColumnName("phone").HasMaxLength(20).IsRequired();
        builder.Property(t => t.Specialization).HasColumnName("specialization").HasMaxLength(100);
        builder.Property(t => t.HireDate).HasColumnName("hire_date");
        builder.Property(t => t.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(t => t.SchoolId).HasColumnName("school_id").IsRequired();
        builder.Property(t => t.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(t => t.UpdatedAt).HasColumnName("updated_at");
        builder.Property(t => t.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
        builder.Property(t => t.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
        builder.Property(t => t.UpdatedBy).HasColumnName("updated_by").HasMaxLength(50);

        builder.HasOne(t => t.School).WithMany(s => s.Teachers).HasForeignKey(t => t.SchoolId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(t => t.Identification).IsUnique().HasDatabaseName("ix_teachers_identification");
        builder.Ignore(t => t.DomainEvents);
    }
}
