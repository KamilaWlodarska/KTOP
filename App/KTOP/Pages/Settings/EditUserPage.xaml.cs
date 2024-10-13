using KTOP.Models;
using KTOP.Services;

namespace KTOP.Pages.Settings;

public partial class EditUserPage : ContentPage
{
	public EditUserPage()
	{
		InitializeComponent();
	}

    void BackImgBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        UserModel user = await UserService.GetUserById();
        if (user != null)
        {
            this.EntUserName.Text = user.UserName;
            this.EntUserEmail.Text = user.Email;
        }        
    }

    private async void EditUserBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (EntUserName.Text == null || EntUserName.Text.Length.Equals(0) || EntUserEmail.Text == null || EntUserEmail.Text.Length.Equals(0))
            {
                await DisplayAlert("", "Uzupe�nij wszystkie dane", "Ok");
            }
            else
            {
                var result = await UserService.EditUserData(EntUserName.Text, EntUserEmail.Text);
                if (result)
                {
                    await DisplayAlert("", "Dane zosta�y zmienione", "Ok");
                    await Navigation.PopAsync();
                }
                else await DisplayAlert("", "Zmiana danych nie powiod�a si�", "Spr�buj ponownie");
            }
        }            
        catch (Exception)
        {
            await DisplayAlert("", "Zmiana danych nie powiod�a si�", "Spr�buj ponownie");
        }
    }
}