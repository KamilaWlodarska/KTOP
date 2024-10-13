using KTOP.Models;
using KTOP.Services;

namespace KTOP.Pages.Homes;

public partial class OwnerUsersHomePage : ContentPage
{
    private readonly int homeId;
	public OwnerUsersHomePage(int homeId)
	{
		InitializeComponent();
        this.homeId = homeId;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        List<UsersHomeModel> homeUsersList = await HomeService.GetAllHomeUsers(homeId);
        if(homeUsersList != null) this.CVHomeUsers.ItemsSource = homeUsersList;
        this.CVHomeUsers.SelectedItem = null;
    }

    void BackImgBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void TapAddUser_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new OwnerAddUserHomePage(homeId));
    }

    private async void TapDelUser_Tapped(object sender, TappedEventArgs e)
    {
        UsersHomeModel selectedUser = CVHomeUsers.SelectedItem as UsersHomeModel;
        if(selectedUser != null)
        {
            var result = await DisplayAlert("", "Czy napewno chcesz usun¹æ wybranego u¿ytkownika?", "Tak", "Nie");
            try
            {
                if (result)
                {
                    var deleteUser = await HomeService.DeleteHomeUser(homeId, selectedUser.UserId);
                    if (deleteUser)
                    {
                        await DisplayAlert("", "U¿ytkownik zosta³ usuniêty", "Ok");
                        CVHomeUsers.ItemsSource = await HomeService.GetAllHomeUsers(homeId);
                        CVHomeUsers.SelectedItem = null;
                    }
                    else await DisplayAlert("", "Usuwanie u¿ytkownika nie powiod³o siê", "Spróbuj ponownie");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("", "Usuniêcie u¿ytkownika nie powiod³o siê", "Spróbuj ponownie");
            }
        }
        else await DisplayAlert("", "Wybierz u¿ytkownika", "Ok");
    }
}