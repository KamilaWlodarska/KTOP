namespace KTOP.Pages.Settings;

public partial class AccountPage : ContentPage
{
	public AccountPage()
	{
		InitializeComponent();
	}

    async void TapLogout_Tapped(object sender, TappedEventArgs e)
    {
        bool logout = await DisplayAlert("Wylogowanie", "Czy chcesz opuœciæ aplikacjê?", "Tak", "Nie");
        if(logout)
        {
            Preferences.Set("accesstoken", string.Empty);
            Application.Current.MainPage = new NavigationPage(new WelcomePage());
        }
    }

    void BackImgBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    async void TapEditAcc_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new EditUserPage());
    }

    async void TapChngPwd_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ChangeUserPwdPage());
    }

    async void TapDelAcc_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new DeleteUserAccount());
    }
}