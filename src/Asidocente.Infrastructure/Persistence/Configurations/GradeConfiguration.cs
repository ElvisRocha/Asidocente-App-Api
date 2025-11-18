using Asidocente.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asidocente.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Grade
/// </summary>
public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.ToTable("grades");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id).HasColumnName("id");
        builder.Property(g => g.Score).HasColumnName("score").HasColumnType("decimal(5,2)").IsRequired();
        builder.Property(g => g.MaxScore).HasColumnName("max_score").HasColumnType("decimal(5,2)").IsRequired();
        builder.Property(g => g.Percentage).HasColumnName("percentage").HasColumnType("decimal(5,2)").IsRequired();
        builder.Property(g => g.Comments).HasColumnName("comments").HasMaxLength(500);
        builder.Property(g => g.GradeDate).HasColumnName("grade_date").IsRequired();
        builder.Property(g => g.StudentId).HasColumnName("student_id").IsRequired();
        builder.Property(g => g.SubjectId).HasColumnName("subject_id").IsRequired();
        builder.Property(g => g.AcademicPeriodId).HasColumnName("academic_period_id").IsRequired();
        builder.Property(g => g.TeacherId).HasColumnName("teacher_id");
        builder.Property(g => g.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(g => g.UpdatedAt).HasColumnName("updated_at");
        builder.Property(g => g.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
        builder.Property(g => g.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
        builder.Property(g => g.UpdatedBy).HasColumnName("updated_by").HasMaxLength(50);

        // Relationships
        builder.HasOne(g => g.Student)
            .WithMany(s => s.Grades)
            .HasForeignKey(g => g.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(g => g.Subject)
            .WithMany(sub => sub.Grades)
            .HasForeignKey(g => g.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(g => g.AcademicPeriod)
            .WithMany(p => p.Grades)
            .HasForeignKey(g => g.AcademicPeriodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(g => g.Teacher)
            .WithMany()
            .HasForeignKey(g => g.TeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Ignore(g => g.DomainEvents);
    }
}
