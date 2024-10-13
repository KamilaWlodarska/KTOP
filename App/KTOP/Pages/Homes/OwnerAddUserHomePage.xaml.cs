using KTOP.Services;

namespace KTOP.Pages.Homes;

public partial class OwnerAddUserHomePage : ContentPage
{
    private readonly int homeId;
    public OwnerAddUserHomePage(int homeId)
	{
		InitializeComponent();
        this.homeId = homeId;
    }

    void BackImgBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void AddUserBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (EntUserEmail.Text == null || EntUserEmail.Text.Length.Equals(0))
            {
                await DisplayAlert("", "Uzupe³nij wszystkie dane", "Ok");
            }
            else
            {
                var result = await HomeService.AddHomeUser(homeId, EntUserEmail.Text);
                if(result)
                {
                    await DisplayAlert("", "Dodano u¿ytkownika", "Ok");
                    await Navigation.PopAsync();
                }
                else await DisplayAlert("", "Dodawanie u¿ytkownika nie powiod³o siê", "Spróbuj ponownie");
            }
        }
        catch (Exception)
        {
            await DisplayAlert("", "Dodawanie u¿ytkownika nie powiod³o siê", "Spróbuj ponownie");
        }
    }
}