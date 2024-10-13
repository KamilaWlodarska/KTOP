using KTOP.Models;
using KTOP.Services;

namespace KTOP.Pages.Homes;

public partial class SearchHomePage : ContentPage
{
	public SearchHomePage()
	{
		InitializeComponent();
	}

    void BackImgBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void HomeSB_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            SearchBar searchBar = (SearchBar)sender;
            if (string.IsNullOrWhiteSpace(searchBar.Text)) CVHomeSearch.ItemsSource = null;
            CVHomeSearch.ItemsSource = await HomeService.SearchUserHomeByName(searchBar.Text);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private async void CVHomeSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelection = e.CurrentSelection.FirstOrDefault() as HomeModel;
        if (currentSelection == null) return;
        if(currentSelection.OwnerId == UserService.UserID) await Navigation.PushAsync(new OwnerHomeDetailsPage(currentSelection.HomeId));
        else await Navigation.PushAsync(new HomeDetailsPage(currentSelection.HomeId));
        ((CollectionView)sender).SelectedItem = null;
    }
}