using CLI.LMS.Helpers.Services;
using Library.LMS.Models;
using Library.LMS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Maui.LMS.ViewModels
{
    internal class CourseMainMenuViewViewModel : INotifyPropertyChanged
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
        
        public ObservableCollection<String> Announcements
        {
            get
            {
                if(Currentcourse == null)
                {
                    return new ObservableCollection<String>();
                }
                return new ObservableCollection<String>(Currentcourse.Announcements);
            }
        }

        public void AddAnnouncment(string announcment)
        {
            if(announcment != null)
            {
                Currentcourse.Announcements.Add(announcment);
            }

            Refresh();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void Refresh()
        {

            NotifyPropertyChanged("Announcements");
        }
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
