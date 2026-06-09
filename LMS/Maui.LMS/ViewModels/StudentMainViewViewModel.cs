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
    internal class StudentMainViewViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Student> Students
        {
            get
            {
                return new ObservableCollection<Student>(StudentServiceProxy.Current.Students);
            }
        }

        public Student? SelectedItem { get; set; }

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
