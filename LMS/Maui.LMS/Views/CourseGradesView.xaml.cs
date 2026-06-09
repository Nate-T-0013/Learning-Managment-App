using CLI.LMS.Helpers.Services;
using Library.LMS.Services;
using Maui.LMS.ViewModels;

namespace Maui.LMS.Views;

public partial class CourseGradesView : ContentPage
{
    public CourseGradesView()
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

    private void AssignmentsClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//CourseAssignments");
    }

    private void StudentListClicked(object sender, EventArgs e)
    {
        string currentRoute = Shell.Current.CurrentState.Location.ToString();
        int courseId = CourseServiceProxy.Current.ActiveCourse;
        Shell.Current.GoToAsync($"//StudentManagment?prevPath={currentRoute}&courseId={courseId}");
    }

    private void AddGroupClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"//AssignmentGroupDetail?groupId=0");
    }

    private void EditGroupClicked(object sender, EventArgs e)
    {
        var bc = BindingContext as CourseGradesViewViewModel;
        var selectedGroup = bc?.SelectedGroup;
        if (selectedGroup == null) return;

        Shell.Current.GoToAsync($"//AssignmentGroupDetail?groupId={selectedGroup.Id}");
    }

    private void DeleteGroupClicked(object sender, EventArgs e)
    {
        var bc = BindingContext as CourseGradesViewViewModel;
        var selectedGroup = bc?.SelectedGroup;
        if (selectedGroup == null) return;
        bc?.DeleteGroup();
    }

    private void AssignmentClicked(object sender, EventArgs e)
    {
        var bc = BindingContext as CourseGradesViewViewModel;
        var selectedItem = bc?.SelectedItem;
        if (selectedItem == null) return;
        var assignmentId = selectedItem?.Id ?? 0;
        CourseServiceProxy.Current.ActiveAssignment = assignmentId;
        string currentRoute = Shell.Current.CurrentState.Location.ToString();
        Shell.Current.GoToAsync($"//AssignmentMain?prevPath={currentRoute}");
    }

    private void AddAssignmentClicked(object sender, EventArgs e)
    {
        var bc = BindingContext as CourseGradesViewViewModel;
        var selectedGroup = bc?.SelectedGroup;
        if (selectedGroup == null) return;
        if (!int.TryParse(AddAssignmentByIdEntry.Text, out int assignmentId)) {
            return;
        }
        else
        {
            if(assignmentId == 0)
            {
                return;
            }
            bc?.AddAssignment(assignmentId);
        }
    }

    private void DeleteAssignmentClicked(object sender, EventArgs e)
    {
        var bc = BindingContext as CourseGradesViewViewModel;
        var selectedGroup = bc?.SelectedGroup;
        var selectedItem = bc?.SelectedItem;
        if (selectedGroup == null || selectedItem == null) return;
        bc?.DeleteGroupAssignment();
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
            GradeLabel.IsVisible = true;
            GradeInfo.IsVisible = true;
            AddGoup.IsVisible = false;
            EditGoup.IsVisible = false;
            DeleteGoup.IsVisible = false;
            DeleteAssignment.IsVisible = false;
            AddAssignment.IsVisible = false;
        }
        else
        {
            StudentGrade.IsVisible = false;
            StudentListButton.IsVisible = true;
            GradeLabel.IsVisible = false;
            GradeInfo.IsVisible = false;
            AddGoup.IsVisible = true;
            EditGoup.IsVisible = true;
            DeleteGoup.IsVisible = true;
            DeleteAssignment.IsVisible = true;
            AddAssignment.IsVisible = true;
        }
        BindingContext = new CourseGradesViewViewModel();
        (BindingContext as CourseGradesViewViewModel)?.Refresh();
    }

    
}