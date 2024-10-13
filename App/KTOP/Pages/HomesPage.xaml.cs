using KTOP.Models;
using KTOP.Services;
using KTOP.Pages.Homes;

namespace KTOP.Pages;

public partial class HomesPage : ContentPage
{
    public HomesPage()
	{
		InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        List<HomeModel> homesList = await HomeService.GetAllUserHomes();
        if(homesList != null) this.CVHome.ItemsSource = homesList;
        this.CVHome.SelectedItem = null;
    }
    
    async void CVHome_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelection = e.CurrentSelection.FirstOrDefault() as HomeModel;
        if (currentSelection == null) return;
        if (currentSelection.OwnerId == UserService.UserID) await Navigation.PushAsync(new OwnerHomeDetailsPage(currentSelection.HomeId));
        else await Navigation.PushAsync(new HomeDetailsPage(currentSelection.HomeId));
    }

    async void TapHomeSearch_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new SearchHomePage());
    }

    async void AddHomeImgBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddHomePage());
    }
}