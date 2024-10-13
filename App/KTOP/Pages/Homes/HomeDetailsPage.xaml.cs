using KTOP.Models;
using KTOP.Services;

namespace KTOP.Pages.Homes;

public partial class HomeDetailsPage : ContentPage
{
    private readonly int homeId;
    public HomeDetailsPage(int homeId)
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

    async void TapHomeProd_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new HomeProductsPage(homeId));
    }

    async void TapHomeUsrs_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new UsersHomePage(homeId));
    }
}