using KTOP.Models;
using KTOP.Services;

namespace KTOP.Pages.Homes;

public partial class OwnerEditHomePage : ContentPage
{
    private readonly int homeId;
    public OwnerEditHomePage(int homeId)
	{
		InitializeComponent();
        this.homeId = homeId;
	}

    void BackImgBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        List<UsersHomeModel> usersHomeList = await HomeService.GetAllHomeUsers(homeId);
        if (usersHomeList != null) this.ownerPicker.ItemsSource = usersHomeList.ToList();
        HomeModel home = await HomeService.GetHomeById(homeId);
        if (home == null) return;
        else
        {
            UserModel user = await UserService.GetUserById();
            UsersHomeModel selectedUserHome = usersHomeList.FirstOrDefault(u => u.UserId == user.UserId);
            this.EntHomeName.Text = home.HomeName;
            this.ownerPicker.Title = user.UserName;
            this.ownerPicker.SelectedItem = selectedUserHome;
        }
    }

    private async void EditHomeBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (EntHomeName.Text == null || EntHomeName.Text.Length.Equals(0))
            {
                await DisplayAlert("", "Uzupe³nij wszystkie dane", "Ok");
            }
            else
            {
                UsersHomeModel userhome = this.ownerPicker.SelectedItem as UsersHomeModel;
                HomeModel editHome = new()
                {
                    HomeName = this.EntHomeName.Text,
                    OwnerId = userhome.UserId
                };

                var result = await HomeService.EditUserHome(homeId, editHome);
                if (result)
                {
                    await DisplayAlert("", "Edytowano dane miejsca", "Ok");
                    if(editHome.OwnerId == UserService.UserID) await Navigation.PopAsync();
                    else await Navigation.PopToRootAsync();
                }
                else await DisplayAlert("", "Edycja danych miejsca nie powiod³a siê", "Spróbuj ponownie");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("", "Edycja danych miejsca nie powiod³a siê", "Spróbuj ponownie");
            Console.WriteLine(ex);
        }
    }
}