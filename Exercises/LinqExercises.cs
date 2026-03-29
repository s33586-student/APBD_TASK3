using System.Security.Cryptography;
using APBD_TASK3.Data;

namespace APBD_TASK3.Exercises;

public sealed class LinqExercises
{
    /// <summary>
    /// Task:
    /// Find all students who live in Warsaw.
    /// Return the index number, full name, and city.
    ///
    /// SQL:
    /// SELECT IndexNumber, FirstName, LastName, City
    /// FROM Students
    /// WHERE City = 'Warsaw';
    /// </summary>
    public IEnumerable<string> Task01_StudentsFromWarsaw()
    {
        return UniversityData.Students.Select(s => $"{s.IndexNumber} {s.FirstName} {s.LastName} {s.City}").ToList();
    }

    /// <summary>
    /// Task:
    /// Build a list of all student email addresses.
    /// Use projection so that you do not return whole objects.
    ///
    /// SQL:
    /// SELECT Email
    /// FROM Students;
    /// </summary>
    public IEnumerable<string> Task02_StudentEmailAddresses()
    {
        return UniversityData.Students.Select(s => s.Email).ToList();
    }

    /// <summary>
    /// Task:
    /// Sort students alphabetically by last name and then by first name.
    /// Return the index number and full name.
    ///
    /// SQL:
    /// SELECT IndexNumber, FirstName, LastName
    /// FROM Students
    /// ORDER BY LastName, FirstName;
    /// </summary>
    public IEnumerable<string> Task03_StudentsSortedAlphabetically()
    {
        return UniversityData.Students.OrderBy(s => s.LastName).ThenBy(s => s.FirstName)
            .Select(s => $"{s.IndexNumber} {s.FirstName} {s.LastName}").ToList();
    }

    /// <summary>
    /// Task:
    /// Find the first course from the Analytics category.
    /// If such a course does not exist, return a text message.
    ///
    /// SQL:
    /// SELECT TOP 1 Title, StartDate
    /// FROM Courses
    /// WHERE Category = 'Analytics';
    /// </summary>
    public IEnumerable<string> Task04_FirstAnalyticsCourse()
    {
        return [UniversityData.Courses.Where(c => c.Category == "Analytics").Select(e => $"{e.Title}").FirstOrDefault() ?? "No Analytics course found"];
    }

    /// <summary>
    /// Task:
    /// Check whether there is at least one inactive enrollment in the data set.
    /// Return one line with a True/False or Yes/No answer.
    ///
    /// SQL:
    /// SELECT CASE WHEN EXISTS (
    ///     SELECT 1
    ///     FROM Enrollments
    ///     WHERE IsActive = 0
    /// ) THEN 1 ELSE 0 END;
    /// </summary>
    public IEnumerable<string> Task05_IsThereAnyInactiveEnrollment()
    {
        return [UniversityData.Enrollments.Exists(e => e.IsActive) ? "Yes" : "No"];
    }

    /// <summary>
    /// Task:
    /// Check whether every lecturer has a department assigned.
    /// Use a method that validates the condition for the whole collection.
    ///
    /// SQL:
    /// SELECT CASE WHEN COUNT(*) = COUNT(Department)
    /// THEN 1 ELSE 0 END
    /// FROM Lecturers;
    /// </summary>
    public IEnumerable<string> Task06_DoAllLecturersHaveDepartment()
    {
        return [UniversityData.Lecturers.Count(e => !string.IsNullOrEmpty(e.Department)) == UniversityData.Lecturers.Count ? "Yes" : "No"];
    }

    /// <summary>
    /// Task:
    /// Count how many active enrollments exist in the system.
    ///
    /// SQL:
    /// SELECT COUNT(*)
    /// FROM Enrollments
    /// WHERE IsActive = 1;
    /// </summary>
    public IEnumerable<string> Task07_CountActiveEnrollments()
    {
        return [UniversityData.Enrollments.Count(e => e.IsActive).ToString()];
    }

    /// <summary>
    /// Task:
    /// Return a sorted list of distinct student cities.
    ///
    /// SQL:
    /// SELECT DISTINCT City
    /// FROM Students
    /// ORDER BY City;
    /// </summary>
    public IEnumerable<string> Task08_DistinctStudentCities()
    {
        return UniversityData.Students.OrderBy(s => s.City).Select(s => $"{s.City}").Distinct();
    }

