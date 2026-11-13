using LearningPortal.Models;

namespace LearningPortal.Services;

public class SampleDataService
{
    private List<Course> _courses;
    private List<Enrollment> _enrollments;
    private List<LearningContentItem> _contents;
    private List<CourseSectionEvent> _events;
    private int _nextCourseId = 100;
    private int _nextEventId = 100;
    private int _nextContentId = 100;

    public SampleDataService()
    {
        _courses = SeedCourses();
        _enrollments = SeedEnrollments();
        _contents = SeedContents();
        _events = SeedEvents();
    }

    public Task<List<Course>> GetCoursesAsync() => Task.FromResult(_courses);

    public Task<List<Enrollment>> GetEnrollmentsAsync() => Task.FromResult(_enrollments);

    public Task<List<LearningContentItem>> GetContentsAsync() => Task.FromResult(_contents);

    public Task<List<CourseSectionEvent>> GetEventsAsync() => Task.FromResult(_events);

    public void UpsertCourse(Course course)
    {
        var existing = _courses.FirstOrDefault(c => c.Id == course.Id);
        if (existing is null)
        {
            course.Id = _nextCourseId++;
            _courses.Add(course);
        }
        else
        {
            _courses[_courses.IndexOf(existing)] = course;
        }
    }

    public void DeleteCourse(int id) => _courses.RemoveAll(c => c.Id == id);

    public void UpsertEvent(CourseSectionEvent ev)
    {
        var existing = _events.FirstOrDefault(e => e.Id == ev.Id);
        if (existing is null)
        {
            ev.Id = _nextEventId++;
            _events.Add(ev);
        }
        else
        {
            _events[_events.IndexOf(existing)] = ev;
        }
    }

    public void DeleteEvent(int id) => _events.RemoveAll(e => e.Id == id);

    public void UpsertContent(LearningContentItem item)
    {
        var existing = _contents.FirstOrDefault(c => c.Id == item.Id);
        if (existing is null)
        {
            item.Id = _nextContentId++;
            item.LastUpdated = DateTime.Now;
            _contents.Add(item);
        }
        else
        {
            item.LastUpdated = DateTime.Now;
            _contents[_contents.IndexOf(existing)] = item;
        }
    }

    public void DeleteContent(int id) => _contents.RemoveAll(c => c.Id == id);

    public Task<List<Enrollment>> GetRecentEnrollmentsAsync()
        => Task.FromResult(_enrollments.OrderByDescending(e => e.EnrolledDate).Take(8).ToList());

    public Task<List<MonthlyEnrollmentPoint>> GetMonthlyEnrollmentsAsync()
    {
        var rnd = new Random(7);
        var points = new List<MonthlyEnrollmentPoint>();
        string[] monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        for (int m = 0; m < 12; m++)
        {
            int enrollments = 320 + rnd.Next(0, 260) + (m >= 5 && m <= 7 ? 180 : 0);
            points.Add(new MonthlyEnrollmentPoint
            {
                Month = monthNames[m],
                Enrollments = enrollments,
                Completions = (int)(enrollments * (0.55 + rnd.NextDouble() * 0.25))
            });
        }
        return Task.FromResult(points);
    }

