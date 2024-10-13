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
        var response = await DisplayAlert("", "Czy napewno chcesz usun¹æ konto?", "Tak", "Nie");
        if (response)
        {
            try
            {
                if (EntPwd.Text == null || EntPwd.Text.Length.Equals(0))
                {
                    await DisplayAlert("", "Uzupe³nij wszystkie dane", "Ok");
                }
                else
                {
                    var result = await UserService.DeleteUser(EntPwd.Text);
                    if (result)
                    {
                        await DisplayAlert("", "Konto zosta³o usuniête", "Ok");
                        Preferences.Set("accesstoken", string.Empty);
                        Application.Current.MainPage = new NavigationPage(new WelcomePage());
                    }
                    else await DisplayAlert("", "Usuwanie konta nie powiod³o siê", "Spróbuj ponownie");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("", "Usuwanie konta nie powiod³o siê", "Spróbuj ponownie");
            }
        }
    }
}