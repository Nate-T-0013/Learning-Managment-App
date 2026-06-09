using Library.LMS.Models;
using Library.LMS.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maui.LMS.ViewModels
{
    public class TeacherMainViewViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Course> Courses
        {
            get
            {
                return new ObservableCollection<Course>(CourseServiceProxy.Current.Courses);
            }
        }

        public Course? SelectedItem { get; set; }

        public class CourseGroup : List<Course>
        {
            public string Heading { get; set; }
            public CourseGroup(string heading, List<Course> courses) : base(courses)
            {
                Heading = heading;
            }
        }

        public ObservableCollection<CourseGroup> GroupedCourses { get; set; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Delete()
        {
            CourseServiceProxy.Current.Delete(SelectedItem);
            Refresh();
        }

        public void Copy()
        {
            var newCourse = new Course();
            newCourse.Code = SelectedItem?.Code;
            newCourse.Name = SelectedItem?.Name;
            newCourse.Description = SelectedItem?.Description;
            newCourse.Modules = SelectedItem?.Modules ?? new List<Module>();
            newCourse.Assignments = SelectedItem?.Assignments ?? new List<Assignment>();
            newCourse.AssignmentGroups = SelectedItem?.AssignmentGroups ?? new List<AssignmentGroup>();
            newCourse.Grades = SelectedItem?.Grades ?? new List<Grade>();
            newCourse.Season = SelectedItem?.Season ?? new Season();
            newCourse.SemesterSeason = SelectedItem?.SemesterSeason ?? "";
            newCourse.SemesterYear = SelectedItem?.SemesterYear ?? 0;
            newCourse.Sections = SelectedItem?.Sections ?? new List<int>();
            newCourse.Announcements = SelectedItem?.Announcements ?? new List<string>();
            if (newCourse == null) return;
            newCourse.Id = 0;
            newCourse.Roster.Clear();
            foreach (var assignment in newCourse.Assignments)
            {
                assignment.Submissions.Clear();
            }

            CourseServiceProxy.Current.AddOrEdit(newCourse);
            Refresh();
        }

        public void Refresh(string? seasonFilter = null, string? yearFilter = null)
        {
            var allCourses = CourseServiceProxy.Current.Courses.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(seasonFilter))
            {
                allCourses = allCourses.Where(c =>
                    c.SemesterSeason.Contains(seasonFilter, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(yearFilter) && int.TryParse(yearFilter, out int year))
            {
                allCourses = allCourses.Where(c => c.SemesterYear == year);
            }

            var groups = allCourses
                .OrderByDescending(c => c.SemesterYear)
                .ThenBy(c => c.Season)
                .GroupBy(c => $"{c.SemesterSeason} {c.SemesterYear}")
                .Select(g => new CourseGroup(g.Key, g.ToList()))
                .ToList();

            GroupedCourses = new ObservableCollection<CourseGroup>(groups);

            NotifyPropertyChanged(nameof(Courses));
            NotifyPropertyChanged(nameof(GroupedCourses));
            //NotifyPropertyChanged("Courses");
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
