using CLI.LMS.Helpers.Services;
using Library.LMS.Models;
using System.Net.Http.Headers;

namespace Library.LMS.Services
{
    public class CourseServiceProxy
    {
        private static CourseServiceProxy? instance;
        private static object objectLock = new object();
        public int ActiveCourse {  get; set; }
        public int ActiveAssignment { get; set; }
        public List<Student> SavedRoster { get; set; } = new List<Student>();

        public Course? GetById(int id)
        {
            if (id == 0)
            {
                return null;
            }

            return Courses.FirstOrDefault(i => i.Id == id);
        }

        public Student? GetByIdStudent(int courseId, int studentId)
        {
            if (courseId == 0 || studentId == 0)
            {
                return null;
            }

            return Courses.FirstOrDefault(i => i.Id == courseId)?.Roster.FirstOrDefault(s => studentId == s.Id);
        }

        public Submission? GetByIdStudentSubmission(int courseId, int studentId, int assignmentId)
        {
            if (courseId == 0 || studentId == 0 || assignmentId == 0)
            {
                return null;
            }

            return Courses.FirstOrDefault(i => i.Id == courseId)?.Assignments.FirstOrDefault(a => assignmentId == a.Id)?.Submissions.FirstOrDefault(s => studentId == s.StudentId);
        }

        private List<Course> courses;
        public List<Course> Courses => courses;

        private CourseServiceProxy()
        {

            courses = new List<Course>()
            {
                new Course() { Id = 1, Code = "ABC123", Name = "test course", Description = "a course for testing", SemesterSeason = "Spring", SemesterYear = 2026,
                    Roster = {StudentServiceProxy.Current.GetById(1) ?? new Student(), StudentServiceProxy.Current.GetById(2) ?? new Student()},
                    Modules = { new Module(){ Id = 1 },
                        new Module(){ Id = 2, Assignments = new List<Assignment>(), 
                            Pages = new List<Page>{ new Page(){ Id = 1, Name = "summary", Content = "long winded description of summary"},
                                new Page(){ Id = 2, Name = "notes", Content = "long winded description of notes"} },
                            Files = new List<Models.File>{ new Models.File(){ Id = 1, Name = "Syllabus", Path = "/files/syllabus.pdf"} }
                        }
                    },
                    Assignments = { new Assignment(){ Id = 1, Name = "Attendance", Description = "Attendance for something", DueDate = new DateTime(2026, 2, 3), AvailablePoints = 50, 
                            Submissions = new List<Submission>{ new Submission(){ Id = 1, StudentId = 1, AssignmentId = 1, Content = "John was here.", SubmissionDate = DateTime.Now} }
                        },
                        new Assignment(){ Id = 2, Name = "Test", Description = "Test for something", DueDate = new DateTime(2026, 4, 5), AvailablePoints = 100}
                    },
                    AssignmentGroups = new List<AssignmentGroup>{ new AssignmentGroup{ Id = 1, Name = "Attendance", Assignments = new List<Assignment>(), Weight = 40},
                        new AssignmentGroup{ Id = 2, Name = "Tests", Assignments = new List<Assignment>(), Weight = 60} 
                    }
                }

                , new Course() { Id = 2, Code = "DEF456", Name = "test course 2", Description = "another course for testing", SemesterSeason = "Fall", SemesterYear = 2026,
                    Roster = {StudentServiceProxy.Current.GetById(4) ?? new Student(), StudentServiceProxy.Current.GetById(2) ?? new Student(), StudentServiceProxy.Current.GetById(3) ?? new Student() },
                    Modules = { new Module(){ Id = 1 },
                        new Module(){ Id = 2 } 
                    } 
                }
            };

            courses[0].Modules[1].Assignments.Add(courses[0].Assignments[0]);
            courses[0].AssignmentGroups[0].Assignments.Add(courses[0].Assignments[0]);
            courses[0].AssignmentGroups[1].Assignments.Add(courses[0].Assignments[1]);
        }

        public static CourseServiceProxy Current
        {
            get
            {
                lock (objectLock)
                {
                    if (instance == null)
                    {
                        instance = new CourseServiceProxy();
                    }
                    return instance;
                }
            }
        }

