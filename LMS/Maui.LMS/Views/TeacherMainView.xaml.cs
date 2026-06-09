using CLI.LMS.Helpers.Services;
using Library.LMS.Services;
using Maui.LMS.ViewModels;

namespace Maui.LMS.Views;

public partial class TeacherMainView : ContentPage
{
	public TeacherMainView()
	{
		InitializeComponent();
        BindingContext = new TeacherMainViewViewModel();
	}

    private void GoBackClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void FilterClicked(object sender, EventArgs e)
    {
        var viewModel = BindingContext as TeacherMainViewViewModel;
        if (viewModel != null)
        {
            if(!YearEntry.Text.IsWhiteSpace())
            {
                if (!int.TryParse(YearEntry.Text, out int year))
                    return;
                if (year <= 0)
                    return;
            }
            if(!SeasonEntry.Text.IsWhiteSpace())
            {
                if (SeasonEntry.Text.ToLower() != "winter" && SeasonEntry.Text.ToLower() != "spring" && SeasonEntry.Text.ToLower() != "summer" && SeasonEntry.Text.ToLower() != "fall")
                    return;
            }
            

            viewModel.Refresh(SeasonEntry.Text, YearEntry.Text);
        }
    }

    private void DeleteClicked(object sender, EventArgs e)
    {
        var context = (BindingContext as TeacherMainViewViewModel);
        if(context != null)
            context.Delete();
    }

    private void AddNewClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//CourseDetails?courseId=0");
    }

    private void EditClicked(object sender, EventArgs e)
    {
        var courseId = (BindingContext as TeacherMainViewViewModel)?.SelectedItem?.Id ?? 0;
        Shell.Current.GoToAsync($"//CourseDetails?courseId={courseId}");
    }

    private void CopyClicked(object sender, EventArgs e)
    {
        (BindingContext as TeacherMainViewViewModel)?.Copy();
    }

    private void SelectClicked(object sender, EventArgs e)
    {
        var courseId = (BindingContext as TeacherMainViewViewModel)?.SelectedItem?.Id ?? 0;
        CourseServiceProxy.Current.ActiveCourse = courseId;
        StudentServiceProxy.Current.ActiveStudent = 0;
        Shell.Current.GoToAsync($"//CourseMainMenu");
    }

    private void ManageStudentsClicked(object sender, EventArgs e)
    {
        string currentRoute = Shell.Current.CurrentState.Location.ToString();
        Shell.Current.GoToAsync($"//StudentManagment?prevPath={currentRoute}&courseId=0");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as TeacherMainViewViewModel)?.Refresh();
    }
}