using Library.LMS.Models;
using Library.LMS.Services;

namespace Maui.LMS.Views;

[QueryProperty(nameof(CourseId), "courseId")]

public partial class CourseDetailView : ContentPage
{
    public int CourseId { get; set; }

    public CourseDetailView()
	{
		InitializeComponent();
	}

    private void GoBackClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//TeacherMenu");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        var course = BindingContext as Course;
        if (course == null) return;
        course.SemesterSeason = char.ToUpper(course.SemesterSeason[0]) + course.SemesterSeason[1..].ToLower();

        if(string.IsNullOrWhiteSpace(course.SemesterSeason))
        {
            SemesterWarning.IsVisible = true;
            return;
        }
        else if(course.SemesterSeason != "Winter" && course.SemesterSeason != "Spring" && course.SemesterSeason != "Summer" && course.SemesterSeason != "Fall")
        {
            SemesterWarning.IsVisible = true;
            return;
        }
        if(course.SemesterYear <= 0)
        {
            SemesterWarning.IsVisible = true;
            return;
        }

        CourseServiceProxy.Current.AddOrEdit(BindingContext as Course);
        Shell.Current.GoToAsync("//TeacherMenu");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        SemesterWarning.IsVisible = false;

        if (CourseId == 0)
        {
            BindingContext = new Course();
        }
        else
        {
            BindingContext = CourseServiceProxy.Current.GetById(CourseId) ?? new Course();
        }
    }
}