        private int LastKey => Courses.Any() ? Courses.Select(s => s.Id).Max() : 0;
        private int LastKeyModules => Courses.SelectMany(c => c.Modules).Any() ? Courses.SelectMany(c => c.Modules).Max(m => m.Id) : 0;
        private int LastKeyPages => Courses.SelectMany(c => c.Modules).SelectMany(m => m.Pages).Any() ? 
            Courses.SelectMany(c => c.Modules).SelectMany(m => m.Pages).Max(p => p.Id) : 0;
        private int LastKeyFiles => Courses.SelectMany(c => c.Modules).SelectMany(m => m.Files).Any() ? 
            Courses.SelectMany(c => c.Modules).SelectMany(m => m.Files).Max(f => f.Id) : 0;
        private int LastKeyAssignments => Courses.SelectMany(c => c.Assignments).Any() ? Courses.SelectMany(c => c.Assignments).Max(a => a.Id) : 0;
        private int LastKeySubmissions => Courses.SelectMany(c => c.Assignments).SelectMany(a => a.Submissions).Any() ? 
            Courses.SelectMany(c => c.Assignments).SelectMany(a => a.Submissions).Max(s => s.Id) : 0;
        private int LastKeyAssignmentGroups => Courses.SelectMany(c => c.AssignmentGroups).Any() ? Courses.SelectMany(c => c.AssignmentGroups).Max(a => a.Id) : 0;

        public Course? AddOrEdit(Course? course)
        {
            if(course == null)
            {
                return course;
            }

            if(course.Id == 0)
            {
                course.Id = LastKey + 1;
                courses.Add(course);
            }
            
            return course;
        }

        public Student? AddOrEditStudent(Student? student, int courseId)
        {
            if (student == null)
            {
                return student;
            }
            if(courseId == 0)
            {
                return student;
            }

            courses.FirstOrDefault(c => courseId == c.Id)?.Roster.Add(student);

            return student;
        }

        public Module? AddModule( int courseId)
        {
            Module module = new Module();

            if (module.Id == 0)
            {
                module.Id = LastKeyModules + 1;
                courses.FirstOrDefault(c => courseId == c.Id)?.Modules.Add(module);
            }

            return module;
        }

        public Page? AddOrEditPage(Page? page, int courseId, int moduleId)
        {
            if (page == null)
            {
                return page;
            }

            if (page.Id == 0)
            {
                page.Id = LastKeyPages + 1;
                courses.FirstOrDefault(c => courseId == c.Id)?.Modules.FirstOrDefault(m => m.Id == moduleId)?.Pages.Add(page);
            }

            return page;
        }

        public Models.File? AddOrEditFile(Models.File? file, int courseId, int moduleId)
        {
            if (file == null)
            {
                return file;
            }

            if (file.Id == 0)
            {
                file.Id = LastKeyFiles + 1;
                courses.FirstOrDefault(c => courseId == c.Id)?.Modules.FirstOrDefault(m => m.Id == moduleId)?.Files.Add(file);
            }

            return file;
        }

        public Assignment? AddOrEditAssignment(Assignment? assignment, int courseId, int moduleId)
        {
            if (assignment == null)
            {
                return assignment;
            }

            if(assignment.Id == 0)
            {
                assignment.Id = LastKeyAssignments + 1;
                courses.FirstOrDefault(c => courseId == c.Id)?.Assignments.Add(assignment);
            }
            if (moduleId != 0)
            {
                courses.FirstOrDefault(c => courseId == c.Id)?.Modules.FirstOrDefault(m => m.Id == moduleId)?.Assignments.Add(assignment);
            }

            return assignment;
        }

        public Submission? AddOrEditSubmission(Submission? submission, int courseId, int assignmentId)
        {
            if (submission == null)
            {
                return submission;
            }
            if(assignmentId == 0)
            {
                return submission;
            }

            if(submission.Id == 0)
            {
                submission.Id = LastKeySubmissions + 1;
                courses.FirstOrDefault(c => courseId == c.Id)?.Assignments.FirstOrDefault(a => assignmentId == a.Id)?.Submissions.Add(submission);
            }

            return submission;
        }

        public AssignmentGroup? AddOrEditAssignmentGroup(AssignmentGroup? group, int courseId)
        {
            if(group == null)
            {
                return group;
            }

            if (group.Id == 0)
            {
                group.Id = LastKeyAssignmentGroups + 1;
                courses.FirstOrDefault(c => courseId == c.Id)?.AssignmentGroups.Add(group);
            }

            return group;
        }

