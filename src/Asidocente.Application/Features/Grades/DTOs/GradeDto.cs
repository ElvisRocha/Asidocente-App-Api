namespace Asidocente.Application.Features.Grades.DTOs;

public record GradeDto
{
    public int Id { get; init; }
    public decimal Score { get; init; }
    public decimal MaxScore { get; init; }
    public decimal Percentage { get; init; }
    public string LetterGrade { get; init; } = string.Empty;
    public bool IsPassing { get; init; }
    public string? Comments { get; init; }
    public DateTime GradeDate { get; init; }
    public int StudentId { get; init; }
    public string StudentName { get; init; } = string.Empty;
    public int SubjectId { get; init; }
    public string SubjectName { get; init; } = string.Empty;
    public int AcademicPeriodId { get; init; }
    public string PeriodName { get; init; } = string.Empty;
}

public record GradeReportDto
{
    public int SubjectId { get; init; }
    public string SubjectName { get; init; } = string.Empty;
    public decimal Score { get; init; }
    public decimal MaxScore { get; init; }
    public decimal Percentage { get; init; }
    public string LetterGrade { get; init; } = string.Empty;
    public bool IsPassing { get; init; }
    public string? Comments { get; init; }
}
