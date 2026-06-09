using CLI.LMS.Helpers.Services;
using Library.LMS.Models;
using Library.LMS.Services;

namespace Maui.LMS.Views;

[QueryProperty(nameof(PrevPath), "prevPath")]
[QueryProperty(nameof(StudentId), "studentId")]

public partial class SubmissionDetailView : ContentPage
{
    public string? PrevPath { get; set; }
    public int StudentId { get; set; }

    public SubmissionDetailView()
	{
		InitializeComponent();
	}

    public int getPercent(float percent)
    {
        float total = (CourseServiceProxy.Current.GetById(CourseServiceProxy.Current.ActiveCourse))?
            .Assignments.FirstOrDefault(a => CourseServiceProxy.Current.ActiveAssignment == a.Id)?.AvailablePoints ?? 0;
        percent = percent / 100;
        return (int)(percent * total);
    }

    private void OkClicked(object sender, EventArgs e)
    {
        
        int studentId = StudentServiceProxy.Current.ActiveStudent;
        int courseId = CourseServiceProxy.Current.ActiveCourse;
        int assignmentId = CourseServiceProxy.Current.ActiveAssignment;
        var course = CourseServiceProxy.Current.GetById(courseId);

        if(studentId == 0)
        {
            if (PercentBox.IsChecked)
            {
                (BindingContext as Submission)?.Points = getPercent((float)((BindingContext as Submission)?.Points ?? 0));
            }
            CourseServiceProxy.Current.AddOrEditSubmission((BindingContext as Submission), courseId, assignmentId);
            int grade = CourseServiceProxy.Current.GetWeightedGrade(courseId, StudentId);
            course?.Grades.FirstOrDefault(g => studentId == g.StudentId)?.TotalPercent = grade;
        }
        else
        {
            (BindingContext as Submission)?.StudentId = studentId;
            (BindingContext as Submission)?.AssignmentId = assignmentId;
            (BindingContext as Submission)?.SubmissionDate = DateTime.Now;
            CourseServiceProxy.Current.AddOrEditSubmission((BindingContext as Submission), courseId, assignmentId);
        }
        if(StudentServiceProxy.Current.ActiveStudent != 0)
            Shell.Current.GoToAsync($"//AssignmentMain?prevPath={PrevPath}");
        else
        {
            Shell.Current.GoToAsync($"//SubmissionList?prevPath={PrevPath}");
        }
    }

    private void GoBackClicked(object sender, EventArgs e)
    {
        if(StudentServiceProxy.Current.ActiveStudent != 0)
            Shell.Current.GoToAsync($"//AssignmentMain?prevPath={PrevPath}");
        else
        {
            Shell.Current.GoToAsync($"//SubmissionList?prevPath={PrevPath}");
        }
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        var course = CourseServiceProxy.Current.GetById(CourseServiceProxy.Current.ActiveCourse);
        int studentId = StudentServiceProxy.Current.ActiveStudent;
        var submission = new Submission();
        if (studentId == 0)
        {
            TeacherContent.IsVisible = true;
            TeacherPointsLabel.IsVisible = true;
            TeacherPointsEntry.IsVisible = true;
            TeacherAvailablePoints.IsVisible = true;
            PercentLabel.IsVisible = true;
            PercentBox.IsVisible = true;
            TeacherCommentLabel.IsVisible = true;
            TeacherCommentEntry.IsVisible = true;
            StudentContentLabel.IsVisible = false;
            StudentContentEntry.IsVisible = false;
            submission = course?.Assignments.FirstOrDefault(a => CourseServiceProxy.Current.ActiveAssignment == a.Id)?.Submissions.FirstOrDefault(s => StudentId == s.StudentId);
        }
        else
        {
            TeacherContent.IsVisible = false;
            TeacherPointsLabel.IsVisible = false;
            TeacherPointsEntry.IsVisible = false;
            TeacherAvailablePoints.IsVisible= false;
            PercentLabel.IsVisible = false;
            PercentBox.IsVisible = false;
            TeacherCommentLabel.IsVisible = false;
            TeacherCommentEntry.IsVisible = false;
            StudentContentLabel.IsVisible = true;
            StudentContentEntry.IsVisible = true;
            submission = course?.Assignments.FirstOrDefault(a => CourseServiceProxy.Current.ActiveAssignment == a.Id)?.Submissions.FirstOrDefault(s => studentId == s.StudentId);
        }

        var assignment = course?.Assignments.FirstOrDefault(a => CourseServiceProxy.Current.ActiveAssignment == a.Id);
        TeacherAvailablePoints.Text = $"Available Points: {assignment?.AvailablePoints}";

        if (submission == null)
        {
            BindingContext = new Submission();
        }
        else
        {
            BindingContext = submission;
        }
    }
}