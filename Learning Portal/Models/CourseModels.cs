namespace LearningPortal.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Instructor { get; set; } = string.Empty;
    public string InstructorEmail { get; set; } = string.Empty;
    public int DurationHours { get; set; }
    public decimal Price { get; set; }
    public string Level { get; set; } = "Beginner";
    public DateTime StartDate { get; set; }
    public int EnrolledCount { get; set; }
    public int Capacity { get; set; }
    public double CompletionRate { get; set; }
    public string Status { get; set; } = "Open";
}

public class Enrollment
{
    public int Id { get; set; }
    public string LearnerName { get; set; } = string.Empty;
    public string LearnerEmail { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Instructor { get; set; } = string.Empty;
    public DateTime EnrolledDate { get; set; }
    public int ProgressPercent { get; set; }
    public bool Completed { get; set; }
    public string District { get; set; } = string.Empty;
    public string Role { get; set; } = "Teacher";
}

public class MonthlyEnrollmentPoint
{
    public string Month { get; set; } = string.Empty;
    public int Enrollments { get; set; }
    public int Completions { get; set; }
}

public class CourseSectionEvent
{
    public int Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsAllDay { get; set; }
    public string RecurrenceRule { get; set; } = string.Empty;
    public int? RecurrenceID { get; set; }
    public string Instructor { get; set; } = string.Empty;
    public string CourseCategory { get; set; } = string.Empty;
}

public class LearningContentItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
    public string Status { get; set; } = "Draft";
}

public class KpiStat
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Trend { get; set; } = string.Empty;
    public string TrendDirection { get; set; } = "up";
    public string Icon { get; set; } = string.Empty;
}
