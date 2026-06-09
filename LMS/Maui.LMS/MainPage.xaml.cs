namespace Maui.LMS
{
    public partial class MainPage : ContentPage     //partial keyword is important!
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void TeacherClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//TeacherMenu");
        }

        private void StudentClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//StudentMenu");
        }
    }
}
