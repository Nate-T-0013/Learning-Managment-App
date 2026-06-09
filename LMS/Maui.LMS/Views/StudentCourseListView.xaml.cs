using CLI.LMS.Helpers.Services;
using Library.LMS.Services;
using Maui.LMS.ViewModels;

namespace Maui.LMS.Views;

[QueryProperty(nameof(StudentId), "studentId")]

public partial class StudentCourseListView : ContentPage
{
    public int StudentId { get; set; }

    public StudentCourseListView()
	{
		InitializeComponent();
        BindingContext = new StudentCourseListViewViewModel();
	}

    private void GoBackClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//StudentMenu");
    }

    private void SelectClicked(object sender, EventArgs e)
    {
        var courseId = (BindingContext as StudentCourseListViewViewModel)?.SelectedItem?.Id ?? 0;
        StudentServiceProxy.Current.ActiveStudent = StudentId;
        CourseServiceProxy.Current.ActiveCourse = courseId;
        Shell.Current.GoToAsync($"//CourseMainMenu");
    }

    private void DeleteClicked(object sender, EventArgs e)
    {
        var context = (BindingContext as StudentCourseListViewViewModel);
        if (context != null)
            context.Delete(StudentId);
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        StudentServiceProxy.Current.ActiveStudent = StudentId;
        (BindingContext as StudentCourseListViewViewModel)?.Refresh();
    }
}