using KTOP.Models;
using KTOP.Services;

namespace KTOP.Pages.Homes;

public partial class OwnerHomeDetailsPage : ContentPage
{
    private readonly int homeId;
	public OwnerHomeDetailsPage(int homeId)
	{
		InitializeComponent();
        this.homeId = homeId;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        HomeModel home = await HomeService.GetHomeById(homeId);
        homeNameL.Text = home.HomeName;
    }

    void BackImgBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    async void TapDelHome_Tapped(object sender, TappedEventArgs e)
    {
        var response = await DisplayAlert("","Czy napewno chcesz usun�� to miejsce wraz z jego produktami?","Tak","Nie");
        try
        {
            if (response)
            {
                var request = await HomeService.DeleteUserHome(homeId);
                if (request)
                {
                    await DisplayAlert("", "Miejsce zosta�o usuni�te", "Ok");
                    await Navigation.PopAsync();
                }
                else await DisplayAlert("", "Usuni�cie miejsca nie powiod�o si�", "Spr�buj ponownie");
            }
        }
        catch (Exception)
        {
            await DisplayAlert("", "Usuni�cie miejsca nie powiod�o si�", "Spr�buj ponownie");
        }
    }

    async void TapEditHome_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new OwnerEditHomePage(homeId));
    }

    async void TapHomeProd_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new HomeProductsPage(homeId));
    }

    async void TapHomeUsrs_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new OwnerUsersHomePage(homeId));
    }
}