using Library.LMS.Models;
using Library.LMS.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maui.LMS.ViewModels
{
    internal class SubmissionListViewViewModel : INotifyPropertyChanged
    {
        public Course Currentcourse
        {
            get
            {
                int activeCourse = CourseServiceProxy.Current.ActiveCourse;
                return CourseServiceProxy.Current.GetById(activeCourse) ?? new Course();
            }
        }

        public ObservableCollection<Submission> Submissions
        {
            get
            {
                var assignmentId = CourseServiceProxy.Current.ActiveAssignment;
                return new ObservableCollection<Submission>(Currentcourse.Assignments.FirstOrDefault(a => assignmentId == a.Id)?.Submissions ?? new List<Submission>());
            }
        }

        public Submission? SelectedItem { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void Refresh()
        {
            NotifyPropertyChanged("Students");
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
