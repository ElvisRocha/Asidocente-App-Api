using Asidocente.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asidocente.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Student
/// </summary>
public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("students");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id");

        builder.Property(s => s.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Identification)
            .HasColumnName("identification")
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(s => s.Identification)
            .IsUnique()
            .HasDatabaseName("ix_students_identification");

        builder.Property(s => s.GradeLevel)
            .HasColumnName("grade_level")
            .IsRequired();

        builder.Property(s => s.DateOfBirth)
            .HasColumnName("date_of_birth")
            .IsRequired();

        builder.Property(s => s.Email)
            .HasColumnName("email")
            .HasMaxLength(100);

        builder.Property(s => s.Phone)
            .HasColumnName("phone")
            .HasMaxLength(20);

        builder.Property(s => s.Province)
            .HasColumnName("province")
            .HasMaxLength(100);

        builder.Property(s => s.Canton)
            .HasColumnName("canton")
            .HasMaxLength(100);

        builder.Property(s => s.District)
            .HasColumnName("district")
            .HasMaxLength(100);

        builder.Property(s => s.DetailedAddress)
            .HasColumnName("detailed_address")
            .HasMaxLength(500);

        builder.Property(s => s.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(s => s.SchoolId)
            .HasColumnName("school_id")
            .IsRequired();

        builder.Property(s => s.SectionId)
            .HasColumnName("section_id");

        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(s => s.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);

        builder.Property(s => s.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(50);

        builder.Property(s => s.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(50);

        // Relationships
        builder.HasOne(s => s.School)
            .WithMany(sc => sc.Students)
            .HasForeignKey(s => s.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Section)
            .WithMany(sec => sec.Students)
            .HasForeignKey(s => s.SectionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(s => s.Parents)
            .WithMany(p => p.Students)
            .UsingEntity(j => j.ToTable("student_parents"));

        // Ignore domain events
        builder.Ignore(s => s.DomainEvents);
    }
}
