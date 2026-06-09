using Library.LMS.Models;
using Library.LMS.Services;
namespace Maui.LMS.Views;

[QueryProperty(nameof(GroupId), "groupId")]

public partial class AssignmentGroupDetailView : ContentPage
{
    public int GroupId { get; set; }

    public AssignmentGroupDetailView()
	{
		InitializeComponent();
	}

    private void OkClicked(object sender, EventArgs e)
    {
        int courseId = CourseServiceProxy.Current.ActiveCourse;
        CourseServiceProxy.Current.AddOrEditAssignmentGroup((BindingContext as AssignmentGroup), courseId);

        Shell.Current.GoToAsync("//CourseGrades");
    }

    private void GoBackClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//CourseGrades");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        var course = CourseServiceProxy.Current.GetById(CourseServiceProxy.Current.ActiveCourse);
        if(course ==  null)
        {
            course = new Course();
        }

        if(GroupId == 0)
        {
            BindingContext = new AssignmentGroup();
        }
        else
        {
            BindingContext = course?.AssignmentGroups.FirstOrDefault(a => a.Id == GroupId);
        }
    }
}