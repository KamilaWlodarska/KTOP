using KTOP.Services;

namespace KTOP.Pages.Settings;

public partial class DeleteUserAccount : ContentPage
{
	public DeleteUserAccount()
	{
		InitializeComponent();
	}

    void BackImgBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void DelUserBtn_Clicked(object sender, EventArgs e)
    {
        var response = await DisplayAlert("", "Czy napewno chcesz usun�� konto?", "Tak", "Nie");
        if (response)
        {
            try
            {
                if (EntPwd.Text == null || EntPwd.Text.Length.Equals(0))
                {
                    await DisplayAlert("", "Uzupe�nij wszystkie dane", "Ok");
                }
                else
                {
                    var result = await UserService.DeleteUser(EntPwd.Text);
                    if (result)
                    {
                        await DisplayAlert("", "Konto zosta�o usuni�te", "Ok");
                        Preferences.Set("accesstoken", string.Empty);
                        Application.Current.MainPage = new NavigationPage(new WelcomePage());
                    }
                    else await DisplayAlert("", "Usuwanie konta nie powiod�o si�", "Spr�buj ponownie");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("", "Usuwanie konta nie powiod�o si�", "Spr�buj ponownie");
            }
        }
    }
}