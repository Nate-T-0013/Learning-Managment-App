using CLI.LMS.Helpers.Services;
using Library.LMS.Models;
using Library.LMS.Services;
using Maui.LMS.ViewModels;

namespace Maui.LMS.Views;

public partial class CourseModulesView : ContentPage
{
	public CourseModulesView()
	{
		InitializeComponent();
	}

    private void HomePageclicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//CourseMainMenu");
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

    private void ItemClicked(object sender, EventArgs e)
    {
        var bc = BindingContext as CourseModulesViewViewModel;
        var selectedItem = bc?.SelectedItem;
        if (selectedItem == null) return;

        if (selectedItem is Library.LMS.Models.Page)
        {
            int pageId = (selectedItem as Library.LMS.Models.Page)?.Id ?? 0;
            int moduleId = bc?.Modules.FirstOrDefault(m => m.Items.Contains(selectedItem))?.Id ?? 0;
            Shell.Current.GoToAsync($"//ItemMain?itemId={pageId}&moduleId={moduleId}&IsPage={true}");
        }
        else if (selectedItem is Library.LMS.Models.File)
        {
            int fileId = (selectedItem as Library.LMS.Models.File)?.Id ?? 0;
            int moduleId = bc?.Modules.FirstOrDefault(m => m.Items.Contains(selectedItem))?.Id ?? 0;
            Shell.Current.GoToAsync($"//ItemMain?itemId={fileId}&moduleId={moduleId}&IsPage={false}");
        }
        else if (selectedItem is Assignment)
        {
            int assignmentId = (selectedItem as Assignment)?.Id ?? 0;
            CourseServiceProxy.Current.ActiveAssignment = assignmentId;
            string currentRoute = Shell.Current.CurrentState.Location.ToString();
            Shell.Current.GoToAsync($"//AssignmentMain?prevPath={currentRoute}");
        }
    }

    private void AddModuleClicked(object sender, EventArgs e)
    {
        CourseServiceProxy.Current.AddModule(CourseServiceProxy.Current.ActiveCourse);
        (BindingContext as CourseModulesViewViewModel)?.Refresh();
    }

    private void AddPageClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"//ItemDetail?moduleId=0&itemId=0&isPage={true}");
    }

    private void AddFileClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"//ItemDetail?moduleId=0&itemId=0&isPage={false}");
    }

    private void AddAssignmentClicked(object sender, EventArgs e)
    {
        string currentRoute = Shell.Current.CurrentState.Location.ToString();
        Shell.Current.GoToAsync($"//AssignmentDetail?prevPath={currentRoute}&moduleId=0&assignmentId=0");
    }

    private void EditItemClicked(object sender, EventArgs e)
    {
        var bc = BindingContext as CourseModulesViewViewModel;
        var selectedItem = bc?.SelectedItem;
        if (selectedItem == null) return;

        if (selectedItem is Library.LMS.Models.Page)
        {
            int pageId = (selectedItem as Library.LMS.Models.Page)?.Id ?? 0;
            int moduleId = bc?.Modules.FirstOrDefault(m => m.Items.Contains(selectedItem))?.Id ?? 0;
            Shell.Current.GoToAsync($"//ItemDetail?moduleId={moduleId}&itemId={pageId}&isPage={true}");
        }
        else if (selectedItem is Library.LMS.Models.File)
        {
            int fileId = (selectedItem as Library.LMS.Models.File)?.Id ?? 0;
            int moduleId = bc?.Modules.FirstOrDefault(m => m.Items.Contains(selectedItem))?.Id ?? 0;
            Shell.Current.GoToAsync($"//ItemDetail?moduleId={moduleId}&itemId={fileId}&isPage={false}");
        }
        else if (selectedItem is Assignment)
        {
            int assignmentId = (selectedItem as Assignment)?.Id ?? 0;
            string currentRoute = Shell.Current.CurrentState.Location.ToString();
            Shell.Current.GoToAsync($"//AssignmentDetail?prevPath={currentRoute}&assignmentId={assignmentId}");
        }
    }

    private void DeleteModuleClicked(object sender, EventArgs e)
    {
        if(!int.TryParse(DeleteModuleEntry.Text, out int moduleId))
        {
            return;
        }
        else
        {
            if(moduleId == 0)
            {
                return;
            }
            (BindingContext as CourseModulesViewViewModel)?.DeleteModule(moduleId);
        }
    }

    private void DeleteItemClicked(object sender, EventArgs e)
    {
        var bc = BindingContext as CourseModulesViewViewModel;
        var selectedItem = bc?.SelectedItem;
        if (selectedItem == null) return;

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
            AddPageButton.IsVisible = false;
            AddFileButton.IsVisible = false;
            AddAssignmentButton.IsVisible = false;
            EditItemButton.IsVisible = false;
            DeleteItemButton.IsVisible = false;
            AddModuleButton.IsVisible = false;
            DeleteModuleButton.IsVisible = false;
            DeleteModuleEntry.IsVisible = false;
        }
        else
        {
            StudentGrade.IsVisible = false;
            StudentListButton.IsVisible = true;
            AddPageButton.IsVisible = true;
            AddFileButton.IsVisible = true;
            AddAssignmentButton.IsVisible = true;
            EditItemButton.IsVisible = true;
            DeleteItemButton.IsVisible = true;
            AddModuleButton.IsVisible = true;
            DeleteModuleButton.IsVisible = true;
            DeleteModuleEntry.IsVisible = true;
        }

        BindingContext = new CourseModulesViewViewModel();
        (BindingContext as CourseModulesViewViewModel)?.Refresh();
    }

    
}