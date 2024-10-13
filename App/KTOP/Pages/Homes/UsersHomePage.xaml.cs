using KTOP.Models;
using KTOP.Services;

namespace KTOP.Pages.Homes;

public partial class UsersHomePage : ContentPage
{
    private readonly int homeId;
    public UsersHomePage(int homeId)
	{
		InitializeComponent();
        this.homeId = homeId;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        List<UsersHomeModel> homeUsersList = await HomeService.GetAllHomeUsers(homeId);
        if (homeUsersList != null) this.CVHomeUsers.ItemsSource = homeUsersList;
        this.CVHomeUsers.SelectedItem = null;
    }

    void BackImgBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}