    /// <summary>
    /// Task:
    /// Return the three newest enrollments.
    /// Show the enrollment date, student identifier, and course identifier.
    ///
    /// SQL:
    /// SELECT TOP 3 EnrollmentDate, StudentId, CourseId
    /// FROM Enrollments
    /// ORDER BY EnrollmentDate DESC;
    /// </summary>
    public IEnumerable<string> Task09_ThreeNewestEnrollments()
    {
        return UniversityData.Enrollments.OrderByDescending(e => e.EnrollmentDate)
            .Select(e => $"{e.EnrollmentDate} {e.StudentId} {e.CourseId}").Take(3);
    }

    /// <summary>
    /// Task:
    /// Implement simple pagination for the course list.
    /// Assume a page size of 2 and return the second page of data.
    ///
    /// SQL:
    /// SELECT Title, Category
    /// FROM Courses
    /// ORDER BY Title
    /// OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Task10_SecondPageOfCourses()
    {
        return UniversityData.Courses.OrderBy(c => c.Title)
            .Select(c => $"{c.Title} {c.Category}").Skip(2).Take(2);
    }

    /// <summary>
    /// Task:
    /// Join students with enrollments by StudentId.
    /// Return the full student name and the enrollment date.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, e.EnrollmentDate
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId;
    /// </summary>
    public IEnumerable<string> Task11_JoinStudentsWithEnrollments()
    {
        return UniversityData.Students.Join(UniversityData.Enrollments, s => s.Id, e => e.StudentId, (s,e) => $"{s.FirstName} {s.LastName} {e.EnrollmentDate}");
    }

    /// <summary>
    /// Task:
    /// Prepare all student-course pairs based on enrollments.
    /// Use an approach that flattens the data into a single result sequence.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, c.Title
    /// FROM Enrollments e
    /// JOIN Students s ON s.Id = e.StudentId
    /// JOIN Courses c ON c.Id = e.CourseId;
    /// </summary>
    public IEnumerable<string> Task12_StudentCoursePairs()
    {
        return UniversityData.Enrollments.Join(UniversityData.Students, e => e.StudentId, s => s.Id, (e,s) => new {e, s}).Join(UniversityData.Courses, es => es.e.CourseId, c => c.Id, (es, c) => $"{es.s.FirstName} {es.s.LastName} {c.Title}");
    }

    /// <summary>
    /// Task:
    /// Group enrollments by course and return the course title together with the number of enrollments.
    ///
    /// SQL:
    /// SELECT c.Title, COUNT(*)
    /// FROM Enrollments e
    /// JOIN Courses c ON c.Id = e.CourseId
    /// GROUP BY c.Title;
    /// </summary>
    public IEnumerable<string> Task13_GroupEnrollmentsByCourse()
    {
        return UniversityData.Enrollments.Join(UniversityData.Courses, e => e.CourseId, c => c.Id, (e,c) => c.Title).GroupBy(title => title).Select(g => $"{g.Key} {g.Count()}");
    }

    /// <summary>
    /// Task:
    /// Calculate the average final grade for each course.
    /// Ignore records where the final grade is null.
    ///
    /// SQL:
    /// SELECT c.Title, AVG(e.FinalGrade)
    /// FROM Enrollments e
    /// JOIN Courses c ON c.Id = e.CourseId
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY c.Title;
    /// </summary>
    public IEnumerable<string> Task14_AverageGradePerCourse()
    {
        return UniversityData.Enrollments.Join(UniversityData.Courses, e => e.CourseId, c => c.Id, (e,c) => new {e, c}).Where(ec => ec.e.FinalGrade != null).GroupBy(ec => ec.c.Title).Select(g => $"{g.Key} {g.Average(ec => ec.e.FinalGrade)}");
    }

    /// <summary>
    /// Task:
    /// For each lecturer, count how many courses are assigned to that lecturer.
    /// Return the full lecturer name and the course count.
    ///
    /// SQL:
    /// SELECT l.FirstName, l.LastName, COUNT(c.Id)
    /// FROM Lecturers l
    /// LEFT JOIN Courses c ON c.LecturerId = l.Id
    /// GROUP BY l.FirstName, l.LastName;
    /// </summary>
    public IEnumerable<string> Task15_LecturersAndCourseCounts()
    {
        return UniversityData.Lecturers.GroupJoin(UniversityData.Courses, l => l.Id, c => c.LecturerId, (l, c) => new {Name = $"{l.FirstName} {l.LastName}", Count = c.Count()}).Select(x => $"{x.Name} {x.Count}");
    }