    private static List<Course> SeedCourses() =>
    [
        new Course { Id = 1, Title = "Differentiated Instruction Strategies", Category = "Pedagogy", Instructor = "Dr. Maria Santos", InstructorEmail = "m.santos@edulearn.org", DurationHours = 12, Price = 249m, Level = "Intermediate", StartDate = new DateTime(2026, 1, 12), EnrolledCount = 142, Capacity = 180, CompletionRate = 78.5, Status = "Open" },
        new Course { Id = 2, Title = "Classroom Behavior Management", Category = "Pedagogy", Instructor = "James Whitfield", InstructorEmail = "j.whitfield@edulearn.org", DurationHours = 8, Price = 179m, Level = "Beginner", StartDate = new DateTime(2026, 1, 26), EnrolledCount = 96, Capacity = 120, CompletionRate = 71.2, Status = "Open" },
        new Course { Id = 3, Title = "Introduction to Inclusive Education", Category = "Inclusion", Instructor = "Dr. Amina Yusuf", InstructorEmail = "a.yusuf@edulearn.org", DurationHours = 15, Price = 299m, Level = "Beginner", StartDate = new DateTime(2026, 2, 9), EnrolledCount = 203, Capacity = 220, CompletionRate = 83.6, Status = "Open" },
        new Course { Id = 4, Title = "Assessment Design for Deeper Learning", Category = "Assessment", Instructor = "Prof. Daniel Cho", InstructorEmail = "d.cho@edulearn.org", DurationHours = 10, Price = 219m, Level = "Advanced", StartDate = new DateTime(2026, 2, 23), EnrolledCount = 64, Capacity = 90, CompletionRate = 68.9, Status = "Open" },
        new Course { Id = 5, Title = "Digital Tools for Hybrid Classrooms", Category = "Technology", Instructor = "Sarah Kim", InstructorEmail = "s.kim@edulearn.org", DurationHours = 6, Price = 149m, Level = "Beginner", StartDate = new DateTime(2026, 3, 2), EnrolledCount = 178, Capacity = 200, CompletionRate = 74.1, Status = "Open" },
        new Course { Id = 6, Title = "Trauma-Informed Teaching Practices", Category = "Wellbeing", Instructor = "Dr. Elena Marsh", InstructorEmail = "e.marsh@edulearn.org", DurationHours = 9, Price = 199m, Level = "Intermediate", StartDate = new DateTime(2026, 3, 16), EnrolledCount = 121, Capacity = 150, CompletionRate = 80.3, Status = "Open" },
        new Course { Id = 7, Title = "Data Literacy for School Leaders", Category = "Leadership", Instructor = "Dr. Robert Njoku", InstructorEmail = "r.njoku@edulearn.org", DurationHours = 14, Price = 279m, Level = "Advanced", StartDate = new DateTime(2026, 4, 6), EnrolledCount = 52, Capacity = 80, CompletionRate = 62.7, Status = "Open" },
        new Course { Id = 8, Title = "Literacy Development in Early Grades", Category = "Pedagogy", Instructor = "Laura Bennett", InstructorEmail = "l.bennett@edulearn.org", DurationHours = 11, Price = 229m, Level = "Intermediate", StartDate = new DateTime(2026, 4, 20), EnrolledCount = 88, Capacity = 140, CompletionRate = 76.4, Status = "Open" },
        new Course { Id = 9, Title = "STEM Integration Across Curriculum", Category = "Technology", Instructor = "Dr. Priya Raman", InstructorEmail = "p.raman@edulearn.org", DurationHours = 13, Price = 259m, Level = "Intermediate", StartDate = new DateTime(2026, 5, 4), EnrolledCount = 73, Capacity = 110, CompletionRate = 69.8, Status = "Draft" },
        new Course { Id = 10, Title = "Culturally Responsive Teaching", Category = "Inclusion", Instructor = "Dr. Amina Yusuf", InstructorEmail = "a.yusuf@edulearn.org", DurationHours = 10, Price = 239m, Level = "Intermediate", StartDate = new DateTime(2026, 5, 18), EnrolledCount = 0, Capacity = 160, CompletionRate = 0, Status = "Planned" },
    ];

