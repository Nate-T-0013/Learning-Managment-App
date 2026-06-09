using CLI.LMS.Helpers.Services;
using Maui.LMS.ViewModels;

namespace Maui.LMS.Views;

public partial class StudentMainView : ContentPage
{
    public StudentMainView()
	{
		InitializeComponent();
        BindingContext = new StudentMainViewViewModel();

    }

    private void GoBackClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void SelectClicked(object sender, EventArgs e)
    {
        var studentId = (BindingContext as StudentMainViewViewModel)?.SelectedItem?.Id ?? 0;
        Shell.Current.GoToAsync($"//StudentCourseList?studentId={studentId}");
    }
}