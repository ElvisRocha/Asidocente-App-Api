using Asidocente.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asidocente.Infrastructure.Persistence.Configurations;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.ToTable("subjects");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(s => s.Code).HasColumnName("code").HasMaxLength(20).IsRequired();
        builder.Property(s => s.Description).HasColumnName("description").HasMaxLength(500);
        builder.Property(s => s.Credits).HasColumnName("credits").IsRequired();
        builder.Property(s => s.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(s => s.SchoolId).HasColumnName("school_id").IsRequired();
        builder.Property(s => s.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(s => s.UpdatedAt).HasColumnName("updated_at");
        builder.Property(s => s.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
        builder.Property(s => s.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
        builder.Property(s => s.UpdatedBy).HasColumnName("updated_by").HasMaxLength(50);

        builder.HasOne(s => s.School).WithMany().HasForeignKey(s => s.SchoolId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(s => s.Teachers).WithMany(t => t.Subjects).UsingEntity(j => j.ToTable("subject_teachers"));
        builder.HasIndex(s => new { s.SchoolId, s.Code }).IsUnique().HasDatabaseName("ix_subjects_school_code");
        builder.Ignore(s => s.DomainEvents);
    }
}
