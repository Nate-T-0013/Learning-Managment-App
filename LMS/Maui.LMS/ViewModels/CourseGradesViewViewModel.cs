using CLI.LMS.Helpers.Services;
using Library.LMS.Models;
using Library.LMS.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maui.LMS.ViewModels
{
    internal class CourseGradesViewViewModel : INotifyPropertyChanged
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
                if(Currentcourse == null)
                {
                    return new Grade();
                }
                var studentId = StudentServiceProxy.Current.ActiveStudent;
                return Currentcourse.Grades.FirstOrDefault(g => studentId == g.StudentId) ?? new Grade();
            }
        }

        public ObservableCollection<AssignmentGroup> AssignmentGroups
        {
            get
            {
                if(Currentcourse == null)
                {
                    return new ObservableCollection<AssignmentGroup>();
                }
                return new ObservableCollection<AssignmentGroup>(Currentcourse.AssignmentGroups);
            }
        }

        public Assignment? SelectedItem { get; set; }
        public AssignmentGroup? SelectedGroup { get; set; }

        public void DeleteGroup()
        {
            if (SelectedGroup == null) return;
            CourseServiceProxy.Current.DeleteAssignmentGroup(SelectedGroup, Currentcourse.Id);
            Refresh();
        }

        public void DeleteGroupAssignment()
        {
            if(SelectedGroup == null || SelectedItem == null) return;

            CourseServiceProxy.Current.DeleteAssignment(SelectedItem, Currentcourse.Id, 0, SelectedGroup.Id);
            Refresh();
        }

        public void AddAssignment(int assignmentId)
        {
            var assignment = Currentcourse?.Assignments.FirstOrDefault(a => a.Id == assignmentId);
            if(assignment == null)
            {
                return;
            }

            Currentcourse?.AssignmentGroups.FirstOrDefault(g => g.Id == SelectedGroup?.Id)?.Assignments.Add(assignment);
            Refresh();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void Refresh()
        {
            int activeStudent = StudentServiceProxy.Current.ActiveStudent;
            int activeCourse = CourseServiceProxy.Current.ActiveCourse;
            int totalGrade = CourseServiceProxy.Current.GetWeightedGrade(activeCourse, activeStudent);
            var studentGrade = Currentcourse.Grades.FirstOrDefault(g => g.StudentId == activeStudent);
            if (studentGrade == null)
            {
                studentGrade = new Grade { StudentId = activeStudent };
                Currentcourse.Grades.Add(studentGrade);
            }
            studentGrade.TotalPercent = totalGrade;

            NotifyPropertyChanged("Grade");
            NotifyPropertyChanged("AssignmentGroups");
        }
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