    /// <summary>
    /// Task:
    /// For each student, find the highest final grade.
    /// Skip students who do not have any graded enrollment yet.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, MAX(e.FinalGrade)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY s.FirstName, s.LastName;
    /// </summary>
    public IEnumerable<string> Task16_HighestGradePerStudent()
    {
        return UniversityData.Students.Join(UniversityData.Enrollments, s => s.Id, e => e.StudentId, (s, e) => new {Name = $"{s.FirstName} {s.LastName}", E = e}).Where(se => se.E.FinalGrade != null).GroupBy(se => se.Name).Select(g => $"{g.Key} {g.Max(se => se.E.FinalGrade)}");
    }

    /// <summary>
    /// Challenge:
    /// Find students who have more than one active enrollment.
    /// Return the full name and the number of active courses.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, COUNT(*)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.IsActive = 1
    /// GROUP BY s.FirstName, s.LastName
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Challenge01_StudentsWithMoreThanOneActiveCourse()
    {
        return UniversityData.Students.Join(UniversityData.Enrollments, s => s.Id, e => e.StudentId, (s, e) => new {s, e})
        .Where(se => se.e.IsActive).
        GroupBy(se => $"{se.s.FirstName} {se.s.LastName}")
        .Where(g => g.Count() > 1)
        .Select(g => $"{g.Key} {g.Count()}");
    }

    /// <summary>
    /// Challenge:
    /// List the courses that start in April 2026 and do not have any final grades assigned yet.
    ///
    /// SQL:
    /// SELECT c.Title
    /// FROM Courses c
    /// JOIN Enrollments e ON c.Id = e.CourseId
    /// WHERE MONTH(c.StartDate) = 4 AND YEAR(c.StartDate) = 2026
    /// GROUP BY c.Title
    /// HAVING SUM(CASE WHEN e.FinalGrade IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Challenge02_AprilCoursesWithoutFinalGrades()
    {
        return UniversityData.Courses.Join(UniversityData.Enrollments, c => c.Id, e => e.CourseId, (c, e) => new {c, e})
        .Where(ce => ce.c.StartDate.Month == 4 && ce.c.StartDate.Year == 2026)
        .GroupBy(ce => ce.c.Title)
        .Where(g => g.Sum(ce => (ce.e != null && ce.e.FinalGrade != null) ? 1 : 0) == 0)
        .Select(g => $"{g.Key}");
    }

    /// <summary>
    /// Challenge:
    /// Calculate the average final grade for every lecturer across all of their courses.
    /// Ignore missing grades but still keep the lecturers in mind as the reporting dimension.
    ///
    /// SQL:
    /// SELECT l.FirstName, l.LastName, AVG(e.FinalGrade)
    /// FROM Lecturers l
    /// LEFT JOIN Courses c ON c.LecturerId = l.Id
    /// LEFT JOIN Enrollments e ON e.CourseId = c.Id
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY l.FirstName, l.LastName;
    /// </summary>
    public IEnumerable<string> Challenge03_LecturersAndAverageGradeAcrossTheirCourses()
    {
        return UniversityData.Lecturers.GroupJoin(UniversityData.Courses, l => l.Id, c => c.LecturerId, (l, c) => new {l, c})
        .SelectMany(x => x.c.DefaultIfEmpty(), (x, c) => new {x.l, c})
        .GroupJoin(UniversityData.Enrollments, lc => (lc.c != null) ? lc.c.Id : -1, e => e.CourseId, (lc, e) => new {lc.l, e})
        .SelectMany(x => x.e.DefaultIfEmpty(), (x, e) => new {x.l, e}).Where(x => x.e != null && x.e.FinalGrade != null)
        .GroupBy(x => $"{x.l.FirstName} {x.l.LastName}")
        .Select(g => $"{g.Key} {g.Average(x => x.e.FinalGrade)}");
    }

    /// <summary>
    /// Challenge:
    /// Show student cities and the number of active enrollments created by students from each city.
    /// Sort the result by the active enrollment count in descending order.
    ///
    /// SQL:
    /// SELECT s.City, COUNT(*)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.IsActive = 1
    /// GROUP BY s.City
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Challenge04_CitiesAndActiveEnrollmentCounts()
    {
        return UniversityData.Students.Join(UniversityData.Enrollments, s => s.Id, e => e.StudentId, (s,e) => new {s, e})
        .Where(se => se.e.IsActive)
        .GroupBy(se => se.s.City)
        .OrderByDescending(g => g.Count())
        .Select(g => $"{g.Key} {g.Count()}");
    }

    private static NotImplementedException NotImplemented(string methodName)
    {
        return new NotImplementedException(
            $"Complete method {methodName} in Exercises/LinqExercises.cs and run the command again.");
    }
}