    private static List<Enrollment> SeedEnrollments()
    {
        var learners = new[]
        {
            ("Emily Carter", "Teacher"), ("Marcus Reed", "Teacher"), ("Sofia Alvarez", "Teacher"),
            ("Liam O'Connor", "Teacher"), ("Hannah Schmidt", "Principal"), ("Devon Blake", "Teacher"),
            ("Grace Nakamura", "Counselor"), ("Tomas Herrera", "Teacher"), ("Priya Patel", "Staff"),
            ("Owen Walsh", "Principal"), ("Fatima Hassan", "Teacher"), ("Chen Wei", "Staff"),
            ("Isabella Rossi", "Teacher"), ("Noah Kim", "Teacher"), ("Amara Okafor", "Counselor"),
            ("Lucas Meyer", "Teacher"), ("Zoe Anderson", "Staff"), ("Diego Fernandez", "Teacher"),
            ("Maya Thompson", "Teacher"), ("Erik Johansson", "Principal"), ("Nadia Karim", "Teacher"),
            ("Samuel Porter", "Teacher"), ("Ines Dubois", "Counselor"), ("Kofi Mensah", "Teacher"),
        };
        var courses = new[]
        {
            ("Differentiated Instruction Strategies", "Pedagogy", "Dr. Maria Santos"),
            ("Classroom Behavior Management", "Pedagogy", "James Whitfield"),
            ("Introduction to Inclusive Education", "Inclusion", "Dr. Amina Yusuf"),
            ("Assessment Design for Deeper Learning", "Assessment", "Prof. Daniel Cho"),
            ("Digital Tools for Hybrid Classrooms", "Technology", "Sarah Kim"),
            ("Trauma-Informed Teaching Practices", "Wellbeing", "Dr. Elena Marsh"),
            ("Data Literacy for School Leaders", "Leadership", "Dr. Robert Njoku"),
            ("Literacy Development in Early Grades", "Pedagogy", "Laura Bennett"),
        };
        var districts = new[] { "North Metro SD", "Riverside SD", "Lakeview SD", "Hillcrest SD", "Eastgate SD", "Summit SD" };
        var rnd = new Random(42);
        var enrollments = new List<Enrollment>();
        int id = 1;
        for (int i = 0; i < 48; i++)
        {
            var learner = learners[rnd.Next(learners.Length)];
            var course = courses[rnd.Next(courses.Length)];
            int progress = rnd.Next(5, 101);
            enrollments.Add(new Enrollment
            {
                Id = id++,
                LearnerName = learner.Item1,
                LearnerEmail = $"{learner.Item1.ToLower().Replace(' ', '.').Replace("'", "")}@school.org",
                CourseTitle = course.Item1,
                Category = course.Item2,
                Instructor = course.Item3,
                EnrolledDate = DateTime.Today.AddDays(-rnd.Next(1, 150)),
                ProgressPercent = progress,
                Completed = progress >= 100 || rnd.Next(100) < 20,
                District = districts[rnd.Next(districts.Length)],
                Role = learner.Item2
            });
        }
        return enrollments;
    }

    private static List<LearningContentItem> SeedContents()
    {
        var welcomeHtml = """
            <h2>Welcome to the Module</h2>
            <p>This module introduces <b>differentiated instruction</b> — tailoring teaching to individual learner readiness, interest, and learning profile.</p>
            <h3>Learning Objectives</h3>
            <ol>
                <li>Define differentiated instruction and its core principles.</li>
                <li>Identify student readiness levels using quick formative checks.</li>
                <li>Design tiered activities for a mixed-ability classroom.</li>
            </ol>
            <blockquote>Every student can learn — just not on the same day, or in the same way. — George Evans</blockquote>
            """;
        var reflectionHtml = """
            <h2>Reflection Prompt</h2>
            <p>After watching the video, describe a moment when you <i>adjusted your instruction mid-lesson</i>. What signals did you notice, and what did you change?</p>
            <p>Aim for <b>200–300 words</b>. Your response will be visible to your course facilitator.</p>
            """;
        return
        [
            new LearningContentItem
            {
                Id = 1,
                Title = "Understanding Differentiated Instruction",
                Author = "Dr. Maria Santos",
                Summary = "Foundational module introducing differentiated instruction principles.",
                HtmlContent = welcomeHtml,
                LastUpdated = DateTime.Now.AddDays(-3),
                Status = "Published"
            },
            new LearningContentItem
            {
                Id = 2,
                Title = "Reflection: Adjusting Instruction Mid-Lesson",
                Author = "James Whitfield",
                Summary = "Reflection prompt for learners following the observation activity.",
                HtmlContent = reflectionHtml,
                LastUpdated = DateTime.Now.AddDays(-1),
                Status = "Published"
            },
            new LearningContentItem
            {
                Id = 3,
                Title = "Inclusive Classroom Checklist (Draft)",
                Author = "Dr. Amina Yusuf",
                Summary = "Draft checklist supporting the inclusive education course.",
                HtmlContent = "<p>Checklist draft — to be expanded with images and tables.</p>",
                LastUpdated = DateTime.Now.AddHours(-6),
                Status = "Draft"
            },
        ];
    }

