using Library.LMS.Models;
using Library.LMS.Services;

namespace Maui.LMS.Views;

[QueryProperty(nameof(ItemId), "itemId")]
[QueryProperty(nameof(ModuleId), "moduleId")]
[QueryProperty(nameof(IsPage), "IsPage")]

public partial class ItemMainView : ContentPage
{
	public int ItemId { get; set; }
	public int ModuleId { get; set; }
	public bool IsPage { get; set; }

    public ItemMainView()
	{
		InitializeComponent();
	}

    private void GoBackClicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//CourseModules");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
	{
        var course = CourseServiceProxy.Current.GetById(CourseServiceProxy.Current.ActiveCourse);
        BindingContext = null;  //set to null to force a refresh when getting item values
        if (IsPage)
		{
			ContentLabel.IsVisible = true;
			PathLabel.IsVisible = false;
            BindingContext = course?.Modules.FirstOrDefault(m => ModuleId == m.Id)?.
				Pages.FirstOrDefault(p => ItemId == p.Id);
        }
		else
		{
            ContentLabel.IsVisible = false;
            PathLabel.IsVisible = true;
            BindingContext = course?.Modules.FirstOrDefault(m => ModuleId == m.Id)?.
                Files.FirstOrDefault(p => ItemId == p.Id);
        }
	}
}