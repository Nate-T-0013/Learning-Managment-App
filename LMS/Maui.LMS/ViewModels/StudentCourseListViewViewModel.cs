using CLI.LMS.Helpers.Services;
using Library.LMS.Models;
using Library.LMS.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maui.LMS.ViewModels
{
    internal class StudentCourseListViewViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Course> Courses
        {
            get
            {
                var list = new ObservableCollection<Course>(CourseServiceProxy.Current.Courses);
                var studentList = new ObservableCollection<Course>();
                var student = StudentServiceProxy.Current.GetById(StudentServiceProxy.Current.ActiveStudent);
                if(student != null)
                {
                    foreach(var course in list)
                    {
                        if (course.Roster.Contains(student))
                        {
                            studentList.Add(course);
                        }
                    }
                }
                
                return studentList;
            }
        }

        public Course? SelectedItem { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Delete(int studentId)
        {
            var student = StudentServiceProxy.Current.GetById(studentId);
            if (SelectedItem == null) return;
            CourseServiceProxy.Current.DeleteStudent(student, SelectedItem.Id);
            Refresh();
        }

        public void Refresh()
        {
            NotifyPropertyChanged("Courses");
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
