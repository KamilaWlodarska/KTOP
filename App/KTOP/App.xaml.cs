using KTOP.Pages;

namespace KTOP
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var accessToken = Preferences.Get("accesstoken", string.Empty);
            if (string.IsNullOrEmpty(accessToken))
            {
                MainPage = new NavigationPage(new WelcomePage());
            }
            else
            {
                MainPage = new AppShell();
            }
        }
    }
}
