namespace Asidocente.Shared.Constants;

/// <summary>
/// Application-wide constants
/// </summary>
public static class ApplicationConstants
{
    public const string ApplicationName = "Asidocente";
    public const string ApplicationVersion = "1.0.0";

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Teacher = "Teacher";
        public const string Parent = "Parent";
        public const string Student = "Student";
        public const string SchoolAdmin = "SchoolAdmin";
    }

    public static class GradingSystem
    {
        public const decimal PassingPercentage = 65.0m;
        public const decimal MaxPercentage = 100.0m;
    }

    public static class Pagination
    {
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 100;
    }
}
