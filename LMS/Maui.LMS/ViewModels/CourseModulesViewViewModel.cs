using CLI.LMS.Helpers.Services;
using Library.LMS.Models;
using Library.LMS.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using File = Library.LMS.Models.File;
using Page = Library.LMS.Models.Page;

namespace Maui.LMS.ViewModels
{
    internal class CourseModulesViewViewModel : INotifyPropertyChanged
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

        private ObservableCollection<Module>? _modules;
        public ObservableCollection<Module> Modules
        {
            get
            {
                if (_modules == null)
                {
                    var course = CourseServiceProxy.Current.GetById(CourseServiceProxy.Current.ActiveCourse);
                    if(course == null)
                    {
                        course = new Course();
                    }

                    _modules = new ObservableCollection<Module>(course.Modules ?? new List<Module>());
                }
                return _modules;
            }
        }

        private ObservableCollection<object>? _items;
        public ObservableCollection<object> Items
        {
            get
            {
                if(_items == null)
                {
                    return new ObservableCollection<object>();
                }
                return _items;
            }
        }

        private object? _selectedItem;
        public object? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }

        public void DeleteItem(int activeCourse)
        {
            int moduleId = Modules.FirstOrDefault(m => m.Items.Contains(_selectedItem))?.Id ?? 0;
            if (SelectedItem is Page page)
            {
                CourseServiceProxy.Current.DeletePage(SelectedItem as Page, activeCourse, moduleId);
            }
            else if (SelectedItem is File file)
            {
                CourseServiceProxy.Current.DeleteFile(SelectedItem as File, activeCourse, moduleId);
            }
            else if (SelectedItem is Assignment assignment)
            {
                CourseServiceProxy.Current.DeleteAssignment(SelectedItem as Assignment, activeCourse, moduleId, 0);
            }
            Refresh();
        }

        public void DeleteModule(int moduleId)
        {
            if (moduleId == 0)
                return;
            var module = Currentcourse.Modules.FirstOrDefault(m => moduleId == m.Id);
            CourseServiceProxy.Current.DeleteModule(module, Currentcourse.Id);
            Refresh();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void Refresh()
        {
            _modules = null;
            var allItems = new List<object>();

            foreach(var module in Modules)
            {
                if(module.Pages != null) allItems.AddRange(module.Pages);
                if (module.Files != null) allItems.AddRange(module.Files);
                if (module.Assignments != null) allItems.AddRange(module.Assignments);
            }
            _items = new ObservableCollection<object>(allItems);

            NotifyPropertyChanged("Modules");
            NotifyPropertyChanged("Items");
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
