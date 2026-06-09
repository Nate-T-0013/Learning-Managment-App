using Maui.LMS.ViewModels;

namespace Maui.LMS.Views;

[QueryProperty(nameof(PrevPath), "prevPath")]

public partial class SubmissionListView : ContentPage
{
    public string? PrevPath { get; set; }

    public SubmissionListView()
	{
		InitializeComponent();
	}

    private void SelectClicked(object sender, EventArgs e)
    {
        int studentId = (BindingContext as SubmissionListViewViewModel)?.SelectedItem?.StudentId ?? 0;
        if(studentId == 0) { return; }
        Shell.Current.GoToAsync($"//SubmissionDetail?prevPath={PrevPath}&studentId={studentId}");
    }

    private void GoBackClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"//AssignmentMain?prevPath={PrevPath}");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        BindingContext = new SubmissionListViewViewModel();
        (BindingContext as SubmissionListViewViewModel)?.Refresh();
    }
}