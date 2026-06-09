using CLI.LMS.Helpers.Services;
using Library.LMS.Models;
using Library.LMS.Services;
using Maui.LMS.ViewModels;

namespace Maui.LMS.Views;

[QueryProperty(nameof(StudentId), "studentId")]

public partial class CourseMainMenuView : ContentPage
{
    public int StudentId { get; set; }

    public CourseMainMenuView()
	{
		InitializeComponent();
	}

    private void ModulesClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//CourseModules");
    }

    private void AssignmentsClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//CourseAssignments");
    }

    private void GradesClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//CourseGrades");
    }

    private void StudentListClicked(object sender, EventArgs e)
    {
        string currentRoute = Shell.Current.CurrentState.Location.ToString();
        int courseId = CourseServiceProxy.Current.ActiveCourse;
        Shell.Current.GoToAsync($"//StudentManagment?prevPath={currentRoute}&courseId={courseId}");
    }

    private void MakeAnnouncementClicked(object sender, EventArgs e)
    {
        if(AnnouncementEntry.Text.Trim() == "")
        {
            AnnouncementWarning.IsVisible = true;
            return;
        }
        (BindingContext as CourseMainMenuViewViewModel)?.AddAnnouncment(AnnouncementEntry.Text);
        AnnouncementEntry.Text = "";
    }

    private void GoBackClicked(object sender, EventArgs e)
    {
        if (StudentServiceProxy.Current.ActiveStudent == 0)
        {
            StudentServiceProxy.Current.ActiveStudent = 0;
            CourseServiceProxy.Current.ActiveCourse = 0;
            Shell.Current.GoToAsync("//TeacherMenu");
        }
        else
        {
            int studentId = StudentServiceProxy.Current.ActiveStudent;
            StudentServiceProxy.Current.ActiveStudent = 0;
            CourseServiceProxy.Current.ActiveCourse = 0;
            Shell.Current.GoToAsync($"//StudentCourseList?studentId={studentId}");
        }
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (StudentServiceProxy.Current.ActiveStudent != 0)
        {
            StudentGrade.IsVisible = true;
            StudentListButton.IsVisible = false;
            AnnouncementEntry.IsVisible = false;
            MakeAnnouncement.IsVisible = false;
            AnnouncementWarning.IsVisible = false;
        }
        else
        {
            StudentGrade.IsVisible = false;
            StudentListButton.IsVisible = true;
            AnnouncementEntry.IsVisible = true;
            MakeAnnouncement.IsVisible = true;
            AnnouncementWarning.IsVisible = false;
        }

        //int activeCourse = CourseServiceProxy.Current.ActiveCourse;
        //BindingContext = CourseServiceProxy.Current.GetById(activeCourse);
        BindingContext = new CourseMainMenuViewViewModel();
    }
}