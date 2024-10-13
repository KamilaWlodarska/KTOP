using KTOP.Services;

namespace KTOP.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
    }

    async void TapRegister_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }

    async void BtnLogin_Clicked(object sender, EventArgs e)
    {
        if (EntEmail.Text == null || EntEmail.Text.Length.Equals(0) || EntPwd.Text == null || EntPwd.Text.Length.Equals(0))
        {
            await DisplayAlert("", "Uzupe³nij wszystkie dane", "Ok");
        }
        else
        {
            var response = await UserService.Login(EntEmail.Text, EntPwd.Text);
            if (response) Application.Current.MainPage = new AppShell();
            else await DisplayAlert("", "Logowanie nie powiod³o siê", "Spróbuj ponownie");
        }
    }
}