        public Course? Delete(Course? course)
        {
            if(course == null)
            {
                return null;
            }
            
            course.Roster.Clear();
            foreach(var module  in course.Modules)
            {
                module.Pages.Clear();
                module.Files.Clear();
                module.Assignments.Clear();
            }
            course.Modules.Clear();
            course.Assignments.Clear();

            courses.Remove(course);
            return course;
        }

        public Student? DeleteStudent(Student? student, int courseId)
        {
            if (student == null)
            {
                return null;
            }

            courses.FirstOrDefault(c => courseId == c.Id)?.Roster.Remove(student);
            return student;
        }

        public Module? DeleteModule(Module? module, int courseId)
        {
            if(module == null)
            {
                return null;
            }

            courses.FirstOrDefault(c => courseId == c.Id)?.Modules.Remove(module);
            return module;
        }

        public Page? DeletePage(Page? page, int courseId, int moduleId)
        {
            if (page == null) return null;

            courses.FirstOrDefault(c => courseId == c.Id)?.Modules.FirstOrDefault(f => f.Id == moduleId)?.Pages.Remove(page);
            return page;
        }
        public Models.File? DeleteFile(Models.File? file, int courseId, int moduleId)
        {
            if (file == null) return null;

            courses.FirstOrDefault(c => courseId == c.Id)?.Modules.FirstOrDefault(f => f.Id == moduleId)?.Files.Remove(file);
            return file;
        }
        public Assignment? DeleteAssignment(Assignment? assignment, int courseId, int moduleId, int groupId)
        {
            if (assignment == null) return null;

            if (moduleId != 0)
                courses.FirstOrDefault(c => courseId == c.Id)?.Modules.FirstOrDefault(f => f.Id == moduleId)?.Assignments.Remove(assignment);
            else if (groupId != 0) 
                courses.FirstOrDefault(c => courseId == c.Id)?.AssignmentGroups.FirstOrDefault(g => g.Id == groupId)?.Assignments.Remove(assignment);
            else
            {
                foreach (var module in Courses.FirstOrDefault(c => c.Id == ActiveCourse)?.Modules ?? new List<Module>())
                {
                    if (module == null) break;
                    if (module.Assignments.Contains(assignment))
                    {
                        module.Assignments.Remove(assignment);
                    }
                }
                foreach(var group in Courses.FirstOrDefault(c => c.Id == ActiveCourse)?.AssignmentGroups ?? new List<AssignmentGroup>())
                {
                    if (group == null) break;
                    if (group.Assignments.Contains(assignment))
                    {
                        group.Assignments.Remove(assignment);
                    }
                }

                courses.FirstOrDefault(c => courseId == c.Id)?.Assignments.Remove(assignment);
            }
            return assignment;
        }

        public AssignmentGroup? DeleteAssignmentGroup(AssignmentGroup? group, int courseId)
        {
            if (group == null)
            {
                return null;
            }

            courses.FirstOrDefault(c => courseId == c.Id)?.AssignmentGroups.Remove(group);
            return group;
        }

        public int GetWeightedGrade(int courseId, int studentId)
        {
            var course = GetById(courseId);
            if (course == null || !course.AssignmentGroups.Any() || studentId == 0)
            {
                return 0;
            }

            double totalWeightedScore = 0;
            double totalWeightConfigured = 0;

            foreach (var group in course.AssignmentGroups)
            {
                double groupPointsPossible = 0;
                double groupPointsEarned = 0;

                foreach (var assignment in group.Assignments)
                {
                    // Find student's submission for this assignment
                    var submission = assignment.Submissions
                        .FirstOrDefault(s => s.StudentId == studentId);

                    if (submission != null)
                    {
                        groupPointsEarned += submission.Points;
                        groupPointsPossible += assignment.AvailablePoints;
                    }
                }

                // If the group has assignments and submissions, calculate weighted contribution
                if (groupPointsPossible > 0)
                {
                    double groupPercentage = groupPointsEarned / groupPointsPossible;
                    totalWeightedScore += (groupPercentage * group.Weight);
                    totalWeightConfigured += group.Weight;
                }
            }

            // Normalizing the grade (in case weights don't add to 100% yet)
            if (totalWeightConfigured == 0) return 0;

            return (int)((totalWeightedScore / totalWeightConfigured) * 100);
        }
    }
}