    private static List<CourseSectionEvent> SeedEvents()
    {
        // Anchor the schedule to the current week so the demo always shows data.
        var today = DateTime.Today;
        while (today.DayOfWeek != DayOfWeek.Monday) today = today.AddDays(-1);
        var monday = today;

        return
        [
            new CourseSectionEvent
            {
                Id = 1,
                Subject = "Differentiated Instruction — Section A",
                StartTime = monday.AddDays(0).AddHours(9),
                EndTime = monday.AddDays(0).AddHours(11),
                Location = "Virtual — Zoom Room 1",
                Description = "Live session: readiness assessment and tiered task design.",
                Instructor = "Dr. Maria Santos",
                CourseCategory = "Pedagogy",
                RecurrenceRule = string.Empty
            },
            new CourseSectionEvent
            {
                Id = 2,
                Subject = "Behavior Management Workshop",
                StartTime = monday.AddDays(1).AddHours(13),
                EndTime = monday.AddDays(1).AddHours(15),
                Location = "North Metro SD — Room 214",
                Description = "De-escalation strategies and positive behavior supports.",
                Instructor = "James Whitfield",
                CourseCategory = "Pedagogy",
                RecurrenceRule = "FREQ=WEEKLY;BYDAY=TU;INTERVAL=1;COUNT=8"
            },
            new CourseSectionEvent
            {
                Id = 3,
                Subject = "Inclusive Education — Section B",
                StartTime = monday.AddDays(2).AddHours(10),
                EndTime = monday.AddDays(2).AddHours(12),
                Location = "Virtual — Teams Meeting",
                Description = "UDL guidelines walkthrough with case studies.",
                Instructor = "Dr. Amina Yusuf",
                CourseCategory = "Inclusion",
                RecurrenceRule = "FREQ=WEEKLY;BYDAY=WE;INTERVAL=1;COUNT=6"
            },
            new CourseSectionEvent
            {
                Id = 4,
                Subject = "Assessment Design Lab",
                StartTime = monday.AddDays(3).AddHours(14),
                EndTime = monday.AddDays(3).AddHours(16),
                Location = "Riverside SD — Lab 3",
                Description = "Hands-on lab: building performance tasks and rubrics.",
                Instructor = "Prof. Daniel Cho",
                CourseCategory = "Assessment",
                RecurrenceRule = "FREQ=WEEKLY;BYDAY=TH;INTERVAL=2;COUNT=4"
            },
            new CourseSectionEvent
            {
                Id = 5,
                Subject = "Digital Tools Clinic",
                StartTime = monday.AddDays(4).AddHours(9),
                EndTime = monday.AddDays(4).AddHours(10),
                Location = "Virtual — Zoom Room 2",
                Description = "Open clinic for hybrid classroom technology questions.",
                Instructor = "Sarah Kim",
                CourseCategory = "Technology",
                RecurrenceRule = string.Empty
            },
            new CourseSectionEvent
            {
                Id = 6,
                Subject = "Trauma-Informed Practices Seminar",
                StartTime = monday.AddDays(7).AddHours(11),
                EndTime = monday.AddDays(7).AddHours(13),
                Location = "Lakeview SD — Auditorium",
                Description = "Understanding ACEs and building supportive classrooms.",
                Instructor = "Dr. Elena Marsh",
                CourseCategory = "Wellbeing",
                RecurrenceRule = "FREQ=MONTHLY;BYMONTHDAY=15;COUNT=4"
            },
        ];
    }
}
