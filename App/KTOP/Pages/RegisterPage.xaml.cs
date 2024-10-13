using KTOP.Services;

namespace KTOP.Pages;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
    }

    async void BtnRegister_Clicked(object sender, EventArgs e)
    {
        if (EntUserName.Text == null || EntUserName.Text.Length.Equals(0) || EntEmail.Text == null || EntEmail.Text.Length.Equals(0) || EntPwd.Text == null || EntPwd.Text.Length.Equals(0))
        {
            await DisplayAlert("", "Uzupe�nij wszystkie dane", "Ok");
        }
        else
        {
            var response = await UserService.Register(EntUserName.Text, EntEmail.Text, EntPwd.Text);
            if (response)
            {
                await DisplayAlert("", "Konto zosta�o za�o�one", "Ok");
                await Navigation.PushAsync(new LoginPage());
            }
            else await DisplayAlert("", "Rejestracja nie powiod�a si�", "Spr�buj ponownie");
        }
    }

    async void TapLogin_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }
}