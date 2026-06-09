using Library.LMS.Models;
using Library.LMS.Services;
using Microsoft.Maui.Graphics.Text;

namespace Maui.LMS.Views;

[QueryProperty(nameof(ModuleId), "moduleId")]
[QueryProperty(nameof(ItemId), "itemId")]
[QueryProperty(nameof(IsPage), "isPage")]

public partial class ItemDetailView : ContentPage
{
    public int ModuleId { get; set; }
    public int ItemId { get; set; }
    public bool IsPage {  get; set; }

	public ItemDetailView()
	{
		InitializeComponent();
	}

    private void OkClicked(object sender, EventArgs e)
    {
        if(ModuleEntry.IsVisible)
        {
            if (!int.TryParse(ModuleEntry.Text, out int moduleId))
            {
                WarningLabel.IsVisible = true;
                return;
            }
            else
            {
                if (moduleId == 0)
                {
                    WarningLabel.IsVisible = true;
                    return;
                }
                foreach (var module in CourseServiceProxy.Current.GetById(CourseServiceProxy.Current.ActiveCourse)?.Modules ?? new List<Module>())
                {
                    if (module.Id == moduleId)
                    {
                        if (IsPage)
                            CourseServiceProxy.Current.AddOrEditPage((BindingContext as Library.LMS.Models.Page), CourseServiceProxy.Current.ActiveCourse, moduleId);
                        else
                            CourseServiceProxy.Current.AddOrEditFile((BindingContext as Library.LMS.Models.File), CourseServiceProxy.Current.ActiveCourse, moduleId);
                        Shell.Current.GoToAsync("//CourseModules");
                    }
                }
                WarningLabel.IsVisible = true;
                return;
            }
        }
        else
        {
            if (IsPage)
                CourseServiceProxy.Current.AddOrEditPage((BindingContext as Library.LMS.Models.Page), CourseServiceProxy.Current.ActiveCourse, ModuleId);
            else
                CourseServiceProxy.Current.AddOrEditFile((BindingContext as Library.LMS.Models.File), CourseServiceProxy.Current.ActiveCourse, ModuleId);
            Shell.Current.GoToAsync("//CourseModules");
        }
    }

    private void GoBackClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//CourseModules");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        var module = new Module();
        if(ModuleId != 0)
        {
            module = CourseServiceProxy.Current.GetById(CourseServiceProxy.Current.ActiveCourse)?.Modules.FirstOrDefault(m => m.Id == ModuleId);
        }

        WarningLabel.IsVisible = false;
        if (ItemId == 0)
        {
            if (IsPage)
            {
                ContentLabel.IsVisible = true;
                ContentEntry.IsVisible = true;
                PathLabel.IsVisible = false;
                PathEntry.IsVisible = false;
                ModuleLabel.IsVisible = true;
                ModuleEntry.IsVisible = true;
                BindingContext = new Library.LMS.Models.Page();
            }
            else
            {
                ContentLabel.IsVisible = false;
                ContentEntry.IsVisible = false;
                PathLabel.IsVisible = true;
                PathEntry.IsVisible = true;
                ModuleLabel.IsVisible = true;
                ModuleEntry.IsVisible = true;
                BindingContext = new Library.LMS.Models.File();
            }
        }
        else
        {
            if (IsPage)
            {
                ContentLabel.IsVisible = true;
                ContentEntry.IsVisible = true;
                PathLabel.IsVisible = false;
                PathEntry.IsVisible = false;
                ModuleLabel.IsVisible = false;
                ModuleEntry.IsVisible = false;
                BindingContext = module?.Pages.FirstOrDefault(i => i.Id == ItemId);
            }
            else
            {
                ContentLabel.IsVisible = false;
                ContentEntry.IsVisible = false;
                PathLabel.IsVisible = true;
                PathEntry.IsVisible = true;
                ModuleLabel.IsVisible = false;
                ModuleEntry.IsVisible = false;
                BindingContext = module?.Files.FirstOrDefault(i => i.Id == ItemId);
            }
        }
    }
}