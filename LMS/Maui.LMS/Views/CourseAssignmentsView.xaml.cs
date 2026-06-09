using CLI.LMS.Helpers.Services;
using Library.LMS.Services;
using Maui.LMS.ViewModels;

namespace Maui.LMS.Views;

public partial class CourseAssignmentsView : ContentPage
{
	public CourseAssignmentsView()
	{
		InitializeComponent();
	}

    private void HomePageclicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//CourseMainMenu");
    }

    private void ModulesClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//CourseModules");
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

    private void AssignmentClicked(object sender, EventArgs e)
    {
        var assignmentId = (BindingContext as CourseAssignmentsViewViewModel)?.SelectedItem?.Id ?? 0;
        CourseServiceProxy.Current.ActiveAssignment = assignmentId;
        string currentRoute = Shell.Current.CurrentState.Location.ToString();
        Shell.Current.GoToAsync($"//AssignmentMain?prevPath={currentRoute}");
    }

    private void AddAssignmentClicked(object sender, EventArgs e)
    {
        string currentRoute = Shell.Current.CurrentState.Location.ToString();
        Shell.Current.GoToAsync($"//AssignmentDetail?prevPath={currentRoute}&moduleId=0&assignmentId=0");
    }

    private void EditAssignmentClicked(object sender, EventArgs e)
    {
        var assignmentId = (BindingContext as CourseAssignmentsViewViewModel)?.SelectedItem?.Id ?? 0;
        CourseServiceProxy.Current.ActiveAssignment = assignmentId;
        string currentRoute = Shell.Current.CurrentState.Location.ToString();
        Shell.Current.GoToAsync($"//AssignmentDetail?prevPath={currentRoute}");
    }

    private void CopyAssignmentClicked(object sender, EventArgs e)
    {
        var bc = BindingContext as CourseAssignmentsViewViewModel;
        if(!int.TryParse(CopyAssignmentEntry.Text, out int courseId))
        {
            return;
        }
        if(courseId == 0) return;
        var course = CourseServiceProxy.Current.GetById(courseId);
        if(course != null)
        {
            var assignment = bc?.SelectedItem;
            assignment?.Id = 0;
            CourseServiceProxy.Current.AddOrEditAssignment(assignment, courseId, 0);
        }
        CopyAssignmentEntry.Text = "";
    }

    private void DeleteAssignmentClicked(object sender, EventArgs e)
    {
        var bc = BindingContext as CourseAssignmentsViewViewModel;
        bc?.DeleteItem(CourseServiceProxy.Current.ActiveCourse);
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
            AddAssignmentButton.IsVisible = false;
            EditAssignmentButton.IsVisible = false;
            DeleteAssignmentButton.IsVisible = false;
            CopyAssignment.IsVisible = false;
        }
        else
        {
            StudentGrade.IsVisible = false;
            StudentListButton.IsVisible = true;
            AddAssignmentButton.IsVisible = true;
            EditAssignmentButton.IsVisible = true;
            DeleteAssignmentButton.IsVisible = true;
            CopyAssignment.IsVisible = true;
        }

        int activeCourse = CourseServiceProxy.Current.ActiveCourse;
        BindingContext = new CourseAssignmentsViewViewModel();
        (BindingContext as CourseAssignmentsViewViewModel)?.Refresh();
    }

    
}