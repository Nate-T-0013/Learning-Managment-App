using Library.LMS.Models;
using CLI.LMS.Helpers.Services;
using Library.LMS.Services;

namespace Maui.LMS.Views;

[QueryProperty(nameof(StudentId), "studentId")]
[QueryProperty(nameof(PrevPath), "prevPath")]
[QueryProperty(nameof(CourseId), "courseId")]

public partial class StudentDetailView : ContentPage
{
    public int StudentId { get; set; }
    public string? PrevPath { get; set; }
    public int CourseId { get; set; }

    public StudentDetailView()
	{
		InitializeComponent();
	}

    private void GoBackClicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync($"//StudentManagment?prevPath={PrevPath}&courseId={CourseId}");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        if (CourseId == 0)
            StudentServiceProxy.Current.AddOrEdit(BindingContext as Student);
        else
            StudentServiceProxy.Current.AddOrEdit(BindingContext as Student);
            CourseServiceProxy.Current.AddOrEditStudent((BindingContext as Student), CourseId);
        Shell.Current.GoToAsync($"//StudentManagment?prevPath={PrevPath}&courseId={CourseId}");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if(StudentId == 0)
        {
            BindingContext = new Student();
        }
        else
        {
            if (CourseId == 0)
                BindingContext = StudentServiceProxy.Current.GetById(StudentId) ?? new Student();
            else
                BindingContext = CourseServiceProxy.Current.GetByIdStudent(CourseId, StudentId);
        }
    }
}