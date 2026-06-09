using Library.LMS.Models;
using Library.LMS.Services;
using Maui.LMS.ViewModels;
namespace Maui.LMS.Views;

[QueryProperty(nameof(PrevPath), "prevPath")]
[QueryProperty(nameof(CourseId), "courseId")]

public partial class StudentManagmentView : ContentPage
{
    public string? PrevPath { get; set; }
    public int CourseId { get; set; }

    public StudentManagmentView()
	{
		InitializeComponent();
    }

    private void GoBackClicked(object sender, EventArgs e)
    {
        if(PrevPath == null) 
            Shell.Current.GoToAsync($"//MainPage");
        else 
            Shell.Current.GoToAsync($"{PrevPath}");
    }

    private void InlineDeleteClicked(object sender, EventArgs e)
    {

    }

    private void DeleteClicked(object sender, EventArgs e)
    {
        var context = (BindingContext as StudentManagmentViewViewModel);
        if (context != null)
            context.Delete(CourseId);
    }

    private void AddNewClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"//StudentDetails?studentId=0&prevPath={PrevPath}&courseId={CourseId}");
    }

    private void EditClicked(object sender, EventArgs e)
    {
        var studentId = (BindingContext as StudentManagmentViewViewModel)?.SelectedItem?.Id ?? 0;
        Shell.Current.GoToAsync($"//StudentDetails?studentId={studentId}&prevPath={PrevPath}&courseId={CourseId}");
    }

    private void ExportClicked(object sender, EventArgs e)
    {
        var bc = BindingContext as StudentManagmentViewViewModel;
        if (CourseId == 0) return;
        var course = CourseServiceProxy.Current.GetById(CourseId);
        if(course == null) return;
        var roster = new List<Student>();
        foreach(var student in course.Roster)
        {
            roster.Add(student);
        }
        if(roster == null) return;
        CourseServiceProxy.Current.SavedRoster = roster;
        CopyLabel.IsVisible = true;
    }

    private void ImportClicked(object sender, EventArgs e)
    {
        if(CourseId == 0) return;
        var course = CourseServiceProxy.Current.GetById(CourseId);
        var roster = course?.Roster;
        if( roster == null) return;
        bool isPresent = false;
        foreach(var newstudent in CourseServiceProxy.Current.SavedRoster)
        {
            foreach(var student in roster)
            {
                if(newstudent ==  student)
                {
                    isPresent = true;
                }
            }
            if(!isPresent)
            {
                CourseServiceProxy.Current.AddOrEditStudent(newstudent, CourseId);
            }
            isPresent = false;
        }
        CourseServiceProxy.Current.SavedRoster.Clear();
        CopyLabel.IsVisible = false;
        (BindingContext as StudentManagmentViewViewModel)?.Refresh();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if(CourseId != 0)
            CopyLogic.IsVisible = true;
        else
            CopyLogic.IsVisible = false;
        if(CourseServiceProxy.Current.SavedRoster.Any())
            CopyLabel.IsVisible = true;
        else
            CopyLabel.IsVisible = false;


        BindingContext = new StudentManagmentViewViewModel(CourseId);
        (BindingContext as StudentManagmentViewViewModel)?.Refresh();
    }
}