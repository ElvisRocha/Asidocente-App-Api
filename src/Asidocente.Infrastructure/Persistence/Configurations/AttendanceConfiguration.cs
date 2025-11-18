using Asidocente.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asidocente.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Attendance
/// </summary>
public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.ToTable("attendances");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.AttendanceDate).HasColumnName("attendance_date").IsRequired();
        builder.Property(a => a.Status).HasColumnName("status").IsRequired();
        builder.Property(a => a.Notes).HasColumnName("notes").HasMaxLength(500);
        builder.Property(a => a.ArrivalTime).HasColumnName("arrival_time");
        builder.Property(a => a.StudentId).HasColumnName("student_id").IsRequired();
        builder.Property(a => a.TeacherId).HasColumnName("teacher_id");
        builder.Property(a => a.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(a => a.UpdatedAt).HasColumnName("updated_at");
        builder.Property(a => a.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
        builder.Property(a => a.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
        builder.Property(a => a.UpdatedBy).HasColumnName("updated_by").HasMaxLength(50);

        // Relationships
        builder.HasOne(a => a.Student)
            .WithMany(s => s.Attendances)
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Teacher)
            .WithMany()
            .HasForeignKey(a => a.TeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(a => new { a.StudentId, a.AttendanceDate })
            .HasDatabaseName("ix_attendances_student_date");

        builder.Ignore(a => a.DomainEvents);
    }
}
