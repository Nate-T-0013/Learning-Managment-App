using Library.LMS.Models;
using Library.LMS.Services;

namespace Maui.LMS.Views;

[QueryProperty(nameof(PrevPath), "prevPath")]
[QueryProperty(nameof(AssignmentId), "assignmentId")]

public partial class AssignmentDetailView : ContentPage
{
    public string? PrevPath { get; set; }
    public int AssignmentId { get; set; }

    public AssignmentDetailView()
	{
		InitializeComponent();
	}

    private void OkClicked(object sender, EventArgs e)
    {
        int points;
        if(!int.TryParse(PointsEntry.Text, out points))
        {
            PointsWarning.IsVisible = true;
            return;
        }
        (BindingContext as Assignment)?.AvailablePoints = points;

        DateTime date;
        if(!DateTime.TryParse($"{MonthEntry.Text}/{DayEntry.Text}/{YearEntry.Text} 11:59:00 PM", out date))
        {
            DateWarning.IsVisible = true;
            return;
        }
        (BindingContext as Assignment)?.DueDate = date;

        if (!int.TryParse(ModuleEntry.Text, out int moduleId))
        {
            WarningLabel.IsVisible = true;
            return;
        }

        if(moduleId == 0)
        {
            CourseServiceProxy.Current.AddOrEditAssignment((BindingContext as Assignment), CourseServiceProxy.Current.ActiveCourse, 0);
            if (PrevPath == null)
                Shell.Current.GoToAsync($"//CourseAssignments");
            else
                Shell.Current.GoToAsync($"//{PrevPath}");
        }
        else
        {
            foreach (var module in CourseServiceProxy.Current.GetById(CourseServiceProxy.Current.ActiveCourse)?.Modules ?? new List<Module>())
            {
                if(module.Id == moduleId)
                {
                    CourseServiceProxy.Current.AddOrEditAssignment((BindingContext as Assignment), CourseServiceProxy.Current.ActiveCourse, moduleId);
                }
            }
            if (PrevPath == null)
                Shell.Current.GoToAsync($"//CourseAssignments");
            else
                Shell.Current.GoToAsync($"//{PrevPath}");
        }
    }

    private void GoBackClicked(object sender, EventArgs e)
    {
        if (PrevPath == null)
            Shell.Current.GoToAsync($"//CourseAssignments");
        else
            Shell.Current.GoToAsync($"//{PrevPath}");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        PointsWarning.IsVisible = false;
        DateWarning.IsVisible = false;

        if(AssignmentId == 0)
        {
            BindingContext = new Assignment();
        }
        else
        {
            BindingContext = CourseServiceProxy.Current.GetById(CourseServiceProxy.Current.ActiveCourse)?.Assignments.FirstOrDefault(a => a.Id == AssignmentId);
            PointsEntry.Text = (BindingContext as Assignment)?.AvailablePoints.ToString();
            MonthEntry.Text = (BindingContext as Assignment)?.DueDate.Month.ToString();
            DayEntry.Text = (BindingContext as Assignment)?.DueDate.Day.ToString();
            YearEntry.Text = (BindingContext as Assignment)?.DueDate.Year.ToString();
        }
        ModuleEntry.Text = "0";
    }
}