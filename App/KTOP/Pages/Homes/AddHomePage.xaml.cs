using KTOP.Models;
using KTOP.Services;

namespace KTOP.Pages.Homes;

public partial class AddHomePage : ContentPage
{
	public AddHomePage()
	{
		InitializeComponent();
	}

    void BackImgBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void AddHomeBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (EntHomeName.Text == null || EntHomeName.Text.Length.Equals(0))
            {
                await DisplayAlert("", "Uzupe³nij wszystkie dane", "Ok");
            }
            else
            {
                HomeOwnerAdd addHome = new()
                {
                    HomeName = this.EntHomeName.Text
                };
                
                var result = await HomeService.AddUserHome(addHome);
                if (result)
                {
                    await DisplayAlert("", "Dodano nowe miejsce", "Ok");
                    await Navigation.PopAsync();
                }
                else await DisplayAlert("", "Dodawanie miejsca nie powiod³o siê", "Spróbuj ponownie");
            }
        }
        catch (Exception)
        {
            await DisplayAlert("", "Dodawanie miejsca nie powiod³o siê", "Spróbuj ponownie");
        }
    }
}