using CLI.LMS.Helpers.Services;
using Library.LMS.Models;
using Library.LMS.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maui.LMS.ViewModels
{
    internal class StudentManagmentViewViewModel : INotifyPropertyChanged
    {
        private int _courseId;
        public ObservableCollection<Student> Students => _courseId != 0 ? new ObservableCollection<Student>(CourseServiceProxy.Current.Courses.FirstOrDefault(c => _courseId == c.Id)?.Roster ?? new List<Student>())
        : new ObservableCollection<Student>(StudentServiceProxy.Current.Students);

        public StudentManagmentViewViewModel() {
            SetBindingContext(0);
        }

        public StudentManagmentViewViewModel(int courseId)
        {
            SetBindingContext(courseId);
            _courseId = courseId;
        }

        public void SetBindingContext(int courseId)
        {
            _courseId = courseId;
        }

        public Student? SelectedItem { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void Delete(int courseId)
        {
            if (courseId == 0)
            {
                StudentServiceProxy.Current.Delete(SelectedItem);
                Refresh();
            }
            else
            {
                CourseServiceProxy.Current.DeleteStudent(SelectedItem, courseId);
                Refresh();
            }
        }

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
