using System;
using System.Linq;

class Program
{
    static void Main()
    {
        using (var dbContext = new SchoolDbContext())
        {
            dbContext.Database.EnsureCreated();

            dbContext.Students.Add(new Student { Sname = "Alice", Major = "Computer Science", Standing = "Junior" });
            dbContext.Students.Add(new Student { Sname = "Bob", Major = "Mathematics", Standing = "Senior" });

            dbContext.Faculties.Add(new Faculty { FacultyName = "Dr. Smith", Department = "Computer Science" });
            dbContext.Faculties.Add(new Faculty { FacultyName = "Dr. Johnson", Department = "Mathematics" });

            dbContext.Classes.Add(new Class { ClassName = "CS101", RoomNumber = "101", Fid = 1 });
            dbContext.Classes.Add(new Class { ClassName = "Math101", RoomNumber = "102", Fid = 2 });

            dbContext.Enrollments.Add(new Enrolled { Cid = 1, Sid = 1 });
            dbContext.Enrollments.Add(new Enrolled { Cid = 2, Sid = 2 });

            dbContext.SaveChanges();

            var classesWithMoreThan100Students = dbContext.Classes
                .Where(c => dbContext.Enrollments.Count(e => e.Cid == c.Cid) > 0)
                .Select(c => new { ClassName = c.ClassName, RoomNumber = c.RoomNumber });

            Console.WriteLine("a. Names and room numbers for classes with more than 100 students:");
            foreach (var result in classesWithMoreThan100Students)
            {
                Console.WriteLine($"Class: {result.ClassName}, Room Number: {result.RoomNumber}");
            }

            var studentsWithNoClassesInDept22 = dbContext.Students
                .Where(s => !dbContext.Enrollments.Any(e => e.Sid == s.Sid &&
                                                             dbContext.Classes.Any(c => c.Fid == 22)))
                .Select(s => new { StudentID = s.Sid, Major = s.Major });

            Console.WriteLine("\nb. IDs and majors of students who take no classes with teachers in department 22:");
            foreach (var result in studentsWithNoClassesInDept22)
            {
                Console.WriteLine($"Student ID: {result.StudentID}, Major: {result.Major}");
            }
        }
    }
}
