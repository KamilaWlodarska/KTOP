using KTOP.Services;

namespace KTOP.Pages.Settings;

public partial class ChangeUserPwdPage : ContentPage
{
	public ChangeUserPwdPage()
	{
		InitializeComponent();
	}

    void BackImgBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void ChngPwdBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (EntCurPwd.Text == null || EntCurPwd.Text.Length.Equals(0) || EntNewPwd.Text == null || EntNewPwd.Text.Length.Equals(0) || EntConfNewPwd.Text == null || EntConfNewPwd.Text.Length.Equals(0))
            {
                await DisplayAlert("", "Uzupe�nij wszystkie dane", "Ok");
            }
            else
            {
                var result = await UserService.ChangePassword(EntCurPwd.Text, EntNewPwd.Text, EntConfNewPwd.Text);
                if (result)
                {
                    await DisplayAlert("", "Has�o zosta�o zmienione", "Ok");
                    await Navigation.PopAsync();
                }
                else await DisplayAlert("", "Zmiana has�a nie powiod�a si�", "Spr�buj ponownie");
            }
        }
        catch (Exception)
        {
            await DisplayAlert("", "Zmiana has�a nie powiod�a si�", "Spr�buj ponownie");
        }
    }
}