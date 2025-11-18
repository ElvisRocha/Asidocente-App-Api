namespace Asidocente.Domain.Enums;

/// <summary>
/// Status of student attendance
/// </summary>
public enum AttendanceStatus
{
    Present = 0,
    Absent = 1,
    Late = 2,
    Excused = 3,
    Medical = 4
}
