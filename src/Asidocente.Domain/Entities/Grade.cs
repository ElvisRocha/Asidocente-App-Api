using Asidocente.Domain.Common;
using Asidocente.Domain.Events;

namespace Asidocente.Domain.Entities;

/// <summary>
/// Represents an academic grade/score
/// </summary>
public class Grade : BaseEntity, IAuditableEntity
{
    public decimal Score { get; private set; }
    public decimal MaxScore { get; private set; }
    public decimal Percentage { get; private set; }
    public string? Comments { get; private set; }
    public DateTime GradeDate { get; private set; }
    public int StudentId { get; private set; }
    public int SubjectId { get; private set; }
    public int AcademicPeriodId { get; private set; }
    public int? TeacherId { get; private set; }

    // Audit properties
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public Student Student { get; private set; } = null!;
    public Subject Subject { get; private set; } = null!;
    public AcademicPeriod AcademicPeriod { get; private set; } = null!;
    public Teacher? Teacher { get; private set; }

    private Grade()
    {
    }

    /// <summary>
    /// Create a new Grade
    /// </summary>
    public static Grade Create(
        decimal score,
        decimal maxScore,
        int studentId,
        int subjectId,
        int academicPeriodId,
        int? teacherId = null,
        string? comments = null)
    {
        if (score < 0)
            throw new DomainException("Score cannot be negative");

        if (maxScore <= 0)
            throw new DomainException("Max score must be greater than zero");

        if (score > maxScore)
            throw new DomainException("Score cannot exceed max score");

        var percentage = (score / maxScore) * 100;

        var grade = new Grade
        {
            Score = score,
            MaxScore = maxScore,
            Percentage = Math.Round(percentage, 2),
            StudentId = studentId,
            SubjectId = subjectId,
            AcademicPeriodId = academicPeriodId,
            TeacherId = teacherId,
            Comments = comments,
            GradeDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        grade.AddDomainEvent(new GradeRegisteredEvent(grade));

        return grade;
    }

    /// <summary>
    /// Update grade score
    /// </summary>
    public void UpdateScore(decimal score, string? comments = null)
    {
        if (score < 0)
            throw new DomainException("Score cannot be negative");

        if (score > MaxScore)
            throw new DomainException("Score cannot exceed max score");

        Score = score;
        Percentage = Math.Round((score / MaxScore) * 100, 2);
        Comments = comments;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Check if grade is passing (>= 65% in Costa Rica)
    /// </summary>
    public bool IsPassing()
    {
        return Percentage >= 65;
    }

    /// <summary>
    /// Get letter grade based on Costa Rican grading scale
    /// </summary>
    public string GetLetterGrade()
    {
        return Percentage switch
        {
            >= 90 => "A",
            >= 80 => "B",
            >= 70 => "C",
            >= 65 => "D",
            _ => "F"
        };
    }
}
