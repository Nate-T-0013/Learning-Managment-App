using CLI.LMS.Helpers.Services;
using Library.LMS.Models;
using Library.LMS.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maui.LMS.ViewModels
{
    internal class CourseAssignmentsViewViewModel : INotifyPropertyChanged
    {
        public Course Currentcourse
        {
            get
            {
                int activeCourse = CourseServiceProxy.Current.ActiveCourse;
                return CourseServiceProxy.Current.GetById(activeCourse) ?? new Course();
            }
        }

        public Grade Grade
        {
            get
            {
                if (Currentcourse == null)
                {
                    return new Grade();
                }
                var studentId = StudentServiceProxy.Current.ActiveStudent;
                return Currentcourse.Grades.FirstOrDefault(g => studentId == g.StudentId) ?? new Grade();
            }
        }

        public ObservableCollection<Assignment> Assignments
        {
            get
            {
                var currentCourse = CourseServiceProxy.Current.GetById(CourseServiceProxy.Current.ActiveCourse);
                if(currentCourse == null)
                {
                    return new ObservableCollection<Assignment>();
                }
                return new ObservableCollection<Assignment>(currentCourse.Assignments);
            }
        }

        public Assignment? SelectedItem { get; set; }

        public void DeleteItem(int activeCourse)
        {
            CourseServiceProxy.Current.DeleteAssignment(SelectedItem as Assignment, activeCourse, 0, 0);
            Refresh();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void Refresh()
        {

            NotifyPropertyChanged("Assignments");
        }
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
