using CLI.LMS.Helpers.Services;
using Library.LMS.Services;

namespace Maui.LMS.Views;

[QueryProperty(nameof(PrevPath), "prevPath")]

public partial class AssignmentMainView : ContentPage
{
    public string? PrevPath { get; set; }

    public AssignmentMainView()
	{
		InitializeComponent();
	}

    private void ViewSubmissionsClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"//SubmissionList?prevPath={PrevPath}");
    }

    private void NewSubmissionClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"//SubmissionDetail?prevPath={PrevPath}");
    }

    private void GoBackClicked(object sender, EventArgs e)
    {
        CourseServiceProxy.Current.ActiveAssignment = 0;
        if (PrevPath == null)
            Shell.Current.GoToAsync($"//CourseAssignments");
        else
            Shell.Current.GoToAsync($"//{PrevPath}");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        BindingContext = null;  //set to null to force a refresh when getting item values
        if (StudentServiceProxy.Current.ActiveStudent != 0)
        {
            NewSubmission.IsVisible = true;
            ViewSubmissions.IsVisible = false;
            StudentPoints.IsVisible = true;
            CommentLabel.IsVisible = true;
        }
        else
        {
            NewSubmission.IsVisible = false;
            ViewSubmissions.IsVisible = true;
            StudentPoints.IsVisible = false;
            CommentLabel.IsVisible = false;
        }
        var course = CourseServiceProxy.Current.GetById(CourseServiceProxy.Current.ActiveCourse);
        var assignmentId = CourseServiceProxy.Current.ActiveAssignment;
        int studentId = StudentServiceProxy.Current.ActiveStudent;
        var Submission = CourseServiceProxy.Current.GetByIdStudentSubmission(CourseServiceProxy.Current.ActiveCourse, studentId, assignmentId);

        if (Submission?.Points != -1)
        {
            StudentPoints.Text = $"Your Earned Points: {Submission?.Points}";
            CommentLabel.Text = $"Teacher Feedback: {Submission?.Comment}";
        }
        else
            StudentPoints.Text = $"Currently ungraded";


        BindingContext = course?.Assignments.FirstOrDefault(a => a.Id == assignmentId);
    }

    
}